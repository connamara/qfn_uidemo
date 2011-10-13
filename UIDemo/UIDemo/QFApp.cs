using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIDemo.ViewModel;
using System.Diagnostics;

namespace UIDemo
{
    public class QFApp : QuickFix.MessageCracker, QuickFix.Application
    {
        public ConnectionViewModel ConnectionVM { get; set; } // for a kludge
        public QuickFix.SessionID SessionID { get; set; }

        public QFApp()
        {
            SessionID = null;
        }

        public void Send(QuickFix.Message m)
        {
            if (this.SessionID != null)
                QuickFix.Session.SendToTarget(m, this.SessionID);
        }

        #region Application Members

        public void FromAdmin(QuickFix.Message message, QuickFix.SessionID sessionID)
        {
            Trace.WriteLine("## FromAdmin: " + message.ToString());
        }

        public void FromApp(QuickFix.Message message, QuickFix.SessionID sessionID)
        {
            Trace.WriteLine("## FromApp: " + message.ToString());
            Crack(message, sessionID);
        }

        public void OnCreate(QuickFix.SessionID sessionID)
        {
            Trace.WriteLine("## OnCreate: " + sessionID.ToString());
        }

        public void OnLogon(QuickFix.SessionID sessionID)
        {
            this.SessionID = sessionID;
            Trace.WriteLine(String.Format("==OnLogon: {0}==", this.SessionID.ToString()));
            if (ConnectionVM != null)
                ConnectionVM.IsConnected = true;
        }

        public void OnLogout(QuickFix.SessionID sessionID)
        {
            Trace.WriteLine("==OnLogout==");
            if (ConnectionVM != null)
                ConnectionVM.IsConnected = false;
        }

        public void ToAdmin(QuickFix.Message message, QuickFix.SessionID sessionID)
        {
            Trace.WriteLine("## ToAdmin: " + message.ToString());
        }

        public void ToApp(QuickFix.Message message, QuickFix.SessionID sessionId)
        {
            Trace.WriteLine("## ToApp: " + message.ToString());
        }

        #endregion


        // FIX40-41 don't have rejects
        public void OnMessage(QuickFix.FIX42.BusinessMessageReject msg, QuickFix.SessionID s) { }
        public void OnMessage(QuickFix.FIX43.BusinessMessageReject msg, QuickFix.SessionID s) { }
        public void OnMessage(QuickFix.FIX44.BusinessMessageReject msg, QuickFix.SessionID s) { }
        public void OnMessage(QuickFix.FIX50.BusinessMessageReject msg, QuickFix.SessionID s) { }

        public void OnMessage(QuickFix.FIX40.ExecutionReport msg, QuickFix.SessionID s) { }
        public void OnMessage(QuickFix.FIX41.ExecutionReport msg, QuickFix.SessionID s) { }
        public void OnMessage(QuickFix.FIX42.ExecutionReport msg, QuickFix.SessionID s) { }
        public void OnMessage(QuickFix.FIX43.ExecutionReport msg, QuickFix.SessionID s) { }
        public void OnMessage(QuickFix.FIX44.ExecutionReport msg, QuickFix.SessionID s) { }
        public void OnMessage(QuickFix.FIX50.ExecutionReport msg, QuickFix.SessionID s) { }
    }
}
