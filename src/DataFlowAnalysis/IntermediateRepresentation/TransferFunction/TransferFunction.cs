using DataFlowAnalysis.BasicBlockCode.Model;
using DataFlowAnalysis.GenKillCalculator;
using DataFlowAnalysis.GenKillCalculator.Model;
using System.Collections.Generic;

namespace DataFlowAnalysis.TransferFunction
{
    public static class TransferFunction
    {
        public static ISet<CommandNumber> CalculateTransferFunction(GenKillOneCommandCalculator calc, BasicBlock block, ISet<CommandNumber> x)
        {
            var result = x;
            foreach (var c in block.Commands)
            {
                var genKill = calc.CalculateGenAndKill(block, c);
                result.ExceptWith(genKill.Kill);
                result.UnionWith(new SortedSet<CommandNumber> { genKill.Gen });
            }
            return result;
        }
    }
}
