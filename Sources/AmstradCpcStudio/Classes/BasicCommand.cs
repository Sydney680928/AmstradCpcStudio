using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmstradCpcStudio.Classes
{
    internal class BasicCommand : Keyword
    {
        public BasicCommand(string name, string description, params string[] samples) : base(name, description, samples)
        {

        }
    }
}
