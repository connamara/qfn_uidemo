using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIDemo.Model;
using System.Windows.Input;
using System.Diagnostics;
using System.Collections.ObjectModel;
using UIDemo.Util;
using UIDemo.Enums;

namespace UIDemo.ViewModel
{
    public class OrderViewModel : ViewModelBase
    {
        #region Static Initialization
        static private readonly List<OrderType> _ORDERTYPE_CHOICES = new List<OrderType>();
        static OrderViewModel() // (static constructor)
        {
            _ORDERTYPE_CHOICES.Add(OrderType.Market);
            _ORDERTYPE_CHOICES.Add(OrderType.Limit);
        }
        #endregion


        // instance stuff
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

        public List<OrderType> OrderTypeChoices { get { return _ORDERTYPE_CHOICES; } }
        private OrderType _orderType = OrderType.Market;
        public OrderType OrderType
        {
            get { return _orderType; }
            set
            {
                if (_orderType != value)
                {
                    _orderType = value;
                    base.OnPropertyChanged("OrderType");
                }
            }
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
            try
            {
                switch (this.OrderType)
                {
                    case OrderType.Market:
                        SendMarketOrder(isBuy);
                        break;
                    case OrderType.Limit:
                        SendLimitOrder(isBuy);
                        break;
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine("Failed to send order\n" + e.ToString());
            }
        }

        private void SendMarketOrder(bool isBuy)
        {
            Trace.WriteLine(String.Format("Send Market Order: Side={0} Symbol=[{1}] Qty=[{2}]",
                (isBuy ? "Buy" : "Sell"), this.Symbol, this.OrderQtyString));

            QuickFix.FIX42.NewOrderSingle nos = MessageCreator42.MarketOrder(isBuy, this.Symbol, Int32.Parse(this.OrderQtyString));

            lock (_ordersLock)
            {
                decimal price = -1;
                if (nos.OrdType.Obj != QuickFix.Fields.OrdType.MARKET)
                    price = nos.Price.Obj;

                OrderRecord r = new OrderRecord(
                    nos.ClOrdID.Obj,
                    nos.Symbol.Obj,
                    isBuy,
                    FixEnumTranslator.Translate(nos.OrdType),
                    price,
                    "New");

                Orders.Add(r);
            }

            _qfapp.Send(nos);
        }

        private void SendLimitOrder(bool isBuy)
        {
            Trace.WriteLine(String.Format("Send Limit Order: Side={0} Symbol=[{1}] Qty=[{2}] LimitPrice=[{3}]",
                (isBuy ? "Buy" : "Sell"), this.Symbol, this.OrderQtyString));
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
