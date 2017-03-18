namespace DataFlowAnalysis.GenKillCalculator.Model
{
    // Information about the "coordinates" of the instruction
    public class CommandNumber
    {
        public int BlockId { get; set; }

        public int CommandId { get; set; }

        public CommandNumber(int blockId, int commandId)
        {
            BlockId = blockId;
            CommandId = commandId;
        }
    }
}
