namespace DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode.Model
{
    public abstract class ThreeAddressCommand
    {
        public string Label { get; set; } = null;

        public override string ToString()
        {
            return Label == null ? "" : $"{Label}: ";
        }

        protected bool Equals(ThreeAddressCommand other)
        {
            return string.Equals(Label, other.Label);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ThreeAddressCommand) obj);
        }

        public override int GetHashCode()
        {
            return (Label != null ? Label.GetHashCode() : 0);
        }
    }
}
