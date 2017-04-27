namespace DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode.Model
{
    public class Identifier : SimpleExpression
    {
        public string Name { get; set; }

        public Identifier() { }

        public Identifier(string name)
        {
            Name = name;
        }

        public override string ToString() => Name;

        protected bool Equals(Identifier other)
        {
            return string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Identifier) obj);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }

        public override bool HasIdentifiedSubexpression(Identifier expression) => this.Equals(expression);
    }
}
