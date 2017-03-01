using SyntaxTree.Visitors;

namespace SyntaxTree.SyntaxNodes
{
    public class Identifier : Expression
    {
        public string Name
        {
            get;
            set;
        }

        public Identifier(string name)
        {
            this.Name = name;
        }

        public override void Accept(AbstractVisitor visitor)
        {
            visitor.VisitIdentifier(this);
        }

        public override T Accept<T>(AbstractVisitor<T> visitor)
        {
            return visitor.VisitIdentifier(this);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
