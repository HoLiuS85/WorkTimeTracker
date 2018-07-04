using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Media;
using System.Xml;

namespace WorkTimeTracker
{
    public static class Config
    {
        private static String confFile = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + Path.DirectorySeparatorChar + Assembly.GetEntryAssembly().GetName().Name + ".xml";
        public static List<ConfValue> lConfValues = new List<ConfValue>()
        {
            new ConfValue("listThresholds","AAEAAAD/////AQAAAAAAAAAMAgAAAEZXb3JrVGltZVRyYWNrZXIsIFZlcnNpb249MS4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsBAEAAACGAVN5c3RlbS5Db2xsZWN0aW9ucy5HZW5lcmljLkxpc3RgMVtbV29ya1RpbWVUcmFja2VyLlRocmVzaG9sZCwgV29ya1RpbWVUcmFja2VyLCBWZXJzaW9uPTEuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49bnVsbF1dAwAAAAZfaXRlbXMFX3NpemUIX3ZlcnNpb24EAAAbV29ya1RpbWVUcmFja2VyLlRocmVzaG9sZFtdAgAAAAgICQMAAAADAAAAAAAAAAcDAAAAAAEAAAADAAAABBlXb3JrVGltZVRyYWNrZXIuVGhyZXNob2xkAgAAAAkEAAAACQUAAAAJBgAAAAUEAAAAGVdvcmtUaW1lVHJhY2tlci5UaHJlc2hvbGQDAAAABV9uYW1lBl9jb2xvcgZfdmFsdWUBBAAWV29ya1RpbWVUcmFja2VyLkNvbG91cgIAAAAIAgAAAAYHAAAAA2JhZAX4////FldvcmtUaW1lVHJhY2tlci5Db2xvdXIEAAAAAUEBUgFHAUIAAAAAAgICAgIAAAD//wAAAAAAAAEFAAAABAAAAAYJAAAABm1lZGl1bQH2////+P//////gEAyAAAAAQYAAAAEAAAABgsAAAAEZ29vZAH0////+P////9BwRxQAAAACw=="),
            new ConfValue("listBreaks","AAEAAAD/////AQAAAAAAAAAMAgAAAEZXb3JrVGltZVRyYWNrZXIsIFZlcnNpb249MS4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsBAEAAACCAVN5c3RlbS5Db2xsZWN0aW9ucy5HZW5lcmljLkxpc3RgMVtbV29ya1RpbWVUcmFja2VyLkJyZWFrLCBXb3JrVGltZVRyYWNrZXIsIFZlcnNpb249MS4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsXV0DAAAABl9pdGVtcwVfc2l6ZQhfdmVyc2lvbgQAABdXb3JrVGltZVRyYWNrZXIuQnJlYWtbXQIAAAAICAkDAAAAAwAAAAAAAAAHAwAAAAABAAAAAwAAAAQVV29ya1RpbWVUcmFja2VyLkJyZWFrAgAAAAkEAAAACQUAAAAJBgAAAAUEAAAAFVdvcmtUaW1lVHJhY2tlci5CcmVhawQAAAAFX25hbWUIX2VuYWJsZWQJX2R1cmF0aW9uCl9zdGFydHRpbWUBAAAAAQwNAgAAAAYHAAAACWJyZWFrZmFzdAEAGnEYAgAAAACQdCzQA9SIAQUAAAAEAAAABggAAAAFbHVuY2gBADTiMAQAAAAAyMFR6QPUiAEGAAAABAAAAAYJAAAABmRpbm5lcgEAGnEYAgAAAAA4XJwbBNSICw=="),
            new ConfValue("listSubtitles","AAEAAAD/////AQAAAAAAAAAMAgAAAEZXb3JrVGltZVRyYWNrZXIsIFZlcnNpb249MS4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsBAEAAACFAVN5c3RlbS5Db2xsZWN0aW9ucy5HZW5lcmljLkxpc3RgMVtbV29ya1RpbWVUcmFja2VyLlN1YnRpdGxlLCBXb3JrVGltZVRyYWNrZXIsIFZlcnNpb249MS4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsXV0DAAAABl9pdGVtcwVfc2l6ZQhfdmVyc2lvbgQAABpXb3JrVGltZVRyYWNrZXIuU3VidGl0bGVbXQIAAAAICAkDAAAACQAAAAAAAAAHAwAAAAABAAAACQAAAAQYV29ya1RpbWVUcmFja2VyLlN1YnRpdGxlAgAAAAkEAAAACQUAAAAJBgAAAAkHAAAACQgAAAAJCQAAAAkKAAAACQsAAAAJDAAAAAUEAAAAGFdvcmtUaW1lVHJhY2tlci5TdWJ0aXRsZQMAAAALX3Jhbmdlc3RhcnQJX3JhbmdlZW5kCV9zdWJ0aXRsZQAAAQgIAgAAAAAAAAAKAAAABg0AAAATR29vZCBtb3JuaW5nLCBjdW50IQEFAAAABAAAAAAAAAAKAAAABg4AAAAQU3RpbGwgZHJ1bmssIGVoPwEGAAAABAAAAAoAAAAyAAAABg8AAAARWW91J3JlIGRvb21lZC4uLi4BBwAAAAQAAAAKAAAAMgAAAAYQAAAAEkV0ZXJuaXR5IGF3YWl0cy4uLgEIAAAABAAAADIAAABaAAAABhEAAAAZQmVlciBpcyBnZXR0aW5nIGNsb3Nlci4uLgEJAAAABAAAADIAAABaAAAABhIAAAAWSnV1dXVzdCBhIGJpdCBtb3JlIG5vdwEKAAAABAAAAFoAAABkAAAABhMAAAAJRU5EUFNVUlQhAQsAAAAEAAAAZAAAAGQAAAAGFAAAABFGdWNrIG9mZiBhbHJlYWR5IQEMAAAABAAAAGQAAABkAAAABhUAAAAPR2V0IGxvc3QsIGN1bnQhCw=="),
            new ConfValue("listDays","AAEAAAD/////AQAAAAAAAAAMAgAAAEZXb3JrVGltZVRyYWNrZXIsIFZlcnNpb249MS4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsBAEAAACAAVN5c3RlbS5Db2xsZWN0aW9ucy5HZW5lcmljLkxpc3RgMVtbV29ya1RpbWVUcmFja2VyLkRheSwgV29ya1RpbWVUcmFja2VyLCBWZXJzaW9uPTEuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49bnVsbF1dAwAAAAZfaXRlbXMFX3NpemUIX3ZlcnNpb24EAAAVV29ya1RpbWVUcmFja2VyLkRheVtdAgAAAAgICQMAAAAAAAAAAAAAAAcDAAAAAAEAAAAAAAAABBNXb3JrVGltZVRyYWNrZXIuRGF5AgAAAAs="),
            new ConfValue("intWorkDuration","456"),
            new ConfValue("intInterval","1000"),
            new ConfValue("dtLastLockTime",DateTime.MinValue.ToString()),
            new ConfValue("dtWorkStartTime",DateTime.MinValue.ToString()),
            new ConfValue("colorTrayIcon","AAEAAAD/////AQAAAAAAAAAMAgAAAEZXb3JrVGltZVRyYWNrZXIsIFZlcnNpb249MS4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsBQEAAAAWV29ya1RpbWVUcmFja2VyLkNvbG91cgQAAAABQQFSAUcBQgAAAAACAgICAgAAAP////8L")
        };

        static Config()
        {
            if (File.Exists(confFile))
                Read();
            else
                Save();
        }

        public static void Save()
        {
            XmlDocument xmlDocument = new XmlDocument();
            try { xmlDocument.Load(@confFile); }
            catch { }

            foreach (ConfValue cv in lConfValues)
            {
                XmlElement userconfigNode = xmlDocument.SelectSingleNode("/userconfig") as XmlElement;
                if (userconfigNode == null)
                {
                    userconfigNode = xmlDocument.CreateElement("userconfig");
                    xmlDocument.AppendChild(userconfigNode);
                }

                XmlElement valueNode = xmlDocument.SelectSingleNode("/userconfig/" + cv.name) as XmlElement;
                if (valueNode == null)
                {
                    valueNode = xmlDocument.CreateElement(cv.name);
                    userconfigNode.AppendChild(valueNode);
                }
                valueNode.RemoveAll();

                valueNode.SetAttribute("value", cv.value);
            }

            xmlDocument.Save(@confFile);
        }

        public static void Read()
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(@confFile);

            foreach (ConfValue cv in lConfValues)
            {
                String temp = xmlDocument.DocumentElement.SelectSingleNode("/userconfig/" + cv.name).Attributes["value"].Value;

                if (temp != String.Empty)
                    cv.value = temp;
            }
        }        
    }

    [Serializable]
    public struct MARGINS
    {
        public int Left;
        public int Right;
        public int Top;
        public int Bottom;
    }

    [Serializable]
    public class ConfValue
    {
        #region Declaration
        private String _name;
        private String _value;
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
        public String value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }
        #endregion

        public ConfValue(String name,String value)
        {
            _name = name;
            _value = value;
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
        public String overtime
        {
            get
            {
                try
                {
                    //Convert to string to get "-" formatting right (TimeSpan requires stupidity)
                    TimeSpan result = TimeSpan.FromMinutes(worktime.TotalMinutes - UserData.getWorkDuration());

                    return result.ToString((result < TimeSpan.Zero ? "\\-" : "' '") + "hh\\:mm");
                }
                catch
                { return TimeSpan.Zero.ToString(@"hh\:mm"); }
            }
        }
        public TimeSpan worktime
        {
            get
            {
                try
                {
                    DateTime endtime = _endtime;
                    foreach (Break lBreak in UserData.getBreaks())
                    {
                        if (lBreak.enabled && _starttime.TimeOfDay < lBreak.starttime.TimeOfDay && _endtime.TimeOfDay > lBreak.starttime.TimeOfDay)
                            endtime = endtime.Subtract(lBreak.duration);
                    }
                    return endtime - _starttime;
                }
                catch
                { return TimeSpan.Zero; }
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
        private Colour _color;
        private Int32 _value;
        #endregion

        #region Getter/Setter
        public Color color { get { return Color.FromArgb(_color.A, _color.R, _color.G, _color.B); } set { _color = value; } }
        public Colour colour { get { return _color; } set { _color = value; } }
        public Int32 value { get { return _value; } set { _value = value; } }
#endregion

        public Threshold(Color color, Int32 value)
        {
            _color = color;
            _value = value;
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