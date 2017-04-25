﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataFlowAnalysis.BasicBlockCode.Model;
using DataFlowAnalysis.IterativeAlgorithmParameters;
using DataFlowAnalysis.ThreeAddressCode.Model;
using DataFlowAnalysis.Utilities;
using DataFlowAnalysis.DefUseCalculator;

namespace DataFlowAnalysis.DeadAliveVariables
{
    public class DeadAliveIterativeAlgorithmParameters : SetIterativeAlgorithmParameters<string>
    {
        public override ISet<string> GatherOperation(IEnumerable<ISet<string>> blocks)
        {
            ISet<string> union = SetFactory.GetSet((IEnumerable<string>)blocks.First());
            foreach (var block in blocks.Skip(1))
            {
                union.UnionWith(block);
            }

            return union;
        }

        public override ISet<string> GetGen(BasicBlock block)
        {
            //Gen == Use
            DefUseBlockCalculator DefUseCalc = new DefUseBlockCalculator();
            return DefUseCalc.GetDefUseSetsByBlock(block).Item2;
        }

        public override ISet<string> GetKill(BasicBlock block)
        {
            //Kill = Def
            DefUseBlockCalculator DefUseCalc = new DefUseBlockCalculator();
            return DefUseCalc.GetDefUseSetsByBlock(block).Item1;
        }

        public override ISet<string> TransferFunction(ISet<string> input, BasicBlock block)
        {
            return SetFactory.GetSet(GetGen(block).Union(input.Except(GetKill(block))));
        }

        public override bool ForwardDirection { get { return false; } }

        public override ISet<string> FirstValue { get { return SetFactory.GetSet<string>(); } }

        public override ISet<string> StartingValue { get { return SetFactory.GetSet<string>(); } }
    }
}
