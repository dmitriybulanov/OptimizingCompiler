using System;
using SyntaxTree.Visitors;

namespace SyntaxTree.SyntaxNodes
{
    public class BinaryExpression : Expression
    {

        public Expression Left
        {
            get;
            set;
        }

        public Expression Right
        {
            get;
            set;
        }

        public Operation Operation
        {
            get;
            set;
        }

        public BinaryExpression(Expression left, Operation op, Expression right)
        {
            Left = left;
            Right = right;
            Operation = op;
        }

        public override void Accept(AbstractVisitor visitor)
        {
            visitor.VisitBinaryExpression(this);
        }

        public override T Accept<T>(AbstractVisitor<T> visitor)
        {
            return visitor.VisitBinaryExpression(this);
        }

        protected bool Equals(BinaryExpression other)
        {
            return Equals(Left, other.Left) && Equals(Right, other.Right) && Operation == other.Operation;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BinaryExpression) obj);
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
    }
}
