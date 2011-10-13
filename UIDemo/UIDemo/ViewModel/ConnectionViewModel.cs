using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using UIDemo.Model;
using System.Diagnostics;
using System.Windows.Input;

namespace UIDemo.ViewModel
{
    public class ConnectionViewModel : ViewModelBase
    {
        private ConnectionModel _model = null;

        public ConnectionViewModel(ConnectionModel cm)
        {
            _model = cm;

            // initialize SessionString
            HashSet<QuickFix.SessionID> sidset = _model.SessionSettings.GetSessions();
            Trace.WriteLine("Sessions count in config: " + sidset.Count);
            foreach (QuickFix.SessionID sid in sidset)
                Trace.WriteLine("-> " + sid.ToString());
            this.SessionString = sidset.First().ToString();
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


        private RelayCommand connectCommand;
        public ICommand ConnectCommand
        {
            get
            {
                if (connectCommand == null)
                    connectCommand = new RelayCommand(Connect);
                return connectCommand;
            }
        }

        private void Connect(object ignored)
        {
            Trace.WriteLine("ConnectionViewModel::Connect called");
            _model.Connect();
            Trace.WriteLine("ConnectionViewModel::Connect finished");
        }


        private RelayCommand disconnectCommand;
        public ICommand DisconnectCommand
        {
            get
            {
                if (disconnectCommand == null)
                    disconnectCommand = new RelayCommand(Disconnect);
                return disconnectCommand;
            }
        }

        private void Disconnect(object ignored)
        {
            Trace.WriteLine("ConnectionViewModel::Disconnect called");
            _model.Disconnect();
        }
    }
}
