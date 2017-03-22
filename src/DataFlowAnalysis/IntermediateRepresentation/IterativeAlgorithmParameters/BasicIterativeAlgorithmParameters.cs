using DataFlowAnalysis.BasicBlockCode.Model;
using DataFlowAnalysis.IterativeAlgorithmParameters.Model;
using System.Collections.Generic;

namespace DataFlowAnalysis.IntermediateRepresentation.IterativeAlgorithmParameters
{
    public abstract class BasicIterativeAlgorithmParameters
    {
        public bool ForwardDirection { get; }

        public ISet<CommandNumber> StartingValue { get; }

        public abstract ISet<CommandNumber> GatherOperation(IEnumerable<BasicBlock> blocks);

        public abstract ISet<CommandNumber> TransferFunction(ISet<CommandNumber> input, BasicBlock block);
    }
}
