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
    }
}
