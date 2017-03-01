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
    }
}
