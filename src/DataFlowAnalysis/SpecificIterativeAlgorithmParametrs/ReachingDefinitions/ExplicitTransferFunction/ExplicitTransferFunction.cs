using System;
using System.Collections.Generic;
using DataFlowAnalysis.IterativeAlgorithmParameters;
using DataFlowAnalysis.BasicBlockCode.Model;
using DataFlowAnalysis.IterativeAlgorithmParameters.Model;
using DataFlowAnalysis.ControlFlowGraph;
using DataFlowAnalysis.GenKillCalculator;
using DataFlowAnalysis.Utilities;

namespace DataFlowAnalysis.SpecificIterativeAlgorithmParametrs.ReachingDefinitions.ExplicitTransferFunction
{
    class ExplicitTransferFunction : SetIterativeAlgorithmParameters<CommandNumber>
    {
        private GenKillOneCommandCalculator commandCalc;

        public ExplicitTransferFunction(Graph g)
        {
            commandCalc = new GenKillOneCommandCalculator(g);
        }

        public override ISet<CommandNumber> GatherOperation(IEnumerable<ISet<CommandNumber>> blocks)
        {
            ISet<CommandNumber> res = SetFactory.GetSet<CommandNumber>();
            foreach (var command in blocks)
                res.UnionWith(command);
            return res;
        }

        public override ISet<CommandNumber> GetGen(BasicBlock block)
        {
            //list of kill-sets for every command in block
            List<ISet<CommandNumber>> listKill = new List<ISet<CommandNumber>>(block.Commands.Count);
            foreach (var command in block.Commands)
                listKill.Add(commandCalc.CalculateGenAndKill(block, command).Kill);

            ISet<CommandNumber> genB = SetFactory.GetSet<CommandNumber>();

            for (int i = block.Commands.Count - 1; i >= 0; i--)
            {
                ISet<CommandNumber> genS = SetFactory.GetSet<CommandNumber>();
                genS.Add(commandCalc.CalculateGenAndKill(block, block.Commands[i]).Gen);
                for (int j = i; j < block.Commands.Count - 1; j++)
                {
                    genS.IntersectWith(listKill[i]);
                }
                genB.UnionWith(genS);
            }

            return genB;
        }

        public override ISet<CommandNumber> GetKill(BasicBlock block)
        {
            ISet<CommandNumber> killB = SetFactory.GetSet<CommandNumber>();
            foreach (var command in block.Commands)
                killB.UnionWith(commandCalc.CalculateGenAndKill(block, command).Kill);
            return killB;
        }
    }
}

