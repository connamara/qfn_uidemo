using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace UIDemo.Model
{
    public class OrderRecord : NotifyPropertyChangedBase
    {
        public OrderRecord(string pClOrdID, string pSymbol, bool pIsBuy, string pOrdType, decimal pPrice, string pState)
        {
            ClOrdID = pClOrdID;
            Symbol = pSymbol;
            Side = pIsBuy ? "Buy" : "Sell";
            OrdType = pOrdType;
            Price = pPrice;
            Status = pState;
        }

        private string _clOrdID = "";
        public string ClOrdID
        {
            get { return _clOrdID; }
            set { _clOrdID = value; OnPropertyChanged("ClOrdID"); }
        }

        private string _symbol = "";
        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; OnPropertyChanged("Symbol"); }
        }

        private string _side = "";
        public string Side
        {
            get { return _side; }
            set { _side = value; OnPropertyChanged("Side"); }
        }

        private string _ordType = "";
        public string OrdType
        {
            get { return _ordType; }
            set { _ordType = value; OnPropertyChanged("OrdType"); }
        }

        private decimal _price = 0m;
        public decimal Price
        {
            get { return _price; }
            set { _price = value; OnPropertyChanged("Price"); }
        }

        private string _status { get; set; }
        public string Status
        {
            get { return _status; }
            set { _status = value; OnPropertyChanged("Status"); }
        }




    }
}
