using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;

namespace WorkTimeTracker
{
    public static class Config
	{
        public static int iWorkDuration;
        public static int iCalcInterval;
        public static List<Threshold> lThresholds;
        public static List<Subtitle> lSubtitles;
        public static List<Break> lBreaks;
        public static List<Day> lDays;
        public static DateTime dtWorkStartTime;
        public static DateTime dtWorkEndTime;
        public static TimeSpan tsWorkTimeRemaining;
        public static TimeSpan tsWorkTimeElapsed;
        public static BitmapImage imgHead;
        public static BitmapImage imgClock;
        public static Color cHead;

        public static OpenForm openForm;

        static Config()
        {
            lThresholds = new List<Threshold>();
            lSubtitles = new List<Subtitle>();
            lBreaks = new List<Break>();
            lDays = new List<Day>();
        }

        public static void Export()
        {
            using (XmlWriter xmlWriter = XmlWriter.Create("WorkTimeTracker.xml", new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "\t",
                NewLineChars = "\r\n",
                NewLineHandling = NewLineHandling.Replace
            }))
            {
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("worktimetracker");
                xmlWriter.WriteAttributeString("interval", Config.iCalcInterval.ToString());
                xmlWriter.WriteAttributeString("workduration", Config.iWorkDuration.ToString());
                xmlWriter.WriteAttributeString("headcolor", ColorToArgb(Config.cHead).ToString());
                xmlWriter.WriteStartElement("breaks");
                foreach (Break lBreak in Config.lBreaks)
                {
                    xmlWriter.WriteStartElement("break");
                    xmlWriter.WriteAttributeString("name", lBreak.strName);
                    xmlWriter.WriteAttributeString("enabled", lBreak.isEnabled.ToString());
                    xmlWriter.WriteAttributeString("start", lBreak.dtStartTime.ToBinary().ToString());
                    xmlWriter.WriteAttributeString("duration", lBreak.tsDuration.Ticks.ToString());
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("thresholds");
                foreach (Threshold lThreshold in Config.lThresholds)
                {
                    xmlWriter.WriteStartElement("threshold");
                    xmlWriter.WriteAttributeString("name", lThreshold.strName);
                    xmlWriter.WriteAttributeString("value", lThreshold.iValue.ToString());
                    xmlWriter.WriteAttributeString("color", ColorToArgb(lThreshold.cColor).ToString());
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("phrases");
                foreach (Subtitle lSubtitle in Config.lSubtitles)
                {
                    xmlWriter.WriteStartElement("phrase");
                    xmlWriter.WriteAttributeString("start", lSubtitle.iRangeStart.ToString());
                    xmlWriter.WriteAttributeString("end", lSubtitle.iRangeEnd.ToString());
                    xmlWriter.WriteAttributeString("string", lSubtitle.strSubtitle);
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("history");
                foreach (Day lDay in Config.lDays)
                {
                    xmlWriter.WriteStartElement("day");
                    xmlWriter.WriteAttributeString("start", lDay.dtStartTime.ToBinary().ToString());
                    xmlWriter.WriteAttributeString("end", lDay.dtEndTime.ToBinary().ToString());
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();
            }
        }

        public static void Import()
        {
            imgClock = new BitmapImage(new Uri(@"pack://application:,,,/res/icon_clock_128.png"));
            imgHead = new BitmapImage(new Uri(@"pack://application:,,,/res/icon_head_128.png"));

            foreach (XmlNode childNode in OpenConfigFile().ChildNodes)
            {
                if (childNode.Name.ToLower().Equals("worktimetracker"))
                {
                    iCalcInterval = Convert.ToInt32(childNode.Attributes["interval"].Value);
                    iWorkDuration = Convert.ToInt32(childNode.Attributes["workduration"].Value);
                    cHead = ColorFromArgb(Convert.ToInt32(childNode.Attributes["headcolor"].Value));
                    foreach (XmlNode xmlNodes in childNode.ChildNodes)
                    {
                        string lower = xmlNodes.Name.ToLower();
                        if (lower == "breaks")
                        {
                            foreach (XmlNode childNode1 in xmlNodes.ChildNodes)
                            {
                                if (childNode1.Name.ToLower().Equals("break"))
                                {
                                    lBreaks.Add(new Break(Convert.ToBoolean(childNode1.Attributes["enabled"].Value), DateTime.FromBinary(Convert.ToInt64(childNode1.Attributes["start"].Value)), TimeSpan.FromTicks(Convert.ToInt64(childNode1.Attributes["duration"].Value)), childNode1.Attributes["name"].Value));
                                }
                            }
                        }
                        else if (lower == "thresholds")
                        {
                            foreach (XmlNode xmlNodes1 in xmlNodes.ChildNodes)
                            {
                                if (xmlNodes1.Name.ToLower().Equals("threshold"))
                                {
                                    lThresholds.Add(new Threshold(ColorFromArgb(Convert.ToInt32(xmlNodes1.Attributes["color"].Value)), Convert.ToInt32(xmlNodes1.Attributes["value"].Value), xmlNodes1.Attributes["name"].Value));
                                }
                            }
                        }
                        else if (lower == "history")
                        {
                            foreach (XmlNode childNode2 in xmlNodes.ChildNodes)
                            {
                                if (childNode2.Name.ToLower().Equals("day"))
                                {
                                    lDays.Add(new Day(lDays.Count + 1, DateTime.FromBinary(Convert.ToInt64(childNode2.Attributes["start"].Value)), DateTime.FromBinary(Convert.ToInt64(childNode2.Attributes["end"].Value))));
                                }
                            }
                        }
                        else if (lower == "phrases")
                        {
                            foreach (XmlNode xmlNodes2 in xmlNodes.ChildNodes)
                            {
                                if (xmlNodes2.Name.ToLower().Equals("phrase"))
                                {
                                    lSubtitles.Add(new Subtitle(Convert.ToInt32(xmlNodes2.Attributes["start"].Value), Convert.ToInt32(xmlNodes2.Attributes["end"].Value), xmlNodes2.Attributes["string"].Value));
                                }
                            }
                        }
                    }
                }
            }
        }

        private static XmlDocument OpenConfigFile()
        {
            XmlDocument xmlDocument = new XmlDocument();
            string directoryName = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            char directorySeparatorChar = Path.DirectorySeparatorChar;
            xmlDocument.Load(string.Concat(directoryName, directorySeparatorChar.ToString(), Assembly.GetEntryAssembly().GetName().Name, ".xml"));
            return xmlDocument;
        }

        private static int ColorToArgb(Color color)
        {
            byte[] bytes = new byte[] { color.A, color.R, color.G, color.B };
            return BitConverter.ToInt32(bytes, 0);
        }

        private static Color ColorFromArgb(int argb)
        {
            byte[] bytes = BitConverter.GetBytes(argb);
            return Color.FromArgb(bytes[3], bytes[2], bytes[1], bytes[0]);
        }

    }


    [Serializable]
    public class Break
    {
        public string strName { get; set; }
        public bool isEnabled { get; set; }
        public TimeSpan tsDuration { get; set; }
        public DateTime dtStartTime { get; set; }

        public Break(bool enabled, DateTime starttime, TimeSpan duration, string name)
        {
            this.isEnabled = enabled;
            this.dtStartTime = starttime;
            this.tsDuration = duration;
            this.strName = name;
        }
    }

    [Serializable]
    public class Day
    {
        public DateTime dtStartTime { get; set; }
        public DateTime dtEndTime { get; set; }
        public TimeSpan tsOvertime
        {
            get
            {
                if (!dtEndTime.Equals(DateTime.MinValue))
                {
                    return tsWorktime.Subtract(TimeSpan.FromMinutes(UserData.getWorkDuration()));
                }
                else
                {
                    return TimeSpan.FromTicks((long)0);
                }
            }
        }
        public TimeSpan tsWorktime
        {
            get
            {
                if (!dtEndTime.Equals(DateTime.MinValue))
                {
                    DateTime dateTime = dtEndTime;
                    foreach (Break lBreak in UserData.getBreaks())
                    {
                        if ((!lBreak.isEnabled || !(dtStartTime.TimeOfDay < lBreak.dtStartTime.TimeOfDay) ? false : dtEndTime.TimeOfDay > lBreak.dtStartTime.TimeOfDay))
                            dateTime = dateTime.Add(-lBreak.tsDuration);
                    }
                    return dateTime - dtStartTime;
                }
                else
                {
                    return TimeSpan.Zero;
                }
            }
        }
        public int iID { get; set; }

        public Day()
        {
        }

        public Day(int ID, DateTime StartTime, DateTime EndTime)
        {
            this.iID = ID;
            this.dtStartTime = StartTime;
            this.dtEndTime = EndTime;
        }
    }

    [Serializable]
    public class Subtitle
    {
        public int iRangeStart;
        public int iRangeEnd;
        public string strSubtitle;

        public Subtitle(int RangeStart, int RangeEnd, string Subtitle)
        {
            this.iRangeStart = RangeStart;
            this.iRangeEnd = RangeEnd;
            this.strSubtitle = Subtitle;
        }
    }

    [Serializable]
    public class Threshold
    {
        public string strName;
        public Colour cColor;
        public int iValue;

        public Threshold(Color color, int value, string name)
        {
            this.cColor = color;
            this.iValue = value;
            this.strName = name;
        }
    }

    [Serializable]
    public struct Colour
    {
        public byte A;
        public byte R;
        public byte G;
        public byte B;

        public Colour(byte a, byte r, byte g, byte b)
        {
            A = a;
            R = r;
            G = g;
            B = b;
        }

        public Colour(System.Windows.Media.Color color)
            : this(color.A, color.R, color.G, color.B)
        {
        }

        public Colour(System.Drawing.Color color)
            : this(color.A, color.R, color.G, color.B)
        {
        }

        public static implicit operator Colour(Color color)
        {
            return new Colour(color);
        }

        public static implicit operator Color(Colour colour)
        {
            return Color.FromArgb(colour.A, colour.R, colour.G, colour.B);
        }
    }
    
    [Flags]
    public enum OpenForm
    {
        None,
        StartModify,
        End
    }
}