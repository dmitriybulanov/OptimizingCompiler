using SyntaxTree.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxTree.SyntaxNodes
{
    public class GotoStatement : Statement
    {
        public string Label { get; set; }

        public GotoStatement() { }

        public GotoStatement(string label)
        {
            Label = label;
        }

        public override void Accept(AbstractVisitor visitor)
        {
            visitor.VisitGotoStatement(this);
        }

        public override T Accept<T>(AbstractVisitor<T> visitor)
        {
            return visitor.VisitGotoStatement(this);
        }
    }
}
