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
            QFApp app = null;
            SessionThatTracksOutbound session = null;
            MockInitiator init = null;
            TestTool.SetupAppAndSession(out app, out session, out init);

            // login
            app.OnLogon(session.SessionID);
            init.IsLoggedOnValue = true; //kludge

            NewsSenderViewModel vm = new NewsSenderViewModel(app);

            vm.Headline = "AAAAA";
            vm.SendNewsCommand.Execute(null);

            Assert.AreEqual(1, session.MsgLookup[QuickFix.FIX42.News.MsgType].Count);

            string msg = session.MsgLookup[QuickFix.FIX42.News.MsgType].First().ToString();

            string nul = QuickFix.Message.SOH;
            StringAssert.Contains(nul + "33=0" + nul, msg);
        }
    }
}
