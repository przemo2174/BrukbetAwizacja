using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrukbetAwizacja;

namespace BrukbetAwizacjaTest
{
    [TestFixture]
    public class EthernetCommunicationTests
    {
        string path = @"D:\Awizacja.txt";

        [Test]
        public void TestMethod()
        {
            EthernetCommunication connection = new EthernetCommunication("127.0.0.1", 5544);
            TextReader reader = new TextReader(path);
            reader.ReadText();
            byte[] message = TextParser.CreateMessage(reader.CurrentNotifications, reader.TimeCurrentNotifications, NotificationType.GreenNotification);
            string response = connection.SendMessage(message);
            TestContext.WriteLine(response);
        }
    }
}
