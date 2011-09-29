using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using QuickFix;

namespace UIDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        public App()
            : base()
        {

        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Trace.WriteLine("Uncaught exception:");
            Trace.WriteLine(e.Exception.ToString());
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Trace.WriteLine("Application started.");

            if (e.Args.Length != 1)
            {
                string msg = "usage: UIDemo CONFIG_FILENAME";
                MessageBox.Show(msg);
                Trace.WriteLine(msg);
                Trace.WriteLine("aborting.");
                System.Environment.Exit(1);
            }

            SessionSettings settings = new SessionSettings(e.Args[0]);


            MainWindow mainWindow = new MainWindow(settings);
            mainWindow.Show();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Trace.WriteLine("Application exit.");
        }
    }
}
