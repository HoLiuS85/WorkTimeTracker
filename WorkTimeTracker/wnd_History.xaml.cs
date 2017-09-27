using Microsoft.Win32;
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
                    MessageBox.Show("Error while importing Workday History failed: " + openFileDialog.FileName, "Import failed", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    MessageBox.Show("Workday History successfully exported to: " + saveFileDialog.FileName, "Export successful", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch
                {
                    MessageBox.Show("Error while exporting Workday History to: " + saveFileDialog.FileName, "Export failed", MessageBoxButton.OK, MessageBoxImage.Error);
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
            lvDays.Items.Refresh();
        }
    }
}
