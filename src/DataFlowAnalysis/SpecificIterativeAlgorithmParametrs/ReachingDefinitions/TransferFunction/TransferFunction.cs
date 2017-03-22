using DataFlowAnalysis.BasicBlockCode.Model;
using DataFlowAnalysis.GenKillCalculator;
using DataFlowAnalysis.GenKillCalculator.Model;
using DataFlowAnalysis.Utilities;
using DataFlowAnalysis.IterativeAlgorithmParameters.Model;
using System.Collections.Generic;

namespace DataFlowAnalysis.ReachingDefinitions.TransferFunction
{
    public static class TransferFunction
    {
        public static ISet<CommandNumber> CalculateTransferFunction(GenKillOneCommandCalculator calc, BasicBlock block, ISet<CommandNumber> x)
        {
            var result = x;
            foreach (var c in block.Commands)
            {
                GenKillOneCommand genKill = calc.CalculateGenAndKill(block, c);
                result.ExceptWith(genKill.Kill);
                result.UnionWith(SetFactory.GetSet(genKill.Gen));
            }
            return result;
        }
    }
}
