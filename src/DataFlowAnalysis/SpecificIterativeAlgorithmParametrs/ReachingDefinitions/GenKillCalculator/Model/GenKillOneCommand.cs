using System.Collections.Generic;
using DataFlowAnalysis.IterativeAlgorithm.IterativeAlgorithmParameters.Model;

namespace DataFlowAnalysis.SpecificIterativeAlgorithmParametrs.ReachingDefinitions.GenKillCalculator.Model
{
    public class GenKillOneCommand
    {
        public CommandNumber Gen { get; set; }

        public ISet<CommandNumber> Kill { get; set; }

        public GenKillOneCommand(CommandNumber gen, ISet<CommandNumber> kill)
        {
            Gen = gen;
            Kill = kill;
        }
    }
}
