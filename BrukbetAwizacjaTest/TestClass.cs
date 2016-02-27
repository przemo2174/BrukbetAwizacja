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
    public class TextReaderTests
    {
        string path = @"D:\Awizacja.txt";
        TextReader textReader;

        [Test]
        public void CurrentNotificationTest()
        {
            textReader = new TextReader(path);
            textReader.ReadText();
            PrintDictionary(textReader.CurrentNotifications);
            Assert.Pass("No errors");
        }

        [Test]
        public void PendingNotificationTest()
        {
            textReader = new TextReader(path);
            textReader.ReadText();
            PrintDictionary(textReader.PendingNotifications);
            Assert.Pass();
        }

        [Test]
        public void CurrentTimeTest()
        {
            textReader = new TextReader(path);
            textReader.ReadText();
            PrintList(textReader.TimeCurrentNotifications);
        }

        [Test]
        public void PendingTimeTest()
        {
            textReader = new TextReader(path);
            textReader.ReadText();
            PrintList(textReader.TimePendingNotifications);
        }

        public void PrintDictionary(Dictionary<string, string> dict)
        {
            foreach(KeyValuePair<string, string> kvp in dict)
            {
                TestContext.WriteLine("Key: {0}, Value: {1}", kvp.Key, kvp.Value);
            }
        }

        public void PrintList(List<TimeSpan> list)
        {
            foreach (TimeSpan time in list)
                TestContext.WriteLine("Time: {0}", time);
        }
    }
}
