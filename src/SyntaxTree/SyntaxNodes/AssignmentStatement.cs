using System;
using SyntaxTree.Visitors;

namespace SyntaxTree.SyntaxNodes
{
    public class AssignmentStatement : Statement
    {

        public Identifier VariableName
        {
            get;
            set;
        }

        public Expression Value
        {
            get;
            set;
        }

        public  AssignmentStatement() { }

        public AssignmentStatement(Identifier variableName, Expression value)
        {
            VariableName = variableName;
            Value = value;
        }

        public override void Accept(AbstractVisitor visitor)
        {
            visitor.VisitAssignment(this);
        }

        public override T Accept<T>(AbstractVisitor<T> visitor)
        {
            return visitor.VisitAssignment(this);
        }

        protected bool Equals(AssignmentStatement other)
        {
            return Equals(VariableName, other.VariableName) && Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((AssignmentStatement) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((VariableName != null ? VariableName.GetHashCode() : 0) * 397) ^ (Value != null ? Value.GetHashCode() : 0);
            }
        }
    }
}
