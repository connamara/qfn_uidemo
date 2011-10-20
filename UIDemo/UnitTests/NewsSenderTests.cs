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
    public class NewsSenderTests
    {
        readonly string nul = QuickFix.Message.SOH;

        [Test]
        public void HeadlineOnly()
        {
            UnitTestContext context = new UnitTestContext();
            context.Login();

            NewsSenderViewModel vm = new NewsSenderViewModel(context.App);
            vm.Headline = "AAAAA";
            vm.SendNewsCommand.Execute(null);

            Assert.AreEqual(1, context.Session.MsgLookup[QuickFix.FIX42.News.MsgType].Count);

            string msg = context.Session.MsgLookup[QuickFix.FIX42.News.MsgType].First().ToString();
            StringAssert.Contains(nul + "148=AAAAA" + nul, msg);
            StringAssert.Contains(nul + "33=0" + nul, msg);
        }

        [Test]
        public void SingleLines()
        {
            UnitTestContext context = new UnitTestContext();
            context.Login();

            NewsSenderViewModel vm = new NewsSenderViewModel(context.App);
            vm.Headline = "AAAAA";
            vm.IsLine1Enabled = true;
            vm.IsLine2Enabled = true;
            vm.IsLine3Enabled = true;
            vm.Line1Text = "line1";
            vm.Line2Text = "line2";
            vm.Line3Text = "line3";
            vm.SendNewsCommand.Execute(null);

            vm.Headline = "BBBBB";
            vm.IsLine2Enabled = false;
            vm.SendNewsCommand.Execute(null);

            Assert.AreEqual(2, context.Session.MsgLookup[QuickFix.FIX42.News.MsgType].Count);

            string msg1 = context.Session.MsgLookup[QuickFix.FIX42.News.MsgType][0].ToString();
            string msg2 = context.Session.MsgLookup[QuickFix.FIX42.News.MsgType][1].ToString();

            StringAssert.Contains(nul + "148=AAAAA" + nul, msg1);
            StringAssert.Contains(String.Format("{0}33=3{0}58=line1{0}58=line2{0}58=line3{0}", nul), msg1);
            StringAssert.Contains(nul + "148=BBBBB" + nul, msg2);
            StringAssert.Contains(String.Format("{0}33=2{0}58=line1{0}58=line3{0}", nul), msg2);
        }
    }
}
