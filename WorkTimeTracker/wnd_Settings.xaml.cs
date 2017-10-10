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
    /// Interaktionslogik für wnd_Settings.xaml
    /// </summary>
    public partial class wnd_Settings : Window
    {
        Int32 iWorkDuration;
        Color cTrayIconColor;
        ObservableCollection<Break> lBreak;
        ObservableCollection<Subtitle> lSubtitle;
        ObservableCollection<Threshold> lThreshold;

        public wnd_Settings()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Import configuration into temporary variables
            iWorkDuration = UserData.getWorkDuration();
            cTrayIconColor = UserData.getTrayIconColor();
            lBreak = new ObservableCollection<Break>(UserData.getBreaks());
            lSubtitle = new ObservableCollection<Subtitle>(UserData.getSubtitles());
            lThreshold = new ObservableCollection<Threshold>(UserData.getThresholds());

            //Intitialize user control values
            iudWorkDuration.Value = iWorkDuration;
            cpTrayIcon.SelectedColor = cTrayIconColor;
            lvBreaks.ItemsSource = lBreak;
            lvSubtitles.ItemsSource = lSubtitle;
            lvThresholds.ItemsSource = lThreshold;
        }

        #region UI Event Handler
        private void buttonItemDelete_OnClick(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;

            if(b.CommandParameter is Break)
                lBreak.Remove(b.CommandParameter as Break);

            if (b.CommandParameter is Subtitle)
                lSubtitle.Remove(b.CommandParameter as Subtitle);

            if (b.CommandParameter is Threshold)
                lThreshold.Remove(b.CommandParameter as Threshold);
        }

        private void buttonItemAdd_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;

            if (b.Tag.ToString() == "break")
                lBreak.Add(new Break(String.Empty, true, TimeSpan.Zero, DateTime.Now));

            if (b.Tag.ToString() == "subtitle")
                lSubtitle.Add(new Subtitle(0,0,String.Empty));

            if (b.Tag.ToString() == "threshold")
                lThreshold.Add(new Threshold(Colors.Transparent, 0 ));
        }

        private void buttonCancel_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
        
        private void buttonSave_OnClick(object sender, RoutedEventArgs e)
        {
            UserData.setWorkDuration(iudWorkDuration.Value.Value);
            UserData.setTrayIconColor(cpTrayIcon.SelectedColor.Value);
            UserData.setBreaks(lBreak.ToList());
            UserData.setSubtitles(lSubtitle.ToList());
            UserData.setThresholds(lThreshold.ToList());

            Close();
        }
        
        private void buttonImport_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog() { CheckFileExists = false, Filter = "XML Files (*.xml)|*.xml" };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    UserData.XMLToConfig(openFileDialog.FileName);
                    Window_Loaded(null, new RoutedEventArgs());
                }
                catch
                {
                    MessageBox.Show("Error while importing of configuration failed: " + openFileDialog.FileName, "Import failed", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    UserData.ConfigToXML(saveFileDialog.FileName);
                    MessageBox.Show("Configuration successfully exported to: " + saveFileDialog.FileName,"Export successful",MessageBoxButton.OK,MessageBoxImage.Information);
                }
                catch
                {
                    MessageBox.Show("Error while exporting configuration to: " + saveFileDialog.FileName, "Export failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        #endregion
    }
}
