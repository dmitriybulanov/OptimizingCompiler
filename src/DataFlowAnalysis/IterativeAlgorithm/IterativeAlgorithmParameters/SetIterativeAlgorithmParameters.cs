﻿using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode.Model;
using DataFlowAnalysis.IterativeAlgorithm.IterativeAlgorithmParameters.Model;
using DataFlowAnalysis.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace DataFlowAnalysis.IterativeAlgorithm.IterativeAlgorithmParameters
{
    public abstract class SetIterativeAlgorithmParameters<T> : BasicIterativeAlgorithmParameters<ISet<T>>
    {
        public override ISet<T> TransferFunction(ISet<T> input, BasicBlock block)
        {
            return SetFactory.GetSet<T>(GetGen(block).Union(input.Except(GetKill(block))));
        }

        public abstract ISet<T> GetGen(BasicBlock block);

        public abstract ISet<T> GetKill(BasicBlock block);

        public override bool AreEqual(ISet<T> t1, ISet<T> t2)
        {
            return t1.IsSubsetOf(t2) && t2.IsSubsetOf(t1);
        }    
    }
   
}
