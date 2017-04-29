using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode.Model;
using DataFlowAnalysis.IntermediateRepresentation.EdgeClassification.Model;
using QuickGraph;

namespace DataFlowAnalysis.IntermediateRepresentation.EdgeClassification
{
    public static class EdgeClassification
    {
        public static Dictionary<Edge<BasicBlock>, EdgeType> ClassifyEdge(ControlFlowGraph.Graph g)
        {
            Dictionary<Edge<BasicBlock>, EdgeType> edgeTypes = new Dictionary<Edge<BasicBlock>, EdgeType>();
            Dictionary<int, int> dfn = g.GetDFN();
            foreach (Edge<BasicBlock> e in g.GetEdges())
            {
                if (dfn[e.Source.BlockId] >= dfn[e.Target.BlockId])
                    edgeTypes.Add(e, EdgeType.Retreating);
                else if (g.IsAncestor(e.Target.BlockId, e.Source.BlockId))
                    edgeTypes.Add(e, EdgeType.Advancing);
                else
                    edgeTypes.Add(e, EdgeType.Cross);
            }

            return edgeTypes;
        }
    }
}

