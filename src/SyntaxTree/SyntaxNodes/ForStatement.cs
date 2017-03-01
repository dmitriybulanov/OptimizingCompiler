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
    }
}
