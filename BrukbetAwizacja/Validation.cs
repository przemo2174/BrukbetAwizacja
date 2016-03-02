using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BrukbetAwizacja
{
    public static class Validation
    {
        public static bool IsIPValid(string ip)
        {
            try
            {
                string[] octets = ip.Split('.');
                if (octets.Length != 4)
                    return false;
                foreach(string octet in octets)
                {
                    int number = int.Parse(octet);
                    if (!(number <= 255 && number >= 0))
                        return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
            
        }
    }
}
