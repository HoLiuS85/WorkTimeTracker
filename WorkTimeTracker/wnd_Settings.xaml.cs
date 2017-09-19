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
                lBreak.Add(new Break(true, DateTime.Now, TimeSpan.Zero, String.Empty));

            if (b.Tag.ToString() == "subtitle")
                lSubtitle.Add(new Subtitle(0,0,String.Empty));

            if (b.Tag.ToString() == "threshold")
                lThreshold.Add(new Threshold(Colors.Transparent, 0, String.Empty));


        }

        private void buttonCancel_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void buttonSave_OnClick(object sender, RoutedEventArgs e)
        {
            UserData.setWorkDuration(iWorkDuration);
            UserData.setTrayIconColor(cTrayIconColor);
            UserData.setBreaks(lBreak.ToList());
            UserData.setSubtitles(lSubtitle.ToList());
            UserData.setThresholds(lThreshold.ToList());

            Close();
        }
    }
}
