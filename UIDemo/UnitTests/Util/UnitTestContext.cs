using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIDemo;
using FIXApplication;

namespace UnitTests.Util
{
    public class UnitTestContext
    {
        public QFApp App { get; private set; }
        public MockInitiator Initiator { get; private set; }
        public SessionThatTracksOutbound Session { get; private set; }

        public UnitTestContext()
        {
            QuickFix.Dictionary config = CreateConfig();
            QuickFix.SessionID sessionID = new QuickFix.SessionID("FIX.4.2", "SENDER", "TARGET");
            QuickFix.SessionSettings settings = new QuickFix.SessionSettings();
            settings.Set(sessionID, config);

            App = new QFApp(settings);
            Initiator = new MockInitiator();
            App.Initiator = Initiator;

            Session = new SessionThatTracksOutbound(
                App,
                new QuickFix.MemoryStoreFactory(),
                sessionID,
                new QuickFix.DataDictionaryProvider(),
                new QuickFix.SessionSchedule(config),
                0,
                new QuickFix.ScreenLogFactory(settings),
                new QuickFix.DefaultMessageFactory(),
                "blah");
            Session.MaxLatency = 120;
        }

        private static QuickFix.Dictionary CreateConfig()
        {
            QuickFix.Dictionary config = new QuickFix.Dictionary();
            config.SetBool(QuickFix.SessionSettings.PERSIST_MESSAGES, false);
            config.SetString(QuickFix.SessionSettings.CONNECTION_TYPE, "initiator");
            config.SetString(QuickFix.SessionSettings.START_TIME, "00:00:00");
            config.SetString(QuickFix.SessionSettings.END_TIME, "00:00:00");
            return config;
        }

        public void Login()
        {
            App.OnLogon(Session.SessionID);
            Initiator.IsLoggedOnValue = true;
        }

        public void Logout()
        {
            App.OnLogout(Session.SessionID);
            Initiator.IsLoggedOnValue = false;
        }
    }
}
