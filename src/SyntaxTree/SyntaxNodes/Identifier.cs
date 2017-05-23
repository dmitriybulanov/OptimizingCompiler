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

        protected bool Equals(Identifier other)
        {
            return string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Identifier) obj);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }
    }
}
