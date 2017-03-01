using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeAddressCode.Model
{
    public abstract class ThreeAddressCommand
    {
        public string Label { get; set; } = null;

        public override string ToString()
        {
            return Label == null ? "" : $"{Label}: ";
        }
    }
}
