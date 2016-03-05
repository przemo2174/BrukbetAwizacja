using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BrukbetAwizacja
{
    public class LogFile
    {
        public string Path { get; }

        public LogFile(string pathToCreate)
        {
            Path = pathToCreate;
        }

        public string AddLogMessage(string message, NotificationType notificationType)
        {
            using (StreamWriter writer = File.AppendText(Path))
            {
                string notificationString = notificationType == NotificationType.GreenNotification ? "(BIEŻĄCA)" : "(OCZEKUJĄCA)";
                string log = string.Format("{0} {1} => {2}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString(), message + notificationString);
                writer.WriteLine(log);
                return log;
            }
        }
    }
}
