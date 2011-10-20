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

            string nul = QuickFix.Message.SOH;
            StringAssert.Contains(nul + "33=0" + nul, msg);
        }
    }
}
