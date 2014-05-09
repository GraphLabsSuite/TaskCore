using System;
using System.Collections.Generic;
using System.Windows;
using GraphLabs.CommonUI.Configuration;

namespace GraphLabs.CommonUI
{
    /// <summary> Базовая точка входа модуля-задания </summary>
    public abstract class TaskApplicationBase<TView, TViewModel> : Application
        where TView : TaskViewBase<TViewModel>, new()
        where TViewModel : TaskViewModelBase, new()
    {
        /// <summary> Параметры запуска задания </summary>
        protected StartupParameters StartupParameters { get; private set; }

        /// <summary> Базовая точка входа модуля-задания </summary>
        protected TaskApplicationBase(params IDependencyResolverConfigurator[] iocConfigurators)
        {
            SetupDependencyResolver(iocConfigurators);
            SubscribeToEvents();
        }

        private void SetupDependencyResolver(IEnumerable<IDependencyResolverConfigurator> iocConfigurators)
        {
            var containerBuilder = DependencyResolver.GetBuilder();
            foreach (var iocConfigurator in iocConfigurators)
            {
                iocConfigurator.Configure(containerBuilder);
            }

            DependencyResolver.Setup();
        }

        private void SubscribeToEvents()
        {
            Startup += Application_Startup;
            Exit += Application_Exit;
            UnhandledException += Application_UnhandledException;
        }

        /// <summary> Сразу после запуска </summary>
        protected virtual void Application_Startup(object sender, StartupEventArgs e)
        {
            InitStartupParameters(e);

            var viewModel = new TViewModel();
            RootVisual = new TView { ViewModel = viewModel };
        }

        private void InitStartupParameters(StartupEventArgs e)
        {
            var taskId = !IsRunningOutOfBrowser ? e.GetTaskId() : 0;
            var sessionGuid = !IsRunningOutOfBrowser ? e.GetSessionGuid() : new Guid();

            StartupParameters = new StartupParameters(taskId, sessionGuid);
        }

        
        /// <summary> Непосредственно перед завершением </summary>
        protected virtual void Application_Exit(object sender, EventArgs e)
        {
        }

        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // ПРИМЕЧАНИЕ. Это позволит приложению выполняться после того, как исключение было выдано,
                // но не было обработано. 
                // Для рабочих приложений такую обработку ошибок следует заменить на код, 
                // оповещающий веб-сайт об ошибке и останавливающий работу приложения.
                e.Handled = true;
                MessageBox.Show(
                    string.Format("Произошла ошибка:\r\n{0}", e.ExceptionObject),
                    "Ошибка",
                    MessageBoxButton.OK);
            }
            else
            {
                //TODO: redirect...
            }
        }
    }
}