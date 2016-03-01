using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BrukbetAwizacjaTest
{
    [TestFixture]
    public class FileAccessTests
    {
        [Test]
        public void FileAccess()
        {
            File.Copy("\\\\przemo2174-pc\\Users\\przemo2174\\test.txt", "D:\\kopia1.txt");
        }
    }
}
