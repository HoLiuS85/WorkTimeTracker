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
            Config.Import();

            ui_NotifyIcon Icon = new ui_NotifyIcon();
            Icon.trayIcon.Click += OnTrayClick;
            Icon.trayIcon.ContextMenu.MenuItems["settings"].Click += OnSettingsClick;
            Icon.trayIcon.ContextMenu.MenuItems["history"].Click += OnHistoryClick;
            Icon.trayIcon.ContextMenu.MenuItems["exit"].Click += OnExitClick;
        }



        private void OnSettingsClick(object sender, EventArgs e)
        {
            //this.FrmOpener(new frmSettings());
        }

        private void OnTrayClick(object sender, EventArgs e)
        {

            if ((e as System.Windows.Forms.MouseEventArgs).Button == System.Windows.Forms.MouseButtons.Left)
            {
                WindowOpener(new wndNotification());
            }
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
            window.SizeChanged += OnWindowSizeChanged;
            window.Closed += OnWindowClosed;
            window.Show();
            window.Activate();
        }

        private void OnWindowClosed(object sender, EventArgs e)
        {
            string asd = e.GetType().ToString();
            OpenForm openForm = Config.openForm;
            if (openForm == OpenForm.StartModify)
            {
                Config.openForm = OpenForm.None;
                //this.WindowOpener(new frmWorkdayStart());
            }
            else if (openForm == OpenForm.End)
            {
                Config.openForm = OpenForm.None;
                //this.WindowOpener(new frmWorkdayEnd());
            }
        }

        private void OnWindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Window tempWindow = (Window)sender;
            Point windowPosition = new Point();

            Int32 screenXsize = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
            windowPosition.Y = (screenXsize - tempWindow.ActualHeight) - tempWindow.ActualHeight;

            Int32 cursorXPos = System.Windows.Forms.Cursor.Position.X;
            windowPosition.X = (cursorXPos - tempWindow.ActualWidth) - tempWindow.ActualWidth;

            tempWindow.Top = 0;
            tempWindow.Left = cursorXPos;
        }
    }
}
