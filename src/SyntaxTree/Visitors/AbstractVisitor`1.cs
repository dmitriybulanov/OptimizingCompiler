using SyntaxTree.SyntaxNodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxTree.Visitors
{
    /// <summary>
    /// Базовый синтаксический визитор с возвращаемым значнием. Посещает только один, поданый на вход узел. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AbstractVisitor<T>
    {
        protected virtual T DefaultVisit(SyntaxNode node)
        {
            return default(T);
        }

        public virtual T Visit(SyntaxNode syntaxNode)
        {
            return syntaxNode != null ? syntaxNode.Accept(this) : default(T);
        }

        public virtual T VisitStatement(Statement statement)
        {
            return DefaultVisit(statement);
        }

        public virtual T VisitExpression(Expression expression)
        {
            return DefaultVisit(expression);
        }

        public virtual T VisitAssignment(AssignmentStatement assignment)
        {
            return DefaultVisit(assignment);
        }

        public virtual T VisitBinaryExpression(BinaryExpression binaryExpression)
        {
            return DefaultVisit(binaryExpression);
        }

        public virtual T VisitBlock(Block block)
        {
            return DefaultVisit(block);
        }

        public virtual T VisitFor(ForStatement forStatement)
        {
            return DefaultVisit(forStatement);
        }

        public virtual T VisitIdentifier(Identifier identifier)
        {
            return DefaultVisit(identifier);
        }

        public virtual T VisitIf(IfStatement ifStatement)
        {
            return DefaultVisit(ifStatement);
        }

        public virtual T VisitInt32Const(Int32Const int32Const)
        {
            return DefaultVisit(int32Const);
        }

        public virtual T VisitPrint(PrintStatement printStatement)
        {
            return DefaultVisit(printStatement);
        }

        public virtual T VisitProgram(Program program)
        {
            return DefaultVisit(program);
        }

        public virtual T VisitStatementList(StatementList statementList)
        {
            return DefaultVisit(statementList);
        }

        public virtual T VisitWhile(WhileStatement whileStatement)
        {
            return DefaultVisit(whileStatement);
        }

        public virtual T VisitUnaryExpression(UnaryExpression unaryExpression)
        {
            return DefaultVisit(unaryExpression);
        }

        public virtual T VisitParenthesizedExpression(ParenthesizedExpression parenthesizedExpression)
        {
            return DefaultVisit(parenthesizedExpression);
        }

        public virtual T VisitGotoStatement(GotoStatement gotoStatement)
        {
            return DefaultVisit(gotoStatement);
        }
    }
}
