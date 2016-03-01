using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrukbetAwizacja
{
    public enum NotificationType
    {
        GreenNotification = 0,
        RedNotification
    }

    public static class TextParser
    {
        public static byte[] CreateMessage(Dictionary<string, string> dictionary, List<TimeSpan> time, NotificationType notificationType)
        {
            StringBuilder message = new StringBuilder();

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
            byte startByte;
            if (notificationType == NotificationType.GreenNotification)
                startByte = 0xFC;
            else 
                startByte = 0xFD;

            byte[] data = Encoding.ASCII.GetBytes(message);
            byte[] frame = new byte[data.Length + 2];
            frame[0] = startByte;
            Array.Copy(data, 0, frame, 1, data.Length);
            byte checksum = CalculateChecksum(frame);
            frame[frame.Length - 1] = (byte)(0x7F & checksum);
            return frame;
        }

        private static byte CalculateChecksum(byte[] bytes)
        {
            byte sum = 0;
            for (int i = 0; i < bytes.Length - 1; i++)
                sum += bytes[i];
            return sum;
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
