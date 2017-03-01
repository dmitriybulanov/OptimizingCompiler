using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeAddressCode.Model
{
    public class Identifier : Expression
    {
        public string Name { get; set; }

        public Identifier() { }

        public Identifier(string name)
        {
            Name = name;
        }

        public override string ToString() => Name;
    }
}
