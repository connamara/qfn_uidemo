using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FIXApplication.Enums
{
    public enum OrderType
    {
        Market,
        Limit
    }

    public enum TimeInForce
    {
        Day,
        GoodTillCancel
        // I'm leaving the others out for now
    }
}
