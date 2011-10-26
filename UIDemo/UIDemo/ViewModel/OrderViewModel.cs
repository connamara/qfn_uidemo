using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIDemo.Model;
using System.Windows.Input;
using System.Diagnostics;
using System.Collections.ObjectModel;
using UIDemo.Util;

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

            _qfapp.Fix42ExecReportEvent += new Action<QuickFix.FIX42.ExecutionReport>(HandleExecutionReport);
        }

        private string _symbol = "IBM";
        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; base.OnPropertyChanged("Symbol"); }
        }

        private string _orderQtyString = "5";
        public string OrderQtyString
        {
            get { return _orderQtyString; }
            set { _orderQtyString = value; base.OnPropertyChanged("OrderQtyString"); }
        }

        private string _ordType = "Market";
        public string OrdType
        {
            get { return _ordType; }
            set { _ordType = value; base.OnPropertyChanged("OrdType"); }
        }

        private bool _isActionsEnabled = true;
        public bool IsActionsEnabled
        {
            get { return _isActionsEnabled; }
            set { _isActionsEnabled = value; base.OnPropertyChanged("IsActionsEnabled"); }
        }

        private void SendBuy(object obj) { SendOrder(true); }
        private void SendSell(object obj) { SendOrder(false); }

        private void SendOrder(bool isBuy)
        {
            Trace.WriteLine(String.Format("Send NewOrder: Side={0} Symbol=[{1}] Qty=[{2}]",
                (isBuy ? "Buy" : "Sell"), this.Symbol, this.OrderQtyString));

            try
            {
                QuickFix.Message m = null;
                lock (_ordersLock)
                {
                    m = _qfapp.SendNewOrder(isBuy, this.Symbol, Int32.Parse(this.OrderQtyString));

                    if (m is QuickFix.FIX42.NewOrderSingle)
                    {
                        var n42 = m as QuickFix.FIX42.NewOrderSingle;

                        decimal price = -1;
                        if (n42.OrdType.Obj != QuickFix.Fields.OrdType.MARKET)
                            price = n42.Price.Obj;

                        OrderRecord r = new OrderRecord(
                            n42.ClOrdID.Obj,
                            n42.Symbol.Obj,
                            isBuy,
                            FixEnumTranslator.Translate(n42.OrdType),
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
            try
            {
                string ordId = msg.ClOrdID.Obj;
                string status = FixEnumTranslator.Translate(msg.OrdStatus);

                Trace.WriteLine("OVM: Handling ExecutionReport: " + ordId + " / " + status);

                lock (_ordersLock)
                {
                    foreach (OrderRecord r in Orders)
                    {
                        if (r.ClOrdID == ordId)
                        {
                            r.Status = status;
                            r.Price = msg.LastPx.Obj;
                            return;
                        }
                    }
                }

                Trace.WriteLine("OVM: No order corresponds to ClOrdID '" + ordId + "'");
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
            }
        }
    }
}
