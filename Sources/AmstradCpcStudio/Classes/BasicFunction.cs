using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmstradCpcStudio.Classes
{
    internal class BasicFunction : Keyword
    {
        public BasicFunction(string name, string description,params string[] samples) : base(name, description, samples)
        {

        }
    }
}
