using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WorkTimeTracker
{
    /// <summary>
    /// Interaktionslogik für wnd_History.xaml
    /// </summary>
    public partial class wnd_History : Window
    {
        ObservableCollection<Day> lDay;

        public wnd_History()
        {
            InitializeComponent();            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lDay = new ObservableCollection<Day>(UserData.getDays());

            lvDays.ItemsSource = lDay;
        }

        private void buttonItemAdd_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;

            lDay.Add(new Day(DateTime.MinValue, DateTime.MinValue));         
        }

        private void buttonItemDelete_OnClick(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;

            if (b.CommandParameter is Day)
                lDay.Remove(b.CommandParameter as Day);
        }

        private void buttonSave_OnClick(object sender, RoutedEventArgs e)
        {
            UserData.setDays(lDay.ToList());

            Close();
        }

        private void buttonCancel_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
