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
        readonly string nul = QuickFix.Message.SOH;
        


        [Test]
        public void OneIncomingExecution()
        {
            UnitTestContext context = new UnitTestContext();
            context.Login();

            ExecutionViewModel vm = new ExecutionViewModel(context.App);

            QuickFix.FIX42.ExecutionReport ex = new QuickFix.FIX42.ExecutionReport(
                new QuickFix.Fields.OrderID("order1"),
                new QuickFix.Fields.ExecID("exec1"),
                new QuickFix.Fields.ExecTransType(QuickFix.Fields.ExecTransType.NEW),
                new QuickFix.Fields.ExecType(QuickFix.Fields.ExecType.NEW),
                new QuickFix.Fields.OrdStatus(QuickFix.Fields.OrdStatus.NEW),
                new QuickFix.Fields.Symbol("IBM"),
                new QuickFix.Fields.Side(QuickFix.Fields.Side.BUY),
                new QuickFix.Fields.LeavesQty(1.23m),
                new QuickFix.Fields.CumQty(4.56m),
                new QuickFix.Fields.AvgPx(7.89m));

            context.App.FromApp(ex, context.Session.SessionID);

            Assert.AreEqual(1, vm.Executions.Count);
        }
    }
}
