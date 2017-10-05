using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit;

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
            //Import configuration into temporary variables
            lDay = new ObservableCollection<Day>(UserData.getDays());

            //Intitialize user control values
            lvDays.ItemsSource = lDay;
        }

        #region UI Event Handler
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

        private void buttonImport_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog() { CheckFileExists = false, Filter = "XML Files (*.xml)|*.xml" };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    UserData.XMLToHistory(openFileDialog.FileName);
                    Window_Loaded(null, new RoutedEventArgs());
                }
                catch
                {
                    System.Windows.MessageBox.Show("Error while importing Workday History failed: " + openFileDialog.FileName, "Import failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void buttonExport_OnClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog() { CheckFileExists = false, ValidateNames = true, Filter = "XML Files (*.xml)|*.xml" };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    UserData.HistoryToXML(saveFileDialog.FileName);
                    System.Windows.MessageBox.Show("Workday History successfully exported to: " + saveFileDialog.FileName, "Export successful", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch
                {
                    System.Windows.MessageBox.Show("Error while exporting Workday History to: " + saveFileDialog.FileName, "Export failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
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
        #endregion

        private void listview_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            //Make sure that the Date portion of start and end time match
            if ((e.Source as DateTimeUpDown).DataContext is Day)
            {
                Day oldDay = (e.Source as DateTimeUpDown).DataContext as Day;
                DateTime newDate = ((DateTime)e.NewValue).Date;

                if (oldDay.starttime.Date > newDate.Date)
                    ((e.Source as DateTimeUpDown).DataContext as Day).endtime = new DateTime(newDate.Year, newDate.Month, newDate.Day, oldDay.endtime.Hour, oldDay.endtime.Minute, oldDay.endtime.Second);
            }

            lvDays.Items.Refresh();
        }
    }
}
