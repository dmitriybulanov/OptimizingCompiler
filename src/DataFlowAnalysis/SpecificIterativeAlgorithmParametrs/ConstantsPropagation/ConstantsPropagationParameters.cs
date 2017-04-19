using DataFlowAnalysis.IterativeAlgorithmParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataFlowAnalysis.BasicBlockCode.Model;

namespace DataFlowAnalysis.SpecificIterativeAlgorithmParametrs.ConstantsPropagation
{
    public class ConstantsPropagationParameters : CompositionIterativeAlgorithmParameters<Dictionary<string, string>>
    {
        public override Dictionary<string, string> CommandTransferFunction(Dictionary<string, string> input, BasicBlock block, int commandNumber)
        {
            throw new NotImplementedException();
        }

        public override bool Compare(Dictionary<string, string> t1, Dictionary<string, string> t2)
        {
            return t1.Count == t2.Count && t1.Keys.All(key => t2.ContainsKey(key) && t1[key] == t2[key]);
        }

        public override Dictionary<string, string> GatherOperation(IEnumerable<Dictionary<string, string>> blocks)
        {
            throw new NotImplementedException();
        }
    }
}
