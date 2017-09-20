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
                    xmlWriter.WriteAttributeString("name", lBreak.name);
                    xmlWriter.WriteAttributeString("enabled", lBreak.enabled.ToString());
                    xmlWriter.WriteAttributeString("start", lBreak.starttime.ToBinary().ToString());
                    xmlWriter.WriteAttributeString("duration", lBreak.duration.Ticks.ToString());
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("thresholds");
                foreach (Threshold lThreshold in Config.lThresholds)
                {
                    xmlWriter.WriteStartElement("threshold");
                    xmlWriter.WriteAttributeString("name", lThreshold.name);
                    xmlWriter.WriteAttributeString("value", lThreshold.value.ToString());
                    xmlWriter.WriteAttributeString("color", ColorToArgb(lThreshold.colour).ToString());
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("phrases");
                foreach (Subtitle lSubtitle in Config.lSubtitles)
                {
                    xmlWriter.WriteStartElement("phrase");
                    xmlWriter.WriteAttributeString("start", lSubtitle.rangestart.ToString());
                    xmlWriter.WriteAttributeString("end", lSubtitle.rangeend.ToString());
                    xmlWriter.WriteAttributeString("string", lSubtitle.subtitle);
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("history");
                foreach (Day lDay in Config.lDays)
                {
                    xmlWriter.WriteStartElement("day");
                    xmlWriter.WriteAttributeString("start", lDay.starttime.ToBinary().ToString());
                    xmlWriter.WriteAttributeString("end", lDay.endtime.ToBinary().ToString());
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
                                    lBreaks.Add(new Break(childNode1.Attributes["name"].Value, Convert.ToBoolean(childNode1.Attributes["enabled"].Value), TimeSpan.FromTicks(Convert.ToInt64(childNode1.Attributes["duration"].Value)), DateTime.FromBinary(Convert.ToInt64(childNode1.Attributes["start"].Value))));
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
                                    lDays.Add(new Day(DateTime.FromBinary(Convert.ToInt64(childNode2.Attributes["start"].Value)), DateTime.FromBinary(Convert.ToInt64(childNode2.Attributes["end"].Value))));
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
        #region Declaration
        private String _name;
        private Boolean _enabled;
        private TimeSpan _duration;
        private DateTime _starttime;
        #endregion

        #region Getter/Setter
        public String name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        public Boolean enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                _enabled = value;
            }
        }
        public TimeSpan duration
        {
            get
            {
                return _duration;
            }
            set
            {
                _duration = value;
            }
        }
        public DateTime durationasdt
        {
            get
            {
                return new DateTime().AddHours(_duration.Hours).AddMinutes(_duration.Minutes);
            }
            set
            {
                _duration = new TimeSpan(0, value.Hour, value.Minute, 0);
            }
        }
        public DateTime starttime
        {
            get
            {
                return _starttime;
            }
            set
            {
                _starttime = value;
            }
        }
        #endregion

        public Break(String name, Boolean enabled, TimeSpan duration, DateTime starttime )
        {
            DateTime asd = new DateTime().AddHours(_duration.Hours).AddMinutes(_duration.Minutes);
            _enabled = enabled;
            _starttime = starttime;
            _duration = duration;
            _name = name;
        }
    }

    [Serializable]
    public class Day
    {
        #region Declaration
        private DateTime _starttime;
        private DateTime _endtime;
        #endregion

        #region Setter/Getter
        public DateTime starttime
        {
            get
            {
                return _starttime;
            }
            set
            {
                _starttime = value;
            }
        }
        public DateTime endtime
        {
            get
            {
                return _endtime;
            }
            set
            {
                _endtime = value;
            }
        }
        public DateTime overtime
        {
            get
            {
                if (!_endtime.Equals(DateTime.MinValue))
                {
                    return worktime.Subtract(TimeSpan.FromMinutes(UserData.getWorkDuration()));
                }
                else
                {
                    return DateTime.MinValue;
                }
            }
        }
        public DateTime worktime
        {
            get
            {
                if (!_endtime.Equals(DateTime.MinValue))
                {
                    DateTime dateTime = _endtime;
                    foreach (Break lBreak in UserData.getBreaks())
                    {
                        if ((!lBreak.enabled || !(_starttime.TimeOfDay < lBreak.starttime.TimeOfDay) ? false : _endtime.TimeOfDay > lBreak.starttime.TimeOfDay))
                            dateTime = dateTime.Add(-lBreak.duration);
                    }
                    dateTime = dateTime.Add(new TimeSpan(_starttime.TimeOfDay.Hours, _starttime.TimeOfDay.Minutes, _starttime.TimeOfDay.Seconds));
                    return dateTime;
                }
                else
                {
                    return DateTime.MinValue;
                }
            }
        }
        #endregion

        public Day()
        {
        }

        public Day( DateTime StartTime, DateTime EndTime)
        {
            this._starttime = StartTime;
            this._endtime = EndTime;
        }
    }

    [Serializable]
    public class Subtitle
    {
        #region Declaration
        private Int32 _rangestart;
        private Int32 _rangeend;
        private String _subtitle;
        #endregion

        #region Getter/Setter
        public Int32 rangestart { get { return _rangestart; } set { _rangestart = value; } }
        public Int32 rangeend { get { return _rangeend; } set { _rangeend = value; } }
        public String subtitle { get { return _subtitle; } set { _subtitle = value; } }
        #endregion

        public Subtitle(Int32 rangestart, Int32 rangeend, String subtitle)
        {
            this._rangestart = rangestart;
            this._rangeend = rangeend;
            this._subtitle = subtitle;
        }
    }

    [Serializable]
    public class Threshold
    {
        #region Declaration
        private String _name;
        private Colour _color;
        private Int32 _value;
        #endregion

        #region Getter/Setter
        public Color color { get { return Color.FromArgb(_color.A, _color.R, _color.G, _color.B); } set { _color = value; } }
        public Colour colour { get { return _color; } set { _color = value; } }
        public String name { get { return _name; } set { _name = value; } }
        public Int32 value { get { return _value; } set { _value = value; } }
#endregion

        public Threshold(Color color, Int32 value, String name)
        {
            _color = color;
            _value = value;
            _name = name;
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

        public override string ToString()
        {
            return Color.FromArgb(A, R, G, B).ToString();
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