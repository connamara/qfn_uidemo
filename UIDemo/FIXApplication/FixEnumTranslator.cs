using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FIXApplication
{
    public static class FixEnumTranslator
    {
        public static string Translate(QuickFix.Fields.OrdStatus ordStatus)
        {
            switch (ordStatus.Obj)
            {
                case '0': return "New";
                case '1': return "PartiallyFilled";
                case '2': return "Filled";
                case '4': return "Canceled";
                case '5': return "Replaced";
                case '8': return "Rejected";
            }
            return "unknown";
        }

        public static string Translate(QuickFix.Fields.OrdType ordType)
        {
            switch (ordType.Obj)
            {
                case '1': return "Market";
                case '2': return "Limit";
            }
            return "unknown";
        }

        public static string Translate(QuickFix.Fields.ExecTransType execTransType)
        {
            switch (execTransType.Obj)
            {
                case QuickFix.Fields.ExecTransType.CANCEL: return "Cancel";
                case QuickFix.Fields.ExecTransType.CORRECT: return "Correct";
                case QuickFix.Fields.ExecTransType.NEW: return "New";
                case QuickFix.Fields.ExecTransType.STATUS: return "Status";
            }
            return "unknown";
        }

        public static string Translate(QuickFix.Fields.ExecType execType)
        {
            switch (execType.Obj)
            {
                case QuickFix.Fields.ExecType.CANCELED: return "Cancelled";
                case QuickFix.Fields.ExecType.FILL: return "Filled";
                case QuickFix.Fields.ExecType.NEW: return "New";
                case QuickFix.Fields.ExecType.REJECTED: return "Rejected";
                case QuickFix.Fields.ExecType.REPLACE: return "Replace";
                case QuickFix.Fields.ExecType.TRADE: return "Trade";
            }
            return "unknown";
        }

        public static string Translate(QuickFix.Fields.Side side)
        {
            switch (side.Obj)
            {
                case QuickFix.Fields.Side.BUY: return "Buy";
                case QuickFix.Fields.Side.SELL: return "Sell";
            }
            return "unknown";
        }
    }
}
