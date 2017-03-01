using SyntaxTree.Visitors;

namespace SyntaxTree.SyntaxNodes
{

    public abstract class SyntaxNode
    {
        public abstract void Accept(AbstractVisitor visitor);

        public abstract T Accept<T>(AbstractVisitor<T> visitor);
    }
}
