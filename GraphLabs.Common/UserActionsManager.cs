using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using GraphLabs.Common.UserActionsRegistrator;
using GraphLabs.Utils;
using GraphLabs.Utils.Services;
using JetBrains.Annotations;

namespace GraphLabs.Common
{
    /// <summary> Менеджер по сохранению действий студентов (по совместительству - ViewModel для InformationBar) </summary>
    public class UserActionsManager : INotifyPropertyChanged, IDisposable
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
        public ReadOnlyObservableCollection<LogEventViewModel> Log
        {
            get { return _log; }
            set
            {
                _log = value;
                OnPropertyChanged(ExpressionUtility.NameForMember(() => Log));
            }
        }

        /// <summary> Текущий балл </summary>
        public int Score
        {
            get { return _score; }
            set
            {
                _score = value;
                OnPropertyChanged(ExpressionUtility.NameForMember(() => Score));
            }
        }

        #endregion


        /// <summary> ViewModel для панели информации </summary>
        public UserActionsManager(long taskId, Guid sessionGuid, IUserActionsRegistratorClient registratorClient, IDateTimeService dateService)
        {
            DateService = dateService;
            TaskId = taskId;
            SessionGuid = sessionGuid;
            InternalLog = new ObservableCollection<LogEventViewModel>();
            Log = new ReadOnlyObservableCollection<LogEventViewModel>(InternalLog);
            NonRegisteredActions = new LinkedList<ActionDescription>();

            InitClient(registratorClient);
        }

        private void InitClient(IUserActionsRegistratorClient registratorClient)
        {
            UserActionsRegistratorClient = registratorClient;
            UserActionsRegistratorClient.CloseCompleted += CloseCompleted;
        }

        #region IDisposable

        /// <summary> Уже уничтожен? </summary>
        protected bool IsDisposed = false;

        /// <summary> Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources. </summary>
        public virtual void Dispose()
        {
            CheckNotDisposed();

            UserActionsRegistratorClient.CloseAsync();
            NonRegisteredActions = null;
            IsDisposed = true;
        }

        /// <summary> Проверить, что мы не уничтожены </summary>
        protected void CheckNotDisposed()
        {
            Contract.Requires(!IsDisposed, "Менеджер задания уже уничтожен.");
        }

        private void CloseCompleted(object sender, AsyncCompletedEventArgs e)
        {
            UserActionsRegistratorClient = null;
        }

        #endregion


        /// <summary> Задание завершено? </summary>
        protected bool IsTaskFinished = false;

        private int _score = StartingScore;
        private ReadOnlyObservableCollection<LogEventViewModel> _log;

        /// <summary> Проверяет, что задание ещё не завершено </summary>
        protected void CheckTaskIsNotFinished()
        {
            Contract.Requires(!IsTaskFinished, "Уже отправлен признак завершения задания.");
        }


        #region Actions

        /// <summary> Добавить событие </summary>
        public virtual void RegisterInfo(string text)
        {
            CheckNotDisposed();
            CheckTaskIsNotFinished();

            AddActionInternal(text);
            if (SendReportOnEveryAction)
                SendReport();
        }

        /// <summary> Добавить ошибку </summary>
        public virtual void RegisterMistake(string description, short penalty)
        {
            CheckNotDisposed();
            CheckTaskIsNotFinished();

            AddActionInternal(description, penalty);
            SendReport();
        }

        /// <summary> Отправить сообщение о том, что задание завершено </summary>
        public void ReportThatTaskFinished()
        {
            CheckNotDisposed();
            CheckTaskIsNotFinished();

            SendReport(true);
        }

        /// <summary> Ставит задание в буфер для последующей отправки </summary>
        protected virtual void AddActionInternal(string description, short penalty = 0)
        {
            NonRegisteredActions.AddLast(new ActionDescription { Description = description, Penalty = penalty, TimeStamp = DateService.Now() });
        }

        #endregion


        #region Отправка данных

        /// <summary> Принудительно отправляет действия </summary>
        protected virtual void SendReport(bool finishTask = false)
        {
            CheckNotDisposed();
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
