using DataFlowAnalysis.BasicBlockCode.Model;
using DataFlowAnalysis.IterativeAlgorithmParameters.Model;
using DataFlowAnalysis.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace DataFlowAnalysis.IntermediateRepresentation.IterativeAlgorithmParameters
{
    public abstract class IterativeAlgorithmParameters : BasicIterativeAlgorithmParameters
    {
        public override ISet<CommandNumber> TransferFunction(ISet<CommandNumber> input, BasicBlock block)
        {
            return SetFactory.GetSet<CommandNumber>(GetGen(block).Union(input.Except(GetKill(block))));
        }

        public abstract ISet<CommandNumber> GetGen(BasicBlock block);

        public abstract ISet<CommandNumber> GetKill(BasicBlock block);

    }
   
}
