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
        public QuickFix.SessionID ActiveSessionID { get; set; }
        public QuickFix.SessionSettings SessionSettings { get; set; }

        public event Action LogonEvent;
        public event Action LogoutEvent;

        public event Action<QuickFix.FIX42.ExecutionReport> Fix42ExecReportEvent;

        private QuickFix.Initiator _initiator;

        public QFApp()
        {
            ActiveSessionID = null;
            SessionSettings = new QuickFix.SessionSettings("quickfix.cfg");
        }

        public void Start()
        {
            Trace.WriteLine("QFApp::Start() called");

            if (_initiator == null)
            {
                Trace.WriteLine("Creating new initiator");
                QuickFix.MessageStoreFactory storeFactory = new QuickFix.FileStoreFactory(SessionSettings);
                QuickFix.LogFactory logFactory = new QuickFix.ScreenLogFactory(SessionSettings);
                _initiator = new QuickFix.Transport.SocketInitiator(this, storeFactory, SessionSettings, logFactory);
            }
            _initiator.Start();
        }

        public void Stop()
        {
            Trace.WriteLine("QFApp::Stop() called");
            if (_initiator != null)
                _initiator.Stop();
        }

        private void Send(QuickFix.Message m)
        {
            if (this.ActiveSessionID != null)
                QuickFix.Session.SendToTarget(m, this.ActiveSessionID);
        }

        private bool CanSendMessage()
        {
            if (_initiator.IsLoggedOn() == false)
            {
                Trace.WriteLine("Can't send a message.  We're not logged on.");
                return false;
            }
            if (ActiveSessionID == null)
            {
                Trace.WriteLine("Can't send a message.  ActiveSessionID is null.");
                return false;
            }
            return true;
        }

        public void SendNewsMessage(string headline_str, IList<string> lines)
        {
            if (CanSendMessage() == false)
                return;

            QuickFix.Fields.Headline headline = new QuickFix.Fields.Headline(headline_str);

            QuickFix.Message m = null;
            switch (ActiveSessionID.BeginString)
            {
                //case QuickFix.FixValues.BeginString.FIX40:

                case QuickFix.FixValues.BeginString.FIX41:
                    {
                        QuickFix.FIX41.News news41 = new QuickFix.FIX41.News(headline);
                        QuickFix.FIX41.News.LinesOfTextGroup group = new QuickFix.FIX41.News.LinesOfTextGroup();
                        foreach (string s in lines)
                        {
                            group.Text = new QuickFix.Fields.Text(s);
                            news41.AddGroup(group);
                        }
                        m = news41;
                    }
                    break;

                case QuickFix.FixValues.BeginString.FIX42:
                    {
                        QuickFix.FIX42.News news42 = new QuickFix.FIX42.News(headline);
                        QuickFix.FIX42.News.LinesOfTextGroup group = new QuickFix.FIX42.News.LinesOfTextGroup();
                        foreach (string s in lines)
                        {
                            group.Text = new QuickFix.Fields.Text(s);
                            news42.AddGroup(group);
                        }
                        m = news42;
                    }
                    break;

                case QuickFix.FixValues.BeginString.FIX43:
                    {
                        QuickFix.FIX43.News news43 = new QuickFix.FIX43.News(headline);
                        QuickFix.FIX43.News.LinesOfTextGroup group = new QuickFix.FIX43.News.LinesOfTextGroup();
                        foreach (string s in lines)
                        {
                            group.Text = new QuickFix.Fields.Text(s);
                            news43.AddGroup(group);
                        }
                        m = news43;
                    }
                    break;

                case QuickFix.FixValues.BeginString.FIX44:
                    {
                        QuickFix.FIX44.News news44 = new QuickFix.FIX44.News(headline);
                        QuickFix.FIX44.News.LinesOfTextGroup group = new QuickFix.FIX44.News.LinesOfTextGroup();
                        foreach (string s in lines)
                        {
                            group.Text = new QuickFix.Fields.Text(s);
                            news44.AddGroup(group);
                        }
                        m = news44;
                    }
                    break;

                case QuickFix.FixValues.BeginString.FIX50:
                    {
                        QuickFix.FIX50.News news50 = new QuickFix.FIX50.News(headline);
                        QuickFix.FIX50.News.NoLinesOfTextGroup group = new QuickFix.FIX50.News.NoLinesOfTextGroup();
                        foreach (string s in lines)
                        {
                            group.Text = new QuickFix.Fields.Text(s);
                            news50.AddGroup(group);
                        }
                        m = news50;
                    }
                    break;

                default:
                    Trace.WriteLine("FIX version unsupported for type 'News': " + ActiveSessionID.BeginString);
                    return;
            }//end switch on BeginString

            this.Send(m);
        }

        /// <summary>
        /// Returns the message that is sent
        /// </summary>
        /// <param name="isBuy"></param>
        /// <param name="symbol"></param>
        /// <param name="orderQty"></param>
        /// <returns></returns>
        public QuickFix.Message SendNewOrder(bool isBuy, string symbol, int orderQty)
        {
            if (CanSendMessage() == false)
                throw new Exception("Couldn't send it");

            char side_enum = isBuy ? QuickFix.Fields.Side.BUY : QuickFix.Fields.Side.SELL;

            QuickFix.Fields.Side fSide = new QuickFix.Fields.Side(side_enum);
            QuickFix.Fields.HandlInst fHandlInst = new QuickFix.Fields.HandlInst(QuickFix.Fields.HandlInst.MANUAL_ORDER);
            QuickFix.Fields.Symbol fSymbol = new QuickFix.Fields.Symbol(symbol);
            QuickFix.Fields.TransactTime fTransactTime = new QuickFix.Fields.TransactTime(DateTime.Now);
            QuickFix.Fields.OrdType fOrdType = new QuickFix.Fields.OrdType(QuickFix.Fields.OrdType.MARKET);
            QuickFix.Fields.ClOrdID fClOrdID = new QuickFix.Fields.ClOrdID(DateTime.Now.ToString("HHmmssfff"));

            QuickFix.Fields.OrderQty fOrderQty = new QuickFix.Fields.OrderQty(orderQty);

            QuickFix.Message m = null;
            switch (ActiveSessionID.BeginString)
            {
                case QuickFix.FixValues.BeginString.FIX42:
                    QuickFix.FIX42.NewOrderSingle nos = new QuickFix.FIX42.NewOrderSingle(
                        fClOrdID, fHandlInst, fSymbol, fSide, fTransactTime, fOrdType);
                    nos.OrderQty = fOrderQty;
                    m = nos;
                    break;
                default:
                    Trace.WriteLine("Orders are only supported in FIX.4.2 right now");
                    throw new Exception("Couldn't send it");
            }

            this.Send(m);
            return m;
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
        public void OnMessage(QuickFix.FIX42.ExecutionReport msg, QuickFix.SessionID s) 
        {
            if (Fix42ExecReportEvent != null)
                Fix42ExecReportEvent(msg);
        }
        public void OnMessage(QuickFix.FIX43.ExecutionReport msg, QuickFix.SessionID s) { }
        public void OnMessage(QuickFix.FIX44.ExecutionReport msg, QuickFix.SessionID s) { }
        public void OnMessage(QuickFix.FIX50.ExecutionReport msg, QuickFix.SessionID s) { }
    }
}
