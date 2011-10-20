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
        int seqNum = 1;

        /* don't need this?
        public QuickFix.Message CreateLogonMessage(QuickFix.SessionID sessionID)
        {
            QuickFix.FIX42.Logon login = new QuickFix.FIX42.Logon();
            login.Header.SetField(new QuickFix.Fields.TargetCompID(sessionID.SenderCompID));
            login.Header.SetField(new QuickFix.Fields.SenderCompID(sessionID.TargetCompID));
            login.Header.SetField(new QuickFix.Fields.MsgSeqNum(seqNum++));
            login.Header.SetField(new QuickFix.Fields.SendingTime(System.DateTime.UtcNow));
            login.SetField(new QuickFix.Fields.HeartBtInt(30));
            return login;
        }
         */

        private QuickFix.Dictionary CreateConfig()
        {
            QuickFix.Dictionary config = new QuickFix.Dictionary();
            config.SetBool(QuickFix.SessionSettings.PERSIST_MESSAGES, false);
            config.SetString(QuickFix.SessionSettings.CONNECTION_TYPE, "initiator");
            config.SetString(QuickFix.SessionSettings.START_TIME, "00:00:00");
            config.SetString(QuickFix.SessionSettings.END_TIME, "00:00:00");
            return config;
        }

        [Test]
        public void HeadlineOnly()
        {
            QuickFix.Dictionary config = CreateConfig();
            QuickFix.SessionID sessionID = new QuickFix.SessionID("FIX.4.2", "SENDER", "TARGET");
            QuickFix.SessionSettings settings = new QuickFix.SessionSettings(); //CreateSettings(sessionID);
            settings.Set(sessionID, config);

            QFApp app = new QFApp(settings);
            MockInitiator init = new MockInitiator();
            app.Initiator = init;

            SessionThatTracksOutbound session = new SessionThatTracksOutbound(
                app,
                new QuickFix.MemoryStoreFactory(),
                sessionID,
                new QuickFix.DataDictionaryProvider(),
                new QuickFix.SessionSchedule(config),
                0,
                new QuickFix.ScreenLogFactory(settings),
                new QuickFix.DefaultMessageFactory(),
                "blah");
            session.MaxLatency = 120;
            //session.SetResponder(responder);

            app.OnLogon(sessionID);
            init.IsLoggedOnValue = true; //kludge


            // For a test, we need
            // * app
            // * session
            // * vm


            //===========================
            // setup stuff complete
            //===========================
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
