using System.Collections.Generic;

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
    }
}
