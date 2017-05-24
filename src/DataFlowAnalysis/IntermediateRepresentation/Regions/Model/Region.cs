using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode.Model;
using System.Collections.Generic;

namespace DataFlowAnalysis.IntermediateRepresentation.Regions.Model
{
    public abstract class Region
    {
        public abstract List<int> OutputBlocks { get; }

        public abstract BasicBlock Header { get; }

        public abstract Region RegionParent { get; set; }
    }
}
