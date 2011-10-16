using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UIDemo.Model
{
    public class OrderRecord
    {
        public string ClOrdID { get; set; }
        public string Symbol { get; set; }
        public string Side { get; set; }
        public string OrdType { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }

        public OrderRecord(string pClOrdID, string pSymbol, bool pIsBuy, string pOrdType, decimal pPrice, string pState)
        {
            ClOrdID = pClOrdID;
            Symbol = pSymbol;
            Side = pIsBuy ? "Buy" : "Sell";
            OrdType = pOrdType;
            Price = pPrice;
            Status = pState;
        }
    }
}
