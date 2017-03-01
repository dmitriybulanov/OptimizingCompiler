using SyntaxTree.Visitors;

namespace SyntaxTree.SyntaxNodes
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

        public override void Accept(AbstractVisitor visitor)
        {
            visitor.VisitInt32Const(this);
        }

        public override T Accept<T>(AbstractVisitor<T> visitor)
        {
            return visitor.VisitInt32Const(this);
        }
    }
}
