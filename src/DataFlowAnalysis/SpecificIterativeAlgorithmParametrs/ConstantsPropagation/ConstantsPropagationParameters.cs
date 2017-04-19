using DataFlowAnalysis.IterativeAlgorithmParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataFlowAnalysis.BasicBlockCode.Model;
using SyntaxTree;

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
            throw new NotImplementedException();
        }

        public override bool Compare(Dictionary<string, string> t1, Dictionary<string, string> t2)
        {
            return t1.Count == t2.Count && t1.Keys.All(key => t2.ContainsKey(key) && t1[key] == t2[key]);
        }
        string calculateVal(string x1, Operation op)
        {
            return calculateVal(x1, "0", op);
        }
        int calculateOperation(int lx, int rx, Operation op)
        {
            switch (op)
            {
                case Operation.Add:
                    return lx + rx;
                case Operation.Divide:
                    return lx / rx;
                case Operation.Equal:
                    return lx == rx ? 1 : 0;
                case Operation.Greater:
                    return lx > rx ? 1 : 0;
                case Operation.GreaterEqual:
                    return lx >= rx ? 1 : 0;
                case Operation.Lesser:
                    return lx < rx ? 1 : 0;
                case Operation.LesserEqual:
                    return lx <= rx ? 1 : 0;
                case Operation.LogicalAnd:
                    return lx != 0 && rx != 0 ? 1 : 0;
                case Operation.LogicalOr:
                    return lx != 0 || rx != 0 ? 1 : 0;
                case Operation.Multiply:
                    return lx * rx;
                case Operation.NotEqual:
                    return lx != rx ? 1 : 0;
                case Operation.Subtract:
                    return lx - rx;
                case Operation.UnaryMinus:
                    return -lx;
                case Operation.UnaryNot:
                    return lx == 0 ? 1 : 0;
            }
            return 0;
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
                return calculateOperation(lx, rx, op).ToString();
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
