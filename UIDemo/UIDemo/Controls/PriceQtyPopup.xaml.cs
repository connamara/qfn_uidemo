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
using System.ComponentModel;

namespace UIDemo.Controls
{
    /// <summary>
    /// Interaction logic for PriceQtyPopup.xaml
    /// </summary>
    public partial class PriceQtyPopup : Window
    {
        public bool IsCancelled { get; set; }

        public string QtyString
        {
            get { return (string)GetValue(QtyStringProperty); }
            set { SetValue(QtyStringProperty, value); }
        }

        public string PriceString
        {
            get { return (string)GetValue(PriceStringProperty); }
            set { SetValue(PriceStringProperty, value); }
        }

        public bool IsSetOMFOverride
        {
            get { return (bool)GetValue(IsSetOMFOverrideProperty); }
            set { SetValue(IsSetOMFOverrideProperty, value); }
        }

        public static readonly DependencyProperty QtyStringProperty =
            DependencyProperty.Register("QtyString", typeof(string), typeof(PriceQtyPopup), new UIPropertyMetadata("0"));
        public static readonly DependencyProperty PriceStringProperty =
            DependencyProperty.Register("PriceString", typeof(string), typeof(PriceQtyPopup), new UIPropertyMetadata("0.0"));
        public static readonly DependencyProperty IsSetOMFOverrideProperty =
            DependencyProperty.Register("IsSetOMFOverride", typeof(bool), typeof(PriceQtyPopup), new UIPropertyMetadata(false));




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
