using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Input;
using UIDemo.Model;

namespace UIDemo.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            this.DisplayName = "QuickFIX/N UIDemo App";
        }
    }
}
