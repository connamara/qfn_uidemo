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
using UIDemo.Model;
using UIDemo.ViewModel;

namespace UIDemo.View
{
    /// <summary>
    /// Interaction logic for OrderView.xaml
    /// </summary>
    public partial class OrderView : Grid
    {
        public OrderView()
        {
            InitializeComponent();
        }

        private void DeleteCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            CustomFieldRecord cfr = lvCustomFields.SelectedItem as CustomFieldRecord;
            (DataContext as OrderViewModel).DeleteCustomField(cfr);
        }

        private void CanDeleteExecuteHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (lvCustomFields.Items.Count > 0) && (lvCustomFields.SelectedItem != null);
            e.Handled = true;
        }
    }
}
