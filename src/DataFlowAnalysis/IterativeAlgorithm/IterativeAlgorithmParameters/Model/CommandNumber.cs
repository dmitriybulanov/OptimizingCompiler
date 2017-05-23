using System;

namespace DataFlowAnalysis.IterativeAlgorithm.IterativeAlgorithmParameters.Model
{
    // Information about the "coordinates" of the instruction
    public class CommandNumber : IComparable<CommandNumber>, IEquatable<CommandNumber>
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
        public bool Equals(CommandNumber other)
        {
            return BlockId == other.BlockId && CommandId == other.CommandId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CommandNumber)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (BlockId * 397) ^ CommandId;
            }
        }
    }
}
