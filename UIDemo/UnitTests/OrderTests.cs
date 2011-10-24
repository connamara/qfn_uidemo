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
        [Test]
        public void SubmitOneBuyOrderWithDefaultValues()
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
    }
}
