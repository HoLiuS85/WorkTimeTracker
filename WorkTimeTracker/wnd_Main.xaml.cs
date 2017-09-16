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
    /// Interaction logic for wndMain.xaml
    /// </summary>
    public partial class wndMain : Window
    {
        public wndMain()
        {
            /*
            Config.Import();
            UserData.ExportOldConfig();
            */

            // read user settings
            UserData.Import();

            if (UserData.getDays().LastOrDefault().dtEndTime == DateTime.MinValue)
                WorkdayHandler.WorkdayStart(UserData.getWorkDuration(), UserData.getWorkTimeStart());

            ui_NotifyIcon Icon = new ui_NotifyIcon();
            Icon.trayIcon.Click += OnTrayClick;
            Icon.trayIcon.ContextMenu.MenuItems["settings"].Click += OnSettingsClick;
            Icon.trayIcon.ContextMenu.MenuItems["history"].Click += OnHistoryClick;
            Icon.trayIcon.ContextMenu.MenuItems["exit"].Click += OnExitClick;
        }

        private void OnTrayClick(object sender, EventArgs e)
        {
            if ((e as System.Windows.Forms.MouseEventArgs).Button == System.Windows.Forms.MouseButtons.Left)
                WindowOpener(new wndNotification());
        }

        private void OnSettingsClick(object sender, EventArgs e)
        {
            //this.FrmOpener(new frmSettings());
        }

        private void OnHistoryClick(object sender, EventArgs e)
        {
            // this.FrmOpener(new frmHistory());
        }

        private void OnExitClick(object sender, EventArgs e)
        {
            //Stop the Workday caluclation properly
            WorkdayHandler.WorkdayCalculationStop();

            //Exit the application
            Application.Current.Shutdown(0);
        }

        private void WindowOpener(Window window)
        {
            //Reset the Openform Flag
            Config.openForm = OpenForm.None;

            window.SizeChanged += OnWindowSizeChanged;
            window.Closed += OnWindowClosed;
            window.Show();
            window.Activate();
        }

        private void OnWindowClosed(object sender, EventArgs e)
        {
            if (Config.openForm == OpenForm.StartModify)
                WindowOpener(new wnd_WorkdayStart());

            if (Config.openForm == OpenForm.End)
                WindowOpener(new wnd_WorkdayEnd());
        }

        private void OnWindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Window tempWindow = (Window)sender;
            Point windowPosition = new Point();
            Point mousePositon = Helper.getMousePosition(tempWindow);
            Rect ScreenSize = Helper.getScreenSize(mousePositon);
            
            // Set Initial y-position on to of the start menu and x-position centered over the mouse
            windowPosition.X = mousePositon.X - (tempWindow.ActualWidth / 2);
            windowPosition.Y = ScreenSize.Y + ScreenSize.Height - tempWindow.ActualHeight;

            // Adjust X position to not overlap screen borders
            if (windowPosition.X + tempWindow.ActualWidth > ScreenSize.Right)
                windowPosition.X -= windowPosition.X + tempWindow.ActualWidth - ScreenSize.Right;
            
            // Write numbers to the window
            tempWindow.Top = windowPosition.Y;
            tempWindow.Left = windowPosition.X;
        }
    }
}
