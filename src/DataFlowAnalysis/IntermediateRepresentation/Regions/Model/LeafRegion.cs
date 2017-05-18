using System;
using System.Collections.Generic;
using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode.Model;

namespace DataFlowAnalysis.IntermediateRepresentation.Regions.Model
{
    public class LeafRegion : Region
    {
        public BasicBlock Block { get; set; }

        public override List<int> OutputBlocks
        {
            get
            {
                return Block.OutputBlocks;
            }
        }
        public LeafRegion(BasicBlock block)
        {
            Block = block;
        }

        protected bool Equals(LeafRegion other)
        {
            return Equals(Block, other.Block);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((LeafRegion) obj);
        }

        public override int GetHashCode()
        {
            return (Block != null ? Block.GetHashCode() : 0);
        }
    }
}
