using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using System.Windows;
using GraphLabs.Components.Controls;
using GraphLabs.Components.Controls.ViewModels;
using GraphLabs.Components.UserActionsRegistrator;

namespace GraphLabs.Components
{
    /// <summary> Менеджер по сохранению действий студентов (по совместительству - ViewModel для InformationBar) </summary>
    public class UserActionsManager : DependencyObject, IDisposable
    {
        /// <summary> Сообщения </summary>
        protected ObservableCollection<LogEventViewModel> InternalLog { get; private set; }

        /// <summary> Сервис регистрации действий студента </summary>
        protected UserActionsRegistratorClient UserActionsRegistratorClient { get; private set; }

        /// <summary> Идентификатор задания </summary>
        protected long TaskId { get; private set; }

        /// <summary> Идентификатор сессии </summary>
        protected Guid SessionGuid { get; private set; }

        /// <summary> Ещё не зарегистрированные действия </summary>
        protected virtual LinkedList<ActionDescription> NonRegisteredActions { get; private set; }

        /// <summary> Начальный балл </summary>
        public const int STARTING_SCORE = 100;

        #region DependencyProperties

        /// <summary> Идёт отправка отчёта? </summary>
        public static readonly DependencyProperty IsSendingReportProperty =
            DependencyProperty.Register("IsSendingReport", typeof(bool), typeof(UserActionsManager), new PropertyMetadata(false));

        /// <summary> Идёт отправка отчёта? </summary>
        public bool IsSendingReport
        {
            get { return (bool)GetValue(IsSendingReportProperty); }
            set { SetValue(IsSendingReportProperty, value); }
        }

        /// <summary> Отправлять отчёт после каждого действия? По-умолчанию false, т.е. отправка отчёта происходит только при совершении ошибки. </summary>
        public static readonly DependencyProperty SendReportOnEveryActionProperty =
            DependencyProperty.Register("SendReportOnEveryAction", typeof(bool), typeof(UserActionsManager), new PropertyMetadata(false));

        /// <summary> Отправлять отчёт после каждого действия? По-умолчанию false, т.е. отправка отчёта происходит только при совершении ошибки. </summary>
        public bool SendReportOnEveryAction
        {
            get { return (bool)GetValue(SendReportOnEveryActionProperty); }
            set { SetValue(SendReportOnEveryActionProperty, value); }
        }

        /// <summary> Сообщения </summary>
        public static readonly DependencyProperty LogProperty =
            DependencyProperty.Register("Log", typeof(ReadOnlyObservableCollection<LogEventViewModel>), typeof(InformationBar), new PropertyMetadata(default(ReadOnlyObservableCollection<LogEventViewModel>)));

        /// <summary> Сообщения </summary>
        public ReadOnlyObservableCollection<LogEventViewModel> Log
        {
            get { return (ReadOnlyObservableCollection<LogEventViewModel>)GetValue(LogProperty); }
            set { SetValue(LogProperty, value); }
        }

        /// <summary> Текущий балл </summary>
        public static readonly DependencyProperty ScoreProperty =
            DependencyProperty.Register("Score", typeof(int), typeof(InformationBar), new PropertyMetadata(STARTING_SCORE));

        /// <summary> Текущий балл </summary>
        public int Score
        {
            get { return (int)GetValue(ScoreProperty); }
            set { SetValue(ScoreProperty, value); }
        }

        #endregion

        /// <summary> ViewModel для панели информации </summary>
        public UserActionsManager(long taskId, Guid sessionGuid)
        {
            TaskId = taskId;
            SessionGuid = sessionGuid;
            InternalLog = new ObservableCollection<LogEventViewModel>();
            Log = new ReadOnlyObservableCollection<LogEventViewModel>(InternalLog);
            NonRegisteredActions = new LinkedList<ActionDescription>();

            InitClient();
        }

        private void InitClient()
        {
            UserActionsRegistratorClient = new UserActionsRegistratorClient();
            UserActionsRegistratorClient.RegisterUserActionsCompleted += RegisterUserActionsCompleted;
        }

        /// <summary> Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources. </summary>
        public virtual void Dispose()
        {
            const int CHECK_SENDING_COMPLETE_INTERVAL = 100;
            while (IsSendingReport)
            {
                Thread.Sleep(CHECK_SENDING_COMPLETE_INTERVAL);
            }
        }

        #region Actions

        /// <summary> Добавить событие </summary>
        public virtual void AddInfoEvent(string text)
        {
            AddActionInternal(text);
            if (SendReportOnEveryAction)
                SendReport();
        }

        /// <summary> Добавить ошибку </summary>
        public virtual void RegisterMistake(string description, short penalty)
        {
            AddActionInternal(description, penalty);
            SendReport();
        }

        /// <summary> Принудительно отправляет действия </summary>
        public virtual void SendReport()
        {
            Contract.Requires(!IsSendingReport, "Вызвана повторная отправка отчёта в то время, как предыдущая ещё не завершена.");

            UserActionsRegistratorClient.RegisterUserActionsAsync(TaskId, SessionGuid, NonRegisteredActions.ToArray());
            NonRegisteredActions.Clear();
            IsSendingReport = true;
        }

        private void RegisterUserActionsCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                throw new Exception(string.Format("Не удалось отправить отчёт о действиях: {0}", e.Error));
            }
            IsSendingReport = false;
        }

        /// <summary> Ставит задание в буфер для последующей отправки </summary>
        protected virtual void AddActionInternal(string description, short penalty = 0)
        {
            NonRegisteredActions.AddLast(new ActionDescription { Description = description, Penalty = penalty, TimeStamp = DateTime.Now });
        }

        #endregion
    }
}
