using System;
using SyntaxTree.Visitors;

namespace SyntaxTree.SyntaxNodes
{
    public class PrintStatement : Statement
    {
        public Expression Expression
        {
            get;
            set;
        }

        public bool NewLine
        {
            get;
            set;
        }

        public PrintStatement(Expression expression, bool newLine)
        {
            this.Expression = expression;
            this.NewLine = newLine;
        }

        public override void Accept(AbstractVisitor visitor)
        {
            visitor.VisitPrint(this);
        }

        public override T Accept<T>(AbstractVisitor<T> visitor)
        {
            return visitor.VisitPrint(this);
        }

        protected bool Equals(PrintStatement other)
        {
            return Equals(Expression, other.Expression) && NewLine == other.NewLine;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PrintStatement) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Expression != null ? Expression.GetHashCode() : 0) * 397) ^ NewLine.GetHashCode();
            }
        }
    }
}
