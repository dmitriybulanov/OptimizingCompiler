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
        public static IEnumerable<List<BasicBlock>> FindAllPaths(Graph graph, int idTargetBlock)
        {
            var pathEnumerators = new Stack<IEnumerator<BasicBlock>>();
            var root = graph.getRoot();
            
            // when target and root are the same
            if (root.BlockId == idTargetBlock)
                yield break;

            var rootChildrens = graph.getChildren(root.BlockId).GetEnumerator();
            
            // check enumerator for emptiness 
            if (!rootChildrens.MoveNext())
                yield break;
            pathEnumerators.Push(rootChildrens);

            while (pathEnumerators.Count > 0)
            {
                var currentBlockEnumerator = pathEnumerators.Peek();
                if (currentBlockEnumerator.Current.BlockId == idTargetBlock)
                {
                    yield return pathEnumerators.Select(x => x.Current).ToList();
                    if (currentBlockEnumerator.MoveNext())
                    {
                        continue;
                    }
                }
                else
                {
                    var tmpEnumerator = graph.getChildren(currentBlockEnumerator.Current.BlockId).GetEnumerator();
            
                    if (tmpEnumerator.MoveNext())
                    {
                        pathEnumerators.Push(tmpEnumerator);
                        continue;
                    }
                }
                while (!pathEnumerators.Peek().MoveNext())
                {
                    pathEnumerators.Pop();
                }
            }
        }
    }
}
