using SyntaxTree;

namespace DataFlowAnalysis.ThreeAddressCode.Model
{
    public class UnaryOperation : Expression
    {
        public Operation Operation { get; set; }
        public Expression Operand { get; set; }

        public UnaryOperation(Operation operation, Expression operand)
        {
            Operation = operation;
            Operand = operand;
        }

        public UnaryOperation() { }

        public override string ToString()
        {
            return Operation.ToText() + Operand;
        }
    }
}
