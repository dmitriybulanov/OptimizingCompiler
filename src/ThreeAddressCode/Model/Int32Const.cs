using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeAddressCode.Model
{
    public class Int32Const : Expression
    {
        public int Value
        {
            get;
            set;
        }

        public Int32Const(int value)
        {
            this.Value = value;
        }

        public override string ToString() => Value.ToString();
    }
}
