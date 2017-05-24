using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataFlowAnalysis.IntermediateRepresentation.ControlFlowGraph;
using DataFlowAnalysis.IterativeAlgorithm;
using DataFlowAnalysis.SpecificIterativeAlgorithmParametrs.ConstantsPropagation;

namespace Optimizer.Optimizations
{
    public class ConstantPropagation : IOptimization
    {
        public Graph Apply(Graph graph)
        {
            var constants = IterativeAlgorithm.Apply(graph, new ConstantsPropagationParameters());

            return graph;
        }
    }
}
