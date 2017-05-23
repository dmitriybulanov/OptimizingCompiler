using System;
using SyntaxTree.Visitors;

namespace SyntaxTree.SyntaxNodes
{
    public class ForStatement : Statement
    {
        public Identifier ForVariable
        {
            get;
            set;
        }

        public Expression From
        {
            get;
            set;
        }

        public Expression To
        {
            get;
            set;
        }

        public Statement Body
        {
            get;
            set;
        }

        public  ForStatement() { }

        public ForStatement(Identifier forVariable, Expression from, Expression to, Statement body)
        {
            ForVariable = forVariable;
            From = from;
            To = to;
            Body = body;
        }

        public override  void Accept(AbstractVisitor visitor)
        {
            visitor.VisitFor(this);
        }

        public override T Accept<T>(AbstractVisitor<T> visitor)
        {
            return visitor.VisitFor(this);
        }

        protected bool Equals(ForStatement other)
        {
            return Equals(ForVariable, other.ForVariable) && Equals(From, other.From) && Equals(To, other.To) && Equals(Body, other.Body);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ForStatement) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (ForVariable != null ? ForVariable.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (From != null ? From.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (To != null ? To.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Body != null ? Body.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
