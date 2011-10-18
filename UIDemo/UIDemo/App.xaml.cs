using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using QuickFix;
using UIDemo.View;
using UIDemo.ViewModel;

namespace UIDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        QFApp _qfapp = null;

        public App()
            : base()
        { }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Trace.WriteLine("Uncaught exception:");
            Trace.WriteLine(e.Exception.ToString());
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Trace.WriteLine("Application started.");

            _qfapp = new QFApp();

            MainWindow mainWindow = new MainWindow();
            mainWindow.DataContext = new MainWindowViewModel();

            mainWindow.ExecutionView.DataContext = new ExecutionViewModel(_qfapp);

            // TODO - pull these views out of MainWindow into their own xml files
            mainWindow.ConnectionView.DataContext = new ConnectionViewModel(_qfapp);
            mainWindow.OrderView.DataContext = new OrderViewModel(_qfapp);
            mainWindow.NewsSenderView.DataContext = new NewsSenderViewModel(_qfapp);

            mainWindow.Show();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            try
            {
                Trace.WriteLine("Application exit.");

                _qfapp.Stop();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
    }
}
