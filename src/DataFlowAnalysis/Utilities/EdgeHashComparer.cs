using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode.Model;
using QuickGraph;

namespace DataFlowAnalysis.Utilities
{

    public class EdgeHashComparer : IEqualityComparer<Edge<BasicBlock>>
    {
        public bool Equals(Edge<BasicBlock> x, Edge<BasicBlock> y)
        {
            if (ReferenceEquals(null, x)) return false;
            if (ReferenceEquals(y, x)) return true;
            return x.Source.Equals(y.Source) && x.Target.Equals(y.Target);
        }

        public int GetHashCode(Edge<BasicBlock> obj)
        {
            unchecked
            {
                return obj.Source.BlockId * 397 ^ obj.Target.BlockId;
            }
        }
    }
}
