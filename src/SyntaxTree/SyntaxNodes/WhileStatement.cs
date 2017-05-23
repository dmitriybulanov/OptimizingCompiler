using SyntaxTree.Visitors;

namespace SyntaxTree.SyntaxNodes
{
    public class WhileStatement : Statement
    {
        public Expression Condition
        {
            get;
            set;
        }

        public Statement Body
        {
            get;
            set;
        }
        
        public WhileStatement() { }

        public WhileStatement(Expression condition, Statement body)
        {
            Condition = condition;
            Body = body;
        }

        public override void Accept(AbstractVisitor visitor)
        {
            visitor.VisitWhile(this);
        }

        public override T Accept<T>(AbstractVisitor<T> visitor)
        {
            return visitor.VisitWhile(this);
        }

        protected bool Equals(WhileStatement other)
        {
            return Equals(Condition, other.Condition) && Equals(Body, other.Body);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((WhileStatement) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Condition != null ? Condition.GetHashCode() : 0) * 397) ^ (Body != null ? Body.GetHashCode() : 0);
            }
        }
    }
}
