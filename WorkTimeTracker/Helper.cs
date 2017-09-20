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
            Threshold thresholdBad = UserData.getThresholds().Find((Threshold item) => item.name == "bad");
            Threshold thresholdMedium = UserData.getThresholds().Find((Threshold item) => item.name == "medium");
            Threshold thresholdGood = UserData.getThresholds().Find((Threshold item) => item.name == "good");

            if (percent < thresholdMedium.value)
                return thresholdBad.colour;

            if (percent > thresholdMedium.value && percent < thresholdGood.value)
                return thresholdMedium.colour;

            if (percent > thresholdGood.value)
                return thresholdGood.colour;

            return new Colour(Colors.LightGreen);
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
            Bitmap bmpResult = new Bitmap(152 , 152);

            using (Bitmap bmpHead = changeBitmapColor(Properties.Resources.icon_head_128, cHead))
            {
                using (Bitmap bmpClock = changeBitmapColor(Properties.Resources.icon_clock_128, cClock))
                {
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
    }
}
