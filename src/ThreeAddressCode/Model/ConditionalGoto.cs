using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeAddressCode.Model
{
    public class ConditionalGoto : Goto
    {
        public Expression Condition { get; set; }

        public ConditionalGoto(string gotoLabel, Expression condition) : base(gotoLabel)
        {
            Condition = condition;
        }

        public override string ToString()
        {
            return base.ToString() + $" if {Condition}";
        }
    }
}
