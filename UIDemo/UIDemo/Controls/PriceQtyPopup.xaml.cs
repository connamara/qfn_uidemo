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

        public bool IsSetOMFOverride { get; set; }

        public PriceQtyPopup(OrderRecord or)
        {
            DataContext = this;

            IsCancelled = true;

            QtyString = or.OriginalNOS.OrderQty.Obj.ToString();

            InitializeComponent();

            if (or.OriginalNOS.OrdType.Obj == QuickFix.Fields.OrdType.MARKET)
            {
                lblPrice.Visibility = Visibility.Collapsed;
                txtPrice.Visibility = Visibility.Collapsed;
                PriceString = "0";
            }
            else
                PriceString = or.Price.ToString();
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
