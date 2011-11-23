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
using UIDemo.Controls;
using FIXApplication.Enums;
using FIXApplication;

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

        private void CancelCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            OrderRecord or = lvOrders.SelectedItem as OrderRecord;
            (DataContext as OrderViewModel).CancelOrder(or);
        }

        private void CanCancelExecuteHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            // Ideally, this would check to see that the order's state is one that could be canceled
            // but right now it doesn't.
            e.CanExecute = (lvOrders.Items.Count > 0) && (lvOrders.SelectedItem != null);
            e.Handled = true;
        }

        private void CancelReplaceCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            OrderRecord or = lvOrders.SelectedItem as OrderRecord;

            PriceQtyPopup pop = new PriceQtyPopup(or);
            pop.ShowDialog();

            if (pop.IsCancelled)
                MessageBox.Show("not doing shit");
            else
                MessageBox.Show(String.Format("would send C/R with qty={1} and price={0}", pop.PriceString, pop.QtyString));

            (DataContext as OrderViewModel).CancelReplaceOrder(or, int.Parse(pop.QtyString), decimal.Parse(pop.PriceString));
        }

        private void CanCancelReplaceExecuteHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            // Ideally, this would check to see that the order's state is one that could be cancel/replaced
            // but right now it doesn't.
            e.CanExecute = (lvOrders.Items.Count > 0) && (lvOrders.SelectedItem != null);
            e.Handled = true;
        }
    }
}
