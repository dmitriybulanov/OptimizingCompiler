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

        protected bool Equals(Int32Const other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Int32Const) obj);
        }

        public override int GetHashCode()
        {
            return Value;
        }
    }
}
