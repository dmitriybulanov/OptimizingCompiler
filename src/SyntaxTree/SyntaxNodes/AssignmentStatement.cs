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
    }
}
