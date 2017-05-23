using System.Collections.Generic;
using System.Linq;
using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode.Model;
using DataFlowAnalysis.Utilities;
using QuickGraph;

namespace DataFlowAnalysis.IntermediateRepresentation.NaturalLoops
{
    public static class SearchNaturalLoops
    {
        private static ISet<int> Loop;
        private static Stack<int> Stack;

        private static void Insert(int m)
        {
            if (!Loop.Contains(m))
            {
                Loop.Add(m);
                Stack.Push(m);
            }
        }

        public static Dictionary<Edge<BasicBlock>, ISet<int>> FindAllNaturalLoops(ControlFlowGraph.Graph g)
        {
            var classify = EdgeClassification.EdgeClassification.ClassifyEdge(g);
            var result = new Dictionary<Edge<BasicBlock>, ISet<int>>();
            foreach (var pair in classify)
            {
                if (pair.Value == EdgeClassification.Model.EdgeType.Retreating)
                {
                    Stack = new Stack<int>();
                    Loop = SetFactory.GetSet(new int[] { pair.Key.Target.BlockId });
                    Insert(pair.Key.Source.BlockId);
                    while (Stack.Count() > 0)
                    {
                        int m = Stack.Pop();
                        foreach (BasicBlock p in g.getParents(m))
                            Insert(p.BlockId);
                    }
                    result.Add(pair.Key, Loop);
                }
            }
            return result;
        }
    }
}
