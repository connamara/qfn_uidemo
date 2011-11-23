using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace UIDemo.Controls
{
    public class OrderCommands
    {
        private static RoutedUICommand cancel;
        private static RoutedUICommand cancel_replace;

        static OrderCommands()
        {
            cancel = new RoutedUICommand("Cancel Order", "CancelOrder", typeof(OrderCommands));
            cancel_replace = new RoutedUICommand("Cancel/Replace Order", "CancelReplace", typeof(OrderCommands));
        }

        static public RoutedUICommand Cancel
        {
            get { return cancel; }
        }

        static public RoutedUICommand CancelReplace
        {
            get { return cancel_replace; }
        }
    }
}
