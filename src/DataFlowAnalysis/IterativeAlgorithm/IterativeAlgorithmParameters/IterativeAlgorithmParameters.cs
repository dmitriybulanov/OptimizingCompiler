using DataFlowAnalysis.BasicBlockCode.Model;
using DataFlowAnalysis.IterativeAlgorithmParameters.Model;
using DataFlowAnalysis.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace DataFlowAnalysis.IntermediateRepresentation.IterativeAlgorithmParameters
{
    public abstract class IterativeAlgorithmParameters<T> : BasicIterativeAlgorithmParameters<T>
    {
        public override ISet<T> TransferFunction(ISet<T> input, BasicBlock block)
        {
            return SetFactory.GetSet<T>(GetGen(block).Union(input.Except(GetKill(block))));
        }

        public abstract ISet<T> GetGen(BasicBlock block);

        public abstract ISet<T> GetKill(BasicBlock block);

    }
   
}
