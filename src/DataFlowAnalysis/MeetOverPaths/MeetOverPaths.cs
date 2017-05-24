using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataFlowAnalysis.IterativeAlgorithm.IterativeAlgorithmParameters;
using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode.Model;
using DataFlowAnalysis.IntermediateRepresentation.ControlFlowGraph;
using DataFlowAnalysis.IterativeAlgorithm;

namespace DataFlowAnalysis.MeetOverPaths
{
    public class MeetOverPaths
    {
        public static Dictionary<int, V> Apply<V>(Graph graph, BasicIterativeAlgorithmParameters<V> param, Dictionary<int, int> order = null)
        {
            var MOP = new Dictionary<int, V>();

            foreach (BasicBlock blockTo in graph)
            {
                MOP[blockTo.BlockId] = param.StartingValue;
                foreach (var path in GraphAlgorithms.FindAllPaths(graph, blockTo.BlockId))
                {
                    var value = path.Aggregate(param.FirstValue, param.TransferFunction);
                    MOP[blockTo.BlockId] = param.GatherOperation(new List<V> { MOP[blockTo.BlockId], value});
                }

            }

            return MOP;
        }

    }
}
