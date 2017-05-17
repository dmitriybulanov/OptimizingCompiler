using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode.Model;

namespace DataFlowAnalysis.IntermediateRepresentation.ControlFlowGraph
{
    class GraphAlgorithms
    {
        /// <summary>
        /// Is used for MOP. Works only on non-cyclic graphs.
        /// </summary>
        public static IEnumerable<List<BasicBlock>> FindAllPath(Graph graph, int idTargetBlock)
        {
            //var path = new Stack<BasicBlock>();
            var pathEnumators = new Stack<IEnumerator<BasicBlock>>();
            BasicBlock first = graph.GetRoot();
            //path.Push(first);
            if (first.BlockId == idTargetBlock)
                yield break;

            var firstChildrens = graph.getChildren(first.BlockId).GetEnumerator();
            if (!firstChildrens.MoveNext())
                yield break;
            pathEnumators.Push(firstChildrens);
            while (pathEnumators.Count > 0)
            {
                var currentBlockEnumerator = pathEnumators.Peek();
                if (currentBlockEnumerator.Current.BlockId == idTargetBlock)
                {
                    yield return pathEnumators.Select(x => x.Current).ToList();
                    if (!currentBlockEnumerator.MoveNext())
                    {
                        pathEnumators.Pop();
                    }
                }
                else
                {
                    pathEnumators.Push(graph.getChildren(currentBlockEnumerator.Current.BlockId).GetEnumerator());
                    while (!currentBlockEnumerator.MoveNext())
                    {
                        pathEnumators.Pop();
                    }
                }
            }
        }
    }
}
