using DataFlowAnalysis.BasicBlockCode.Model;
using DataFlowAnalysis.IterativeAlgorithmParameters.Model;
using System.Collections.Generic;

namespace DataFlowAnalysis.IntermediateRepresentation.IterativeAlgorithmParameters
{
    public abstract class BasicIterativeAlgorithmParameters<T>
    {
        public bool ForwardDirection { get; }

        public ISet<T> StartingValue { get; }

        public abstract ISet<T> GatherOperation(IEnumerable<BasicBlock> blocks);

        public abstract ISet<T> TransferFunction(ISet<T> input, BasicBlock block);
    }
}
