using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataFlowAnalysis.IntermediateRepresentation.EdgeClassification.Model;
using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode.Model;
using DataFlowAnalysis.SpecificIterativeAlgorithmParametrs.Dominators;
using DataFlowAnalysis.IntermediateRepresentation.EdgeClassification;
using QuickGraph;

namespace DataFlowAnalysis.IntermediateRepresentation.FindReverseEdges
{
    public static class FindReverseEdge
    {
        public static ISet<Edge<BasicBlock>> FindReverseEdges(ControlFlowGraph.Graph g)
        {
            ISet<Edge<BasicBlock>> res = new SortedSet<Edge <BasicBlock>>();

            Dictionary<Edge<BasicBlock>, EdgeType> ClassifiedEdges =
                EdgeClassification.EdgeClassification.ClassifyEdge(g);

            Dictionary<int, int> Dominators =
                ImmediateDominator.FindImmediateDominator(g);

            var RetreatingEdges = ClassifiedEdges.Where(x => x.Value == EdgeType.Retreating);
            foreach (var edg in RetreatingEdges)
            {
                int key = edg.Key.Source.BlockId;
                int value = edg.Key.Target.BlockId;
                bool isReverse = false;
                while (Dominators.ContainsKey(key) && !isReverse)
                {
                    key = Dominators[key];
                    isReverse = key == value;
                }
                if (isReverse)
                    res.Add(edg.Key);
            }

            return res;
        }
    }
}
