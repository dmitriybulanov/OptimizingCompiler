using System;
using SyntaxTree.Visitors;

namespace SyntaxTree.SyntaxNodes
{
    public class Program : SyntaxNode
    {
        public Statement Body
        {
            get;
            set;
        }

        public Program() { }

        public Program(Statement body)
        {
            Body = body;
        }

        public override void Accept(AbstractVisitor visitor)
        {
            visitor.VisitProgram(this);
        }

        public override T Accept<T>(AbstractVisitor<T> visitor)
        {
            return visitor.VisitProgram(this);
        }

        protected bool Equals(Program other)
        {
            return Equals(Body, other.Body);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Program) obj);
        }

        public override int GetHashCode()
        {
            return (Body != null ? Body.GetHashCode() : 0);
        }
    }
}
