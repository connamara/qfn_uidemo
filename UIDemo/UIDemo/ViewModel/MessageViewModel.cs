using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIDemo.Model;
using System.Collections.ObjectModel;
using System.Diagnostics;
using FIXApplication;

namespace UIDemo.ViewModel
{
    public class MessageViewModel
    {
        private QFApp _qfapp = null;

        private Object _messagesLock = new Object();
        public ObservableCollection<MessageRecord> Messages { get; set; }

        public MessageViewModel(QFApp app)
        {
            _qfapp = app;
            Messages = new ObservableCollection<MessageRecord>();

            _qfapp.MessageEvent += new Action<QuickFix.Message,bool>(HandleMessage);
        }

        public void HandleMessage(QuickFix.Message msg, bool isIncoming)
        {
            try
            {
                MessageRecord mr = new MessageRecord(msg, isIncoming);

                SmartDispatcher.Invoke(new Action<MessageRecord>(AddMessage), mr);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
            }
        }

        public void AddMessage(MessageRecord r)
        {
            try
            {
                Messages.Add(r);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
            }
        }
    }
}
