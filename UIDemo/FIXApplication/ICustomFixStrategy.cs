using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FIXApplication
{
    /// <summary>
    /// Interface to optionally provide hooks for some hacks on outgoing messages.
    /// </summary>
    public interface ICustomFixStrategy
    {
        QuickFix.SessionSettings SessionSettings { get; set; }

        void ProcessToAdmin(QuickFix.Message msg, QuickFix.Session session);
        void ProcessToApp(QuickFix.Message msg, QuickFix.Session session);
    }
}
