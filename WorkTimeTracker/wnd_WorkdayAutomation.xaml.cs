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
using System.Windows.Shapes;

namespace WorkTimeTracker
{
    /// <summary>
    /// Interaktionslogik für wnd_WorkdayAutomation.xaml
    /// </summary>
    public partial class wnd_WorkdayAutomation : Window
    {
        public wnd_WorkdayAutomation()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DateTime dtTempNow = DateTime.Now;

            if (!WorkdayHandler.getIsStarted())
            {
                Title = "First daily logon detected - No workday active";
                cbEndWorkday.IsEnabled = false;
                cbEndWorkday.IsChecked = false;
                pickerEndTime.IsEnabled = false;
                pickerStartTime.Value = dtTempNow;
            }
            else
            {
                Title = "First daily logon detected - Workday on '" + UserData.getLastLockTime().ToShortDateString() + "' still active";
                pickerStartTime.Value = dtTempNow;
                pickerEndTime.Value = UserData.getLastLockTime();
            }
        }

        #region UI Events
        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            if (cbEndWorkday.IsChecked.Value)
                WorkdayHandler.WorkdayEnd(pickerEndTime.Value.Value);

            if (cbStartWorkday.IsChecked.Value)
            {
                WorkdayHandler.WorkdayStart(UserData.getWorkDuration(), pickerStartTime.Value.Value);
                UserData.setLastLockTime(DateTime.Now);
            }

            Close();
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion
    }
}
