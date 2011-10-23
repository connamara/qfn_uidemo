using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;

namespace UIDemo
{
    public class SmartDispatcher
    {
        static public Dispatcher _dispatcher;

        static public void SetDispatcher(Dispatcher d)
        {
            _dispatcher = d;
        }

        static public void Invoke<T>(Action<T> action, T arg)
        {
            if (_dispatcher == null)
                action.Invoke(arg);
            else
                _dispatcher.Invoke(action, arg);
        }
        //                System.Windows.Application.Current.Dispatcher.Invoke(new Action<ExecutionRecord>(AddExecution), exRec);

    }
}
