using System.Collections.Generic;
using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode.Model;

namespace DataFlowAnalysis.IntermediateRepresentation.Regions.Model
{
    public class BodyRegion : IntermediateRegion
    {
        public ISet<Region> Regions { get; set; }

        public BodyRegion(BasicBlock header, ISet<BasicBlock> outputBlocks, ISet<Region> regions) : base(header, outputBlocks)
        {
            Regions = regions;
        }
    }
}
