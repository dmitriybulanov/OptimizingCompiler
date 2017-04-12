using SyntaxTree;

namespace DataFlowAnalysis.ThreeAddressCode.Model
{
    public class UnaryOperation : ComplexExpression
    {
        public Operation Operation { get; set; }
        public SimpleExpression Operand { get; set; }

        public UnaryOperation(Operation operation, SimpleExpression operand)
        {
            Operation = operation;
            Operand = operand;
        }

        public UnaryOperation() { }

        public override string ToString()
        {
            return Operation.ToText() + Operand;
        }

        protected bool Equals(UnaryOperation other)
        {
            return Operation == other.Operation && Equals(Operand, other.Operand);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((UnaryOperation) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int) Operation * 397) ^ (Operand != null ? Operand.GetHashCode() : 0);
            }
        }

        public override bool HasIdentifiedSubexpression(Identifier expression)
        {
            return Operand.Equals(expression);
        }
    }
}
