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
            return _lDays.OrderBy(x => x.starttime).ToList();
        }

        public static void setBreaks(List<Break> lBreaks)
        {
            _lBreaks = lBreaks;
            Properties.Settings.Default.listBreaks = SerializeObject(lBreaks);
            Properties.Settings.Default.Save();
        }
        public static List<Break> getBreaks()
        {
            return _lBreaks.OrderBy(x => x.starttime).ToList();
        }

        public static void setSubtitles(List<Subtitle> lSubtitles)
        {
            _lSubtitles = lSubtitles;
            Properties.Settings.Default.listSubtitles = SerializeObject(lSubtitles);
            Properties.Settings.Default.Save();
        }
        public static List<Subtitle> getSubtitles()
        {
            return _lSubtitles.OrderBy(x => x.rangestart).ToList(); ;
        }

        public static void setThresholds(List<Threshold> lThresholds)
        {
            _lThresholds = lThresholds;
            Properties.Settings.Default.listThresholds = SerializeObject(lThresholds);
            Properties.Settings.Default.Save();
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
            _cTrayIcon = (Colour)DeserializeObject(Properties.Settings.Default.colorTrayIcon);
            _iInterval = Properties.Settings.Default.intInterval;
            _iWorkDuration = Properties.Settings.Default.intWorkDuration;
            _dtWorkEndTime = Properties.Settings.Default.dtWorkEndTime;
            _dtWorkStartTime = Properties.Settings.Default.dtWorkStartTime;
            _tsWorkTimeElapsed = Properties.Settings.Default.tsWorkTimeElapsed;
            _tsWorkTimeRemaining = Properties.Settings.Default.tsWorkTimeRemaining;
            _lDays = DeserializeObject(Properties.Settings.Default.listDays) as List<Day>;
            _lBreaks = DeserializeObject(Properties.Settings.Default.listBreaks) as List<Break>;
            _lThresholds = DeserializeObject(Properties.Settings.Default.listThresholds) as List<Threshold>;
            _lSubtitles = DeserializeObject(Properties.Settings.Default.listSubtitles) as List<Subtitle>;
        }

        public static void ExportOldConfig()
        {
            setTrayIconColor(Config.cHead);
            setInterval(Config.iCalcInterval);
            setWorkDuration(Config.iWorkDuration);
            setWorkTimeEnd(Config.dtWorkEndTime);
            setWorkTimeStart(Config.dtWorkStartTime);
            setWorkTimeElapsed(Config.tsWorkTimeElapsed);
            setWorkTimeRemaining(Config.tsWorkTimeRemaining);
            setThresholds(Config.lThresholds);
            setSubtitles(Config.lSubtitles);
            setBreaks(Config.lBreaks);
            setDays(Config.lDays);
        }

        public static void ConfigToXML(string filename)
        {
            using (XmlWriter xmlWriter = XmlWriter.Create(filename, new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "\t",
                NewLineChars = "\r\n",
                NewLineHandling = NewLineHandling.Replace
            }))
            {
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("worktimetracker");
                xmlWriter.WriteAttributeString("interval", getInterval().ToString());
                xmlWriter.WriteAttributeString("workduration", getWorkDuration().ToString());
                xmlWriter.WriteAttributeString("headcolor", getTrayIconColor().ToString());
                xmlWriter.WriteStartElement("breaks");
                foreach (Break lBreak in getBreaks())
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
                foreach (Threshold lThreshold in getThresholds())
                {
                    xmlWriter.WriteStartElement("threshold");
                    xmlWriter.WriteAttributeString("name", lThreshold.name);
                    xmlWriter.WriteAttributeString("value", lThreshold.value.ToString());
                    xmlWriter.WriteAttributeString("color", lThreshold.colour.ToString());
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("phrases");
                foreach (Subtitle lSubtitle in getSubtitles())
                {
                    xmlWriter.WriteStartElement("phrase");
                    xmlWriter.WriteAttributeString("start", lSubtitle.rangestart.ToString());
                    xmlWriter.WriteAttributeString("end", lSubtitle.rangeend.ToString());
                    xmlWriter.WriteAttributeString("string", lSubtitle.subtitle);
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("history");
                foreach (Day lDay in getDays())
                {
                    xmlWriter.WriteStartElement("day");
                    xmlWriter.WriteAttributeString("start", lDay.starttime.ToBinary().ToString());
                    xmlWriter.WriteAttributeString("end", lDay.endtime.ToBinary().ToString());
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();
            }
        }

        public static void XMLToConfig(string filename)
        {
            XmlDocument xmlDocument = new XmlDocument() ;
            xmlDocument.Load(@filename);

            foreach (XmlNode childNode in xmlDocument.ChildNodes)
            {
                if (childNode.Name.ToLower().Equals("worktimetracker"))
                {
                    setInterval(Convert.ToInt32(childNode.Attributes["interval"].Value));
                    setWorkDuration(Convert.ToInt32(childNode.Attributes["workduration"].Value));
                    setTrayIconColor((Color)ColorConverter.ConvertFromString(childNode.Attributes["headcolor"].Value));
                    
                    foreach (XmlNode xmlNodes in childNode.ChildNodes)
                    {
                        string lower = xmlNodes.Name.ToLower();
                        if (lower == "breaks")
                        {
                            List<Break> lTemp = new List<Break>();

                            foreach (XmlNode childNode1 in xmlNodes.ChildNodes)
                            {
                                if (childNode1.Name.ToLower().Equals("break"))
                                {
                                    String name = childNode1.Attributes["name"].Value;
                                    Boolean enabled = Convert.ToBoolean(childNode1.Attributes["enabled"].Value);
                                    DateTime start = DateTime.FromBinary(Convert.ToInt64(childNode1.Attributes["start"].Value));
                                    TimeSpan duration = TimeSpan.FromTicks(Convert.ToInt64(childNode1.Attributes["duration"].Value));

                                    lTemp.Add(new Break(name, enabled, duration, start));
                                }
                            }

                            setBreaks(lTemp);
                        }
                        else if (lower == "thresholds")
                        {
                            List<Threshold> lTemp = new List<Threshold>();

                            foreach (XmlNode xmlNodes1 in xmlNodes.ChildNodes)
                            {
                                if (xmlNodes1.Name.ToLower().Equals("threshold"))
                                {
                                    String name = xmlNodes1.Attributes["name"].Value;
                                    Int32 value = Convert.ToInt32(xmlNodes1.Attributes["value"].Value);
                                    Color color = (Color)ColorConverter.ConvertFromString(childNode.Attributes["color"].Value);

                                    lTemp.Add(new Threshold(color, value, name ));
                                }
                            }

                            setThresholds(lTemp);
                        }
                        else if (lower == "history")
                        {
                            List<Day> lTemp = new List<Day>();

                            foreach (XmlNode childNode2 in xmlNodes.ChildNodes)
                            {
                                if (childNode2.Name.ToLower().Equals("day"))
                                {
                                    DateTime start = DateTime.FromBinary(Convert.ToInt64(childNode2.Attributes["start"].Value));
                                    DateTime end = DateTime.FromBinary(Convert.ToInt64(childNode2.Attributes["end"].Value));

                                    lTemp.Add(new Day(start, end));
                                }
                            }

                            setDays(lTemp);
                        }
                        else if (lower == "phrases")
                        {
                            List<Subtitle> lTemp = new List<Subtitle>();
                            
                            foreach (XmlNode xmlNodes2 in xmlNodes.ChildNodes)
                            {
                                if (xmlNodes2.Name.ToLower().Equals("phrase"))
                                {
                                    String subtitle = xmlNodes2.Attributes["string"].Value;
                                    Int32 end = Convert.ToInt32(xmlNodes2.Attributes["end"].Value);
                                    Int32 start = Convert.ToInt32(xmlNodes2.Attributes["start"].Value);

                                    lTemp.Add(new Subtitle(start, end, subtitle));
                                }
                            }

                            setSubtitles(lTemp);
                        }
                    }
                }
            }


        }

    }
}
