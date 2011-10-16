using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIDemo.Model;
using System.Windows.Input;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace UIDemo.ViewModel
{
    public class OrderViewModel : ViewModelBase
    {
        private QFApp _qfapp = null;

        private Object _ordersLock = new Object();
        public ObservableCollection<OrderRecord> Orders { get; set; }


        public ICommand SendBuyCommand { get; set; }
        public ICommand SendSellCommand { get; set; }

        public OrderViewModel(QFApp app)
        {
            _qfapp = app;
            Orders = new ObservableCollection<OrderRecord>();

            // command definitions
            SendBuyCommand = new RelayCommand(SendBuy);
            SendSellCommand = new RelayCommand(SendSell);
        }

        private string _symbol = "IBM";
        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; base.OnPropertyChanged("Symbol"); }
        }

        private int _orderQty = 5;
        public int OrderQty
        {
            get { return _orderQty; }
            set { _orderQty = value; base.OnPropertyChanged("OrderQty"); }
        }

        private string _ordType = "Market";
        public string OrdType
        {
            get { return _ordType; }
            set { _ordType = value; base.OnPropertyChanged("OrdType"); }
        }

        private void SendBuy(object obj) { SendOrder(true); }
        private void SendSell(object obj) { SendOrder(false); }

        private void SendOrder(bool isBuy)
        {
            Trace.WriteLine(String.Format("Send NewOrder: Side={0} Symbol=[{1}] Qty=[{2}]",
                (isBuy ? "Buy" : "Sell"), this.Symbol, this.OrderQty));

            try
            {
                QuickFix.Message m = null;
                lock (_ordersLock)
                {
                    m = _qfapp.SendNewOrder(isBuy, this.Symbol, this.OrderQty);

                    if (m is QuickFix.FIX42.NewOrderSingle)
                    {
                        var n42 = m as QuickFix.FIX42.NewOrderSingle;

                        string ordertype = "";
                        switch (n42.OrdType.Obj)
                        {
                            case '1': ordertype = "Market"; break;
                            case '2': ordertype = "Limit"; break;
                            default: ordertype = "unknown"; break;
                        }

                        decimal price = 0;
                        if (n42.OrdType.Obj != '1')
                            price = n42.Price.Obj;

                        OrderRecord r = new OrderRecord(
                            n42.ClOrdID.Obj,
                            n42.Symbol.Obj,
                            isBuy,
                            ordertype,
                            price,
                            "New");

                        Orders.Add(r);
                    }
                }
            }
            catch (Exception e)
            {
                string s = "Couldn't send order.\n" + e.ToString();
                Trace.WriteLine(s);
            }
        }


        public void HandleExecutionReport(QuickFix.FIX42.ExecutionReport msg)
        {
            string ordId = msg.ClOrdID.Obj;
            string status = "";
            switch (msg.OrdStatus.Obj)
            {
                case '0': status = "New"; break;
                case '1': status = "PartiallyFilled"; break;
                case '2': status = "Filled"; break;
                case '4': status = "Canceled"; break;
                case '5': status = "Replaced"; break;
                case '8': status = "Rejected"; break;
                default: status = "unknown"; break;
            }

            lock (_ordersLock)
            {
                foreach (OrderRecord r in Orders)
                {
                    if (r.ClOrdID == ordId)
                    {
                        r.Status = status;
                    }
                }
            }
        }
    }
}
