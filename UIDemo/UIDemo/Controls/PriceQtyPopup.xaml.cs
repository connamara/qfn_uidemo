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

namespace UIDemo.Controls
{
    /// <summary>
    /// Interaction logic for PriceQtyPopup.xaml
    /// </summary>
    public partial class PriceQtyPopup : Window
    {
        public bool IsCancelled { get; set; }

        public string QtyString { get; set; }
        public string PriceString { get; set; }

        public PriceQtyPopup(
            FIXApplication.Enums.OrderType otype,
            int initialQty,
            decimal initialPrice)
        {
            DataContext = this;

            IsCancelled = true;

            QtyString = initialQty.ToString();
            PriceString = initialPrice.ToString();

            InitializeComponent();

            if (otype == FIXApplication.Enums.OrderType.Market)
            {
                lblPrice.Visibility = Visibility.Collapsed;
                txtPrice.Visibility = Visibility.Collapsed;
            }
        }

        private void ClickSubmit(object sender, RoutedEventArgs e)
        {
            IsCancelled = false;
            this.Close();
        }

        private void ClickCancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
