using System.Collections.Generic;
using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode.Model;
using DataFlowAnalysis.IntermediateRepresentation.EdgeClassification.Model;
using DataFlowAnalysis.SpecificIterativeAlgorithmParametrs.Dominators;
using QuickGraph;

namespace DataFlowAnalysis.IntermediateRepresentation.EdgeClassification
{
    public static class EdgeClassification
    {
        public static Dictionary<Edge<BasicBlock>, EdgeType> ClassifyEdge(ControlFlowGraph.Graph g)
        {
            Dictionary<Edge<BasicBlock>, EdgeType> edgeTypes = new Dictionary<Edge<BasicBlock>, EdgeType>();
            Dictionary<int, int> dfn = g.GetDFN();
            DominatorsTree tree = new DominatorsTree(g);
            foreach (Edge<BasicBlock> e in g.GetEdges())
            {
                if (dfn[e.Source.BlockId] >= dfn[e.Target.BlockId])
                    edgeTypes.Add(e, EdgeType.Retreating);
                else if (g.IsAncestor(e.Target.BlockId, e.Source.BlockId) && tree.GetParent(e.Target.BlockId) == e.Source.BlockId)
                    edgeTypes.Add(e, EdgeType.Advancing);
                else
                    edgeTypes.Add(e, EdgeType.Cross);
            }

            return edgeTypes;
        }
    }
}

