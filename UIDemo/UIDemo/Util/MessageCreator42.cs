using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UIDemo.Util
{
    public static class MessageCreator42
    {
        /// <summary>
        /// Create a News message.  Nothing unexpected here.
        /// </summary>
        /// <param name="headline_str"></param>
        /// <param name="lines"></param>
        /// <returns></returns>
        static public QuickFix.FIX42.News News(string headline_str, IList<string> lines)
        {
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

            return news;
        }

        /// <summary>
        /// Creates a NewOrderSingle; the ClOrdID will be set based on the time.
        /// </summary>
        /// <param name="isBuy"></param>
        /// <param name="symbol"></param>
        /// <param name="orderQty"></param>
        /// <returns></returns>
        static public QuickFix.FIX42.NewOrderSingle MarketOrder(bool isBuy, string symbol, int orderQty)
        {
            char side_enum = isBuy ? QuickFix.Fields.Side.BUY : QuickFix.Fields.Side.SELL;

            QuickFix.Fields.OrdType fOrdType = new QuickFix.Fields.OrdType(QuickFix.Fields.OrdType.MARKET);

            QuickFix.Fields.Side fSide = new QuickFix.Fields.Side(side_enum);
            QuickFix.Fields.HandlInst fHandlInst = new QuickFix.Fields.HandlInst(QuickFix.Fields.HandlInst.MANUAL_ORDER);
            QuickFix.Fields.Symbol fSymbol = new QuickFix.Fields.Symbol(symbol);
            QuickFix.Fields.TransactTime fTransactTime = new QuickFix.Fields.TransactTime(DateTime.Now);
            QuickFix.Fields.ClOrdID fClOrdID = new QuickFix.Fields.ClOrdID(DateTime.Now.ToString("HHmmssfff"));

            QuickFix.FIX42.NewOrderSingle nos = new QuickFix.FIX42.NewOrderSingle(
                fClOrdID, fHandlInst, fSymbol, fSide, fTransactTime, fOrdType);
            nos.OrderQty = new QuickFix.Fields.OrderQty(orderQty);

            return nos;
        }

        static public QuickFix.FIX42.NewOrderSingle LimitOrder(bool isBuy, string symbol, int orderQty, decimal price)
        {
            char side_enum = isBuy ? QuickFix.Fields.Side.BUY : QuickFix.Fields.Side.SELL;

            QuickFix.Fields.OrdType fOrdType = new QuickFix.Fields.OrdType(QuickFix.Fields.OrdType.LIMIT);

            QuickFix.Fields.Side fSide = new QuickFix.Fields.Side(side_enum);
            QuickFix.Fields.HandlInst fHandlInst = new QuickFix.Fields.HandlInst(QuickFix.Fields.HandlInst.MANUAL_ORDER);
            QuickFix.Fields.Symbol fSymbol = new QuickFix.Fields.Symbol(symbol);
            QuickFix.Fields.TransactTime fTransactTime = new QuickFix.Fields.TransactTime(DateTime.Now);
            QuickFix.Fields.ClOrdID fClOrdID = new QuickFix.Fields.ClOrdID(DateTime.Now.ToString("HHmmssfff"));

            QuickFix.Fields.OrderQty fOrderQty = new QuickFix.Fields.OrderQty(orderQty);

            QuickFix.FIX42.NewOrderSingle nos = new QuickFix.FIX42.NewOrderSingle(
                fClOrdID, fHandlInst, fSymbol, fSide, fTransactTime, fOrdType);
            nos.OrderQty = new QuickFix.Fields.OrderQty(orderQty);
            nos.Price = new QuickFix.Fields.Price(price);

            return nos;
        }
    }
}
