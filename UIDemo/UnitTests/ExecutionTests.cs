using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using UIDemo.ViewModel;
using UnitTests.Util;
using UIDemo;

namespace UnitTests
{
    [TestFixture]
    public class ExecutionTests
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
        public void OneIncomingExecution()
        {
            UnitTestContext context = new UnitTestContext();
            context.Login();

            ExecutionViewModel vm = new ExecutionViewModel(context.App);

            QuickFix.FIX42.ExecutionReport ex = CreateExReport("order1", "exec1", QuickFix.Fields.ExecTransType.NEW,
                QuickFix.Fields.ExecType.NEW, QuickFix.Fields.OrdStatus.NEW, "IBM", QuickFix.Fields.Side.BUY, 1.23m, 4.56m, 7.89m);

            context.App.FromApp(ex, context.Session.SessionID);

            Assert.AreEqual(1, vm.Executions.Count);
        }

        [Test]
        public void ManyExecutions()
        {
            UnitTestContext context = new UnitTestContext();
            context.Login();

            ExecutionViewModel vm = new ExecutionViewModel(context.App);

            QuickFix.FIX42.ExecutionReport ex1 = CreateExReport("order1", "exec1", QuickFix.Fields.ExecTransType.NEW,
                QuickFix.Fields.ExecType.NEW, QuickFix.Fields.OrdStatus.NEW, "IBM", QuickFix.Fields.Side.BUY, 1.23m, 4.56m, 7.89m);
            QuickFix.FIX42.ExecutionReport ex2 = CreateExReport("order1", "exec1", QuickFix.Fields.ExecTransType.CORRECT,
                QuickFix.Fields.ExecType.NEW, QuickFix.Fields.OrdStatus.NEW, "ABC", QuickFix.Fields.Side.SELL, 1.23m, 4.56m, 7.89m);
            QuickFix.FIX42.ExecutionReport ex3 = CreateExReport("order1", "exec1", QuickFix.Fields.ExecTransType.CANCEL,
                QuickFix.Fields.ExecType.NEW, QuickFix.Fields.OrdStatus.NEW, "XYZ", QuickFix.Fields.Side.BUY, 1.23m, 4.56m, 7.89m);

            context.App.FromApp(ex1, context.Session.SessionID);
            context.App.FromApp(ex2, context.Session.SessionID);
            context.App.FromApp(ex3, context.Session.SessionID);

            Assert.AreEqual(3, vm.Executions.Count);

            Assert.AreEqual("order1", vm.Executions[0].OrderID);
            Assert.AreEqual("New", vm.Executions[0].ExecTransType);
            Assert.AreEqual("IBM", vm.Executions[0].Symbol);
            Assert.AreEqual("Buy", vm.Executions[0].Side);

            Assert.AreEqual("order1", vm.Executions[1].OrderID);
            Assert.AreEqual("Correct", vm.Executions[1].ExecTransType);
            Assert.AreEqual("ABC", vm.Executions[1].Symbol);
            Assert.AreEqual("Sell", vm.Executions[1].Side);

            Assert.AreEqual("order1", vm.Executions[2].OrderID);
            Assert.AreEqual("Cancel", vm.Executions[2].ExecTransType);
            Assert.AreEqual("XYZ", vm.Executions[2].Symbol);
            Assert.AreEqual("Buy", vm.Executions[2].Side);
        }
    }
}
