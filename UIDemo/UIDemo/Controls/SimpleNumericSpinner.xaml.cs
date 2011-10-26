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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UIDemo.Controls
{
    /// <summary>
    /// Interaction logic for SimpleNumericSpinner.xaml
    /// </summary>
    public partial class SimpleNumericSpinner : UserControl
    {
        public delegate void OnChangedHandler();
        public event OnChangedHandler OnChanged;

        public SimpleNumericSpinner()
        {
            InitializeComponent();
            textbox.LostFocus += new RoutedEventHandler( textbox_LostFocus );
        }

        void textbox_LostFocus( object sender, RoutedEventArgs e )
        {
            Value = Value;
        }

        public int? Value
        {
            get { return ( int? ) GetValue( ValueProperty ); }
            set 
            {
                int? writeable = FixMinMax( value );
                SetValue( ValueProperty, writeable );
                
                textbox.Text = ( ( ShowSign && value.HasValue && value.Value >= 0 ) ? "+" : "" ) + writeable.ToString();
                textbox.SelectAll();

                if ( OnChanged != null )
                    OnChanged();
            }
        }

        protected int? FixMinMax( int? value )
        {
            if( !value.HasValue )
                return value;

            if( value > MaxValue )
                value = MaxValue;
            else if( value < MinValue )
                value = MinValue;

            return value;
        }

        public int MinValue
        {
            get { return ( int ) GetValue( MinValueProperty ); }
            set { SetValue( MinValueProperty, value ); }
        }

        public int MaxValue
        {
            get { return ( int ) GetValue( MaxValueProperty ); }
            set { SetValue( MaxValueProperty, value ); }
        }

        public bool ShowSign
        {
            get { return ( bool ) GetValue( ShowSignProperty ); }
            set { SetValue( ShowSignProperty, value ); }
        }

        public bool HasError
        {
            get { return ( bool ) GetValue( HasErrorProperty ); }
            set 
            { 
                SetValue( HasErrorProperty, value );
            }
        }

        private void textbox_TextChanged( object sender, TextChangedEventArgs e )
        {
            ApplyText();
        }

        protected void ApplyText()
        {
            int value = 0;

            if( textbox.Text.Trim() == "" )
                Value = null;
            else if( !int.TryParse( textbox.Text, out value ) )
                Value = Value;
            else
                SetValue( ValueProperty, value );
        }

        private void UpButton_Click( object sender, RoutedEventArgs e )
        {
            if( !Value.HasValue )
                Value = 0;
            else
                Value += 1;
        }

        private void DownButton_Click( object sender, RoutedEventArgs e )
        {
            if( !Value.HasValue )
                Value = 0;
            else
                Value -= 1;
        }

        public static readonly DependencyProperty HasErrorProperty = DependencyProperty.Register( "HasError", typeof( bool ), typeof( SimpleNumericSpinner ), new FrameworkPropertyMetadata( false, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsParentMeasure ) );
        public static readonly DependencyProperty ShowSignProperty = DependencyProperty.Register( "ShowSign", typeof( bool ), typeof( SimpleNumericSpinner ), new FrameworkPropertyMetadata( false, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsParentMeasure ) );
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register( "Value", typeof( int? ), typeof( SimpleNumericSpinner ), new FrameworkPropertyMetadata( null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsParentMeasure ) );
        public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register( "MinValue", typeof( int ), typeof( SimpleNumericSpinner ), new FrameworkPropertyMetadata( 1, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsParentMeasure ) );
        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register( "MaxValue", typeof( int ), typeof( SimpleNumericSpinner ), new FrameworkPropertyMetadata( 999, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsParentMeasure ) );
       
        

    }
}
