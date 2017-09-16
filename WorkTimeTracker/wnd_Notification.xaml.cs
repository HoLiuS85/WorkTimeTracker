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
    public partial class wndNotification : Window
    {
        int WorkdayPercent;
        bool isWorkdayStarted;

        public wndNotification()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Aero.enable(5, this);

            WorkdayPercent = WorkdayHandler.WorkdayPercent();
            isWorkdayStarted = WorkdayHandler.isWorkdayStarted();

            labelHeaderSubtitle.Text = Helper.GetSubtitle(WorkdayPercent);
            labelRemainingTime.Text = string.Concat((Config.tsWorkTimeRemaining < TimeSpan.Zero ? "-" : ""), Config.tsWorkTimeRemaining.ToString("hh\\:mm"));
            labelElapseTime.Text = Config.tsWorkTimeElapsed.ToString("hh\\:mm");
            labelEndTime.Text = Config.dtWorkEndTime.ToShortTimeString();
            labelStartTime.Text = Config.dtWorkStartTime.ToShortTimeString();
            progressbarWorktime.Value = WorkdayPercent;
            progressbarWorktime.Foreground = Helper.GetProgressColor(WorkdayPercent);

            if (!isWorkdayStarted)
            {
                labelWorkdayModify = null;
                labelWorkdayStartEnd.Text = "Start new Workday";
            }
            else
            {
                labelWorkdayStartEnd.Text = "End current Workday";
            }
        }
        
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
    }
}
