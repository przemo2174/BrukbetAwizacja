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
        byte[] buffer = new byte[512];
        Socket sender;
        IPEndPoint endPoint;

        public EthernetCommunication(string ipAddress, int port)
        {
            sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            endPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
        }

        public override string SendMessage(string message)
        {
            try
            {
                sender.Connect(endPoint);
                byte[] msg = Encoding.ASCII.GetBytes(message);
                int bytesSend = sender.Send(msg);

                int bytesReceive = sender.Receive(buffer);
                string receiveString = Encoding.ASCII.GetString(buffer);
                return receiveString;
            }
            catch
            {
                throw;
            }
        }
    }
}
