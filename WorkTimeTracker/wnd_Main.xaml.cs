using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace WorkTimeTracker
{
    /// <summary>
    /// Interaction logic for wndMain.xaml
    /// </summary>
    public partial class wndMain : Window
    {
        public wndMain()
        {
            try
            {
                //Config.Import();
                //UserData.ExportOldConfig();

                // read user settings
                // ConfigHandler.Save();

                UserData.ReadConfig();

                if (WorkdayHandler.getIsStarted())
                    WorkdayHandler.WorkdayStart(UserData.getWorkDuration(), UserData.getWorkTimeStart());

                ui_NotifyIcon Icon = new ui_NotifyIcon();
                Icon.trayIcon.Click += OnTrayClick;
                Icon.trayIcon.ContextMenu.MenuItems["settings"].Click += OnSettingsClick;
                Icon.trayIcon.ContextMenu.MenuItems["history"].Click += OnHistoryClick;
                Icon.trayIcon.ContextMenu.MenuItems["exit"].Click += OnExitClick;

                SessionHandler UserSession = new SessionHandler();
                UserSession.MyWorkdayEvent += new SessionHandler.EventDelegate(OnSessionStateChange);
                UserSession.VerifySessionState();
            }
            catch (Exception e) { MessageBox.Show(e.Source + "-----" + e.Message + "-------"+e.StackTrace+"-----------" + e.Data);
                Application.Current.Shutdown(1);
            }
        }
        
        private void WindowOpener(Window window)
        {
            //Reset the Openform Flag
            UserData.setOpenWindow(OpenForm.None);

            window.SizeChanged += OnWindowSizeChanged;
            window.Closed += OnWindowClosed;
            window.Show();
            window.Activate();
        }

        private void OnWindowClosed(object sender, EventArgs e)
        {
            if (UserData.getOpenWindow() == OpenForm.StartModify)
                WindowOpener(new wnd_WorkdayStart());

            if (UserData.getOpenWindow() == OpenForm.End)
                WindowOpener(new wnd_WorkdayEnd());
        }

        private void OnSessionStateChange()
        {
            WindowOpener(new wnd_WorkdayAutomation());
        }

        private void OnWindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (sender is wnd_History || sender is wnd_Settings)
                return;
            
            Window tempWindow = (Window)sender;
            Point windowPosition = new Point();
            Point mousePositon = Helper.getMousePosition(tempWindow);
            Rect ScreenSize = SystemParameters.WorkArea; //Rect ScreenSize = Helper.getScreenSize(mousePositon);

            // Set Initial y-position on to of the start menu and x-position centered over the mouse
            windowPosition.X = mousePositon.X - (tempWindow.ActualWidth / 2);
            windowPosition.Y = ScreenSize.Bottom - tempWindow.ActualHeight;
            //windowPosition.Y = test - tempWindow.ActualHeight;

            // Adjust X position to not overlap screen borders
            if (windowPosition.X + tempWindow.ActualWidth > ScreenSize.Right)
                windowPosition.X -= windowPosition.X + tempWindow.ActualWidth - ScreenSize.Right;
            
            // Write numbers to the window
            tempWindow.Top = windowPosition.Y;
            tempWindow.Left = windowPosition.X;
        }
        
        #region UI Event Handler
        private void OnTrayClick(object sender, EventArgs e)
        {
            if ((e as System.Windows.Forms.MouseEventArgs).Button == System.Windows.Forms.MouseButtons.Left)
                WindowOpener(new wnd_Notification());
        }

        private void OnSettingsClick(object sender, EventArgs e)
        {
            WindowOpener(new wnd_Settings());
        }

        private void OnHistoryClick(object sender, EventArgs e)
        {
            WindowOpener(new wnd_History());
        }

        private void OnExitClick(object sender, EventArgs e)
        {
            //Exit the application
            Application.Current.Shutdown(0);
        }
        #endregion

    }
}
