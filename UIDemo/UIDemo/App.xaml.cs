using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace UIDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        AppContext _appContext = null;

        public App()
            : base()
        {
            _appContext = new AppContext();
            _appContext.ConfigFile = "quickfix.cfg";
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            _appContext.Application_DispatcherUnhandledException(sender, e);
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            _appContext.Application_Startup(sender, e, new FIXApplication.NullFixStrategy());
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            _appContext.Application_Exit(sender, e);
        }
    }
}
