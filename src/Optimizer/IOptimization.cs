using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataFlowAnalysis.IntermediateRepresentation.ControlFlowGraph;

namespace Optimizer
{
    public interface IOptimization
    {
        Graph Apply(Graph graph);
    }
}
