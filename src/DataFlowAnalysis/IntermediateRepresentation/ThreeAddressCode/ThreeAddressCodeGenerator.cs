using System.Collections.Generic;
using SyntaxTree;
using SyntaxTree.SyntaxNodes;
using SyntaxTree.Visitors;
using DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode.Model;
using Expression = DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode.Model.Expression;
using Identifier = DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode.Model.Identifier;
using Int32Const = DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode.Model.Int32Const;
using Program = DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode.Model.Program;

namespace DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode
{
    /// <summary>
    /// Генератор трехадресного кода по синтаксическому дереву
    /// </summary>
    public class ThreeAddressCodeGenerator : WalkingVisitor
    {
        protected int LabelNumber { get; set; } = 1;
        protected string NewLabel => LabelNumber++.ToString();

        protected int GeneratedVariableNumber { get; set; }
        protected string NewVariable => $"t{GeneratedVariableNumber++}";

        public Program Program { get; } = new Program();
        protected Stack<SimpleExpression> ExpressionStack { get; } = new Stack<SimpleExpression>();

        public static ThreeAddressCodeGenerator CreateAndVisit(SyntaxNode root)
        {
            var visitor = new ThreeAddressCodeGenerator();
            visitor.Visit(root);
            return visitor;
        }

        public override void VisitAssignment(AssignmentStatement assignment)
        {
            Visit(assignment.Value);
            var newCommand = new Assignment(assignment.VariableName.Name, ExpressionStack.Pop());
            Program.Add(newCommand);
        }

        public override void VisitBinaryExpression(BinaryExpression binaryExpression)
        {
            Visit(binaryExpression.Left);
            var leftExpression = ExpressionStack.Pop();
            Visit(binaryExpression.Right);
            var rightExpression = ExpressionStack.Pop();
            var variable = new Identifier(NewVariable);
            var binOp = new BinaryOperation(leftExpression, binaryExpression.Operation, rightExpression);
            Program.Add(new Assignment(variable.Name, binOp));
            ExpressionStack.Push(variable);
        }

        public override void VisitUnaryExpression(UnaryExpression unaryExpression)
        {
            Visit(unaryExpression.Operand);
            var unaryOp = new UnaryOperation(unaryExpression.Operation, ExpressionStack.Pop());
            var variable = new Identifier(NewVariable);
            var assignment = new Assignment(variable.Name, unaryOp);
            Program.Add(assignment);
            ExpressionStack.Push(variable);
        }

        public override void VisitFor(ForStatement forStatement)
        {
            Visit(forStatement.From);
            Program.Add(new Assignment(forStatement.ForVariable.Name, ExpressionStack.Pop()));
            var forBegin = NewLabel;
            var forEnd = NewLabel;
            var forVariable = new Identifier(forStatement.ForVariable.Name);
            Visit(forStatement.To);
            var forTo = ExpressionStack.Pop();
            var check = new ConditionalGoto(
                forEnd,
                new BinaryOperation(forVariable, Operation.Greater, forTo)) {Label = forBegin};
            Program.Add(check);
            Visit(forStatement.Body);
            Program.Add(Assignment.Increment(forVariable.Name));
            Program.Add(new Goto(forBegin));
            Program.Add(new NoOperation(forEnd));
        }

        public override void VisitIdentifier(SyntaxTree.SyntaxNodes.Identifier identifier)
        {
            ExpressionStack.Push(new Identifier(identifier.Name));
        }

        public override void VisitIf(IfStatement ifStatement)
        {
            Visit(ifStatement.Condition);
            var condition = ExpressionStack.Pop();
            var endIfLabel = NewLabel;
            var elseLabel = ifStatement.ElseBody == null ? endIfLabel : NewLabel;
            Program.Add(new ConditionalGoto(
                elseLabel, 
                new BinaryOperation(condition, Operation.Equal, 0)));
            Visit(ifStatement.ThenBody);

            if (ifStatement.ElseBody != null)
            {
                Program.Add(new Goto(endIfLabel));
                Program.Add(new NoOperation(elseLabel));
                Visit(ifStatement.ElseBody);
                Program.Add(new NoOperation(endIfLabel));
            }
            else
                Program.Add(new NoOperation(elseLabel));
        }

        public override void VisitInt32Const(SyntaxTree.SyntaxNodes.Int32Const int32Const)
        {
            ExpressionStack.Push(new Int32Const(int32Const.Value));
        }

        public override void VisitWhile(WhileStatement whileStatement)
        {
            Visit(whileStatement.Condition);
            var condition = ExpressionStack.Pop();
            var beginWhile = NewLabel;
            var endWhile = NewLabel;
            Program.Add(new ConditionalGoto(
                endWhile, 
                new BinaryOperation(condition, Operation.Equal, 0)) { Label = beginWhile });
            Visit(whileStatement.Body);
            Program.Add(new Goto(beginWhile));
            Program.Add(new NoOperation(endWhile));
        }

        public override void VisitPrint(PrintStatement printStatement)
        {
            Visit(printStatement.Expression);
            var print = new Print(ExpressionStack.Pop(), printStatement.NewLine);
            Program.Add(print);
        }
    }
}
