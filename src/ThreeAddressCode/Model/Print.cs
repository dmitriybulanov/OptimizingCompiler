using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeAddressCode.Model
{
    public class Print : ThreeAddressCommand
    {
        public Expression Argument { get;set; }

        public bool WithNewLine { get; set; }

        public Print() { }

        public Print(Expression expression, bool withNewLine)
        {
            Argument = expression;
            WithNewLine = withNewLine;
        }

        public override string ToString()
        {
            var funcName = WithNewLine ? "println" : "print";
            return base.ToString() + $"{funcName} {Argument}";
        }
    }
}