using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIDemo;

namespace UnitTests.Util
{
    public static class TestTool
    {
        private static QuickFix.Dictionary CreateConfig()
        {
            QuickFix.Dictionary config = new QuickFix.Dictionary();
            config.SetBool(QuickFix.SessionSettings.PERSIST_MESSAGES, false);
            config.SetString(QuickFix.SessionSettings.CONNECTION_TYPE, "initiator");
            config.SetString(QuickFix.SessionSettings.START_TIME, "00:00:00");
            config.SetString(QuickFix.SessionSettings.END_TIME, "00:00:00");
            return config;
        }

        static public void SetupAppAndSession(out QFApp app, out SessionThatTracksOutbound session, out MockInitiator init)
        {
            QuickFix.Dictionary config = CreateConfig();
            QuickFix.SessionID sessionID = new QuickFix.SessionID("FIX.4.2", "SENDER", "TARGET");
            QuickFix.SessionSettings settings = new QuickFix.SessionSettings(); //CreateSettings(sessionID);
            settings.Set(sessionID, config);

            app = new QFApp(settings);
            init = new MockInitiator();
            app.Initiator = init;

            session = new SessionThatTracksOutbound(
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
        }
    }
}
