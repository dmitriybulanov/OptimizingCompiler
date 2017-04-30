using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyntaxTree.Visitors;

namespace SyntaxTree.SyntaxNodes
{
    public class LabelledStatement : Statement
    {
        public string Label { get; set; }

        public Statement Statement { get; set; }

        public LabelledStatement(string label, Statement statement)
        {
            Label = label;
            Statement = statement;
        }

        public LabelledStatement()
        {
        }

        public override void Accept(AbstractVisitor visitor)
        {
            visitor.VisitLabelledStatement(this);
        }

        public override T Accept<T>(AbstractVisitor<T> visitor)
        {
            return visitor.VisitLabelledStatement(this);
        }
    }
}
