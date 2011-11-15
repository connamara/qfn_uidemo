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

        static OrderCommands()
        {
            cancel = new RoutedUICommand("Cancel Order", "CancelOrder", typeof(OrderCommands));
        }

        public static RoutedUICommand Cancel
        {
            get { return cancel; }
        }
    }
}
