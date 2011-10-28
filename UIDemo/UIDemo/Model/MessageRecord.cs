using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UIDemo.Model
{
    public class MessageRecord : NotifyPropertyChangedBase
    {
        public MessageRecord(QuickFix.Message msg, bool isIncoming)
        {
            MsgText = msg.ToString().Replace(QuickFix.Message.SOH, "|");
            Direction = isIncoming ? "IN" : "OUT";
            Timestamp = msg.Header.GetDateTime(QuickFix.Fields.Tags.SendingTime);
        }

        private DateTime _timestamp = DateTime.MinValue;
        private string _msgText = "";
        private string _direction = "";

        public DateTime Timestamp
        {
            get { return _timestamp; }
            set { _timestamp = value; OnPropertyChanged("Timestamp"); }
        }

        public string MsgText
        {
            get { return _msgText; }
            set { _msgText = value; OnPropertyChanged("MsgText"); }
        }

        public string Direction
        {
            get { return _direction; }
            set { _direction = value; OnPropertyChanged("Direction"); }
        }
    }
}
