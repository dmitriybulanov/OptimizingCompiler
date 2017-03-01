using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyntaxTree;

namespace ThreeAddressCode.Model
{
    public class Assignment : ThreeAddressCommand
    {
        public string Target { get; set; }

        public Expression Value { get; set; }

        public Assignment(string target, Expression value)
        {
            Target = target;
            Value = value;
        }

        public static Assignment Increment(string variable) =>
            new Assignment(variable, new BinaryOperation(variable, Operation.Add, 1));

        public override string ToString() => base.ToString() + $"{Target} = {Value}";
    }
}
