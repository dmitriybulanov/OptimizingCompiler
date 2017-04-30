using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyntaxTree.SyntaxNodes;

namespace SyntaxTree.Visitors
{
    /// <summary>
    /// Базовый синтаксический визитор. Посещает только один, поданый на вход узел.
    /// </summary>
    public abstract class AbstractVisitor
    {
        protected virtual void DefaultVisit(SyntaxNode node) { }

        public virtual void Visit(SyntaxNode syntaxNode)
        {
            syntaxNode?.Accept(this);
        }

        public virtual void VisitStatement(Statement statement)
        {
            DefaultVisit(statement);
        }

        public virtual void VisitExpression(Expression expression)
        {
            DefaultVisit(expression);
        }

        public virtual void VisitAssignment(AssignmentStatement assignment)
        {
            DefaultVisit(assignment);
        }

        public virtual void VisitBinaryExpression(BinaryExpression binaryExpression)
        {
            DefaultVisit(binaryExpression);
        }

        public virtual void VisitBlock(Block block)
        {
            DefaultVisit(block);
        }

        public virtual void VisitFor(ForStatement forStatement)
        {
            DefaultVisit(forStatement);
        }

        public virtual void VisitIdentifier(Identifier identifier)
        {
            DefaultVisit(identifier);
        }

        public virtual void VisitIf(IfStatement ifStatement)
        {
            DefaultVisit(ifStatement);
        }

        public virtual void VisitInt32Const(Int32Const int32Const)
        {
            DefaultVisit(int32Const);
        }

        public virtual void VisitPrint(PrintStatement printStatement)
        { 
            DefaultVisit(printStatement);
        }

        public virtual void VisitProgram(Program program)
        {
            DefaultVisit(program);
        }

        public virtual void VisitStatementList(StatementList statementList)
        {
            DefaultVisit(statementList);
        }

        public virtual void VisitWhile(WhileStatement whileStatement)
        {
            DefaultVisit(whileStatement);
        }

        public virtual void VisitUnaryExpression(UnaryExpression unaryExpression)
        {
            DefaultVisit(unaryExpression);
        }

        public virtual void VisitParenthesizedExpression(ParenthesizedExpression parenthesizedExpression)
        {
            DefaultVisit(parenthesizedExpression);
        }

        public virtual void VisitGotoStatement(GotoStatement gotoStatement)
        {
            DefaultVisit(gotoStatement);
        }

        public virtual void VisitLabelledStatement(LabelledStatement labelledStatement)
        {
            DefaultVisit(labelledStatement);
        }
    }
}
