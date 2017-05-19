using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode.Model;
using DataFlowAnalysis.IntermediateRepresentation.ControlFlowGraph;
using DataFlowAnalysis.IterativeAlgorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataFlowAnalysis.IterativeAlgorithm.IterativeAlgorithmParameters;

namespace DataFlowAnalysis.IterativeAlgorithm
{
    public static class IterativeAlgorithm
    {
        public static IterativeAlgorithmOutput<V> Apply<V>(Graph graph, BasicIterativeAlgorithmParameters<V> param, int[] order = null)
        {
            IterativeAlgorithmOutput<V> result = new IterativeAlgorithmOutput<V>();
            
            foreach (BasicBlock bb in graph)
                result.Out[bb.BlockId] = param.StartingValue;
            IEnumerable<BasicBlock> g = order == null ? graph : order.Select(i => graph.getBlockById(i));
            bool changed = true;
            while (changed)
            {
                changed = false;
                foreach (BasicBlock bb in g)
                {
                    BasicBlocksList parents = param.ForwardDirection ? graph.getParents(bb.BlockId) : graph.getChildren(bb.BlockId);
                    if (parents.Blocks.Count > 0)
                        result.In[bb.BlockId] = param.GatherOperation(parents.Blocks.Select(b => result.Out[b.BlockId]));
                    else
                        result.In[bb.BlockId] = param.FirstValue;
                    V newOut = param.TransferFunction(result.In[bb.BlockId], bb);
                    changed = changed || !param.AreEqual(result.Out[bb.BlockId], newOut);
                    result.Out[bb.BlockId] = param.TransferFunction(result.In[bb.BlockId], bb);
                }
            }
            if (!param.ForwardDirection)
                result = new IterativeAlgorithmOutput<V> { In = result.Out, Out = result.In };
            return result;
        }
    }
}
