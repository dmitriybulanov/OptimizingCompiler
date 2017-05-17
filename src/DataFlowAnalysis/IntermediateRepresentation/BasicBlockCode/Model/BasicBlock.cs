using System;
using System.Collections.Generic;
using System.Linq;
using DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode.Model;

namespace DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode.Model
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

        public BasicBlock(BasicBlock newBlock)
        {
            BlockId = newBlock.BlockId;
            Commands = newBlock.Commands;
            InputBlocks = newBlock.InputBlocks;
            OutputBlocks = newBlock.OutputBlocks;
        }

        public BasicBlock(List<ThreeAddressCommand> commands, List<int> inputBlocks, List<int> outputBlocks)
        {
            BlockId = BlockIdNumber++;
            Commands.AddRange(commands);
            InputBlocks = inputBlocks;
            OutputBlocks = outputBlocks;
        }

        protected bool Equals(BasicBlock other)
        {
            return BlockId == other.BlockId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BasicBlock) obj);
        }

        public override int GetHashCode()
        {
            return BlockId;
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