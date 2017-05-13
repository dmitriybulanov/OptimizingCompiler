using System.Collections.Generic;
using System.Linq;
using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode.Model;

namespace DataFlowAnalysis.IntermediateRepresentation.Regions.Model
{
    public class IntermediateRegion : Region
    {
        public BasicBlock Header { get; set; }
        public List<int> OutputBlocks { get; set; }

        public IntermediateRegion(BasicBlock header, List<int> outputBlocks)
        {
            Header = header;
            OutputBlocks = outputBlocks;
        }

        protected bool Equals(IntermediateRegion other)
        {
            return Equals(Header, other.Header) && OutputBlocks.SequenceEqual(other.OutputBlocks);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((IntermediateRegion) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Header != null ? Header.GetHashCode() : 0) * 397) ^ (OutputBlocks != null ? OutputBlocks.GetHashCode() : 0);
            }
        }
    }
}
