using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace UIDemo.Model
{
    public class OrderRecord : INotifyPropertyChanged
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



        #region INotifyPropertyChanged Members

        /// <summary>
        /// Raised when a property on this object has a new value.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The property that has a new value.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        #endregion // INotifyPropertyChanged Members

    }
}
