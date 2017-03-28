using System;

namespace DataFlowAnalysis.IterativeAlgorithmParameters.Model
{
    // Information about the "coordinates" of the instruction
    public class CommandNumber : IComparable<CommandNumber>
    {
        public int BlockId { get; set; }

        public int CommandId { get; set; }

        public CommandNumber(int blockId, int commandId)
        {
            BlockId = blockId;
            CommandId = commandId;
        }
        
        public int CompareTo(CommandNumber other)
        {
            return BlockId == other.BlockId ?
                CommandId.CompareTo(other.CommandId) : BlockId.CompareTo(other.BlockId);
        }

        public override string ToString()
        {
            return "BlockId: " + BlockId + " CommandId: " + CommandId;
        }
    }
}
