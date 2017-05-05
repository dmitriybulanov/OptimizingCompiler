using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode.Model;
using QuickGraph;

namespace DataFlowAnalysis.IntermediateRepresentation.Regions.Model
{
    public abstract class Region
    {
        public abstract BasicBlock Header { get; }

        public abstract ISet<BasicBlock> Nodes { get; }

        public abstract ISet<Edge<BasicBlock>> Edges { get; }
    }
}
