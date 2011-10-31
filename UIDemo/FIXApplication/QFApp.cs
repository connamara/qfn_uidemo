using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace FIXApplication
{
    public class QFApp : QuickFix.MessageCracker, QuickFix.Application
    {
        public QuickFix.SessionID ActiveSessionID { get; set; }
        public QuickFix.SessionSettings MySessionSettings { get; set; }

        private QuickFix.IInitiator _initiator = null;
        public QuickFix.IInitiator Initiator
        {
            set
            {
                if (_initiator != null)
                    throw new Exception("You already set the initiator");
                _initiator = value;
            }
            get
            {
                if (_initiator == null)
                    throw new Exception("You didn't provide an initiator");
                return _initiator;
            }
        }

        public event Action LogonEvent;
        public event Action LogoutEvent;

        public event Action<QuickFix.FIX42.ExecutionReport> Fix42ExecReportEvent;

        /// <summary>
        /// Triggered on any message sent or received (arg1: isIncoming)
        /// </summary>
        public event Action<QuickFix.Message, bool> MessageEvent;



        public QFApp(QuickFix.SessionSettings settings)
        {
            ActiveSessionID = null;
            MySessionSettings = settings;
        }

        public void Start()
        {
            Trace.WriteLine("QFApp::Start() called");
            Initiator.Start();
        }

        public void Stop()
        {
            Trace.WriteLine("QFApp::Stop() called");
            Initiator.Stop();
        }

        /// <summary>
        /// Tries to send the message; throws if not logged on.
        /// </summary>
        /// <param name="m"></param>
        public void Send(QuickFix.Message m)
        {
            if (Initiator.IsLoggedOn() == false)
                throw new Exception("Can't send a message.  We're not logged on.");
            if (ActiveSessionID == null)
                throw new Exception("Can't send a message.  ActiveSessionID is null (not logged on?).");

            QuickFix.Session.SendToTarget(m, this.ActiveSessionID);
        }


        #region Application Members

        public void FromAdmin(QuickFix.Message message, QuickFix.SessionID sessionID)
        {
            Trace.WriteLine("## FromAdmin: " + message.ToString());
            if (MessageEvent != null)
                MessageEvent(message, false);
        }

        public void FromApp(QuickFix.Message message, QuickFix.SessionID sessionID)
        {
            Trace.WriteLine("## FromApp: " + message.ToString());
            if (MessageEvent != null)
                MessageEvent(message, false);
            Crack(message, sessionID);
        }

        public void OnCreate(QuickFix.SessionID sessionID)
        {
            Trace.WriteLine("## OnCreate: " + sessionID.ToString());
        }

        public void OnLogon(QuickFix.SessionID sessionID)
        {
            this.ActiveSessionID = sessionID;
            Trace.WriteLine(String.Format("==OnLogon: {0}==", this.ActiveSessionID.ToString()));
            if (LogonEvent != null)
                LogonEvent();
        }

        public void OnLogout(QuickFix.SessionID sessionID)
        {
            Trace.WriteLine(String.Format("==OnLogout: {0}==", this.ActiveSessionID.ToString()));
            if (LogoutEvent != null)
                LogoutEvent();
        }

        public void ToAdmin(QuickFix.Message message, QuickFix.SessionID sessionID)
        {
            Trace.WriteLine("## ToAdmin: " + message.ToString());
            if (MessageEvent != null)
                MessageEvent(message, true);
        }

        public void ToApp(QuickFix.Message message, QuickFix.SessionID sessionId)
        {
            Trace.WriteLine("## ToApp: " + message.ToString());
            if (MessageEvent != null)
                MessageEvent(message, true);
        }

        #endregion


        public void OnMessage(QuickFix.FIX42.BusinessMessageReject msg, QuickFix.SessionID s) { }

        public void OnMessage(QuickFix.FIX42.ExecutionReport msg, QuickFix.SessionID s) 
        {
            if (Fix42ExecReportEvent != null)
                Fix42ExecReportEvent(msg);
        }
    }
}
