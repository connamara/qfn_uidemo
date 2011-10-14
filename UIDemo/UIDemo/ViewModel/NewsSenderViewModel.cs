using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIDemo.Model;
using System.Diagnostics;
using System.Windows.Input;

namespace UIDemo.ViewModel
{
    public class NewsSenderViewModel : ViewModelBase
    {
        private QFApp _qfapp = null;

        public ICommand SendNewsCommand { get; set; }


        public NewsSenderViewModel(QFApp app)
        {
            _qfapp = app;

            // command definitions
            SendNewsCommand = new RelayCommand(SendNews);
        }

        // fields
        private string _headline = "";
        public string Headline
        {
            get { return _headline; }
            set { _headline = value; base.OnPropertyChanged("Headline"); }
        }

        private string _line1Text = "";
        public string Line1Text
        {
            get { return _line1Text; }
            set { _line1Text = value; base.OnPropertyChanged("Line1Text"); }
        }

        private bool _isLine1Enabled = false;
        public bool IsLine1Enabled
        {
            get { return _isLine1Enabled; }
            set { _isLine1Enabled = value; base.OnPropertyChanged("IsLine1Enabled"); }
        }

        private string _line2Text = "";
        public string Line2Text
        {
            get { return _line2Text; }
            set { _line2Text = value; base.OnPropertyChanged("Line2Text"); }
        }

        private bool _isLine2Enabled = false;
        public bool IsLine2Enabled
        {
            get { return _isLine2Enabled; }
            set { _isLine2Enabled = value; base.OnPropertyChanged("IsLine2Enabled"); }
        }

        private string _line3Text = "";
        public string Line3Text
        {
            get { return _line3Text; }
            set { _line3Text = value; base.OnPropertyChanged("Line3Text"); }
        }

        private bool _isLine3Enabled = false;
        public bool IsLine3Enabled
        {
            get { return _isLine3Enabled; }
            set { _isLine3Enabled = value; base.OnPropertyChanged("IsLine3Enabled"); }
        }

        // commands
        private void SendNews(object obj)
        {
            Trace.WriteLine(String.Format("Send news: Head=[{0}] Line1=[{1}]", this.Headline, this.Line1Text));
            string h = this.Headline;
            IList<string> lines = new List<string>();
            if (this.IsLine1Enabled)
                lines.Add(this.Line1Text);
            if (this.IsLine2Enabled)
                lines.Add(this.Line2Text);
            if(this.IsLine3Enabled)
                lines.Add(this.Line3Text);

            _qfapp.SendNewsMessage(h, lines);
        }
    }
}
