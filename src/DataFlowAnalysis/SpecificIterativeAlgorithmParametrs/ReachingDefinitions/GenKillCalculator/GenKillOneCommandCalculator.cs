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
            foreach (BasicBlock block in graph)
                for (int i = 0; i < block.Commands.Count(); i++)
                {
                    var command = block.Commands[i];
                    if (command.GetType() == typeof(Assignment))
                        if (CommandStorage.ContainsKey((command as Assignment).Target.Name))
                            CommandStorage[(command as Assignment).Target.Name].Add(new CommandNumber(block.BlockId, i));
                        else
                            CommandStorage.Add((command as Assignment).Target.Name,
                                SetFactory.GetSet(new CommandNumber(block.BlockId, i)));
                }
            Kill = SetFactory.GetSet<CommandNumber>();
        }

        public GenKillOneCommand CalculateGenAndKill(BasicBlock block, ThreeAddressCommand command)
        {
            Kill.Clear();
            if (command.GetType() == typeof(Assignment))
            {
                Gen = new CommandNumber(block.BlockId, block.Commands.IndexOf(command));
                string target = (command as Assignment).Target.Name;
                foreach (var c in CommandStorage[target])
                    if (c.BlockId != block.BlockId && c.CommandId != block.Commands.IndexOf(command))
                        Kill.Add(new CommandNumber(c.BlockId, c.CommandId));
            }
            return new GenKillOneCommand(Gen, Kill);
        }

        public GenKillOneCommand CalculateGenAndKill(int blockId, int commandId)
        {
            var block = graph.getBlockById(blockId);
            return CalculateGenAndKill(block, block.Commands[commandId]);
        }

        public GenKillOneCommand CalculateGenAndKill(BasicBlock block, int commandId)
        {
            return CalculateGenAndKill(block, block.Commands[commandId]);
        }
    }
}
