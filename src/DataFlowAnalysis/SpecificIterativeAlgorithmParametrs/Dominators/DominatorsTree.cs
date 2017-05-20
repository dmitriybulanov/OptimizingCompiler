using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataFlowAnalysis.IntermediateRepresentation.ControlFlowGraph;

using QuickGraph;

namespace DataFlowAnalysis.SpecificIterativeAlgorithmParametrs.Dominators
{
    public class DominatorsTree : IEnumerable
    {
        private AdjacencyGraph<int, Edge<int>> Tree = new AdjacencyGraph<int, Edge<int>>();
        private Dictionary<int, int> Map;

        public DominatorsTree(Graph g)
        {
            Map = ImmediateDominator.FindImmediateDominator(g);

            Tree.AddVertexRange(Map.Keys);

            foreach (int key in Map.Keys.Skip(1))
                Tree.AddEdge(new Edge<int>(Map[key], key));
        }

        public int GetParent(int id)
        {
            return Map[id];
        }

        public List<int> GetAncestors(int id)
        {
            return Map.Where(x => x.Value == id).Select(x => x.Key).ToList();
        }

        public override string ToString()
        {
            string res = "";
            foreach (var v in Tree.Vertices)
                if (Tree.OutEdges(v).Count() > 0)
                    foreach (var e in Tree.OutEdges(v))
                        res += v + " --> " + e.Target + "\n";
            return res;
        }

        public IEnumerator GetEnumerator()
        {
            return Map.Values.GetEnumerator();
        }

        public Dictionary<int, int> GetMap()
        {
            return Map;
        }
    }
}
