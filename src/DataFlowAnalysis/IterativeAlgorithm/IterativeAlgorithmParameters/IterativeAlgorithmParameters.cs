using DataFlowAnalysis.BasicBlockCode.Model;
using DataFlowAnalysis.IterativeAlgorithmParameters.Model;
using DataFlowAnalysis.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace DataFlowAnalysis.IntermediateRepresentation.IterativeAlgorithmParameters
{
    public abstract class IterativeAlgorithmParameters<T> : BasicIterativeAlgorithmParameters<ISet<T>>
    {
        public override ISet<T> TransferFunction(ISet<T> input, BasicBlock block)
        {
            return SetFactory.GetSet<T>(GetGen(block).Union(input.Except(GetKill(block))));
        }

        public abstract ISet<T> GetGen(BasicBlock block);

        public abstract ISet<T> GetKill(BasicBlock block);

        public override bool Compare(ISet<T> t1, ISet<T> t2)
        {
            return t1.IsSubsetOf(t2) && t2.IsSubsetOf(t1);
        }    
    }
   
}
