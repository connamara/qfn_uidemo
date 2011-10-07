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
        private ConnectionModel _model = null;

        public ICommand SendNewsCommand { get; set; }

        public NewsSenderViewModel(ConnectionModel cm)
        {
            _model = cm;

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

        // commands
        private void SendNews(object obj)
        {
            Trace.WriteLine(String.Format("Send news: Head=[{0}] Line1=[{1}]", this.Headline, this.Line1Text));
            string h = this.Headline;
            IList<string> lines = new List<string>();
            if (this.IsLine1Enabled)
                lines.Add(this.Line1Text);

            _model.SendNewsMessage(h, lines);
        }

    }
}
