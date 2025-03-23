using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmstradCpcStudio.Classes
{
    internal class Keyword
    {
        public string Name { get; private set; }

        public string Description { get; private set; }

        public string[] Samples { get; }

        public Keyword(string name, string description, params string[] sample)
        {
            Name = name;
            Description = description;
            Samples = sample;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
