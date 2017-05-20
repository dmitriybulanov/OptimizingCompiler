using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataFlowAnalysis.IntermediateRepresentation.ControlFlowGraph;

namespace DataFlowAnalysis.SpecificIterativeAlgorithmParametrs.Dominators
{
    public static class ImmediateDominator
    {
        public static Dictionary<int, int> FindImmediateDominator(Graph g)
        {
            var _out = IterativeAlgorithm.IterativeAlgorithm.Apply(g, new DominatorsIterativeAlgorithmParametrs(g)).Out;

            int min = _out.Keys.Min();

            return _out.Select(x => new KeyValuePair<int, int>(x.Key,
                                            x.Key > min ? _out[x.Key].Take(_out[x.Key].Count - 1).Last() : min))
                                          .ToDictionary(x => x.Key, x => x.Value);
        }
    }
}
