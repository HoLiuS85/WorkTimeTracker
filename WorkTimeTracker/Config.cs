using System;
using System.Windows.Media;

namespace WorkTimeTracker
{
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
                    TimeSpan result = _endtime - (_starttime.AddMinutes(UserData.getWorkDuration()));

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
                            endtime = endtime.Add(-lBreak.duration);
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