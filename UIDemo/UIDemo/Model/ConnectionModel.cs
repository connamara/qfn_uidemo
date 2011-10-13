using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIDemo.ViewModel;
using System.Diagnostics;
using System.Collections;

namespace UIDemo.Model
{
    public class ConnectionModel
    {
        public QuickFix.SessionSettings SessionSettings { get; set; }

        private QuickFix.Transport.SocketInitiator _initiator = null;
        private QFApp _qfapp = null;

        public ConnectionModel(QFApp app)
        {
            _qfapp = app;
            SessionSettings = new QuickFix.SessionSettings("quickfix.cfg");
        }

        public void Connect()
        {
            try
            {
                QuickFix.MessageStoreFactory storeFactory = new QuickFix.FileStoreFactory(SessionSettings);
                QuickFix.LogFactory logFactory = new QuickFix.ScreenLogFactory(SessionSettings);
                _initiator = new QuickFix.Transport.SocketInitiator(_qfapp, storeFactory, SessionSettings, logFactory);

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
                Trace.WriteLine("ConnectionModel::Disconnect called");
                _initiator.Stop();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
            }
        }

        public void SendNewsMessage(string headline_str, IList<string> lines)
        {
            if (_qfapp.SessionID == null)
            {
                Trace.WriteLine("Can't send News message because SessionID is null.  Probably not connected?");
                return;
            }

            QuickFix.Fields.Headline headline = new QuickFix.Fields.Headline(headline_str);

            QuickFix.Message m = null;
            switch (_qfapp.SessionID.BeginString)
            {
                //case QuickFix.FixValues.BeginString.FIX40:

                case QuickFix.FixValues.BeginString.FIX41:
                    QuickFix.FIX41.News news41 = new QuickFix.FIX41.News(headline);
                    if (lines.Count > 0)
                    {
                        QuickFix.FIX41.News.LinesOfTextGroup group = new QuickFix.FIX41.News.LinesOfTextGroup();
                        foreach (string s in lines)
                        {
                            group.Text = new QuickFix.Fields.Text(s);
                            news41.AddGroup(group);
                        }
                    }
                    m = news41;
                    break;

                case QuickFix.FixValues.BeginString.FIX42:
                    QuickFix.FIX42.News news42 = new QuickFix.FIX42.News(headline);
                    if (lines.Count > 0)
                    {
                        QuickFix.FIX42.News.LinesOfTextGroup group = new QuickFix.FIX42.News.LinesOfTextGroup();
                        foreach (string s in lines)
                        {
                            group.Text = new QuickFix.Fields.Text(s);
                            news42.AddGroup(group);
                        }
                    }
                    m = news42;
                    break;

                case QuickFix.FixValues.BeginString.FIX43:
                    QuickFix.FIX43.News news43 = new QuickFix.FIX43.News(headline);
                    if (lines.Count > 0)
                    {
                        QuickFix.FIX43.News.LinesOfTextGroup group = new QuickFix.FIX43.News.LinesOfTextGroup();
                        foreach (string s in lines)
                        {
                            group.Text = new QuickFix.Fields.Text(s);
                            news43.AddGroup(group);
                        }
                    }
                    m = news43;
                    break;

                case QuickFix.FixValues.BeginString.FIX44:
                    QuickFix.FIX44.News news44 = new QuickFix.FIX44.News(headline);
                    if (lines.Count > 0)
                    {
                        QuickFix.FIX44.News.LinesOfTextGroup group = new QuickFix.FIX44.News.LinesOfTextGroup();
                        foreach (string s in lines)
                        {
                            group.Text = new QuickFix.Fields.Text(s);
                            news44.AddGroup(group);
                        }
                    }
                    m = news44;
                    break;

                case QuickFix.FixValues.BeginString.FIX50:
                    QuickFix.FIX50.News news50 = new QuickFix.FIX50.News(headline);
                    if (lines.Count > 0)
                    {
                        QuickFix.FIX50.News.NoLinesOfTextGroup group = new QuickFix.FIX50.News.NoLinesOfTextGroup();
                        foreach (string s in lines)
                        {
                            group.Text = new QuickFix.Fields.Text(s);
                            news50.AddGroup(group);
                        }
                    }
                    m = news50;
                    break;

                default:
                    Trace.WriteLine("FIX version unsupported for type 'News': " + _qfapp.SessionID.BeginString);
                    return;
            }//end switch on BeginString

            _qfapp.Send(m);
        }

        public void SendNewOrder(bool isBuy, string symbol, int orderQty)
        {
            if (_qfapp.SessionID == null)
            {
                Trace.WriteLine("Can't send News message because SessionID is null.  Probably not connected?");
                return;
            }

            char side_enum = isBuy ? QuickFix.Fields.Side.BUY : QuickFix.Fields.Side.SELL;

            QuickFix.Fields.Side fSide = new QuickFix.Fields.Side(side_enum);
            QuickFix.Fields.HandlInst fHandlInst = new QuickFix.Fields.HandlInst(QuickFix.Fields.HandlInst.MANUAL_ORDER);
            QuickFix.Fields.Symbol fSymbol = new QuickFix.Fields.Symbol(symbol);
            QuickFix.Fields.TransactTime fTransactTime = new QuickFix.Fields.TransactTime(DateTime.Now);
            QuickFix.Fields.OrdType fOrdType = new QuickFix.Fields.OrdType(QuickFix.Fields.OrdType.MARKET);
            QuickFix.Fields.ClOrdID fClOrdID = new QuickFix.Fields.ClOrdID(DateTime.Now.ToString("HHmmssfff"));

            QuickFix.Fields.OrderQty fOrderQty = new QuickFix.Fields.OrderQty(orderQty);

            QuickFix.Message m = null;
            switch (_qfapp.SessionID.BeginString)
            {
                case QuickFix.FixValues.BeginString.FIX42:
                    QuickFix.FIX42.NewOrderSingle nos = new QuickFix.FIX42.NewOrderSingle(
                        fClOrdID, fHandlInst, fSymbol, fSide, fTransactTime, fOrdType);
                    nos.OrderQty = fOrderQty;
                    m = nos;
                    break;
                default:
                    Trace.WriteLine("Orders are only supported in FIX.4.2 right now");
                    return;
            }

            _qfapp.Send(m);
        }
    }
}
