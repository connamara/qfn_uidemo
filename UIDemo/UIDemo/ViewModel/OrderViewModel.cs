using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIDemo.Model;
using System.Windows.Input;
using System.Diagnostics;
using System.Collections.ObjectModel;
using FIXApplication.Enums;
using FIXApplication;

namespace UIDemo.ViewModel
{
    public class OrderViewModel : ViewModelBase
    {
        #region Static Initialization
        static private readonly List<OrderType> _ORDERTYPE_CHOICES = new List<OrderType>();
        static private readonly List<TimeInForce> _TIMEINFORCE_CHOICES = new List<TimeInForce>();
        static OrderViewModel() // (static constructor)
        {
            _ORDERTYPE_CHOICES.Add(OrderType.Market);
            _ORDERTYPE_CHOICES.Add(OrderType.Limit);

            _TIMEINFORCE_CHOICES.Add(TimeInForce.Day);
            _TIMEINFORCE_CHOICES.Add(TimeInForce.GoodTillCancel);
        }

        #endregion


        // instance stuff
        private QFApp _qfapp = null;

        private Object _ordersLock = new Object();
        public ObservableCollection<OrderRecord> Orders { get; set; }

        private Object _customFieldsLock = new Object();
        public ObservableCollection<CustomFieldRecord> CustomFields { get; set; }

        public ICommand SendBuyCommand { get; set; }
        public ICommand SendSellCommand { get; set; }
        public ICommand AddCustomFieldCommand { get; set; }
        public ICommand ClearCustomFieldsCommand { get; set; }

        public OrderViewModel(QFApp app)
        {
            _qfapp = app;
            Orders = new ObservableCollection<OrderRecord>();
            CustomFields = new ObservableCollection<CustomFieldRecord>();

            // command definitions
            SendBuyCommand = new RelayCommand(SendBuy);
            SendSellCommand = new RelayCommand(SendSell);
            AddCustomFieldCommand = new RelayCommand(AddCustomField);
            ClearCustomFieldsCommand = new RelayCommand(ClearCustomFields);

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

        private string _limitPriceString = "0";
        public string LimitPriceString
        {
            get { return _limitPriceString; }
            set { _limitPriceString = value; base.OnPropertyChanged("LimitPriceString"); }
        }

        public List<TimeInForce> TimeInForceChoices { get { return _TIMEINFORCE_CHOICES; } }
        private TimeInForce _timeInForce = TimeInForce.Day;
        public TimeInForce TimeInForce
        {
            get { return _timeInForce; }
            set
            {
                if (_timeInForce != value)
                {
                    _timeInForce = value;
                    base.OnPropertyChanged("TimeInForce");
                }
            }
        }

        private string _customFixTag = "58";
        public string CustomFixTag
        {
            get { return _customFixTag; }
            set { _customFixTag = value; base.OnPropertyChanged("CustomFixTag"); }
        }

        private string _customFixValue = "some string";
        public string CustomFixValue
        {
            get { return _customFixValue; }
            set { _customFixValue = value; base.OnPropertyChanged("CustomFixValue"); }
        }

        // commands
        private void AddCustomField(object obj)
        {
            try
            {
                int tag = int.Parse(this.CustomFixTag);
                lock (_customFieldsLock)
                {
                    foreach (CustomFieldRecord r in CustomFields)
                    {
                        if (r.Tag == tag)
                        {
                            r.Value = this.CustomFixValue;
                            return;
                        }
                    }
                    CustomFields.Add(new CustomFieldRecord(tag, this.CustomFixValue));
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
            }
        }

        private void ClearCustomFields(object obj)
        {
            try
            {
                lock (_customFieldsLock)
                {
                    CustomFields.Clear();
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
            }
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

            QuickFix.FIX42.NewOrderSingle nos = MessageCreator42.MarketOrder(
                isBuy, this.Symbol, Int32.Parse(this.OrderQtyString), _timeInForce);

            RecordAndSendOrder(nos);
        }

        private void SendLimitOrder(bool isBuy)
        {
            Trace.WriteLine(String.Format("Send Limit Order: Side={0} Symbol=[{1}] Qty=[{2}] LimitPrice=[{3}]",
                (isBuy ? "Buy" : "Sell"), this.Symbol, this.OrderQtyString, this.LimitPriceString));

            QuickFix.FIX42.NewOrderSingle nos = MessageCreator42.LimitOrder(
                isBuy, this.Symbol, Int32.Parse(this.OrderQtyString), _timeInForce, Decimal.Parse(this.LimitPriceString));

            RecordAndSendOrder(nos);
        }

        private void RecordAndSendOrder(QuickFix.FIX42.NewOrderSingle nos)
        {
            lock (_ordersLock)
            {
                decimal price = -1;
                if (nos.OrdType.Obj == QuickFix.Fields.OrdType.LIMIT)
                    price = nos.Price.Obj;

                OrderRecord r = new OrderRecord(
                    nos.ClOrdID.Obj,
                    nos.Symbol.Obj,
                    FixEnumTranslator.Translate(nos.Side),
                    FixEnumTranslator.Translate(nos.OrdType),
                    price,
                    "New");

                Orders.Add(r);
            }

            _qfapp.Send(nos);
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
