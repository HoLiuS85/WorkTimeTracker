using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Media;
using System.Xml;

namespace WorkTimeTracker
{
    static class UserData
    {
        private static OpenForm _ofWindow;
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

        public static void setOpenWindow(OpenForm ofWindow)
        {
            _ofWindow = ofWindow;
        }
        public static OpenForm getOpenWindow()
        {
            return _ofWindow;
        }

        public static void setTrayIconColor(Colour cTrayIcon)
        {
            _cTrayIcon = cTrayIcon;
            ConfigHandler.lConfValues.Find(x => x.name == "colorTrayIcon").value = SerializeObject(cTrayIcon);
            ConfigHandler.Save();
        }
        public static Colour getTrayIconColor()
        {
            return _cTrayIcon;
        }

        public static void setInterval(Int32 iInterval)
        {
            _iInterval = iInterval;
            ConfigHandler.lConfValues.Find(x => x.name == "intInterval").value = iInterval.ToString();
            ConfigHandler.Save();
        }
        public static Int32 getInterval()
        {
            return _iInterval;
        }

        public static void setWorkDuration(Int32 iWorkDuration)
        {
            _iWorkDuration = iWorkDuration;
            ConfigHandler.lConfValues.Find(x => x.name == "intWorkDuration").value = iWorkDuration.ToString();
            ConfigHandler.Save();
        }
        public static Int32 getWorkDuration()
        {
            return _iWorkDuration;
        }

        public static void setWorkTimeEnd(DateTime dtWorkEndTime)
        {
            _dtWorkEndTime = dtWorkEndTime;
            ConfigHandler.lConfValues.Find(x => x.name == "dtWorkEndTime").value = dtWorkEndTime.ToString();
            ConfigHandler.Save();
        }
        public static DateTime getWorkTimeEnd()
        {
            if (_dtWorkEndTime != null)
                return _dtWorkEndTime;
            else
                return DateTime.MinValue;
        }

        public static void setWorkTimeStart(DateTime dtWorkStartTime)
        {
            _dtWorkStartTime = dtWorkStartTime;
            ConfigHandler.lConfValues.Find(x => x.name == "dtWorkStartTime").value = dtWorkStartTime.ToString();
            ConfigHandler.Save();
        }
        public static DateTime getWorkTimeStart()
        {
            if (_dtWorkEndTime != null)
                return _dtWorkStartTime;
            else
                return DateTime.MinValue;
        }

        public static void setWorkTimeElapsed(TimeSpan tsWorkTimeElapsed)
        {
            _tsWorkTimeElapsed = tsWorkTimeElapsed;
            ConfigHandler.lConfValues.Find(x => x.name == "tsWorkTimeElapsed").value = tsWorkTimeElapsed.ToString();
            ConfigHandler.Save();
        }
        public static TimeSpan getWorkTimeElapsed()
        {
            return _tsWorkTimeElapsed;
        }

        public static void setWorkTimeRemaining(TimeSpan tsWorkTimeRemaining)
        {
            _tsWorkTimeRemaining = tsWorkTimeRemaining;
            ConfigHandler.lConfValues.Find(x => x.name == "tsWorkTimeRemaining").value = tsWorkTimeRemaining.ToString();
            ConfigHandler.Save();
        }
        public static TimeSpan getWorkTimeRemaining()
        {
            return _tsWorkTimeRemaining;
        }

        public static void setDays(List<Day> lDays)
        {
            _lDays = lDays;
            ConfigHandler.lConfValues.Find(x => x.name == "listDays").value = SerializeObject(lDays);
            ConfigHandler.Save();
        }
        public static List<Day> getDays()
        {
            return _lDays.OrderBy(x => x.starttime).ToList();
        }

        public static void setBreaks(List<Break> lBreaks)
        {
            _lBreaks = lBreaks;
            ConfigHandler.lConfValues.Find(x => x.name == "listBreaks").value = SerializeObject(lBreaks);
            ConfigHandler.Save();
        }
        public static List<Break> getBreaks()
        {
            return _lBreaks.OrderBy(x => x.starttime).ToList();
        }

        public static void setSubtitles(List<Subtitle> lSubtitles)
        {
            _lSubtitles = lSubtitles;
            ConfigHandler.lConfValues.Find(x => x.name == "listSubtitles").value = SerializeObject(lSubtitles);
            ConfigHandler.Save();
        }
        public static List<Subtitle> getSubtitles()
        {
            return _lSubtitles.OrderBy(x => x.rangestart).ToList(); ;
        }

        public static void setThresholds(List<Threshold> lThresholds)
        {
            _lThresholds = lThresholds;
            ConfigHandler.lConfValues.Find(x => x.name == "listThresholds").value = SerializeObject(lThresholds);
            ConfigHandler.Save();
        }
        public static List<Threshold> getThresholds()
        {
            return _lThresholds.OrderBy(x => x.value).ToList();
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

        public static void ReadConfig()
        {
            _cTrayIcon = (Colour)DeserializeObject(ConfigHandler.lConfValues.Find(x => x.name == "colorTrayIcon").value);
            _iInterval = Convert.ToInt32(ConfigHandler.lConfValues.Find(x => x.name == "intInterval").value);
            _iWorkDuration = Convert.ToInt32(ConfigHandler.lConfValues.Find(x => x.name == "intWorkDuration").value);
            _dtWorkEndTime = DateTime.Parse(ConfigHandler.lConfValues.Find(x => x.name == "dtWorkEndTime").value);
            _dtWorkStartTime = DateTime.Parse(ConfigHandler.lConfValues.Find(x => x.name == "dtWorkStartTime").value);
            _tsWorkTimeElapsed = TimeSpan.Parse(ConfigHandler.lConfValues.Find(x => x.name == "tsWorkTimeElapsed").value);
            _tsWorkTimeRemaining = TimeSpan.Parse(ConfigHandler.lConfValues.Find(x => x.name == "tsWorkTimeRemaining").value);
            _lDays = DeserializeObject(ConfigHandler.lConfValues.Find(x => x.name == "listDays").value) as List<Day>;
            _lBreaks = DeserializeObject(ConfigHandler.lConfValues.Find(x => x.name == "listBreaks").value) as List<Break>;
            _lSubtitles = DeserializeObject(ConfigHandler.lConfValues.Find(x => x.name == "listSubtitles").value) as List<Subtitle>;
            _lThresholds = DeserializeObject(ConfigHandler.lConfValues.Find(x => x.name == "listThresholds").value) as List<Threshold>;
        }
    
        public static void ConfigToXML(string filename)
        {
            XmlDocument xmlDocument = new XmlDocument();
            try { xmlDocument.Load(@filename); }
            catch { }

            XmlElement worktimetrackerNode = xmlDocument.SelectSingleNode("/worktimetracker") as XmlElement;
            if (worktimetrackerNode == null)
            {
                worktimetrackerNode = xmlDocument.CreateElement("worktimetracker");
                xmlDocument.AppendChild(worktimetrackerNode);
            }

            XmlElement breaksNode = xmlDocument.SelectSingleNode("/worktimetracker/breaks") as XmlElement;
            if (breaksNode == null)
            {
                breaksNode = xmlDocument.CreateElement("breaks");
                worktimetrackerNode.AppendChild(breaksNode);
            }
            breaksNode.RemoveAll();

            XmlElement phrasesNode = xmlDocument.SelectSingleNode("/worktimetracker/phrases") as XmlElement;
            if (phrasesNode == null)
            {
                phrasesNode = xmlDocument.CreateElement("phrases");
                worktimetrackerNode.AppendChild(phrasesNode);
            }
            phrasesNode.RemoveAll();

            XmlElement thresholdsNode = xmlDocument.SelectSingleNode("/worktimetracker/thresholds") as XmlElement;
            if (thresholdsNode == null)
            {
                thresholdsNode = xmlDocument.CreateElement("thresholds");
                worktimetrackerNode.AppendChild(thresholdsNode);
            }
            thresholdsNode.RemoveAll();
            
            worktimetrackerNode.SetAttribute("interval", getInterval().ToString());
            worktimetrackerNode.SetAttribute("workduration", getWorkDuration().ToString());
            worktimetrackerNode.SetAttribute("headcolor", getTrayIconColor().ToString());

            foreach (Break lBreak in getBreaks())
            {
                XmlElement breakNode = xmlDocument.CreateElement("break");
                breakNode.SetAttribute("name", lBreak.name);
                breakNode.SetAttribute("enabled", lBreak.enabled.ToString());
                breakNode.SetAttribute("start", lBreak.starttime.ToString());
                breakNode.SetAttribute("duration", lBreak.duration.ToString());
                breaksNode.AppendChild(breakNode);
            }

            foreach (Subtitle lSubtitle in getSubtitles())
            {
                XmlElement phraseNode = xmlDocument.CreateElement("phrase");
                phraseNode.SetAttribute("start", lSubtitle.rangestart.ToString());
                phraseNode.SetAttribute("end", lSubtitle.rangeend.ToString());
                phraseNode.SetAttribute("string", lSubtitle.subtitle);
                phrasesNode.AppendChild(phraseNode);
            }

            foreach (Threshold lThreshold in getThresholds())
            {
                XmlElement thresholdNode = xmlDocument.CreateElement("threshold");
                thresholdNode.SetAttribute("name", lThreshold.name);
                thresholdNode.SetAttribute("value", lThreshold.value.ToString());
                thresholdNode.SetAttribute("color", lThreshold.colour.ToString());
                thresholdsNode.AppendChild(thresholdNode);
            }

            xmlDocument.Save(@filename);
        }

        public static void XMLToConfig(string filename)
        {
            Boolean bIsNewFormat = true;
            List<Break> lBreakTemp = new List<Break>();
            List<Subtitle> lSubtitleTemp = new List<Subtitle>();
            List<Threshold> lThresholdTemp = new List<Threshold>();

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(@filename);

            try { ColorConverter.ConvertFromString(xmlDocument.DocumentElement.SelectSingleNode("/worktimetracker").Attributes["headcolor"].Value); }
            catch { bIsNewFormat = false; }

            setInterval(Convert.ToInt32(xmlDocument.DocumentElement.SelectSingleNode("/worktimetracker").Attributes["interval"].Value));
            setWorkDuration(Convert.ToInt32(xmlDocument.DocumentElement.SelectSingleNode("/worktimetracker").Attributes["workduration"].Value));

            if (bIsNewFormat)
                setTrayIconColor((Color)ColorConverter.ConvertFromString(xmlDocument.DocumentElement.SelectSingleNode("/worktimetracker").Attributes["headcolor"].Value));
            else
                setTrayIconColor(Helper.ColorFromArgb(Convert.ToInt32(xmlDocument.DocumentElement.SelectSingleNode("/worktimetracker").Attributes["headcolor"].Value)));

            foreach (XmlNode breakNode in xmlDocument.DocumentElement.SelectSingleNode("/worktimetracker/breaks"))
            {
                String name = breakNode.Attributes["name"].Value;
                Boolean enabled = Convert.ToBoolean(breakNode.Attributes["enabled"].Value);
                DateTime start;
                TimeSpan duration;
                if (bIsNewFormat)
                {
                    start = DateTime.Parse(breakNode.Attributes["start"].Value);
                    duration = TimeSpan.Parse(breakNode.Attributes["duration"].Value);
                }
                else
                {
                    start = DateTime.FromBinary(Convert.ToInt64(breakNode.Attributes["start"].Value));
                    duration = TimeSpan.FromTicks(Convert.ToInt64(breakNode.Attributes["duration"].Value));
                }

                lBreakTemp.Add(new Break(name, enabled, duration, start));
            }
            setBreaks(lBreakTemp);

            foreach (XmlNode thresholdNode in xmlDocument.DocumentElement.SelectSingleNode("/worktimetracker/thresholds"))
            {
                String name = thresholdNode.Attributes["name"].Value;
                Int32 value = Convert.ToInt32(thresholdNode.Attributes["value"].Value);
                Color color;
                if (bIsNewFormat)
                    color = (Color)ColorConverter.ConvertFromString(thresholdNode.Attributes["color"].Value);
                else
                    color = Helper.ColorFromArgb(Convert.ToInt32(thresholdNode.Attributes["color"].Value));

                lThresholdTemp.Add(new Threshold(color, value, name));
            }
            setThresholds(lThresholdTemp);

            foreach (XmlNode phrasesNode in xmlDocument.DocumentElement.SelectSingleNode("/worktimetracker/phrases"))
            {
                String subtitle = phrasesNode.Attributes["string"].Value;
                Int32 end = Convert.ToInt32(phrasesNode.Attributes["end"].Value);
                Int32 start = Convert.ToInt32(phrasesNode.Attributes["start"].Value);

                lSubtitleTemp.Add(new Subtitle(start, end, subtitle));
            }
            setSubtitles(lSubtitleTemp);
        }

        public static void HistoryToXML(string filename)
        {
            XmlDocument xmlDocument = new XmlDocument();
            try { xmlDocument.Load(@filename); }
            catch { }

            XmlElement worktimetrackerNode = xmlDocument.SelectSingleNode("/worktimetracker") as XmlElement;
            if (worktimetrackerNode == null)
            {
               worktimetrackerNode = xmlDocument.CreateElement("worktimetracker");
               xmlDocument.AppendChild(worktimetrackerNode);
            }
            
            XmlElement historyNode = xmlDocument.SelectSingleNode("/worktimetracker/history") as XmlElement;
            if (historyNode == null)
            {
                historyNode = xmlDocument.CreateElement("history");
                worktimetrackerNode.AppendChild(historyNode);
            }
            historyNode.RemoveAll();

            foreach (Day lDay in getDays())
            {
                XmlElement dayNode = xmlDocument.CreateElement("day");
                dayNode.SetAttribute("start", lDay.starttime.ToString());
                dayNode.SetAttribute("end", lDay.endtime.ToString());
                historyNode.AppendChild(dayNode);
            }

            xmlDocument.Save(@filename);
        }

        public static void XMLToHistory(string filename)
        {
            List<Day> lDayTemp = new List<Day>();

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(@filename);

            foreach (XmlNode historyNode in xmlDocument.DocumentElement.SelectSingleNode("/worktimetracker/history"))
            {
                DateTime start = DateTime.Parse(historyNode.Attributes["start"].Value);
                DateTime end = DateTime.Parse(historyNode.Attributes["end"].Value);

                lDayTemp.Add(new Day(start, end));
            }
            setDays(lDayTemp);
        }
    }
}
