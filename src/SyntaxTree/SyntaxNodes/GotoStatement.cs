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

        protected bool Equals(GotoStatement other)
        {
            return string.Equals(Label, other.Label);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((GotoStatement) obj);
        }

        public override int GetHashCode()
        {
            return (Label != null ? Label.GetHashCode() : 0);
        }
    }
}
