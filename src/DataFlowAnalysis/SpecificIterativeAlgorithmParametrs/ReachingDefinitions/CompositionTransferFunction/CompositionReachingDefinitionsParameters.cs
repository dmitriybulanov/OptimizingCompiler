using DataFlowAnalysis.SpecificIterativeAlgorithmParametrs.ReachingDefinitions.GenKillCalculator;
using DataFlowAnalysis.IterativeAlgorithm.IterativeAlgorithmParameters.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode.Model;
using DataFlowAnalysis.IntermediateRepresentation.ControlFlowGraph;
using DataFlowAnalysis.Utilities;
using DataFlowAnalysis.SpecificIterativeAlgorithmParametrs.ReachingDefinitions.GenKillCalculator.Model;
using DataFlowAnalysis.IterativeAlgorithm.IterativeAlgorithmParameters;

namespace DataFlowAnalysis.SpecificIterativeAlgorithmParametrs.ReachingDefinitions.CompositionTransferFunction
{
    public class CompositionReachingDefinitionsParameters : CompositionIterativeAlgorithmParameters<ISet<CommandNumber>>
    {
        public override bool ForwardDirection { get { return true; } }

        public override ISet<CommandNumber> FirstValue { get { return SetFactory.GetSet<CommandNumber>(); } }

        public override ISet<CommandNumber> StartingValue { get { return SetFactory.GetSet<CommandNumber>(); } }

        GenKillOneCommandCalculator calculator;
        public CompositionReachingDefinitionsParameters(Graph g)
        {
            calculator = new GenKillOneCommandCalculator(g);
        }

        public override ISet<CommandNumber> CommandTransferFunction(ISet<CommandNumber> input, BasicBlock block, int commandNumber)
        {
            GenKillOneCommand genKill = calculator.CalculateGenAndKill(block, commandNumber);
            return SetFactory.GetSet<CommandNumber>(SetFactory.GetSet(genKill.Gen).Union(input.Except(genKill.Kill)));
        }

        public override bool AreEqual(ISet<CommandNumber> t1, ISet<CommandNumber> t2)
        {
            return t1.IsSubsetOf(t2) && t2.IsSubsetOf(t1);
        }

        public override ISet<CommandNumber> GatherOperation(IEnumerable<ISet<CommandNumber>> blocks)
        {   
            return blocks.Aggregate(SetFactory.GetSet<CommandNumber>(), (res, b) =>
            {
                res.UnionWith(b);
                return res;
            });
        }
    }
}
