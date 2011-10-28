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

        static public void Invoke<T1,T2>(Action<T1,T2> action, T1 arg1, T2 arg2)
        {
            if (_dispatcher == null)
                action.Invoke(arg1,arg2);
            else
                _dispatcher.Invoke(action, arg1, arg2);
        }
    }
}
