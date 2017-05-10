using System.Collections.Generic;
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
    }
}
