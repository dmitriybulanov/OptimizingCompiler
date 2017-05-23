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

        protected bool Equals(LabelledStatement other)
        {
            return string.Equals(Label, other.Label) && Equals(Statement, other.Statement);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((LabelledStatement) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Label != null ? Label.GetHashCode() : 0) * 397) ^ (Statement != null ? Statement.GetHashCode() : 0);
            }
        }
    }
}
