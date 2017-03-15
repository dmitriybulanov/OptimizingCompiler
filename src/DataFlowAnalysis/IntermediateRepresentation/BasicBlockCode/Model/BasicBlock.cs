using System;
using System.Collections.Generic;
using System.Linq;
using DataFlowAnalysis.ThreeAddressCode.Model;

namespace DataFlowAnalysis.BasicBlockCode.Model
{
    public class BasicBlock
    {
        private static int BlockIdNumber = 0;
        public int BlockId { get; set; }
        public List<ThreeAddressCommand> Commands { get; set; }
        public List<int> InputBlocks { get; set; } = new List<int>();
        public List<int> OutputBlocks { get; set; } = new List<int>();

        public BasicBlock()
        {
            BlockId = BlockIdNumber++;
        }

        public BasicBlock(List<ThreeAddressCommand> commands, List<int> inputBlocks, List<int> outputBlocks)
        {
            BlockId = BlockIdNumber++;
            Commands.AddRange(commands);
            InputBlocks = inputBlocks;
            OutputBlocks = outputBlocks;
        }

        public override string ToString()
        {
            return "BlockId = " + BlockId.ToString() +
                "\nCommands:\n" + string.Join(Environment.NewLine, Commands.Select(x => "   " + x.ToString())) +
                "\nInputBlocksNumbers = {" + string.Join("; ", InputBlocks.Select(x => x.ToString())) +
                "}\nOutputBlocksNumbers = {" + string.Join("; ", OutputBlocks.Select(x => x.ToString())) + "}";
        }
    }
}