namespace DataFlowAnalysis.ThreeAddressCode.Model
{
    public abstract class Expression
    {
        public abstract bool HasIdentifiedSubexpression(Identifier expression);
    }
}
