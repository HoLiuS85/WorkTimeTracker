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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WorkTimeTracker
{
    /// <summary>
    /// Interaction logic for wndNotification.xaml
    /// </summary>
    public partial class wndNotification : Window
    {
        public wndNotification()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            Aero.enable(5, 5, 5, 5, new WindowInteropHelper(this).Handle);
        }
    }
}
