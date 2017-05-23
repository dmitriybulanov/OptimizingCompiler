namespace DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode.Model
{
    public class ConditionalGoto : Goto
    {
        public Expression Condition { get; set; }

        public ConditionalGoto(string gotoLabel, Expression condition) : base(gotoLabel)
        {
            Condition = condition;
        }

        public override string ToString()
        {
            return base.ToString() + $" if {Condition}";
        }

        protected bool Equals(ConditionalGoto other)
        {
            return base.Equals(other) && Equals(Condition, other.Condition);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ConditionalGoto) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ (Condition != null ? Condition.GetHashCode() : 0);
            }
        }
    }
}
