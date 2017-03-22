using DataFlowAnalysis.BasicBlockCode.Model;
using DataFlowAnalysis.ControlFlowGraph;
using DataFlowAnalysis.GenKillCalculator.Model;
using DataFlowAnalysis.Utilities;
using DataFlowAnalysis.IterativeAlgorithmParameters.Model;
using DataFlowAnalysis.ThreeAddressCode.Model;
using System.Collections.Generic;
using System.Linq;

namespace DataFlowAnalysis.GenKillCalculator
{
    public class GenKillOneCommandCalculator
    {
        private CommandNumber Gen;
        private ISet<CommandNumber> Kill;
        private Dictionary<string, ISet<CommandNumber>> CommandStorage;
        private Graph graph;

        public GenKillOneCommandCalculator(Graph g)
        {
            graph = g;
            CommandStorage = new Dictionary<string, ISet<CommandNumber>>();
            for (int i = 0; i < graph.CFG.Vertices.Count(); i++)
                for (int j = 0; j < graph.CFG.Vertices.ElementAt(i).Commands.Count; j++)
                {
                    var command = graph.CFG.Vertices.ElementAt(i).Commands[j];
                    if (command is Assignment)
                        if (CommandStorage.ContainsKey((command as Assignment).Target))
                            CommandStorage[(command as Assignment).Target].Add(new CommandNumber(i, j));
                        else
                            CommandStorage.Add((command as Assignment).Target,
                                SetFactory.GetSet(new CommandNumber(i, j)));
                }
            Kill = SetFactory.GetSet< CommandNumber>();
        }

        public GenKillOneCommand CalculateGenAndKill(BasicBlock block, ThreeAddressCommand command)
        {
            Kill.Clear();
            if (command is Assignment)
            {
                Gen = new CommandNumber(block.BlockId, block.Commands.IndexOf(command));
                string target = (command as Assignment).Target;
                foreach (var c in CommandStorage[target])
                    if (c.BlockId != block.BlockId && c.CommandId != block.Commands.IndexOf(command))
                        Kill.Add(new CommandNumber(c.BlockId, c.CommandId));
            }
            return new GenKillOneCommand(Gen, Kill);
        }

        public GenKillOneCommand CalculateGenAndKill(int blockId, int commandId)
        {
            var block = graph.CFG.Vertices.ElementAt(blockId);
            return CalculateGenAndKill(block, block.Commands[commandId]);
        }

        public GenKillOneCommand CalculateGenAndKill(BasicBlock block, int commandId)
        {
            return CalculateGenAndKill(block, block.Commands[commandId]);
        }
    }
}
