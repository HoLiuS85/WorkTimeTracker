using System;
using System.Drawing;
using System.Windows.Forms;

namespace WorkTimeTracker
{
    class ui_NotifyIcon
    {
        public NotifyIcon trayIcon;
        private System.Timers.Timer tUpdateIcon;
        private Boolean bIcon;

        public ui_NotifyIcon()
        {
            trayIcon = new NotifyIcon();
            trayIcon.Text = "WorkTimeTracker";
            trayIcon.ContextMenu = IconCreateMenu();
            trayIcon.Visible = true;

            tUpdateIcon = new System.Timers.Timer(1000);
            tUpdateIcon.Elapsed += tUpdateIcon_Elapsed;
            tUpdateIcon.Start();
        }

        private ContextMenu IconCreateMenu()
        {
            ContextMenu contextMenu = new ContextMenu();
            contextMenu.MenuItems.Add(new MenuItem() { Text = "Settings", Name = "settings" });
            contextMenu.MenuItems.Add(new MenuItem() { Text = "History", Name = "history" });
            contextMenu.MenuItems.Add("-");
            contextMenu.MenuItems.Add(new MenuItem() { Text = "Exit", Name = "exit" });
            return contextMenu;
        }

        private void tUpdateIcon_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!WorkdayHandler.getIsStarted())
            {
                trayIcon.Icon = Helper.getTrayIcon(UserData.getTrayIconColor(), new Colour(Color.Transparent));
            }
            else
            {
                if (WorkdayHandler.getPercent() == 100)
                {
                    if (bIcon)
                    {
                        trayIcon.Icon = Helper.getTrayIcon(UserData.getTrayIconColor(), Helper.getProgressColor(WorkdayHandler.getPercent()));
                        bIcon = false;
                    }
                    else
                    {
                        trayIcon.Icon = Helper.getTrayIcon(Helper.getProgressColor(WorkdayHandler.getPercent()), UserData.getTrayIconColor());
                        bIcon = true;
                    }
                }
                else
                {
                    trayIcon.Icon = Helper.getTrayIcon(UserData.getTrayIconColor(), Helper.getProgressColor(WorkdayHandler.getPercent()));
                }
            }

            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}
