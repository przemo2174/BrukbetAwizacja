using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrukbetAwizacja
{
    public abstract class Communication
    {
        public Communication()
        {

        }

        public abstract string SendMessage(string message);

        public abstract string SendMessage(byte[] bytes);  
    }
}
