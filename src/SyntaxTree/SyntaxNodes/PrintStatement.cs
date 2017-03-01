using System;
using SyntaxTree.Visitors;

namespace SyntaxTree.SyntaxNodes
{
    public class PrintStatement : Statement
    {
        public Expression Expression
        {
            get;
            set;
        }

        public bool NewLine
        {
            get;
            set;
        }

        public PrintStatement(Expression expression, bool newLine)
        {
            this.Expression = expression;
            this.NewLine = newLine;
        }

        public override void Accept(AbstractVisitor visitor)
        {
            visitor.VisitPrint(this);
        }

        public override T Accept<T>(AbstractVisitor<T> visitor)
        {
            return visitor.VisitPrint(this);
        }
    }
}
