using System.Collections.Generic;
using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode.Model;

namespace DataFlowAnalysis.IntermediateRepresentation.Regions.Model
{
    public class LoopRegion : IntermediateRegion
    {
        public BodyRegion Body { get; set; }

        public LoopRegion(BasicBlock header, ISet<BasicBlock> outputBlocks, BodyRegion body) : base(header, outputBlocks)
        {
            Body = body;
        }

        public LoopRegion(BodyRegion body) : base(body.Header, body.OutputBlocks)
        {
            Body = body;
        }
    }
}
