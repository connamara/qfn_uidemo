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

        private void Send(QuickFix.Message m)
        {
            if (this.ActiveSessionID != null)
                QuickFix.Session.SendToTarget(m, this.ActiveSessionID);
        }

        private bool CanSendMessage()
        {
            if (Initiator.IsLoggedOn() == false)
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

            QuickFix.FIX42.News news = new QuickFix.FIX42.News(headline);
            QuickFix.FIX42.News.LinesOfTextGroup group = new QuickFix.FIX42.News.LinesOfTextGroup();
            foreach (string s in lines)
            {
                group.Text = new QuickFix.Fields.Text(s);
                news.AddGroup(group);
            }

            if (lines.Count == 0)
            {
                QuickFix.Fields.LinesOfText noLines = new QuickFix.Fields.LinesOfText(0);
                news.SetField(noLines, true);
            }

            this.Send(news);
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


            QuickFix.FIX42.NewOrderSingle nos = new QuickFix.FIX42.NewOrderSingle(
                fClOrdID, fHandlInst, fSymbol, fSide, fTransactTime, fOrdType);
            nos.OrderQty = fOrderQty;

            this.Send(nos);
            return nos;
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


        public void OnMessage(QuickFix.FIX42.BusinessMessageReject msg, QuickFix.SessionID s) { }

        public void OnMessage(QuickFix.FIX42.ExecutionReport msg, QuickFix.SessionID s) 
        {
            if (Fix42ExecReportEvent != null)
                Fix42ExecReportEvent(msg);
        }
    }
}
