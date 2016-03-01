using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;

namespace BrukbetAwizacja
{
    public class FileManager : IDisposable
    {
        private FileSystemWatcher watcher;
        public string Path { get; }
        public event FileSystemEventHandler Changed
        {
            add
            {
                watcher.Changed += value;
            }
            remove
            {
                watcher.Changed -= value;
            }
        }

        public event RenamedEventHandler Renamed
        {
            add
            {
                watcher.Renamed += value;
            }
            remove
            {
                watcher.Renamed -= value;
            }
        }

        public FileManager(string path)
        {
            Path = path;
            watcher = new FileSystemWatcher(Path, "*.txt");
            watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName;
            watcher.EnableRaisingEvents = true;
        }

        public static bool FilesAreEqual(string path1, string path2)
        {
            using (MD5 md5 = MD5.Create())
            {
                Stream stream1 = File.OpenRead(path1);
                Stream stream2 = File.OpenRead(path2);
                byte[] firstHash = md5.ComputeHash(stream1);
                byte[] secondHash = md5.ComputeHash(stream2);
                stream1.Dispose(); stream1.Close();
                stream2.Dispose(); stream2.Close();

                for(int i = 0; i < firstHash.Length; i++)
                {
                    if (firstHash[i] != secondHash[i])
                        return false;
                }
                return true;
            }
        }

        public void Dispose()
        {
            ((IDisposable)watcher).Dispose();
        }
    }
}
