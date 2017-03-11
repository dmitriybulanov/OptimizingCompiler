namespace DataFlowAnalysis.ThreeAddressCode.Model
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
    }
}
