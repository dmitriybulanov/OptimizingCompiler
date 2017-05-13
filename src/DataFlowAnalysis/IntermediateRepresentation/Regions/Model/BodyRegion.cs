using System.Collections.Generic;
using System.Linq;
using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode.Model;

namespace DataFlowAnalysis.IntermediateRepresentation.Regions.Model
{
    public class BodyRegion : IntermediateRegion
    {
        public List<Region> Regions { get; set; }

        public BodyRegion(BasicBlock header, List<int> outputBlocks, List<Region> regions) : base(header, outputBlocks)
        {
            Regions = regions;
        }

        protected bool Equals(BodyRegion other)
        {
            return Regions.SequenceEqual(other.Regions);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BodyRegion) obj);
        }

        public override int GetHashCode()
        {
            return (Regions != null ? Regions.GetHashCode() : 0);
        }
    }
}
