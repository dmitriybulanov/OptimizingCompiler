using SyntaxTree;

namespace DataFlowAnalysis.ThreeAddressCode.Model
{
    public class Assignment : ThreeAddressCommand
    {
        public Identifier Target { get; set; }

        public Expression Value { get; set; }

        public Assignment(string target, Expression value)
        {
            Target = new Identifier(target);
            Value = value;
        }

        public static Assignment Increment(string variable) =>
            new Assignment(variable, new BinaryOperation(variable, Operation.Add, 1));

        public override string ToString() => base.ToString() + $"{Target.Name} = {Value}";
    }
}
