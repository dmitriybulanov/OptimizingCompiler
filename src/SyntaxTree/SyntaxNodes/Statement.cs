using System;
using SyntaxTree.Visitors;

namespace SyntaxTree.SyntaxNodes
{
    public abstract class Statement : SyntaxNode
    {
        public override void Accept(AbstractVisitor visitor)
        {
            visitor.VisitStatement(this);
        }

        public override T Accept<T>(AbstractVisitor<T> visitor)
        {
            return visitor.VisitStatement(this);
        }
    }
}
