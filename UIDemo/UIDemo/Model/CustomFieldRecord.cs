using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UIDemo.Model
{
    public class CustomFieldRecord : NotifyPropertyChangedBase
    {
        public CustomFieldRecord(int tag, string value)
        {
            _tag = tag;
            _value = value;
        }

        private int _tag;
        private string _value;

        public int Tag
        {
            get { return _tag; }
            set { _tag = value; OnPropertyChanged("Tag"); }
        }

        public string Value
        {
            get { return _value; }
            set { _value = value; OnPropertyChanged("Value"); }
        }
    }
}
