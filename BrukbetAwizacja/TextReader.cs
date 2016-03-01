using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BrukbetAwizacja
{
    public class TextReader : IDisposable
    {
        StreamReader reader;
        public Dictionary<string, string> CurrentNotifications { get; }
        public Dictionary<string, string> PendingNotifications { get; }
        public List<TimeSpan> TimeCurrentNotifications {get;}
        public List<TimeSpan> TimePendingNotifications { get; }

        private enum NotificationStatus
        {
            Current = 0,
            Pending
        }

        public TextReader(string path)
        {
            reader = new StreamReader(path);
            CurrentNotifications = new Dictionary<string, string>(15);
            PendingNotifications = new Dictionary<string, string>(15);
            TimeCurrentNotifications = new List<TimeSpan>(2);
            TimePendingNotifications = new List<TimeSpan>(2);
        }

        public void ReadText()
        {
            string line;
            NotificationStatus notificationStatus = NotificationStatus.Current;
            while((line = reader.ReadLine()) != null)
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

        private void ParseLine(string line, NotificationStatus notificationStatus)
        {
            if (!line.Contains("Godz"))
            {
                string[] array = line.Split(new char[] { ':' }, StringSplitOptions.None);
                if(array.Length == 2)
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

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    reader.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
         ~TextReader()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
           Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}
