using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode.Model;
using QuickGraph;

namespace DataFlowAnalysis.IntermediateRepresentation.Regions.Model
{
    public class BodyRegion : Region
    {
        protected BasicBlock header;
        protected ISet<BasicBlock> nodes;
        protected ISet<Edge<BasicBlock>> edges;

        public BodyRegion(BasicBlock header, ISet<BasicBlock> nodes, ISet<Edge<BasicBlock>> edges)
        {
            this.header = header;
            this.nodes = nodes;
            this.edges = edges;
        }

        public BodyRegion(BodyRegion body)
        {
            header = body.header;
            nodes = body.nodes;
            edges = body.edges;
        }

        public override BasicBlock Header
        {
            get
            {
                return header;
            }
        }

        public override ISet<BasicBlock> Nodes
        {
            get
            {
                return nodes;
            }
        }

        public override ISet<Edge<BasicBlock>> Edges
        {
            get
            {
                return edges;
            }
        }
    }
}
