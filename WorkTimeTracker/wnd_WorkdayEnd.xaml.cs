using System;
using System.Windows;

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
            Aero.enable(5, this);
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

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            WorkdayHandler.WorkdayEnd(pickerEndtime.Value.Value);
            base.Close();
        }
        #endregion
    }
}
