using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Input;

namespace UIDemo.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ConnectionViewModel ConnectionVM { get; set; }

        
        public MainWindowViewModel()
            : base()
        {
            this.DisplayName = "QuickFIX/N UIDemo App";
            ConnectionVM = new ConnectionViewModel();
        }
    }
}
