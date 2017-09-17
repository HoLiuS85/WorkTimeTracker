using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace WorkTimeTracker
{
    static class UserData
    {
        private static Colour _cTrayIcon;
        private static Int32 _iInterval;
        private static Int32 _iWorkDuration;
        private static DateTime _dtWorkEndTime;
        private static DateTime _dtWorkStartTime;
        private static TimeSpan _tsWorkTimeElapsed;
        private static TimeSpan _tsWorkTimeRemaining;
        private static List<Day> _lDays;
        private static List<Break> _lBreaks;
        private static List<Subtitle> _lSubtitles;
        private static List<Threshold> _lThresholds;


        public static void setTrayIconColor(Colour cTrayIcon)
        {
            _cTrayIcon = cTrayIcon;
            Properties.Settings.Default.colorTrayIcon = SerializeObject(cTrayIcon);
            Properties.Settings.Default.Save();
        }
        public static Colour getTrayIconColor()
        {
            return _cTrayIcon;
        }
        
        public static void setInterval(Int32 iInterval)
        {
            _iInterval = iInterval;
            Properties.Settings.Default.intInterval = iInterval;
            Properties.Settings.Default.Save();
        }
        public static Int32 getInterval()
        {
            return _iInterval;
        }

        public static void setWorkDuration(Int32 iWorkDuration)
        {
            _iWorkDuration = iWorkDuration;
            Properties.Settings.Default.intWorkDuration = iWorkDuration;
            Properties.Settings.Default.Save();
        }
        public static Int32 getWorkDuration()
        {
            return _iWorkDuration;
        }

        public static void setWorkTimeEnd(DateTime dtWorkEndTime)
        {
            _dtWorkEndTime = dtWorkEndTime;
            Properties.Settings.Default.dtWorkEndTime = dtWorkEndTime;
            Properties.Settings.Default.Save();
        }
        public static DateTime getWorkTimeEnd()
        {
            return _dtWorkEndTime;
        }

        public static void setWorkTimeStart(DateTime dtWorkStartTime)
        {
            _dtWorkStartTime = dtWorkStartTime;
            Properties.Settings.Default.dtWorkStartTime = dtWorkStartTime;
            Properties.Settings.Default.Save();
        }
        public static DateTime getWorkTimeStart()
        {
            return _dtWorkStartTime;
        }

        public static void setWorkTimeElapsed(TimeSpan tsWorkTimeElapsed)
        {
            _tsWorkTimeElapsed = tsWorkTimeElapsed;
            Properties.Settings.Default.tsWorkTimeElapsed = tsWorkTimeElapsed;
            Properties.Settings.Default.Save();
        }
        public static TimeSpan getWorkTimeElapsed()
        {
            return _tsWorkTimeElapsed;
        }

        public static void setWorkTimeRemaining(TimeSpan tsWorkTimeRemaining)
        {
            _tsWorkTimeRemaining = tsWorkTimeRemaining;
            Properties.Settings.Default.tsWorkTimeRemaining = tsWorkTimeRemaining;
            Properties.Settings.Default.Save();
        }
        public static TimeSpan getWorkTimeRemaining()
        {
            return _tsWorkTimeRemaining;
        }

        public static void setDays(List<Day> lDays)
        {
            _lDays = lDays;
            Properties.Settings.Default.listDays = SerializeObject(lDays);
            Properties.Settings.Default.Save();
        }
        public static List<Day> getDays()
        {
            return _lDays;
        }

        public static void setBreaks(List<Break> lBreaks)
        {
            _lBreaks = lBreaks;
            Properties.Settings.Default.listBreaks = SerializeObject(lBreaks);
            Properties.Settings.Default.Save();
        }
        public static List<Break> getBreaks()
        {
            return _lBreaks;
        }

        public static void setSubtitles(List<Subtitle> lSubtitles)
        {
            _lSubtitles = lSubtitles;
            Properties.Settings.Default.listSubtitles = SerializeObject(lSubtitles);
            Properties.Settings.Default.Save();
        }
        public static List<Subtitle> getSubtitles()
        {
            return _lSubtitles;
        }

        public static void setThresholds(List<Threshold> lThresholds)
        {
            _lThresholds = lThresholds;
            Properties.Settings.Default.listThresholds = SerializeObject(lThresholds);
            Properties.Settings.Default.Save();
        }
        public static List<Threshold> getThresholds()
        {
            return _lThresholds;
        }

        public static String SerializeObject(object list)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, list);
                ms.Position = 0;
                byte[] buffer = new byte[(int)ms.Length];
                ms.Read(buffer, 0, buffer.Length);
                return Convert.ToBase64String(buffer);
            }
        }

        public static object DeserializeObject(String base64String)
        {
            byte[] bytes = Convert.FromBase64String(base64String);
            using (MemoryStream ms = new MemoryStream(bytes, 0, bytes.Length))
            {
                ms.Write(bytes, 0, bytes.Length);
                ms.Position = 0;
                return new BinaryFormatter().Deserialize(ms);
            }

        }

        public static void Import()
        {
            _cTrayIcon = (Colour)DeserializeObject(Properties.Settings.Default.colorTrayIcon);
            _iInterval = Properties.Settings.Default.intInterval;
            _iWorkDuration = Properties.Settings.Default.intWorkDuration;
            _dtWorkEndTime = Properties.Settings.Default.dtWorkEndTime;
            _dtWorkStartTime = Properties.Settings.Default.dtWorkStartTime;
            _tsWorkTimeElapsed = Properties.Settings.Default.tsWorkTimeElapsed;
            _tsWorkTimeRemaining = Properties.Settings.Default.tsWorkTimeRemaining;
            _lDays = DeserializeObject(Properties.Settings.Default.listDays) as List<Day>;
            _lBreaks = DeserializeObject(Properties.Settings.Default.listBreaks) as List<Break>;
            _lSubtitles = DeserializeObject(Properties.Settings.Default.listSubtitles) as List<Subtitle>;
            _lThresholds = DeserializeObject(Properties.Settings.Default.listThresholds) as List<Threshold>;
        }

        public static void ExportOldConfig()
        {
            //setTrayIconColor(Config.cHead);
            //setInterval(Config.iCalcInterval);
            //setWorkDuration(Config.iWorkDuration);
            //setWorkTimeEnd(Config.dtWorkEndTime);
            //setWorkTimeStart(Config.dtWorkStartTime);
            //setWorkTimeElapsed(Config.tsWorkTimeElapsed);
            //setWorkTimeRemaining(Config.tsWorkTimeRemaining);
            //setThresholds(Config.lThresholds);
            //setSubtitles(Config.lSubtitles);
            //setBreaks(Config.lBreaks);
            //setDays(Config.lDays);
        }
    }
}
