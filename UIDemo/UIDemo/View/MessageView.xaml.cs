using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UIDemo.ViewModel;
using System.Diagnostics;
using UIDemo.Model;

namespace UIDemo.View
{
    /// <summary>
    /// Interaction logic for MessageView.xaml
    /// </summary>
    public partial class MessageView : Grid
    {
        public MessageView()
        {
            InitializeComponent();
        }

        private void CopyCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            MessageRecord mr = lvMessages.SelectedItem as MessageRecord;
            Clipboard.SetText(mr.MsgText);
        }

        private void CanCopyExecuteHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (lvMessages.Items.Count > 0) && (lvMessages.SelectedItem != null);
            e.Handled = true;
        }
    }
}
