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
            byte[] firstHash = CalcualteMD5(path1);
            byte[] secondHash = CalcualteMD5(path2);

            for(int i = 0; i < firstHash.Length; i++)
            {
                if (firstHash[i] != secondHash[i])
                    return false;
            }
            return true;
        }

        public static byte[] CalcualteMD5(string path)
        {
            using (MD5 md5 = MD5.Create())
            {
                using (Stream stream = File.OpenRead(path))
                {
                    return md5.ComputeHash(stream);
                }
            }
        }

        public static bool AreMD5Equal(byte[] hash1, byte[] hash2)
        {
            for (int i = 0; i < hash1.Length; i++)
            {
                if (hash1[i] != hash2[i])
                    return false;
            }
            return true;
        }

        public void Dispose()
        {
            ((IDisposable)watcher).Dispose();
        }
    }
}
