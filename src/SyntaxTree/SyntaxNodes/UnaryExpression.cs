using SyntaxTree.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxTree.SyntaxNodes
{
    public class UnaryExpression : Expression
    {
        public Operation Operation { get; set; }
        public Expression Operand { get; set; }

        public UnaryExpression(Operation operation, Expression operand)
        {
            Operation = operation;
            Operand = operand;
        }

        public UnaryExpression() { }

        public override void Accept(AbstractVisitor visitor)
        {
            visitor.VisitUnaryExpression(this);
        }

        public override T Accept<T>(AbstractVisitor<T> visitor)
        {
            return visitor.VisitUnaryExpression(this);
        }

        protected bool Equals(UnaryExpression other)
        {
            return Operation == other.Operation && Equals(Operand, other.Operand);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((UnaryExpression) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int) Operation * 397) ^ (Operand != null ? Operand.GetHashCode() : 0);
            }
        }
    }
}
