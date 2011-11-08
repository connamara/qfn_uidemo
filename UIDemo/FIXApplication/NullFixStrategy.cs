using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FIXApplication
{
    public class NullFixStrategy : ICustomFixStrategy
    {
        // It's the *null* strategy, thus this class doesn't do anything.

        #region ICustomFixStrategy Members

        public QuickFix.SessionSettings SessionSettings { get; set; }

        public void ProcessToAdmin(QuickFix.Message msg, QuickFix.Session session)
        { }

        public void ProcessToApp(QuickFix.Message msg, QuickFix.Session session)
        { }

        #endregion
    }
}
