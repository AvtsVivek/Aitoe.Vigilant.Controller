using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Aitoe.Vigilant.Controller.WpfController
{
    /// <summary>
    /// Interaction logic for SimpleUserControl.xaml
    /// </summary>
    public partial class SimpleUserControl : UserControl
    {

        public string CellValue
        {
            get { return (string)GetValue(CellValueProperty); }
            set { SetValue(CellValueProperty, value); }
        }
        
        // Using a DependencyProperty as the backing store for LimitValue.  This enables animation, styling, binding, etc...    
        public static readonly DependencyProperty CellValueProperty =
            DependencyProperty.Register("CellValue", typeof(string), typeof(SimpleUserControl), new FrameworkPropertyMetadata
            {
                BindsTwoWayByDefault = true,
            });

        public String CellId
        {
            get { return (String)GetValue(CellIdProperty); }
            set { SetValue(CellIdProperty, value); }
        }

        /// <summary>
        /// Identified the CellId dependency property
        /// </summary>
        public static readonly DependencyProperty CellIdProperty =
            DependencyProperty.Register("CellId", typeof(string), typeof(SimpleUserControl), new PropertyMetadata(""));
        public SimpleUserControl()
        {
            InitializeComponent();
            (this.Content as FrameworkElement).DataContext = this;
        }
    }
}
