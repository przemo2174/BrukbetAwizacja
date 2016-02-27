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
    public class TextParserTests
    {
        string path = @"D:\Awizacja.txt";
        TextReader textReader;

        [Test]
        public void CreateMessageCurrentTest()
        {
            textReader = new TextReader(path);
            textReader.ReadText();
            TestContext.WriteLine(TextParser.CreateMessage(textReader.CurrentNotifications, textReader.TimeCurrentNotifications, NotificationType.GreenNotification));
        }

        [Test]
        public void CreateMessagePendingTest()
        {
            textReader = new TextReader(path);
            textReader.ReadText();
            TestContext.WriteLine(TextParser.CreateMessage(textReader.PendingNotifications, textReader.TimePendingNotifications, NotificationType.GreenNotification));
        }

        [Test]
        public void LengthOfHex()
        {
            StringBuilder s = new StringBuilder();
            s.Append(0xFC);
            TestContext.WriteLine(Encoding.ASCII.GetByteCount(s.ToString()));
        }
    }
}
