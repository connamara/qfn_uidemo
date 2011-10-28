using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using UnitTests.Util;
using UIDemo.ViewModel;
using UIDemo.Model;

namespace UnitTests
{
    [TestFixture]
    public class OrderTests
    {
        private QuickFix.FIX42.ExecutionReport CreateExReport(
            string orderid,
            string execid,
            char exectranstype,
            char exectype,
            char ordstatus,
            string symbol,
            char side,
            decimal leavesqty,
            decimal cumqty,
            decimal avgpx)
        {
            return new QuickFix.FIX42.ExecutionReport(
                new QuickFix.Fields.OrderID(orderid),
                new QuickFix.Fields.ExecID(execid),
                new QuickFix.Fields.ExecTransType(exectranstype),
                new QuickFix.Fields.ExecType(exectype),
                new QuickFix.Fields.OrdStatus(ordstatus),
                new QuickFix.Fields.Symbol(symbol),
                new QuickFix.Fields.Side(side),
                new QuickFix.Fields.LeavesQty(leavesqty),
                new QuickFix.Fields.CumQty(cumqty),
                new QuickFix.Fields.AvgPx(avgpx));
        }


        [Test]
        public void DefaultBuyOrder()
        {
            UnitTestContext context = new UnitTestContext();
            context.Login();

            OrderViewModel vm = new OrderViewModel(context.App);

            vm.SendBuyCommand.Execute(null);

            // messaging of sent order
            Assert.AreEqual(1, context.Session.MsgLookup[QuickFix.FIX42.NewOrderSingle.MsgType].Count);
            QuickFix.FIX42.NewOrderSingle msg = context.Session.MsgLookup[QuickFix.FIX42.NewOrderSingle.MsgType][0] as QuickFix.FIX42.NewOrderSingle;
            Assert.AreEqual("IBM", msg.Symbol.Obj);
            Assert.AreEqual(5, msg.OrderQty.Obj);
            Assert.AreEqual(QuickFix.Fields.OrdType.MARKET, msg.OrdType.Obj);
            Assert.AreEqual(QuickFix.Fields.Side.BUY, msg.Side.Obj);

            // what's in the grid
            Assert.AreEqual(1, vm.Orders.Count);
            OrderRecord o = vm.Orders.First();
            Assert.AreEqual("IBM", o.Symbol);
            Assert.AreEqual(-1, o.Price);
            Assert.AreEqual("Market", o.OrdType);
            Assert.AreEqual("Buy", o.Side);
        }

        [Test]
        public void MarketSell()
        {
            UnitTestContext context = new UnitTestContext();
            context.Login();

            OrderViewModel vm = new OrderViewModel(context.App);

            vm.Symbol = "pants";
            vm.OrderQtyString = "999";
            vm.SendSellCommand.Execute(null);

            // messaging of sent order
            Assert.AreEqual(1, context.Session.MsgLookup[QuickFix.FIX42.NewOrderSingle.MsgType].Count);
            QuickFix.FIX42.NewOrderSingle msg = context.Session.MsgLookup[QuickFix.FIX42.NewOrderSingle.MsgType][0] as QuickFix.FIX42.NewOrderSingle;
            Assert.AreEqual("pants", msg.Symbol.Obj);
            Assert.AreEqual(999, msg.OrderQty.Obj);
            Assert.AreEqual(QuickFix.Fields.Side.SELL, msg.Side.Obj);

            // what's in the grid
            Assert.AreEqual(1, vm.Orders.Count);
            OrderRecord o = vm.Orders.First();
            Assert.AreEqual("pants", o.Symbol);
            Assert.AreEqual(-1, o.Price);
            Assert.AreEqual("Market", o.OrdType);
            Assert.AreEqual("Sell", o.Side);
        }

        [Test]
        public void OrderUpdate()
        {
            UnitTestContext context = new UnitTestContext();
            context.Login();

            OrderViewModel vm = new OrderViewModel(context.App);

            // send an order
            vm.SendBuyCommand.Execute(null);
            Assert.AreEqual(1, context.Session.MsgLookup[QuickFix.FIX42.NewOrderSingle.MsgType].Count);
            QuickFix.FIX42.NewOrderSingle msg = context.Session.MsgLookup[QuickFix.FIX42.NewOrderSingle.MsgType][0] as QuickFix.FIX42.NewOrderSingle; 

            // verify grid content (redundant with earlier tests, but whatever)
            Assert.AreEqual(1, vm.Orders.Count);
            OrderRecord o = vm.Orders.First();
            Assert.AreEqual("IBM", o.Symbol);
            Assert.AreEqual(-1, o.Price);
            Assert.AreEqual("Market", o.OrdType);
            Assert.AreEqual("Buy", o.Side);
            Assert.AreEqual("New", o.Status);

            // send an execution report that will update the grid
            QuickFix.FIX42.ExecutionReport r = CreateExReport(
                o.ClOrdID, "foo", QuickFix.Fields.ExecTransType.NEW, QuickFix.Fields.ExecType.FILL,
                QuickFix.Fields.OrdStatus.FILLED, "IBM", QuickFix.Fields.Side.BUY, 0, 0, 0);
            r.LastPx = new QuickFix.Fields.LastPx(999m);
            r.ClOrdID = new QuickFix.Fields.ClOrdID(msg.ClOrdID.Obj);
            context.App.FromApp(r, context.Session.SessionID);

            // check that it got changed
            Assert.AreEqual(1, vm.Orders.Count);
            Assert.AreEqual(999, o.Price);
            Assert.AreEqual("Filled", o.Status);
        }

        [Test]
        public void LimitBuy()
        {
            UnitTestContext context = new UnitTestContext();
            context.Login();

            OrderViewModel vm = new OrderViewModel(context.App);

            vm.OrderType = UIDemo.Enums.OrderType.Limit;
            vm.Symbol = "LIM";
            vm.OrderQtyString = "9";
            vm.LimitPriceString = "3.45";
            vm.SendBuyCommand.Execute(null);

            // messaging of sent order
            Assert.AreEqual(1, context.Session.MsgLookup[QuickFix.FIX42.NewOrderSingle.MsgType].Count);
            QuickFix.FIX42.NewOrderSingle msg = context.Session.MsgLookup[QuickFix.FIX42.NewOrderSingle.MsgType][0] as QuickFix.FIX42.NewOrderSingle;
            Assert.AreEqual("LIM", msg.Symbol.Obj);
            Assert.AreEqual(9, msg.OrderQty.Obj);
            Assert.AreEqual(3.45m, msg.Price.Obj);
            Assert.AreEqual(QuickFix.Fields.OrdType.LIMIT, msg.OrdType.Obj);
            Assert.AreEqual(QuickFix.Fields.Side.BUY, msg.Side.Obj);

            // what's in the grid
            Assert.AreEqual(1, vm.Orders.Count);
            OrderRecord o = vm.Orders.First();
            Assert.AreEqual("LIM", o.Symbol);
            Assert.AreEqual(3.45m, o.Price);
            Assert.AreEqual("Limit", o.OrdType);
            Assert.AreEqual("Buy", o.Side);
        }
    }
}
