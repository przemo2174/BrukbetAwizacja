using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrukbetAwizacja
{
    

    public class TextParser
    {
        private TextReader textReader;

        public TextParser(TextReader textReader)
        {
            this.textReader = textReader;
        }

        public byte[] CreateMessage(NotificationType notificationType)
        {
            StringBuilder message = new StringBuilder();
            Dictionary<string, string> dictionary = notificationType == NotificationType.GreenNotification ? textReader.CurrentNotifications : textReader.PendingNotifications;
            List<TimeSpan> time = notificationType == NotificationType.GreenNotification ? textReader.TimeCurrentNotifications : textReader.TimePendingNotifications;

            List<string> keys = dictionary.Keys.ToList();
            keys.Sort();
            foreach(string key in keys)
            {
                string id = "N" + key;
                message.Append(id + "=" + FormatNumber(dictionary[key]));
            }
            message.Append("GOD=" + time[0].ToString(@"hh\:mm") + ":");
            message.Append("GDO=" + time[1].ToString(@"hh\:mm") + ":");
            //message.Append(Encoding.ASCII.GetByteCount(message.ToString()));

            return AddHeaderAndChecksum(message.ToString(), notificationType);
        }

        private static byte[] AddHeaderAndChecksum(string message, NotificationType notificationType)
        {
            byte startByte = (byte)(notificationType == NotificationType.GreenNotification ? 0xFC : 0xFD);

            byte[] data = Encoding.ASCII.GetBytes(message); 
            byte[] frame = new byte[data.Length + 2]; // 2 more for startByte and checksum
            frame[0] = startByte;
            Array.Copy(data, 0, frame, 1, data.Length);
            byte checksum = CalculateChecksum(frame);
            frame[frame.Length - 1] = checksum;
            return frame;
        }

        private static byte CalculateChecksum(byte[] bytes)
        {
            byte sum = 0;
            for (int i = 0; i < bytes.Length - 1; i++)
                sum += bytes[i];
            return (byte)(sum & 0x7F);
        }

        private static string FormatNumber(string number)
        {
            int length = number.Length;
            if (number.Length == 6)
                return number;
            if (number == "")
                return "      ";
            StringBuilder builder = new StringBuilder(6);
            for (int i = 1; i <= 6 - length; i++)
                builder.Append(" ");
            return builder.Append(number).ToString();
        }
    }
}
