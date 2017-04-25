using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFlowAnalysis.Dominators
{
    public static class ImmediateDominator
    {
        public static Dictionary<int, int> FindImmediateDominator(ControlFlowGraph.Graph g)
        {
            var _out = IterativeAlgorithm.IterativeAlgorithm.Apply<DominatorsIterativeAlgorithmParametrs, ISet<int>>
                (g, new DominatorsIterativeAlgorithmParametrs(g)).Out;

            return _out.Select(x => new KeyValuePair<int, int>(x.Key,
                                          x.Key > 0 ? _out[x.Key].Intersect(g.getBlockById(x.Key).InputBlocks).First() : 0))
                                          .ToDictionary(x => x.Key, x => x.Value);
        }
    }
}
