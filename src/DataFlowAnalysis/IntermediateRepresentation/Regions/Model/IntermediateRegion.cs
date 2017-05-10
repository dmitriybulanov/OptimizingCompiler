using System.Collections.Generic;

using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode.Model;

namespace DataFlowAnalysis.IntermediateRepresentation.Regions.Model
{
    public class IntermediateRegion : Region
    {
        public BasicBlock Header { get; set; }
        public ISet<BasicBlock> OutputBlocks { get; set; }

        public IntermediateRegion(BasicBlock header, ISet<BasicBlock> outputBlocks)
        {
            Header = header;
            OutputBlocks = outputBlocks;
        }
    }
}
