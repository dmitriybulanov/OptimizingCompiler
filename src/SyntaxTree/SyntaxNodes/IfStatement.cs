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
    }
}
