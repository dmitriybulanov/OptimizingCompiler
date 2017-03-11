namespace DataFlowAnalysis.ThreeAddressCode.Model
{
    public class NoOperation : ThreeAddressCommand
    {
        public NoOperation() { }

        public NoOperation(string label)
        {
            Label = label;
        }

        public override string ToString()
        {
            return base.ToString() + "<no-op>";
        }
    }
}
