using DataFlowAnalysis.IterativeAlgorithmParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataFlowAnalysis.ThreeAddressCode.Model;
using DataFlowAnalysis.BasicBlockCode.Model;
using SyntaxTree;
using DataFlowAnalysis.Utilities;

namespace DataFlowAnalysis.SpecificIterativeAlgorithmParametrs.ConstantsPropagation
{
    public class ConstantsPropagationParameters : CompositionIterativeAlgorithmParameters<Dictionary<string, string>>
    {
        public const string NAC = "NAC";
        public const string UNDEF = "UNDEF";

        public override bool ForwardDirection { get { return true; } }

        public override Dictionary<string, string> FirstValue { get { return new Dictionary<string, string>(); } }

        public override Dictionary<string, string> StartingValue { get { return new Dictionary<string, string>(); } }

        public override Dictionary<string, string> CommandTransferFunction(Dictionary<string, string> input, BasicBlock block, int commandNumber)
        {
            ThreeAddressCommand command = block.Commands[commandNumber];
            if (command.GetType() == typeof(Assignment))
            {
                string newValue = NAC;
                Expression expr = (command as Assignment).Value;
                if (expr.GetType() == typeof(Int32Const) || expr.GetType() == typeof(Identifier))
                    newValue = getConstantFromSimpleExpression(input, (expr as SimpleExpression));
                else if (expr.GetType() == typeof(UnaryOperation))
                {
                    UnaryOperation operation = (expr as UnaryOperation);
                    newValue = calculateVal(getConstantFromSimpleExpression(input, operation.Operand), operation.Operation);
                }
                else if (expr.GetType() == typeof(BinaryOperation))
                {
                    BinaryOperation operation = (expr as BinaryOperation);
                    newValue = calculateVal(getConstantFromSimpleExpression(input, operation.Left), getConstantFromSimpleExpression(input, operation.Right), operation.Operation);
                }
                string leftOperand = (command as Assignment).Target.Name;
                input[leftOperand] = newValue;
            }
            return input;
        }

        string getConstantFromSimpleExpression(Dictionary<string, string> input, SimpleExpression expr)
        {
            string result = NAC;
            if (expr.GetType() == typeof(Int32Const))
                result = (expr as Int32Const).ToString();
            else if (expr.GetType() == typeof(Identifier))
                result = input[(expr as Identifier).ToString()];
            return result;
        }

        public override bool Compare(Dictionary<string, string> t1, Dictionary<string, string> t2)
        {
            return t1.Count == t2.Count && t1.Keys.All(key => t2.ContainsKey(key) && t1[key] == t2[key]);
        }
        string calculateVal(string x1, Operation op)
        {
            return calculateVal(x1, "0", op);
        }
        
        string calculateVal(string x1, string x2, Operation op)
        {
            if (x1 == NAC || x2 == NAC)
                return NAC;
            else if (x1 == UNDEF || x2 == UNDEF)
                return UNDEF;
            else
            {
                int lx = int.Parse(x1);
                int rx = int.Parse(x2);
                return ArithmeticOperationCalculator.Calculate(op, lx, rx).ToString();
            }
        }
        string gatherVal(string x1, string x2)
        {
            if (x1 == x2 || x2 == UNDEF || x1 == NAC)
                return x1;
            else
                return x2;
        }
        public override Dictionary<string, string> GatherOperation(IEnumerable<Dictionary<string, string>> blocks)
        {
            return blocks.Aggregate(new Dictionary<string, string>(), (result, x) =>
            {
                foreach(KeyValuePair<string, string> pair in x)
                    result[pair.Key] = result.ContainsKey(pair.Key) ? gatherVal(result[pair.Key], pair.Value) : pair.Value;
                return result;
            });
        }
    }
}
