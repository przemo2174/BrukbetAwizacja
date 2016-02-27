using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace BrukbetAwizacja
{
    public class EthernetCommunication : Communication
    {
        byte[] buffer = new byte[128];
        Socket sender;
        IPEndPoint endPoint;
        IPAddress ipAddress;
        int port;

        public EthernetCommunication(string ipAddress, int port)
        {
            this.ipAddress = IPAddress.Parse(ipAddress);
            this.port = port;
        }

        public override string SendMessage(string message)
        {
            try
            {
                sender.Connect(endPoint);
                byte[] msg = Encoding.ASCII.GetBytes(message);
                int bytesSend = sender.Send(msg);

                int bytesReceived = sender.Receive(buffer);
                if(bytesReceived > 0)
                    return Encoding.ASCII.GetString(buffer);
                return "";
            }
            catch
            {
                throw;
            }
        }

        public override string SendMessage(byte[] bytes)
        {
            try
            {
                sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                endPoint = new IPEndPoint(ipAddress, port);
                sender.Connect(endPoint);
                int bytesSend = sender.Send(bytes);

                int bytesReceived = sender.Receive(buffer);
                string value = Encoding.ASCII.GetString(buffer, 0, bytesReceived);
                switch(value)
                {
                    case "G":
                        return "Ramka została wysłana pomyślnie";
                    case "S":
                        return "Błąd cyfr";
                    case "R":
                        return "Błąd ramki";
                    case "P":
                        return "Błąd parzystości";
                    case "Q":
                        return "Błąd prefiksu";
                    default:
                        return "Błąd niezydentyfikowany";
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                sender.Close();
            }
        }
    }
}
