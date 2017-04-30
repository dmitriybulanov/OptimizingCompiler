using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyntaxTree.SyntaxNodes;

namespace SyntaxTree.Visitors
{
    /// <summary>
    /// Базовый визитор, посещающий все синтаксическое дерево в depth-first порядке
    /// </summary>
    public abstract class WalkingVisitor : AbstractVisitor
    {
        public override void VisitAssignment(AssignmentStatement assignment)
        {
            Visit(assignment.VariableName);
            Visit(assignment.Value);
        }

        public override void VisitBinaryExpression(BinaryExpression binaryExpression)
        {
            Visit(binaryExpression.Left);
            Visit(binaryExpression.Right);
        }

        public override void VisitBlock(Block block)
        {
            Visit(block.Body);
        }

        public override void VisitFor(ForStatement forStatement)
        {
            Visit(forStatement.ForVariable);
            Visit(forStatement.From);
            Visit(forStatement.To);
            Visit(forStatement.Body);
        }

        public override void VisitIf(IfStatement ifStatement)
        {
            Visit(ifStatement.Condition);
            Visit(ifStatement.ThenBody);
            Visit(ifStatement.ElseBody);
        }

        public override void VisitPrint(PrintStatement printStatement)
        {
            Visit(printStatement.Expression);
        }

        public override void VisitProgram(Program program)
        {
            Visit(program.Body);
        }

        public override void VisitStatementList(StatementList statementList)
        {
            foreach (var statement in statementList.Statements)
            {
                Visit(statement);
            }
        }

        public override void VisitWhile(WhileStatement whileStatement)
        {
            Visit(whileStatement.Condition);
            Visit(whileStatement.Body);
        }

        public override void VisitUnaryExpression(UnaryExpression unaryExpression)
        {
            Visit(unaryExpression.Operand);
        }

        public override void VisitParenthesizedExpression(ParenthesizedExpression parenthesizedExpression)
        {
            Visit(parenthesizedExpression.Expression);
        }

        public override void VisitLabelledStatement(LabelledStatement labelledStatement)
        {
            Visit(labelledStatement.Statement);
        }
    }
}
