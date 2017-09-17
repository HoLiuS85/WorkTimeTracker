using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace WorkTimeTracker
{
    /// <summary>
    /// Interaktionslogik für wnd_Settings.xaml
    /// </summary>
    public partial class wnd_Settings : Window
    {
        public wnd_Settings()
        {
            InitializeComponent();

            this.DataContext = this;
            
            lv.ItemsSource = UserData.getBreaks();
        }

        private void EditCategory(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            //ProductCategory productCategory = b.CommandParameter as ProductCategory;
           // MessageBox.Show(productCategory.Id);
        }

        private void DeleteCategory(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            //ProductCategory productCategory = b.CommandParameter as ProductCategory;
            // MessageBox.Show(productCategory.Id);
        }
    }

    public class Brk
    {        
        public String Name { get; set; }
        public Boolean Enabled { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime StartTime { get; set; }
        
    }
}
