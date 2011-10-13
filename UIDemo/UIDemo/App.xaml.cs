using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using QuickFix;
using UIDemo.ViewModel;

namespace UIDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        ConnectionViewModel _connectionVM = null;

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

            MainWindowViewModel mainWindowVM = new MainWindowViewModel();
            MainWindow mainWindow = new MainWindow();
            mainWindow.DataContext = mainWindowVM;
            mainWindow.ConnectionView.DataContext = mainWindowVM.ConnectionVM;
            mainWindow.OrderView.DataContext = mainWindowVM.OrderVM;
            mainWindow.NewsSenderView.DataContext = mainWindowVM.NewsSenderVM;

            _connectionVM = mainWindowVM.ConnectionVM;

            mainWindow.Show();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Trace.WriteLine("Application exit.");

            _connectionVM.DisconnectCommand.Execute(null);

            this.Shutdown(0);
        }
    }
}
