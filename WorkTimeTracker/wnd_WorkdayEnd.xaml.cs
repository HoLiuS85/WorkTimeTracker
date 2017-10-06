using System;
using System.Windows;
using System.Windows.Interop;

namespace WorkTimeTracker
{
    /// <summary>
    /// Interaction logic for wnd_WorkdayEnd.xaml
    /// </summary>
    public partial class wnd_WorkdayEnd : Window
    {
        public wnd_WorkdayEnd()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Helper.EnableAeroBorder(5, this);

            pickerEndtime.Value = DateTime.Now;
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

        private void buttonCancel_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void buttonSave_OnClick(object sender, RoutedEventArgs e)
        {
            //End the current workday using the start day and todays time 
            WorkdayHandler.WorkdayEnd(
                new DateTime(
                    UserData.getWorkTimeStart().Year,
                    UserData.getWorkTimeStart().Month,
                    UserData.getWorkTimeStart().Day,
                    pickerEndtime.Value.Value.Hour,
                    pickerEndtime.Value.Value.Minute,
                    pickerEndtime.Value.Value.Second));
            base.Close();
        }
        #endregion
    }
}
