using System;
using SyntaxTree.Visitors;

namespace SyntaxTree.SyntaxNodes
{
    public class Program : SyntaxNode
    {
        public Statement Body
        {
            get;
            set;
        }

        public Program() { }

        public Program(Statement body)
        {
            Body = body;
        }

        public override void Accept(AbstractVisitor visitor)
        {
            visitor.VisitProgram(this);
        }

        public override T Accept<T>(AbstractVisitor<T> visitor)
        {
            return visitor.VisitProgram(this);
        }
    }
}
