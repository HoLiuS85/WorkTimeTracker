using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;

namespace WorkTimeTracker
{
    /// <summary>
    /// Interaction logic for wndNotification.xaml
    /// </summary>
    public partial class wnd_Notification : Window
    {
        public wnd_Notification()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Helper.EnableAeroBorder(5, this);

            labelHeaderSubtitle.Text = Helper.getSubtitle(WorkdayHandler.getPercent());
            labelRemainingTime.Text = UserData.getWorkTimeRemaining().ToString("hh\\:mm");
            labelRemainingText.Text = UserData.getWorkTimeRemaining() < TimeSpan.Zero ? "Overtime:" : "Remaining Time";
            labelElapsedTime.Text = UserData.getWorkTimeElapsed().ToString("hh\\:mm");
            labelEndTime.Text = UserData.getWorkTimeEnd().ToShortTimeString();
            labelStartTime.Text = UserData.getWorkTimeStart().ToShortTimeString();
            progressbarWorktime.Value = WorkdayHandler.getPercent();
            progressbarWorktime.Foreground = new SolidColorBrush(Helper.getProgressColor(WorkdayHandler.getPercent()));

            if (!WorkdayHandler.getIsStarted())
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
            if (!WorkdayHandler.getIsStarted())
                UserData.setOpenWindow(OpenForm.StartModify);
            else
                UserData.setOpenWindow(OpenForm.End);

            Close();
        }

        private void LabelModify_OnMouseClick(object sender, MouseButtonEventArgs e)
        {
            UserData.setOpenWindow(OpenForm.StartModify);

            Close();
        }
        #endregion
    }
}
