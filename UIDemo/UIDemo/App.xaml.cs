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
using System.Threading;
using FIXApplication;

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

        /// <summary>
        /// Virtual in case you want to derive this class and pass an ICustomFixStrategy to QFApp
        /// </summary>
        /// <param name="settings"></param>
        /// <returns>a new QFApp object</returns>
        protected virtual QFApp CreateQFApp(QuickFix.SessionSettings settings)
        {
            return new QFApp(settings);
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Trace.WriteLine("Uncaught exception:");
            Trace.WriteLine(e.Exception.ToString());
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Trace.WriteLine("Application started.");

            // FIX application setup
            QuickFix.SessionSettings settings = new QuickFix.SessionSettings("quickfix.cfg");
            QuickFix.MessageStoreFactory storeFactory = new QuickFix.FileStoreFactory(settings);
            QuickFix.LogFactory logFactory = new QuickFix.ScreenLogFactory(settings);

            _qfapp = CreateQFApp(settings);

            QuickFix.IInitiator initiator = new QuickFix.Transport.SocketInitiator(_qfapp, storeFactory, settings, logFactory);
            _qfapp.Initiator = initiator;


            // Window creation and context assignment
            MainWindow mainWindow = new MainWindow();
            mainWindow.DataContext = new MainWindowViewModel();

            mainWindow.MessageView.DataContext = new MessageViewModel(_qfapp);
            mainWindow.ExecutionView.DataContext = new ExecutionViewModel(_qfapp);
            mainWindow.ConnectionView.DataContext = new ConnectionViewModel(_qfapp);
            mainWindow.OrderView.DataContext = new OrderViewModel(_qfapp);
            mainWindow.NewsSenderView.DataContext = new NewsSenderViewModel(_qfapp);

            // Set the main UI dispatcher
            SmartDispatcher.SetDispatcher(mainWindow.Dispatcher);

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
