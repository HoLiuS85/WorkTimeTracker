using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WorkTimeTracker
{
    static class Helper
    {

        public static String GetSubtitle(int percent)
        {
            List<Subtitle> subtitles = Config.lSubtitles.FindAll((Subtitle item) => (percent < item.iRangeStart ? false : percent <= item.iRangeEnd));
            return subtitles[(new Random()).Next(0, subtitles.Count)].strSubtitle;
        }

        public static SolidColorBrush GetProgressColor(int percent)
        {
            Threshold thresholdBad = Config.lThresholds.Find((Threshold item) => item.strName == "bad");
            Threshold thresholdMedium = Config.lThresholds.Find((Threshold item) => item.strName == "medium");
            Threshold thresholdGood = Config.lThresholds.Find((Threshold item) => item.strName == "good");

            if (percent < thresholdMedium.iValue)
                return new SolidColorBrush(thresholdBad.cColor);

            if (percent > thresholdMedium.iValue && percent < thresholdGood.iValue)
                return new SolidColorBrush(thresholdMedium.cColor);

            if (percent > thresholdGood.iValue)
                return new SolidColorBrush(thresholdGood.cColor);

            return new SolidColorBrush(Colors.LightGreen);
        }

        public static Rect GetScreenSize()
        {
            System.Drawing.Rectangle ScreenSize = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;
            
            return new Rect(ScreenSize.Left, ScreenSize.Top, ScreenSize.Width, ScreenSize.Height);
        }

        public static Point GetMousePosition(Window window)
        {
            System.Drawing.Point point = System.Windows.Forms.Control.MousePosition;
            Matrix transform = PresentationSource.FromVisual(window).CompositionTarget.TransformFromDevice;
            return transform.Transform(new Point(point.X, point.Y));
        }
    }
}
