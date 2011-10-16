using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UIDemo.Model
{
    public class ExecutionRecord : NotifyPropertyChangedBase
    {
        public ExecutionRecord(string pExecID, string pOrderID, string pExecTransType, string pExecType, string pSymbol, string pSide)
        {
            ExecID = pExecID;
            OrderID = pOrderID;
            ExecTransType = pExecTransType;
            ExecType = pExecType;
            Symbol = pSymbol;
            Side = pSide;
        }

        private string _execID = "";
        private string _orderID = "";
        private string _execTransType = "";
        private string _execType = "";
        private string _symbol = "";
        private string _side = "";

        public string ExecID
        {
            get { return _execID; }
            set { _execID = value; OnPropertyChanged("ExecID"); }
        }

        public string OrderID
        {
            get { return _orderID; }
            set { _orderID = value; OnPropertyChanged("OrderID"); }
        }

        public string ExecTransType
        {
            get { return _execTransType; }
            set { _execTransType = value; OnPropertyChanged("ExecTransType"); }
        }

        public string ExecType
        {
            get { return _execType; }
            set { _execType = value; OnPropertyChanged("ExecType"); }
        }

        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; OnPropertyChanged("Symbol"); }
        }

        public string Side
        {
            get { return _side; }
            set { _side = value; OnPropertyChanged("Side"); }
        }
    }
}
