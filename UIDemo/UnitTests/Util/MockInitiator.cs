using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTests.Util
{
    public class MockInitiator : QuickFix.IInitiator
    {
        public bool IsLoggedOnValue { get; set; }
        private bool _started = false;


        #region IInitiator Members

        public bool IsStopped { get { return !_started; } }

        public bool IsLoggedOn() { return IsLoggedOnValue; }

        public void Start()
        {
            _started = true;
        }

        public void Stop(bool force)
        {
            Stop();
        }

        public void Stop()
        {
            _started = false;
        }

        #endregion
    }
}
