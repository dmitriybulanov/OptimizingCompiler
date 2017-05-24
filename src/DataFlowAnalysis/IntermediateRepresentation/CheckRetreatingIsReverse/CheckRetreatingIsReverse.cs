using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode.Model;
using DataFlowAnalysis.IntermediateRepresentation.EdgeClassification.Model;
using DataFlowAnalysis.IntermediateRepresentation.FindReverseEdges;
using QuickGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFlowAnalysis.IntermediateRepresentation.CheckRetreatingIsReverse
{
    public class CheckRetreatingIsReverse
    {
        public static bool CheckReverseEdges(ControlFlowGraph.Graph g)
        {
            Dictionary<Edge<BasicBlock>, EdgeType> ClassifiedEdges = EdgeClassification.EdgeClassification.ClassifyEdge(g);
            var RetreatingEdges = ClassifiedEdges.Where(x => x.Value == EdgeType.Retreating).Select(x => x.Key);

            var ReverseEdges = FindReverseEdge.FindReverseEdges(g);

            return ReverseEdges.SetEquals(RetreatingEdges);
        }
    }
}
