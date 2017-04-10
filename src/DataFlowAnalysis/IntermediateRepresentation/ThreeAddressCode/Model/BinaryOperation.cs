using SyntaxTree;

namespace DataFlowAnalysis.ThreeAddressCode.Model
{
    public class BinaryOperation : Expression
    {
        public Expression Left { get; set; }

        public Expression Right { get; set; }

        public Operation Operation { get; set; }

        public BinaryOperation(Expression left, Operation op, Expression right)
        {
            Left = left;
            Operation = op;
            Right = right;
        }

        public BinaryOperation(Expression left, Operation op, int right)
        {
            Left = left;
            Operation = op;
            Right = new Int32Const(right);
        }

        public BinaryOperation(Expression left, Operation op, string right)
        {
            Left = left;
            Operation = op;
            Right = new Identifier(right);
        }

        public BinaryOperation(int left, Operation op, Expression right)
        {
            Left = new Int32Const(left);
            Operation = op;
            Right = right;
        }

        public BinaryOperation(string left, Operation op, Expression right)
        {
            Left = new Identifier(left);
            Operation = op;
            Right = right;
        }

        public BinaryOperation(string left, Operation op, int right)
        {
            Left = new Identifier(left);
            Operation = op;
            Right = new Int32Const(right);
        }

        public override string ToString()
        {
            return $"{Left} {Operation.ToText()} {Right}";
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BinaryOperation) obj);
        }

        protected bool Equals(BinaryOperation other)
        {
            return Equals(Left, other.Left) && Equals(Right, other.Right) && Operation == other.Operation;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Left != null ? Left.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Right != null ? Right.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int) Operation;
                return hashCode;
            }
        }

        public override bool HasIdentifiedSubexpression(Identifier expression)
        {
            return Left.Equals(expression) || Right.Equals(expression);
        }
    }
}
