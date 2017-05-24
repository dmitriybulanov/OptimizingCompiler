using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataFlowAnalysis.IntermediateRepresentation.EdgeClassification.Model;
using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode.Model;
using DataFlowAnalysis.SpecificIterativeAlgorithmParametrs.Dominators;
using DataFlowAnalysis.IntermediateRepresentation.EdgeClassification;
using DataFlowAnalysis.Utilities;
using QuickGraph;

namespace DataFlowAnalysis.IntermediateRepresentation.FindReverseEdges
{
    public static class FindReverseEdge
    {
        public static ISet<Edge<BasicBlock>> FindReverseEdges(ControlFlowGraph.Graph g)
        {
            ISet<Edge<BasicBlock>> res = SetFactory.GetEdgesSet();//new SortedSet<Edge <BasicBlock>>();

            Dictionary<Edge<BasicBlock>, EdgeType> ClassifiedEdges =
                EdgeClassification.EdgeClassification.ClassifyEdge(g);

            Dictionary<int, int> Dominators =
                ImmediateDominator.FindImmediateDominator(g);

            var RetreatingEdges = ClassifiedEdges.Where(x => x.Value == EdgeType.Retreating);
            foreach (var edg in RetreatingEdges)
            {
                var edge = edg.Key;
                int key = edge.Source.BlockId;
                int value = edge.Target.BlockId;
                bool isReverse = false;
                while (Dominators.ContainsKey(key) && Dominators[key] != key && !isReverse)
                {
                    key = Dominators[key];
                    isReverse = (key == value);
                }
                if (isReverse)
                    res.Add(edge);
            }

            return res;
        }
    }
}
