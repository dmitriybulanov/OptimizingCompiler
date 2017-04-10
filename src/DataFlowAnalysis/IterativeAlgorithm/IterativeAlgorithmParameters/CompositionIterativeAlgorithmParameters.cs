using DataFlowAnalysis.BasicBlockCode.Model;
using DataFlowAnalysis.IterativeAlgorithmParameters.Model;
using DataFlowAnalysis.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace DataFlowAnalysis.IterativeAlgorithmParameters
{
    public abstract class CompositionIterativeAlgorithmParameters<T> : BasicIterativeAlgorithmParameters<T>
    {
        public override T TransferFunction(T input, BasicBlock block)
        {
            T result = CommandTransferFunction(input, new CommandNumber(block.BlockId, 0));
            for(int i = 1; i < block.Commands.Count; ++i)
                result = CommandTransferFunction(result, new CommandNumber(block.BlockId, i));
            return result;
        }

        public abstract T CommandTransferFunction(T input, CommandNumber command);
        
    }
   
}
