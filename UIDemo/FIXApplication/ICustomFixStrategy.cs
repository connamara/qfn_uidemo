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
        void ProcessToAdmin(QuickFix.Message msg);
        void ProcessToApp(QuickFix.Message msg);
    }
}
