using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FIXApplication.Enums;

namespace FIXApplication
{
    public static class MessageCreator42
    {
        static private QuickFix.Fields.Account _account = null;

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
        /// Create a NewOrderSingle message.
        /// </summary>
        /// <param name="customFields"></param>
        /// <param name="orderType"></param>
        /// <param name="side"></param>
        /// <param name="symbol"></param>
        /// <param name="orderQty"></param>
        /// <param name="tif"></param>
        /// <param name="price">ignored if orderType=Market</param>
        /// <returns></returns>
        static public QuickFix.FIX42.NewOrderSingle NewOrderSingle(
            Dictionary<int,string> customFields,
            OrderType orderType, Side side, string symbol,
            int orderQty, TimeInForce tif, decimal price)
        {
            // hard-coded fields
            QuickFix.Fields.HandlInst fHandlInst = new QuickFix.Fields.HandlInst(QuickFix.Fields.HandlInst.AUTOMATED_EXECUTION_ORDER_PRIVATE);
            
            // from params
            QuickFix.Fields.OrdType fOrdType = FixEnumTranslator.ToField(orderType);
            QuickFix.Fields.Side fSide = FixEnumTranslator.ToField(side);
            QuickFix.Fields.Symbol fSymbol = new QuickFix.Fields.Symbol(symbol);
            QuickFix.Fields.TransactTime fTransactTime = new QuickFix.Fields.TransactTime(DateTime.Now);
            QuickFix.Fields.ClOrdID fClOrdID = new QuickFix.Fields.ClOrdID(DateTime.Now.ToString("HHmmssfff"));

            QuickFix.FIX42.NewOrderSingle nos = new QuickFix.FIX42.NewOrderSingle(
                fClOrdID, fHandlInst, fSymbol, fSide, fTransactTime, fOrdType);
            nos.OrderQty = new QuickFix.Fields.OrderQty(orderQty);
            nos.TimeInForce = FixEnumTranslator.ToField(tif);

            if (orderType == OrderType.Limit)
                nos.Price = new QuickFix.Fields.Price(price);

            // add custom fields
            foreach (KeyValuePair<int,string> p in customFields)
                nos.SetField(new QuickFix.Fields.StringField(p.Key, p.Value));

            return nos;
        }
    }
}
