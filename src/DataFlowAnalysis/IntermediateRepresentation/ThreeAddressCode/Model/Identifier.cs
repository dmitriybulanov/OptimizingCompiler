namespace DataFlowAnalysis.ThreeAddressCode.Model
{
    public class Identifier : Expression
    {
        public string Name { get; set; }

        public Identifier() { }

        public Identifier(string name)
        {
            Name = name;
        }

        public override string ToString() => Name;
    }
}
