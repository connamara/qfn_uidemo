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
        private ICustomFixStrategy _strategy = null;

        private Object _ordersLock = new Object();
        public ObservableCollection<OrderRecord> Orders { get; set; }

        private Object _customFieldsLock = new Object();
        public ObservableCollection<CustomFieldRecord> CustomFields { get; set; }

        public ICommand SendBuyCommand { get; set; }
        public ICommand SendSellCommand { get; set; }
        public ICommand AddCustomFieldCommand { get; set; }
        public ICommand ClearCustomFieldsCommand { get; set; }

        public OrderViewModel(QFApp app, ICustomFixStrategy strategy)
        {
            _qfapp = app;
            _strategy = strategy;
            Orders = new ObservableCollection<OrderRecord>();
            CustomFields = new ObservableCollection<CustomFieldRecord>();

            // command definitions
            SendBuyCommand = new RelayCommand(SendBuy);
            SendSellCommand = new RelayCommand(SendSell);
            AddCustomFieldCommand = new RelayCommand(AddCustomField);
            ClearCustomFieldsCommand = new RelayCommand(ClearCustomFields);

            _qfapp.Fix42ExecReportEvent += new Action<QuickFix.FIX42.ExecutionReport>(HandleExecutionReport);

            // load pre-set custom fields from strategy, if it has any
            foreach (KeyValuePair<int, string> p in strategy.DefaultNewOrderSingleCustomFields)
                CustomFields.Add(new CustomFieldRecord(p.Key, p.Value));
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

        public void DeleteCustomField(CustomFieldRecord cfr)
        {
            try
            {
                lock (_customFieldsLock)
                {
                    for (int i = CustomFields.Count - 1; i >= 0; i--)
                    {
                        CustomFieldRecord item = CustomFields[i];
                        if (cfr.Tag == item.Tag)
                            CustomFields.RemoveAt(i);
                    }
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

        private void SendBuy(object obj) { SendOrder(Side.Buy); }
        private void SendSell(object obj) { SendOrder(Side.Sell); }

        private void SendOrder(Side side)
        {
            try
            {
                Trace.WriteLine(String.Format("Send New Order: Type={0} Side={1} Symbol=[{2}] Qty=[{3}] LimitPrice=[{4}] TIF={5}",
                    this.OrderType.ToString(), side.ToString(), this.Symbol,
                    this.OrderQtyString, this.LimitPriceString, this.TimeInForce.ToString()));

                Dictionary<int, string> customFieldsDict = new Dictionary<int, string>();
                foreach (CustomFieldRecord cfr in this.CustomFields)
                    customFieldsDict[cfr.Tag] = cfr.Value;

                int orderQty = int.Parse(this.OrderQtyString);
                decimal limitPrice = decimal.Parse(this.LimitPriceString);

                QuickFix.FIX42.NewOrderSingle nos = MessageCreator42.NewOrderSingle(
                    customFieldsDict,
                    this.OrderType, side, this.Symbol, orderQty, this.TimeInForce, limitPrice);

                OrderRecord r = new OrderRecord(nos);
                lock (_ordersLock)
                {
                    Orders.Add(r);
                }

                _qfapp.Send(nos);

            }
            catch (Exception e)
            {
                Trace.WriteLine("Failed to send order\n" + e.ToString());
            }
        }

        public void HandleExecutionReport(QuickFix.FIX42.ExecutionReport msg)
        {
            try
            {
                string clOrdId = msg.ClOrdID.Obj;
                string status = FixEnumTranslator.Translate(msg.OrdStatus);

                Trace.WriteLine("OVM: Handling ExecutionReport: " + clOrdId + " / " + status);

                lock (_ordersLock)
                {
                    foreach (OrderRecord r in Orders)
                    {
                        if (r.ClOrdID == clOrdId)
                        {
                            r.Status = status;
                            if (msg.IsSetLastPx())
                                r.Price = msg.LastPx.Obj;
                            if (msg.IsSetOrderID())
                                r.OrderID = msg.OrderID.Obj;

                            return;
                        }
                    }
                }

                Trace.WriteLine("OVM: No order corresponds to ClOrdID '" + clOrdId + "'");
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
            }
        }

        public void CancelOrder(OrderRecord or)
        {
            try
            {
                QuickFix.FIX42.OrderCancelRequest ocq = new QuickFix.FIX42.OrderCancelRequest(
                    new QuickFix.Fields.OrigClOrdID(or.ClOrdID),
                    MessageCreator42.GenerateClOrdID(),
                    new QuickFix.Fields.Symbol(or.Symbol),
                    or.OriginalNOS.Side,
                    new QuickFix.Fields.TransactTime(DateTime.Now));

                ocq.OrderID = new QuickFix.Fields.OrderID(or.OrderID);

                _strategy.ProcessOrderCancelRequest(or.OriginalNOS, ocq);

                _qfapp.Send(ocq);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="or"></param>
        /// <param name="newQty"></param>
        /// <param name="newPrice">ignored if not applicable for order type</param>
        /// <param name="customFields">other custom fields to be added</param>
        public void CancelReplaceOrder(OrderRecord or, int newQty, decimal newPrice, Dictionary<int, string> customFields)
        {
            try
            {
                QuickFix.FIX42.OrderCancelReplaceRequest ocrq = MessageCreator42.OrderCancelReplaceRequest(
                    customFields, or.OriginalNOS, newQty, newPrice);

                ocrq.OrderID = new QuickFix.Fields.OrderID(or.OrderID);

                _strategy.ProcessOrderCancelReplaceRequest(or.OriginalNOS, ocrq);

                _qfapp.Send(ocrq);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
            }
        }
    }
}
