using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIDemo.ViewModel;
using System.Diagnostics;

namespace UIDemo.Model
{
    public class ConnectionModel
    {
        private ConnectionViewModel _connectionVM;

        public QuickFix.SessionSettings SessionSettings { get; set; }

        public QuickFix.Transport.SocketInitiator _initiator = null;

        public ConnectionModel(ConnectionViewModel connectionVM)
        {
            _connectionVM = connectionVM;
            SessionSettings = new QuickFix.SessionSettings("quickfix.cfg");
        }

        public void Connect()
        {
            try
            {
                QFApp app = new QFApp(_connectionVM);

                QuickFix.MessageStoreFactory storeFactory = new QuickFix.FileStoreFactory(SessionSettings);
                QuickFix.LogFactory logFactory = new QuickFix.ScreenLogFactory(SessionSettings);
                _initiator = new QuickFix.Transport.SocketInitiator(app, storeFactory, SessionSettings, logFactory);

                _initiator.Start();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
            }
        }

        public void Disconnect()
        {
            try
            {
                _initiator.Stop();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
            }
        }
    }
}
