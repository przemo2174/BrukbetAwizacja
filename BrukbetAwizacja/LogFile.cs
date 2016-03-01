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

        public void AddLogMessage(string message)
        {
            using (StreamWriter writer = File.AppendText(Path))
            {
                writer.WriteLine("{0} {1} => {3}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString(), message);
            }
        }
    }
}
