using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode.Model;
using DataFlowAnalysis.Utilities;
using QuickGraph;

namespace DataFlowAnalysis.IntermediateRepresentation.Regions.Model
{
    public class LeafRegion : Region
    {
        private BasicBlock block;

        public LeafRegion(BasicBlock block)
        {
            this.block = block;
        }

        public override BasicBlock Header
        {
            get
            {
                return block;
            }
        }

        public override ISet<BasicBlock> Nodes
        {
            get
            {
                return SetFactory.GetSet(new BasicBlock[] { block });
            }
        }

        public override ISet<Edge<BasicBlock>> Edges
        {
            get
            {
                return SetFactory.GetSet(new Edge<BasicBlock>[] { });
            }
        }
    }
}
