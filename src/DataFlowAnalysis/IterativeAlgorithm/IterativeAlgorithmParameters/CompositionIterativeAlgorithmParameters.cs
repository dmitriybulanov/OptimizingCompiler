﻿using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode.Model;
using DataFlowAnalysis.IterativeAlgorithm.IterativeAlgorithmParameters.Model;
using DataFlowAnalysis.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace DataFlowAnalysis.IterativeAlgorithm.IterativeAlgorithmParameters
{
    public abstract class CompositionIterativeAlgorithmParameters<T> : BasicIterativeAlgorithmParameters<T>
    {
        public override T TransferFunction(T input, BasicBlock block)
        {
            return Enumerable.Range(0, block.Commands.Count).Aggregate(input, (result, c) => CommandTransferFunction(result, block, c));
        }

        public abstract T CommandTransferFunction(T input, BasicBlock block, int commandNumber);
        
    }
   
}
