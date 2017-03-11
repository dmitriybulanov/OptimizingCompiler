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
    }
}
