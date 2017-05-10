using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode.Model;

namespace DataFlowAnalysis.IntermediateRepresentation.Regions.Model
{
    public class LeafRegion : Region
    {
        public BasicBlock Block { get; set; }

        public LeafRegion(BasicBlock block)
        {
            Block = block;
        }
    }
}
