using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode.Model;
using QuickGraph;

namespace DataFlowAnalysis.IntermediateRepresentation.Regions.Model
{
    public class LoopRegion : BodyRegion
    {
        protected ISet<Edge<BasicBlock>> backEdges;

        public LoopRegion(BodyRegion body, ISet<Edge<BasicBlock>> backEdges): base(body)
        {
            this.backEdges = backEdges;
        }

        public LoopRegion(BasicBlock header, ISet<BasicBlock> nodes, ISet<Edge<BasicBlock>> edges, 
                            ISet<Edge<BasicBlock>> backEdges) : base(header, nodes, edges)
        {
            this.backEdges = backEdges;
        }

        public ISet<Edge<BasicBlock>> BackEdges
        {
            get
            {
                return backEdges;
            }
        }
    }
}
