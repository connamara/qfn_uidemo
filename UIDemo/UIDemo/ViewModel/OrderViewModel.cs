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
        private ConnectionModel _model = null;

        public ObservableCollection<OrderRecord> Orders { get; set; }

        public ICommand SendBuyCommand { get; set; }
        public ICommand SendSellCommand { get; set; }

        public OrderViewModel(ConnectionModel cm)
        {
            _model = cm;
            Orders = new ObservableCollection<OrderRecord>();

            Orders.Add(new OrderRecord("aaa", "aaa", true, "x", 5.5m));
            Orders.Add(new OrderRecord("aaa", "aaa", true, "x", 5.5m));
            Orders.Add(new OrderRecord("aaa", "aaa", true, "x", 5.5m));

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

            _model.SendNewOrder(isBuy, this.Symbol, this.OrderQty);
        }
    }
}
