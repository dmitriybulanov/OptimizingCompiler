using System.Collections.Generic;
using System.Linq;
using DataFlowAnalysis.BasicBlockCode.Model;
using DataFlowAnalysis.IterativeAlgorithmParameters;
using DataFlowAnalysis.Utilities;

namespace DataFlowAnalysis.Dominators
{
    public class DominatorsIterativeAlgorithmParametrs : BasicIterativeAlgorithmParameters<ISet<int>>
    {
        private ControlFlowGraph.Graph graph;

        public DominatorsIterativeAlgorithmParametrs(ControlFlowGraph.Graph g)
        {
            graph = g;
        }

        public override ISet<int> GatherOperation(IEnumerable<ISet<int>> blocks)
        {
            ISet<int> intersection = SetFactory.GetSet((IEnumerable<int>)blocks.First());
            foreach (var block in blocks.Skip(1))
                intersection.IntersectWith(block);

            return intersection;
        }

        public override ISet<int> TransferFunction(ISet<int> input, BasicBlock block)
        {
            return SetFactory.GetSet<int>(input.Union(new int[] { block.BlockId }));
        }

        public override bool Compare(ISet<int> t1, ISet<int> t2)
        {
            return t1.IsSubsetOf(t2) && t2.IsSubsetOf(t1);
        }

        public override ISet<int> StartingValue { get { return SetFactory.GetSet<int>(Enumerable.Range(0, graph.Count())); } }

        public override ISet<int> FirstValue { get { return SetFactory.GetSet<int>(Enumerable.Repeat(0, 1)); } }

        public override bool ForwardDirection { get { return true; } }

    }
}
