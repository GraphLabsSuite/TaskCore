using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using GraphLabs.Common.UserActionsRegistrator;
using GraphLabs.Utils.Services;
using JetBrains.Annotations;

namespace GraphLabs.Common
{
    /// <summary> Менеджер по сохранению действий студентов (по совместительству - ViewModel для InformationBar) </summary>
    public sealed class UserActionsManager : INotifyPropertyChanged, IUiBlockerAsyncProcessor
    {
        /// <summary> Сервис даты-времени </summary>
        private readonly IDateTimeService _dateService;

        /// <summary> Сообщения </summary>
        private readonly ObservableCollection<LogEventViewModel> _internalDisplayLog;

        /// <summary> Сервис регистрации действий студента </summary>
        private readonly IUserActionsRegistratorClient _userActionsRegistratorClient;

        /// <summary> Идентификатор задания </summary>
        private readonly long _taskId;

        /// <summary> Идентификатор сессии </summary>
        private readonly Guid _sessionGuid;

        /// <summary> Ещё не зарегистрированные действия </summary>
        private readonly List<ActionDescription> _notRegisteredActions;

        /// <summary> Начальный балл </summary>
        public const int StartingScore = 100;


        #region Публичные свойства

        /// <summary> Отправлять отчёт после каждого действия? По-умолчанию false, т.е. отправка отчёта происходит только при совершении ошибки. </summary>
        public bool SendReportOnEveryAction { get; set; }

        /// <summary> Сообщения </summary>
        public ReadOnlyObservableCollection<LogEventViewModel> Log { get; set; }

        /// <summary> Текущий балл </summary>
        public int Score
        {
            get { return _score; }
            private set
            {
                if (_score == value)
                {
                    return;
                }
                _score = value;
                OnPropertyChanged(nameof(Score));
            }
        }

        /// <summary> Идёт отправка данных? </summary>
        //TODO: перевести на асинхронную модель
        public bool IsBusy
        {
            get { return _isBusy; }
            private set
            {
                if (_isBusy == value)
                {
                    return;
                }
                _isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }

        #endregion


        /// <summary> ViewModel для панели информации </summary>
        public UserActionsManager(long taskId, Guid sessionGuid, 
            DisposableWcfClientWrapper<IUserActionsRegistratorClient> registratorClient, IDateTimeService dateService)
        {
            Contract.Requires<ArgumentNullException>(sessionGuid != null);
            Contract.Requires<ArgumentNullException>(registratorClient != null);
            Contract.Requires<ArgumentNullException>(dateService != null);

            _dateService = dateService;
            _taskId = taskId;
            _sessionGuid = sessionGuid;
            _userActionsRegistratorClient = registratorClient.Instance;
            _userActionsRegistratorClient.RegisterUserActionsCompleted += RegistrationComplete;

            _internalDisplayLog = new ObservableCollection<LogEventViewModel>();
            Log = new ReadOnlyObservableCollection<LogEventViewModel>(_internalDisplayLog);
            _notRegisteredActions = new List<ActionDescription>();
            _responseQueue = new List<RegisterUserActionsCompletedEventArgs>();
        }

        /// <summary> Задание завершено? </summary>
        private bool _isTaskFinished = false;

        private int _score = StartingScore;
        private bool _isBusy = false;

        /// <summary> Проверяет, что задание ещё не завершено </summary>
        private void CheckTaskIsNotFinished()
        {
            Contract.Requires(!_isTaskFinished, "Уже отправлен признак завершения задания.");
        }


        #region Actions

        /// <summary> Добавить событие </summary>
        public void RegisterInfo(string text)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(text));
            CheckTaskIsNotFinished();

            AddActionInternal(text);
            if (SendReportOnEveryAction)
                SendReport();
        }

        /// <summary> Добавить ошибку </summary>
        public void RegisterMistake(string description, short penalty)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(description));
            Contract.Requires(penalty > 0);
            CheckTaskIsNotFinished();

            AddActionInternal(description, penalty);
            SendReport();
        }

        /// <summary> Отправить сообщение о том, что задание завершено </summary>
        public void ReportThatTaskFinishedAsync()
        {
            CheckTaskIsNotFinished();

            SendReport(true);
        }

        /// <summary> Ставит задание в буфер для последующей отправки </summary>
        private void AddActionInternal(string description, short penalty = 0)
        {
            var actionDescr = new ActionDescription
            {
                Description = description,
                Penalty = penalty,
                TimeStamp = _dateService.Now()
            };

            lock (_notRegisteredActions)
            {
                _notRegisteredActions.Add(actionDescr);
            }
            AddToLog(actionDescr);
        }

        private void AddToLog(ActionDescription actionDescr)
        {
            _internalDisplayLog.Insert(0, new LogEventViewModel { Message = actionDescr.Description, Penalty = actionDescr.Penalty, TimeStamp = actionDescr .TimeStamp});
        }

        #endregion


        #region Отправка данных

        /// <summary> Принудительно отправляет действия </summary>
        private void SendReport(bool finishTask = false)
        {
            CheckTaskIsNotFinished();

            ActionDescription[] actionsToSend;

            lock (_notRegisteredActions)
            {
                if (!_notRegisteredActions.Any() && !finishTask)
                {
                    return;
                }

                actionsToSend = _notRegisteredActions.ToArray();
                _notRegisteredActions.Clear();
            }

            IsBusy = true;

            if (finishTask)
                _isTaskFinished = true;

            _userActionsRegistratorClient.RegisterUserActionsAsync(_taskId, _sessionGuid, actionsToSend, finishTask);
        }

        //todo нет гарантии порядка сообщений
        private readonly List<RegisterUserActionsCompletedEventArgs> _responseQueue;

        private void RegistrationComplete(object sender, RegisterUserActionsCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                throw e.Error;
            }

            Score = e.Result;
            IsBusy = false;

            if (_isTaskFinished)
            {
                OnTaskFinished();
            }
        }

        #endregion


        /// <summary> Occurs when a property value changes. </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary> Occurs when a property value changes. </summary>
        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged(string propertyName = null)
        {
            Interlocked.CompareExchange(ref PropertyChanged, null, null)
                ?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary> Задание завершено </summary>
        public event EventHandler TaskFinished;

        private void OnTaskFinished()
        {
            Interlocked.CompareExchange(ref TaskFinished, null, null)
                ?.Invoke(this, EventArgs.Empty);
        }
    }
}
