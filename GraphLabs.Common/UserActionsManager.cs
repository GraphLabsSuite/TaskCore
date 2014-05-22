using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using GraphLabs.Common.UserActionsRegistrator;
using GraphLabs.Common.Utils;
using GraphLabs.Utils;
using GraphLabs.Utils.Services;
using JetBrains.Annotations;

namespace GraphLabs.Common
{
    /// <summary> Менеджер по сохранению действий студентов (по совместительству - ViewModel для InformationBar) </summary>
    public class UserActionsManager : INotifyPropertyChanged, IUiBlockerAsyncProcessor
    {
        /// <summary> Сервис даты-времени </summary>
        protected IDateTimeService DateService { get; private set; }

        /// <summary> Сообщения </summary>
        protected ObservableCollection<LogEventViewModel> InternalLog { get; private set; }

        /// <summary> Сервис регистрации действий студента </summary>
        protected IUserActionsRegistratorClient UserActionsRegistratorClient { get; private set; }

        /// <summary> Идентификатор задания </summary>
        protected long TaskId { get; private set; }

        /// <summary> Идентификатор сессии </summary>
        protected Guid SessionGuid { get; private set; }

        /// <summary> Ещё не зарегистрированные действия </summary>
        protected virtual LinkedList<ActionDescription> NonRegisteredActions { get; private set; }

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
            set
            {
                if (_score == value)
                {
                    return;
                }
                _score = value;
                OnPropertyChanged(ExpressionUtility.NameForMember(() => Score));
            }
        }

        /// <summary> Идёт отправка данных? </summary>
        //TODO: перевести на асинхронную модель
        public bool IsBusy
        {
            get { return false; }
            private set
            {
                if (_isBusy == value)
                {
                    return;
                }
                _isBusy = value;
                OnPropertyChanged(ExpressionUtility.NameForMember(() => IsBusy));
            }
        }

        #endregion


        /// <summary> ViewModel для панели информации </summary>
        public UserActionsManager(long taskId, Guid sessionGuid, 
            DisposableWcfClientWrapper<IUserActionsRegistratorClient> registratorClient, IDateTimeService dateService)
        {
            Contract.Requires(sessionGuid != null);
            Contract.Requires(registratorClient != null);
            Contract.Requires(dateService != null);

            DateService = dateService;
            TaskId = taskId;
            SessionGuid = sessionGuid;
            InternalLog = new ObservableCollection<LogEventViewModel>();
            Log = new ReadOnlyObservableCollection<LogEventViewModel>(InternalLog);
            NonRegisteredActions = new LinkedList<ActionDescription>();

            UserActionsRegistratorClient = registratorClient.Instance;
        }

        /// <summary> Задание завершено? </summary>
        protected bool IsTaskFinished = false;

        private int _score = StartingScore;
        private bool _isBusy = false;

        /// <summary> Проверяет, что задание ещё не завершено </summary>
        protected void CheckTaskIsNotFinished()
        {
            Contract.Requires(!IsTaskFinished, "Уже отправлен признак завершения задания.");
        }


        #region Actions

        /// <summary> Добавить событие </summary>
        public virtual void RegisterInfo(string text)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(text));

            CheckTaskIsNotFinished();

            AddActionInternal(text);
            if (SendReportOnEveryAction)
                SendReport();
        }

        /// <summary> Добавить ошибку </summary>
        public virtual void RegisterMistake(string description, short penalty)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(description));
            Contract.Requires(penalty > 0);
            CheckTaskIsNotFinished();

            AddActionInternal(description, penalty);
            SendReport();
        }

        /// <summary> Отправить сообщение о том, что задание завершено </summary>
        public void ReportThatTaskFinished()
        {
            CheckTaskIsNotFinished();

            SendReport(true);
        }

        /// <summary> Ставит задание в буфер для последующей отправки </summary>
        protected virtual void AddActionInternal(string description, short penalty = 0)
        {
            var actionDescr = new ActionDescription
            {
                Description = description,
                Penalty = penalty,
                TimeStamp = DateService.Now()
            };
            NonRegisteredActions.AddLast(actionDescr);
            AddToLog(actionDescr);
        }

        private void AddToLog(ActionDescription actionDescr)
        {
            InternalLog.Insert(0, new LogEventViewModel { Message = actionDescr.Description, Penalty = actionDescr.Penalty, TimeStamp = actionDescr .TimeStamp});
        }

        #endregion


        #region Отправка данных

        /// <summary> Принудительно отправляет действия </summary>
        protected virtual void SendReport(bool finishTask = false)
        {
            CheckTaskIsNotFinished();

            if (!NonRegisteredActions.Any() && !finishTask)
            {
                return;
            }

            if (finishTask)
                IsTaskFinished = true;


            using (var flag = new AutoResetEvent(false))
            {
                EventHandler<RegisterUserActionsCompletedEventArgs> completedHandler = (s, e) => RegistrationComplete(flag, e);
                UserActionsRegistratorClient.RegisterUserActionsCompleted += completedHandler;
                UserActionsRegistratorClient.RegisterUserActionsAsync(TaskId, SessionGuid, NonRegisteredActions.ToArray(), finishTask);
                flag.WaitOne();
                UserActionsRegistratorClient.RegisterUserActionsCompleted -= completedHandler;
            }
            
            NonRegisteredActions.Clear();
        }

        private void RegistrationComplete(AutoResetEvent flag, RegisterUserActionsCompletedEventArgs registerUserActionsCompletedEventArgs)
        {
            Contract.Requires(flag != null);
            Contract.Requires(registerUserActionsCompletedEventArgs != null);
            Contract.Requires(!registerUserActionsCompletedEventArgs.Cancelled);

            if (registerUserActionsCompletedEventArgs.Error != null)
            {
                throw registerUserActionsCompletedEventArgs.Error;
            }

            var score = registerUserActionsCompletedEventArgs.Result;

            Score = score;

            flag.Set();
        }

        #endregion


        /// <summary> Occurs when a property value changes. </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary> Occurs when a property value changes. </summary>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
