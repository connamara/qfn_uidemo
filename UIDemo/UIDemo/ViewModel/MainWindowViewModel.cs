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
        public ConnectionViewModel ConnectionVM { get; set; }
        public OrderViewModel OrderVM { get; set; }
        public NewsSenderViewModel NewsSenderVM { get; set; }
        
        public MainWindowViewModel()
            : base()
        {
            this.DisplayName = "QuickFIX/N UIDemo App";

            QFApp qfapp = new QFApp();
            ConnectionModel cm = new ConnectionModel(qfapp);

            ConnectionVM = new ConnectionViewModel(cm);
            OrderVM = new OrderViewModel(cm);
            NewsSenderVM = new NewsSenderViewModel(cm);

            // kludge.  Should probably be done with events.
            qfapp.ConnectionVM = ConnectionVM;
        }
    }
}
