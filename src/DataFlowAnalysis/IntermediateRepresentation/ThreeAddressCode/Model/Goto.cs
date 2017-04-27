namespace DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode.Model
{
    public class Goto : ThreeAddressCommand
    {
        public string GotoLabel { get; set; }

        public Goto(string gotoLabel)
        {
            GotoLabel = gotoLabel;
        }

        public override string ToString()
        {
            return base.ToString() + "goto " + GotoLabel;
        }
    }
}
