using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace UIDemo
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string SessionIDString { get; set; }

        public MainWindow(QuickFix.SessionSettings settings)
        {
            this.SetSessionProps(settings);
            this.DataContext = this;

            InitializeComponent();

            /*
            QuickFix.Application qfApp = new QFApp();
            MessageStoreFactory storeFactory = new FileStoreFactory(settings);
            LogFactory logFactory = new ScreenLogFactory(settings);
            Initiator initiator = new QuickFix.Transport.SocketInitiator(qfApp, storeFactory, settings, logFactory);
             */
        }

        private void SetSessionProps(QuickFix.SessionSettings settings)
        {
            HashSet<QuickFix.SessionID> sidset = settings.GetSessions();
            Trace.WriteLine("Sessions count in config: " + sidset.Count);
            foreach (QuickFix.SessionID sid in sidset)
                Trace.WriteLine("-> " + sid.ToString());
            this.SessionIDString = sidset.First().ToString();

            Trace.WriteLine("Now this.SessionIDString is: " + SessionIDString);
        }
    }
}
