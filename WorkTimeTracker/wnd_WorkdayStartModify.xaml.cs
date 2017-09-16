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
    /// Interaction logic for wnd_WorkdayStart.xaml
    /// </summary>
    public partial class wnd_WorkdayStart : Window
    {
        public wnd_WorkdayStart()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Aero.enable(5, this);

            if (!WorkdayHandler.getIsStarted())
            {
                labelHeaderTitle.Text = "Start new Workday";
                pickerStartTime.Value = DateTime.Now;
            }
            else
            {
                labelHeaderTitle.Text = "Modify current Workday";
                pickerStartTime.Value = UserData.getWorkTimeStart();
            }

            pickerWorkDuration.Value = UserData.getWorkDuration();
        }

        #region UI Events
        private void Window_OnDeactivated(object sender, EventArgs e)
        {
            try
            {
                Close();
            }
            catch { /*Can appen if window is already closing*/}
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            WorkdayHandler.WorkdayStart(pickerWorkDuration.Value.Value, pickerStartTime.Value.Value);
            Close();
        }
        #endregion
    }
}
