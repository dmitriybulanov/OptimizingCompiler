using System.Collections.Generic;
using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode.Model;

namespace DataFlowAnalysis.IntermediateRepresentation.Regions.Model
{
    public class LoopRegion : IntermediateRegion
    {
        public BodyRegion Body { get; set; }

        public LoopRegion(BasicBlock header, List<int> outputBlocks, BodyRegion body) : base(header, outputBlocks)
        {
            Body = body;
        }

        public LoopRegion(BodyRegion body) : base(body.Header, body.OutputBlocks)
        {
            Body = body;
        }

        protected bool Equals(LoopRegion other)
        {
            return base.Equals(other) && Equals(Body, other.Body);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((LoopRegion) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ (Body != null ? Body.GetHashCode() : 0);
            }
        }

        public override string ToString()
        {
            return string.Format("[LoopRegion: Body={0}]", Body.ToString());
        }

    }
}
