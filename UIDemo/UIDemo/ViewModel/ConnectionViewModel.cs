using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using UIDemo.Model;
using System.Diagnostics;
using System.Windows.Input;
using FIXApplication;

namespace UIDemo.ViewModel
{
    public class ConnectionViewModel : ViewModelBase
    {
        private QFApp _qfapp = null;

        public ICommand ConnectCommand {get;set;}
        public ICommand DisconnectCommand {get;set;}


        public ConnectionViewModel(QFApp app)
        {
            _qfapp = app;

            // initialize SessionString
            HashSet<QuickFix.SessionID> sidset = _qfapp.MySessionSettings.GetSessions();
            Trace.WriteLine("Sessions count in config: " + sidset.Count);
            foreach (QuickFix.SessionID sid in sidset)
                Trace.WriteLine("-> " + sid.ToString());
            this.SessionString = sidset.First().ToString();

            // command definitions
            ConnectCommand = new RelayCommand(Connect);
            DisconnectCommand = new RelayCommand(Disconnect);

            _qfapp.LogonEvent += new Action(delegate() { IsConnected = true; });
            _qfapp.LogoutEvent += new Action(delegate() { IsConnected = false; });
        }

        private string _session = "";
        public string SessionString
        {
            get { return _session; }
            set { _session = value; base.OnPropertyChanged("SessionString"); }
        }

        private bool _isConnected = false;
        public bool IsConnected
        {
            get { return _isConnected; }
            set { _isConnected = value; base.OnPropertyChanged("IsConnected"); }
        }

        // commands
        private void Connect(object ignored)
        {
            Trace.WriteLine("ConnectionViewModel::Connect called");
            _qfapp.Start();
        }

        private void Disconnect(object ignored)
        {
            Trace.WriteLine("ConnectionViewModel::Disconnect called");
            _qfapp.Stop();
        }
    }
}
