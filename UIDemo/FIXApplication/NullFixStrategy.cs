using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FIXApplication
{
    public class NullFixStrategy : ICustomFixStrategy
    {
        #region ICustomFixStrategy Members

        // It's the *null* strategy, so don't do anything.

        public void ProcessToAdmin(QuickFix.Message msg, QuickFix.Session session)
        { }

        public void ProcessToApp(QuickFix.Message msg, QuickFix.Session session)
        { }

        #endregion
    }
}
