using System;
using SyntaxTree.Visitors;

namespace SyntaxTree.SyntaxNodes
{
    public class IfStatement : Statement
    {
        public Expression Condition
        {
            get;
            set;
        }

        public Statement ThenBody
        {
            get;
            set;
        }

        public Statement ElseBody
        {
            get;
            set;
        }

        public  IfStatement() { }
        

        public IfStatement(Expression condition, Statement thenBody)
        {
            this.Condition = condition;
            this.ThenBody = thenBody;
        }

        public IfStatement(Expression condition, Statement thenBody, Statement elseBody)
        {
            this.Condition = condition;
            this.ThenBody = thenBody;
            this.ElseBody = elseBody;
        }

        public override void Accept(AbstractVisitor visitor)
        {
            visitor.VisitIf(this);
        }

        public override T Accept<T>(AbstractVisitor<T> visitor)
        {
            return visitor.VisitIf(this);
        }

        protected bool Equals(IfStatement other)
        {
            return Equals(Condition, other.Condition) && Equals(ThenBody, other.ThenBody) && Equals(ElseBody, other.ElseBody);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((IfStatement) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Condition != null ? Condition.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ThenBody != null ? ThenBody.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ElseBody != null ? ElseBody.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
