using System;
using System.Collections.Generic;
using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode.Model;
using DataFlowAnalysis.IterativeAlgorithm.IterativeAlgorithmParameters.Model;
using DataFlowAnalysis.IntermediateRepresentation.ControlFlowGraph;
using DataFlowAnalysis.SpecificIterativeAlgorithmParametrs.ReachingDefinitions.GenKillCalculator;
using DataFlowAnalysis.Utilities;
using DataFlowAnalysis.IterativeAlgorithm.IterativeAlgorithmParameters;
using DataFlowAnalysis.SpecificIterativeAlgorithmParametrs.ReachingDefinitions.GenKillCalculator.Model;
using System.Linq;

namespace DataFlowAnalysis.SpecificIterativeAlgorithmParametrs.ReachingDefinitions.ExplicitTransferFunction
{
    public class ExplicitTransferFunction : SetIterativeAlgorithmParameters<CommandNumber>
    {
        private GenKillOneCommandCalculator commandCalc;

        public ExplicitTransferFunction(Graph g)
        {
            commandCalc = new GenKillOneCommandCalculator(g);
        }

        public override ISet<CommandNumber> FirstValue
        {
            get
            {
                return SetFactory.GetSet<CommandNumber>();
            }
        }

        public override bool ForwardDirection
        {
            get
            {
                return true;
            }
        }

        public override ISet<CommandNumber> StartingValue
        {
            get
            {
                return SetFactory.GetSet<CommandNumber>();
            }
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
            List<GenKillOneCommand> listGenKill = block.Commands.Select((x, i) => commandCalc.CalculateGenAndKill(block, i)).ToList();
            
            ISet<CommandNumber> genB = SetFactory.GetSet<CommandNumber>();
            ISet<CommandNumber> killS = SetFactory.GetSet<CommandNumber>(); 
            for (int i = block.Commands.Count - 1; i >= 0; i--)
            {
                ISet<CommandNumber> genS = SetFactory.GetSet<CommandNumber>();
                if(listGenKill[i].Gen != null)
                    genS.Add(listGenKill[i].Gen);
                genS.ExceptWith(killS);
                genB.UnionWith(genS);

                killS.UnionWith(listGenKill[i].Kill);
            }

            return genB;
        }

        public override ISet<CommandNumber> GetKill(BasicBlock block)
        {
            return block.Commands.Select((b, i) => commandCalc.CalculateGenAndKill(block, i).Kill).Aggregate(SetFactory.GetSet<CommandNumber>(), 
            (result, x) =>
            {
                result.UnionWith(x);
                return result;
            });
            
        }
    }
}

