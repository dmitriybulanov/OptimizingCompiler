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

        protected bool Equals(Goto other)
        {
            return base.Equals(other) && string.Equals(GotoLabel, other.GotoLabel);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Goto) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ (GotoLabel != null ? GotoLabel.GetHashCode() : 0);
            }
        }
    }
}
