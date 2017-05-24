using System.Collections.Generic;

namespace DataFlowAnalysis.IntermediateRepresentation.Regions.Model
{
    public abstract class Region
    {
        public abstract List<int> OutputBlocks { get; }
        public abstract Region RegionParent { get; set; }
    }
}
