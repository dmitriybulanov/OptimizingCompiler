using System.Collections.Generic;
using SyntaxTree.Visitors;

namespace SyntaxTree.SyntaxNodes
{
    public class StatementList : Statement
    {
        public List<Statement> Statements { get; set; }  = new List<Statement>();

        public  StatementList() { }

        public StatementList(Statement statement)
        {
            Statements.Add(statement);
        }

        public StatementList(IEnumerable<Statement> statements)
        {
            Statements.AddRange(statements);
        }

        public StatementList Add(Statement statement)
        {
            Statements.Add(statement);
            return this;
        }

        public override void Accept(AbstractVisitor visitor)
        {
            visitor.VisitStatementList(this);
        }

        public override T Accept<T>(AbstractVisitor<T> visitor)
        {
            return visitor.VisitStatementList(this);
        }
    }
}
