using System;
using System.Collections.Generic;
using System.Windows;
using Autofac;
using GraphLabs.CommonUI.Configuration;

namespace GraphLabs.CommonUI
{
    /// <summary> Базовая точка входа модуля-задания </summary>
    public abstract class TaskApplicationBase : Application
    {
        /// <summary> Параметры запуска задания </summary>
        protected StartupParameters StartupParameters { get; private set; }

        private IViewBuilder ViewBuilder
        {
            get { return DependencyResolver.Current.Resolve<IViewBuilder>(); }
        }

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

            RootVisual = ViewBuilder.BuildView(StartupParameters, IsRunningOutOfBrowser);
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