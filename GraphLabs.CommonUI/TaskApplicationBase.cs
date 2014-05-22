using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Windows;
using Autofac;
using GraphLabs.CommonUI.Configuration;
using GraphLabs.Tasks.Contract;

namespace GraphLabs.CommonUI
{
    /// <summary> Базовая точка входа модуля-задания </summary>
    public abstract class TaskApplicationBase : Application
    {
        /// <summary> Параметры запуска задания </summary>
        protected InitParams StartupParameters { get; private set; }

        private IViewBuilder ViewBuilder
        {
            get { return DependencyResolver.Current.Resolve<IViewBuilder>(); }
        }

        /// <summary> Базовая точка входа модуля-задания </summary>
        protected TaskApplicationBase(IEnumerable<IDependencyResolverConfigurator> iocConfigurators)
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
            StartupParameters = !IsRunningOutOfBrowser
                ? GetInitParams(e)
                : new InitParams();
        }

        /// <summary> Получить параметры запуска </summary>
        public InitParams GetInitParams(StartupEventArgs args)
        {
            Contract.Requires(args != null);

            var initParamsProvider = DependencyResolver.Current.Resolve<IInitParamsProvider>();
            if (args.InitParams.ContainsKey(initParamsProvider.ParamsKey))
            {
                return initParamsProvider.ParseInitParamsString(args.InitParams[initParamsProvider.ParamsKey]);
            }

            throw new Exception("Не удалось получить параметры модуля-задания.");
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
                e.Handled = true;
                MessageBox.Show(
                    string.Format("Произошла ошибка:\r\n{0}", e.ExceptionObject),
                    "Ошибка",
                    MessageBoxButton.OK);
            }
        }
    }
}