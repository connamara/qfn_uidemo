using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UIDemo
{
    public class QFApp : QuickFix.MessageCracker, QuickFix.Application
    {
        #region Application Members

        public void FromAdmin(QuickFix.Message message, QuickFix.SessionID sessionID)
        {
        }

        public void FromApp(QuickFix.Message message, QuickFix.SessionID sessionID)
        {
        }

        public void OnCreate(QuickFix.SessionID sessionID)
        {
        }

        public void OnLogon(QuickFix.SessionID sessionID)
        {
        }

        public void OnLogout(QuickFix.SessionID sessionID)
        {
        }

        public void ToAdmin(QuickFix.Message message, QuickFix.SessionID sessionID)
        {
        }

        public void ToApp(QuickFix.Message message, QuickFix.SessionID sessionId)
        {
        }

        #endregion
    }
}
