using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeAddressCode.Model
{
    public class NoOperation : ThreeAddressCommand
    {
        public NoOperation() { }

        public NoOperation(string label)
        {
            Label = label;
        }

        public override string ToString()
        {
            return base.ToString() + "<no-op>";
        }
    }
}
