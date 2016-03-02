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
            return Regex.IsMatch(ip, @"([0-9]{3}\.){3}[0-9]");
        }
    }
}
