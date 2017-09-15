using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Windows.Threading;

namespace WorkTimeTracker
{
    class ui_NotifyIcon
    {
        public NotifyIcon trayIcon;
        private System.Threading.Timer tupdateIcon;
        private Boolean bIcon;

        [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        private static extern bool DestroyIcon(IntPtr handle);


        public ui_NotifyIcon()
        {
            trayIcon = new NotifyIcon();
            trayIcon.Text = "WorkTimeTracker";
            trayIcon.ContextMenu = IconCreateMenu();
            trayIcon.Visible = true;
            //tupdateIcon = new System.Threading.Timer(new System.Threading.TimerCallback(IconUpdate), null, 1000, -1);
            IconUpdate(new object());
        }

        private ContextMenu IconCreateMenu()
        {
            ContextMenu contextMenu = new ContextMenu();
            contextMenu.MenuItems.Add(new MenuItem() { Text = "Settings" , Name = "settings" });
            contextMenu.MenuItems.Add(new MenuItem() { Text = "History", Name = "history" });
            contextMenu.MenuItems.Add("-");
            contextMenu.MenuItems.Add(new MenuItem() { Text = "Exit", Name = "exit" });
            return contextMenu;
        }

        private void IconUpdate(object state)
        {
            Dispatcher.CurrentDispatcher.Invoke(new System.Action(() =>
            {
                if (!WorkdayHandler.isWorkdayStarted())
                {
                    this.trayIcon.Icon = this.IconCreateSymbol( Color.Transparent, Color2Color(Config.cHead));
                }
                else
                {
                    Threshold threshold = Config.lThresholds.Find((Threshold item) => item.strName == "bad");
                    Threshold threshold1 = Config.lThresholds.Find((Threshold item) => item.strName == "good");
                    Threshold threshold2 = Config.lThresholds.Find((Threshold item) => item.strName == "medium");
                    if (WorkdayHandler.WorkdayPercent() < threshold2.iValue)
                    {
                        this.trayIcon.Icon = this.IconCreateSymbol(Color2Color(Config.cHead), Color2Color(threshold.cColor));
                    }
                    if ((WorkdayHandler.WorkdayPercent() < threshold2.iValue ? false : WorkdayHandler.WorkdayPercent() < threshold1.iValue))
                    {
                        this.trayIcon.Icon = this.IconCreateSymbol(Color2Color(Config.cHead), Color2Color(threshold2.cColor));
                    }
                    if ((WorkdayHandler.WorkdayPercent() <= threshold1.iValue ? false : WorkdayHandler.WorkdayPercent() < 100))
                    {
                        this.trayIcon.Icon = this.IconCreateSymbol(Color2Color(Config.cHead), Color2Color(threshold1.cColor));
                    }
                    if (WorkdayHandler.WorkdayPercent() == 100)
                    {
                        if (this.bIcon)
                        {
                            this.trayIcon.Icon = this.IconCreateSymbol(Color2Color(Config.cHead), Color2Color(threshold1.cColor));
                            this.bIcon = false;
                        }
                        else
                        {
                            this.trayIcon.Icon = this.IconCreateSymbol(Color2Color(threshold1.cColor), Color2Color(Config.cHead));
                            this.bIcon = true;
                        }
                    }
                }


            }));

            GC.Collect();
            GC.WaitForPendingFinalizers();
            //this.tupdateIcon.Change(1000, -1);
        }

        private Icon IconCreateSymbol(Color cHead, Color cClock)
        {
            Bitmap bitmap = IconChangeColor(BitmapImage2Bitmap(Config.imgHead), cHead);
            //Bitmap bitmap1 = IconChangeColor(BitmapImage2Bitmap(Config.imgClock), cClock);
            //Bitmap bitmap2 = new Bitmap(128, 128);
            //using (Graphics graphic = Graphics.FromImage(bitmap2))
            //{
            //    graphic.DrawImage(bitmap, 0, 0);
            //    graphic.DrawImage(bitmap1, 0, 0);
            //}
            Icon icon = Icon.FromHandle(bitmap.GetHicon());
            Icon icon1 = (Icon)icon.Clone();
            DestroyIcon(icon.Handle);
            return icon1;
        }

        private Bitmap IconChangeColor(Bitmap bmpTemp, Color targetColor)
        {
            for (int i = 0; i < bmpTemp.Width; i++)
            {
                for (int j = 0; j < bmpTemp.Height; j++)
                {
                    if (bmpTemp.GetPixel(i, j).A > 10)
                    {
                        bmpTemp.SetPixel(i, j, targetColor);
                    }
                }
            }
            return bmpTemp;
        }

        private Color Color2Color(System.Windows.Media.Color color)
        {
            return Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        private Bitmap BitmapImage2Bitmap(System.Windows.Media.Imaging.BitmapImage bitmapImage)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                System.Windows.Media.Imaging.BitmapEncoder enc = new System.Windows.Media.Imaging.BmpBitmapEncoder();
                enc.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                return new Bitmap(outStream);
            }
        }
        
    }
}
