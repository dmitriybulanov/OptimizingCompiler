using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyntaxTree.SyntaxNodes;

namespace SyntaxTree.Visitors
{
    /// <summary>
    /// Визитор создания форматированного кода по синтаксическому дереву.
    /// </summary>
    public class PrettyPrinter : WalkingVisitor
    {
        private const string SingleIndent = "  ";

        private int _indentCount;
        private bool _enteredNewLine = true;
        private readonly StringBuilder _formattedCode = new StringBuilder();

        private void VisitWithPossibleIndent(Statement statement)
        {
            var indentNeeded = !(statement is Block);
            if (indentNeeded)
                IndentCount++;
            Visit(statement);
            if (indentNeeded)
                IndentCount--;
        }

        protected void Write(string code)
        {
            _formattedCode.Append(_enteredNewLine ? Indent + code : code);
            _enteredNewLine = false;
        }

        protected void WriteLine()
        {
            _formattedCode.Append(Environment.NewLine);
            _enteredNewLine = true;
        }

        protected void WriteLine(string codeLine)
        {
            _formattedCode.AppendLine(Indent + codeLine);
            _enteredNewLine = true;
        }

        protected int IndentCount
        {
            get { return _indentCount; }
            set
            {
                Debug.Assert(value >= 0);
                Indent = string.Concat(Enumerable.Repeat(SingleIndent, value));
                _indentCount = value;
            }
        }

        protected string Indent { get; private set; } = "";

        public static PrettyPrinter CreateAndVisit(SyntaxNode root)
        {
            var visitor = new PrettyPrinter();
            visitor.Visit(root);
            return visitor;
        }

        public string FormattedCode => _formattedCode.ToString();

        public override void VisitAssignment(AssignmentStatement assignment)
        {
            Visit(assignment.VariableName);
            Write(" = ");
            Visit(assignment.Value);
            Write(";");
        }

        public override void VisitBinaryExpression(BinaryExpression binaryExpression)
        {
            Visit(binaryExpression.Left);
            Write(" " + binaryExpression.Operation.ToText() + " ");
            Visit(binaryExpression.Right);
        }

        public override void VisitBlock(Block block)
        {
            WriteLine("{");
            IndentCount++;
            Visit(block.Body);
            IndentCount--;
            Write("}");
        }

        public override void VisitFor(ForStatement forStatement)
        {
            Write($"for {forStatement.ForVariable} = ");
            Visit(forStatement.From);
            Write(" .. ");
            Visit(forStatement.To);
            WriteLine();
            VisitWithPossibleIndent(forStatement.Body);
        }

        public override void VisitIdentifier(Identifier identifier)
        {
            Write(identifier.Name);
        }

        public override void VisitIf(IfStatement ifStatement)
        {
            Write("if ");
            Visit(ifStatement.Condition);
            WriteLine();
            VisitWithPossibleIndent(ifStatement.ThenBody);
            if (ifStatement.ElseBody != null)
            {
                WriteLine();
                WriteLine("else");
                VisitWithPossibleIndent(ifStatement.ElseBody);
            }
        }

        public override void VisitInt32Const(Int32Const int32Const)
        {
            Write(int32Const.Value.ToString());
        }

        public override void VisitPrint(PrintStatement printStatement)
        {
            Write(printStatement.NewLine ? "println(" : "print(");
            Visit(printStatement.Expression);
            Write(");");
        }

        public override void VisitStatementList(StatementList statementList)
        {
            foreach (var statement in statementList.Statements)
            {
                Visit(statement);
                WriteLine();
            }
        }

        public override void VisitWhile(WhileStatement whileStatement)
        {
            Write("while ");
            Visit(whileStatement.Condition);
            WriteLine();
            VisitWithPossibleIndent(whileStatement.Body);
        }

        public override void VisitUnaryExpression(UnaryExpression unaryExpression)
        {
            Write(unaryExpression.Operation.ToText());
            Visit(unaryExpression.Operand);
        }

        public override void VisitParenthesizedExpression(ParenthesizedExpression parenthesizedExpression)
        {
            Write("(");
            Visit(parenthesizedExpression.Expression);
            Write(")");
        }

        public override void VisitGotoStatement(GotoStatement gotoStatement)
        {
            WriteLine($"goto {gotoStatement.Label};");
        }

        public override void VisitLabelledStatement(LabelledStatement labelledStatement)
        {
            Write(labelledStatement.Label + ": ");
            Visit(labelledStatement.Statement);
        }
    }
}
