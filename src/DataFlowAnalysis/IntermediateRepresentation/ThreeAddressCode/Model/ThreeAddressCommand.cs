namespace DataFlowAnalysis.ThreeAddressCode.Model
{
    public abstract class ThreeAddressCommand
    {
        public string Label { get; set; } = null;

        public override string ToString()
        {
            return Label == null ? "" : $"{Label}: ";
        }
    }
}
