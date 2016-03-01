using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using BrukbetAwizacja;

namespace BrukbetAwizacjaTest
{
    [TestFixture]
    public class FileAccessTests
    {
        [Test]
        public void FileEquality()
        {
            Assert.IsFalse(FileManager.FilesAreEqual("D:\\Awizacja.txt", "D:\\kopia.txt"));
        }
    }
}
