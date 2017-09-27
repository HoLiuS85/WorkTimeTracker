using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;

namespace WorkTimeTracker
{
    static class Helper
    {
        [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        private static extern bool DestroyIcon(IntPtr handle);

        // Get a random subtitle corresponding to workday progress by percent
        public static String getSubtitle(int percent)
        {
            List<Subtitle> lTemp = UserData.getSubtitles().FindAll((Subtitle item) => (percent < item.rangestart ? false : percent <= item.rangeend));
            return lTemp[(new Random()).Next(0, lTemp.Count)].subtitle;
        }

        // Get a color corresponding to workday progress by percent
        public static Colour getProgressColor(int percent)
        {
            int previous = 0;
            foreach (Threshold threshold in UserData.getThresholds())
            {
                if (percent >= previous && percent <= threshold.value)
                    return threshold.colour;

                previous = threshold.value;
            }
            
            return new Colour(Colors.Pink);
        }

        // Get Size/Resolution of the current screen
        public static Rect getScreenSize(System.Windows.Point mouseposition)
        {
            foreach (var screen in System.Windows.Forms.Screen.AllScreens)
            {
                if(mouseposition.X > screen.Bounds.Left && mouseposition.X < screen.Bounds.Right)
                    return new Rect(screen.WorkingArea.X, screen.WorkingArea.Y, screen.WorkingArea.Width, screen.WorkingArea.Height);
            }

            Rectangle ScreenSize = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;
            return new Rect(ScreenSize.X, ScreenSize.Y, ScreenSize.Width, ScreenSize.Height);
        }

        // Get the current position of the mouse
        public static System.Windows.Point getMousePosition(Window window)
        {
            System.Drawing.Point point = System.Windows.Forms.Control.MousePosition;
            Matrix transform = PresentationSource.FromVisual(window).CompositionTarget.TransformFromDevice;
            return transform.Transform(new System.Windows.Point(point.X, point.Y));
        }

        // Get tray icon that is created at runtime
        public static Icon getTrayIcon(Colour cHead, Colour cClock)
        {
            PointF pDpi = new PointF(Properties.Resources.icon_head_128.HorizontalResolution, Properties.Resources.icon_head_128.VerticalResolution);
            System.Windows.Point pSize = new System.Windows.Point(Properties.Resources.icon_head_128.Width, Properties.Resources.icon_head_128.Height);

            Bitmap bmpResult = new Bitmap((int)pSize.X, (int)pSize.Y);
            bmpResult.SetResolution(pDpi.X, pDpi.Y);

            using (Bitmap bmpHead = changeBitmapColor(Properties.Resources.icon_head_128, cHead))
            {
                bmpHead.SetResolution(pDpi.X, pDpi.Y);

                using (Bitmap bmpClock = changeBitmapColor(Properties.Resources.icon_clock_128, cClock))
                {
                    bmpClock.SetResolution(pDpi.X, pDpi.Y);

                    using (Graphics graphic = Graphics.FromImage(bmpResult))
                    {
                        graphic.DrawImage(bmpHead, 0, 0);
                        graphic.DrawImage(bmpClock, 0, 0);
                    }
                }
            }

            Icon icoTemp = Icon.FromHandle(bmpResult.GetHicon());
            Icon icoResult = (Icon)icoTemp.Clone();
            DestroyIcon(icoTemp.Handle);
            return icoResult;
        }
        
        // change non-transparent pixels of bitmap to color
        public static Bitmap changeBitmapColor(Bitmap bmpTemp, Colour targetColor)
        {
            for (int i = 0; i < bmpTemp.Width; i++)
            {
                for (int j = 0; j < bmpTemp.Height; j++)
                {
                    if (bmpTemp.GetPixel(i, j).A > 10)
                    {
                        bmpTemp.SetPixel(i, j, System.Drawing.Color.FromArgb(targetColor.A, targetColor.R, targetColor.G, targetColor.B));
                    }
                }
            }
            return bmpTemp;
        }

        // convert argb string (WinForms) to media color (wpf)
        public static System.Windows.Media.Color ColorFromArgb(int argb)
        {
            byte[] bytes = BitConverter.GetBytes(argb);
            return System.Windows.Media.Color.FromArgb(bytes[3], bytes[2], bytes[1], bytes[0]);
        }
    }
}
