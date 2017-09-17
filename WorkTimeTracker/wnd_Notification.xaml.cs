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
    public partial class wnd_Notification : Window
    {
        int WorkdayPercent;
        bool isWorkdayStarted;

        public wnd_Notification()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Aero.enable(5, this);

            WorkdayPercent = WorkdayHandler.getPercent();
            isWorkdayStarted = WorkdayHandler.getIsStarted();

            labelHeaderSubtitle.Text = Helper.getSubtitle(WorkdayPercent);
            labelRemainingTime.Text = UserData.getWorkTimeRemaining().ToString("hh\\:mm");
            labelRemainingText.Text = UserData.getWorkTimeRemaining() < TimeSpan.Zero ? "Overtime:" : "Remaining Time";
            labelElapsedTime.Text = UserData.getWorkTimeElapsed().ToString("hh\\:mm");
            labelEndTime.Text = UserData.getWorkTimeEnd().ToShortTimeString();
            labelStartTime.Text = UserData.getWorkTimeStart().ToShortTimeString();
            progressbarWorktime.Value = WorkdayPercent;
            progressbarWorktime.Foreground = new SolidColorBrush(Helper.getProgressColor(WorkdayPercent));

            if (!isWorkdayStarted)
            {
                grid.Children.Remove(labelWorkdayModify);
                labelWorkdayStartEnd.Text = "Start new Workday";
                labelWorkdayStartEnd.Margin = new Thickness(0, 10, 0, 15);
            }
            else
            {
                labelWorkdayStartEnd.Text = "End current Workday";
            }
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

        private void Label_OnMouseEnter(object sender, MouseEventArgs e)
        {
            ((TextBlock)sender).TextDecorations.Add(TextDecorations.Underline[0]);
        }

        private void Label_OnMouseLeave(object sender, MouseEventArgs e)
        {
            ((TextBlock)sender).TextDecorations.Remove(TextDecorations.Underline[0]);
        }

        private void labelStartEnd_OnMouseClick(object sender, MouseButtonEventArgs e)
        {
            if (!isWorkdayStarted)
                Config.openForm = OpenForm.StartModify;
            else
                Config.openForm = OpenForm.End;

            Close();
        }

        private void LabelModify_OnMouseClick(object sender, MouseButtonEventArgs e)
        {
            Config.openForm = OpenForm.StartModify;

            Close();
        }
        #endregion
    }
}
