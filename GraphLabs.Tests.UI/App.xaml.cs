using System;
using System.Windows;
using Autofac;
using GraphLabs.Utils.Services;

namespace GraphLabs.Tests.UI
{
    public partial class App : Application
    {
        private IContainer _container;
        private MainPage _page;

        public App()
        {
            this.Startup += this.Application_Startup;
            this.Exit += this.Application_Exit;
            this.UnhandledException += this.Application_UnhandledException;

            InitializeAutofac();

            InitializeComponent();
        }

        private void InitializeAutofac()
        {
            var builder = new ContainerBuilder();
            builder.RegisterInstance(new DateTimeService()).As<IDateTimeService>();

            _container = builder.Build();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            this.RootVisual = _page = new MainPage(_container);
        }

        private void Application_Exit(object sender, EventArgs e)
        {
            _page.Dispose();
        }

        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            // If the app is running outside of the debugger then report the exception using
            // the browser's exception mechanism. On IE this will display it a yellow alert 
            // icon in the status bar and Firefox will display a script error.
            if (!System.Diagnostics.Debugger.IsAttached)
            {

                // NOTE: This will allow the application to continue running after an exception has been thrown
                // but not handled. 
                // For production applications this error handling should be replaced with something that will 
                // report the error to the website and stop the application.
                e.Handled = true;
                Deployment.Current.Dispatcher.BeginInvoke(delegate { ReportErrorToDOM(e); });
            }
        }

        private void ReportErrorToDOM(ApplicationUnhandledExceptionEventArgs e)
        {
            try
            {
                string errorMsg = e.ExceptionObject.Message + e.ExceptionObject.StackTrace;
                errorMsg = errorMsg.Replace('"', '\'').Replace("\r\n", @"\n");

                System.Windows.Browser.HtmlPage.Window.Eval("throw new Error(\"Unhandled Error in Silverlight Application " + errorMsg + "\");");
            }
            catch (Exception)
            {
            }
        }
    }
}
