using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyntaxTree.Visitors;

namespace SyntaxTree.SyntaxNodes
{
    public class ParenthesizedExpression : Expression
    {
        public Expression Expression { get; set; }

        public ParenthesizedExpression() { }

        public ParenthesizedExpression(Expression expression)
        {
            Expression = expression;
        }

        public override void Accept(AbstractVisitor visitor)
        {
            visitor.VisitParenthesizedExpression(this);
        }

        public override T Accept<T>(AbstractVisitor<T> visitor)
        {
            return visitor.VisitParenthesizedExpression(this);
        }
    }
}
