using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BrukbetAwizacja
{
    public class TextReader
    {
        private StreamReader reader;
        internal Dictionary<string, string> CurrentNotifications { get; }
        internal Dictionary<string, string> PendingNotifications { get; }
        internal List<TimeSpan> TimeCurrentNotifications {get;}
        internal List<TimeSpan> TimePendingNotifications { get; }
        private string path;

        public TextReader(string path)
        {
            this.path = path;
            CurrentNotifications = new Dictionary<string, string>(15);
            PendingNotifications = new Dictionary<string, string>(15);
            TimeCurrentNotifications = new List<TimeSpan>(2);
            TimePendingNotifications = new List<TimeSpan>(2);
        }

        public void ReadText()
        {
            using (reader = new StreamReader(path))
            {
                string line;
                NotificationStatus notificationStatus = NotificationStatus.Current;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith("*"))
                    {
                        if (line.Contains("OCZEK"))
                            notificationStatus = NotificationStatus.Pending;
                        continue;
                    }
                    if (line == string.Empty)
                        continue;

                    ParseLine(line, notificationStatus);
                }
            }
            
                
        }

        private void ParseLine(string line, NotificationStatus notificationStatus)
        {
            try
            {
                if (!line.Contains("Godz"))
                {
                    string[] array = line.Split(new char[] { ':' }, StringSplitOptions.None);
                    if (array.Length == 2)
                    {
                        array[0] = array[0].Substring(1).Trim();
                        array[1] = array[1].Trim();
                        if (notificationStatus == NotificationStatus.Current)
                            CurrentNotifications.Add(array[0], array[1]);
                        else if (notificationStatus == NotificationStatus.Pending)
                            PendingNotifications.Add(array[0], array[1]);
                    }
                    
                    
                }
                else
                {
                    string[] array = line.Split(new char[] { ':' }, 2);
                    array[1] = array[1].Trim();
                    string[] time = array[1].Split(':');
                    int hour = int.Parse(time[0]);
                    int minutes = int.Parse(time[1]);
                    if (notificationStatus == NotificationStatus.Current)
                        TimeCurrentNotifications.Add(new TimeSpan(hour, minutes, 0));
                    else
                        TimePendingNotifications.Add(new TimeSpan(hour, minutes, 0));
                }
            }
            catch
            {
                throw;
            }
            
        }

    }
}
