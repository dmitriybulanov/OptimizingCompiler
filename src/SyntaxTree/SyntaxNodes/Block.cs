using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyntaxTree.Visitors;

namespace SyntaxTree.SyntaxNodes
{
    public class Block : Statement
    {
        public StatementList Body
        {
            get;
            set;
        }

        public Block() { }

        public Block(StatementList statements)
        {
            Body = statements;
        }

        public override void Accept(AbstractVisitor visitor)
        {
            visitor.VisitBlock(this);
        }

        public override T Accept<T>(AbstractVisitor<T> visitor)
        {
            return visitor.VisitBlock(this);
        }

        protected bool Equals(Block other)
        {
            return Equals(Body, other.Body);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Block) obj);
        }

        public override int GetHashCode()
        {
            return (Body != null ? Body.GetHashCode() : 0);
        }
    }
}
