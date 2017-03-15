using System.Collections.Generic;
using System.Linq;
using DataFlowAnalysis.BasicBlockCode.Model;
using DataFlowAnalysis.ThreeAddressCode.Model;

namespace DataFlowAnalysis.BasicBlockCode
{
    public class BasicBlocksGenerator
    {
        public static BasicBlocksList CreateBasicBlocks(Program program)
        {
            BasicBlocksList basicBlocks = new BasicBlocksList();
            List<ThreeAddressCommand> commands = program.Commands;
            Dictionary<string, int> labels = new Dictionary<string, int>();
            List<int> firstCommandsOfBlocks = new List<int>();
            List<int> lastCommandsOfBlocks = new List<int>();

            firstCommandsOfBlocks.Add(0);
            for (int i = 0; i < commands.Count; ++i)
            {
                ThreeAddressCommand currentCommand = commands[i];
                if (currentCommand.Label != null)
                {
                    labels[currentCommand.Label] = i;
                    if (i > 0)
                    {
                        firstCommandsOfBlocks.Add(i);
                        lastCommandsOfBlocks.Add(i - 1);
                    }
                }
                if (currentCommand is Goto && i < commands.Count - 1)
                {
                    firstCommandsOfBlocks.Add(i + 1);
                    lastCommandsOfBlocks.Add(i);
                }
            }
            lastCommandsOfBlocks.Add(commands.Count - 1);
            firstCommandsOfBlocks = firstCommandsOfBlocks.Distinct().ToList();
            lastCommandsOfBlocks = lastCommandsOfBlocks.Distinct().ToList();

            int[] BlockByFirstCommand = new int[commands.Count];
            int[] BlockByLastCommand = new int[commands.Count];
            for (int i = 0; i < firstCommandsOfBlocks.Count; ++i)
                BlockByFirstCommand[firstCommandsOfBlocks[i]] = i;
            for (int i = 0; i < lastCommandsOfBlocks.Count; ++i)
                BlockByLastCommand[lastCommandsOfBlocks[i]] = i;

            List<int>[] PreviousBlocksByBlockID = new List<int>[commands.Count];
            List<int>[] NextBlocksByBlockID = new List<int>[commands.Count];
            for (int i = 0; i < commands.Count; ++i)
            {
                PreviousBlocksByBlockID[i] = new List<int>();
                NextBlocksByBlockID[i] = new List<int>();
            }

            foreach (var currentNumOfLastCommand in lastCommandsOfBlocks)
            {
                ThreeAddressCommand currentCommand = commands[currentNumOfLastCommand];
                int numOfCurrentBlock = BlockByLastCommand[currentNumOfLastCommand];
                if ((currentCommand.GetType() != typeof(Goto)) && (currentNumOfLastCommand < commands.Count - 1))
                {
                    int numOfNextBlock = numOfCurrentBlock + 1;
                    NextBlocksByBlockID[numOfCurrentBlock].Add(numOfNextBlock);
                    PreviousBlocksByBlockID[numOfNextBlock].Add(numOfCurrentBlock);
                }
                if (currentCommand is Goto)
                {
                    int numOfNextBlock = BlockByFirstCommand[labels[(currentCommand as Goto).GotoLabel]];
                    NextBlocksByBlockID[numOfCurrentBlock].Add(numOfNextBlock);
                    PreviousBlocksByBlockID[numOfNextBlock].Add(numOfCurrentBlock);
                }
            }

            for (int i = 0; i < firstCommandsOfBlocks.Count; ++i)
            {
                BasicBlock block = new BasicBlock();
                block.Commands = new List<ThreeAddressCommand>(commands.Take(lastCommandsOfBlocks[i] + 1).Skip(firstCommandsOfBlocks[i]).ToList());
                block.InputBlocks.AddRange(PreviousBlocksByBlockID[i]);
                block.OutputBlocks.AddRange(NextBlocksByBlockID[i]);
                basicBlocks.Blocks.Add(block);
                basicBlocks.BlockByID[block.BlockId] = block;
            }

            return basicBlocks;
        }
    }
}
