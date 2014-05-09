using System;
using System.Windows;
using Autofac;
using GraphLabs.Common;
using GraphLabs.Common.UserActionsRegistrator;
using GraphLabs.Utils;
using GraphLabs.Utils.Services;

namespace GraphLabs.CommonUI
{
    /// <summary> Базовая ViewModel задания </summary>
    public abstract class TaskViewModelBase<TView> : DependencyObject
        where TView : TaskViewBase
    {
        private DisposableWcfClientWrapper<ITasksDataServiceClient> DataServiceClient
        {
            get
            {
                return DependencyResolver.Resolve<DisposableWcfClientWrapper<ITasksDataServiceClient>>();
            }
        }

        private DisposableWcfClientWrapper<IUserActionsRegistratorClient> ActionsRegistratorClient
        {
            get
            {
                return DependencyResolver.Resolve<DisposableWcfClientWrapper<IUserActionsRegistratorClient>>();
            }
        }

        /// <summary> Сервис системного времени </summary>
        protected IDateTimeService DateTimeService
        {
            get
            {
                return DependencyResolver.Resolve<IDateTimeService>();
            }
        }

        /// <summary> IOC-контейнер </summary>
        protected IContainer DependencyResolver
        {
            get { return CommonUI.DependencyResolver.Current; }
        }

        /// <summary> View </summary>
        protected TView View { get; private set; }

        /// <summary> Поставщик варианта </summary>
        protected VariantProvider VariantProvider { get; private set; }

        /// <summary> Допустимые версии генератора, с помощью которого сгенерирован вариант </summary>
        protected abstract Version[] AllowedGeneratorVersions { get; }

        /// <summary> Параметры запуска </summary>
        protected StartupParameters StartupParameters { get; private set; }


        #region Public свойства вьюмодели
        // ReSharper disable StaticFieldInGenericType

        /// <summary> Регистратор действий студента </summary>
        public static readonly DependencyProperty UserActionsManagerProperty =
            DependencyProperty.Register(
            ExpressionUtility.NameForMember((TaskViewModelBase<TView> m) => m.UserActionsManager),
            typeof(UserActionsManager),
            typeof(TaskViewModelBase<TView>), 
            new PropertyMetadata(default(UserActionsManager)));

        /// <summary> Регистратор действий студента </summary>
        public UserActionsManager UserActionsManager
        {
            get { return (UserActionsManager)GetValue(UserActionsManagerProperty); }
            set { SetValue(UserActionsManagerProperty, value); }
        }

        // ReSharper restore StaticFieldInGenericType
        #endregion

        /// <summary> Инициализировать ViewModel и записаться во view.DataContext </summary>
        public void Initialize(TView view, StartupParameters startupParameters, bool sendReportOnEveryAction)
        {
            StartupParameters = startupParameters;

            VariantProvider = new VariantProvider(StartupParameters.TaskId, StartupParameters.SessionGuid, AllowedGeneratorVersions, DataServiceClient);
            VariantProvider.VariantDownloaded += (s, e) => OnTaskLoadingComlete(e);

            UserActionsManager = new UserActionsManager(StartupParameters.TaskId, StartupParameters.SessionGuid, ActionsRegistratorClient, DateTimeService)
            {
                SendReportOnEveryAction = sendReportOnEveryAction
            };

            view.DataContext = this;
            View = view;

            OnInitialized();
        }

        /// <summary> Инициализация завершена </summary>
        protected virtual void OnInitialized()
        {
        }

        /// <summary> Вариант загружен </summary>
        protected abstract void OnTaskLoadingComlete(VariantDownloadedEventArgs e);
    }
}
