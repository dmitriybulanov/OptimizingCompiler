using System;
using SyntaxTree.Visitors;

namespace SyntaxTree.SyntaxNodes
{
    public abstract class Expression : SyntaxNode
    {
        public override void Accept(AbstractVisitor visitor)
        {
            visitor.VisitExpression(this);
        }

        public override T Accept<T>(AbstractVisitor<T> visitor)
        {
            return visitor.VisitExpression(this);
        }
    }
}
