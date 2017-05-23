using SyntaxTree;

namespace DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode.Model
{
    public class Assignment : ThreeAddressCommand
    {
        public Identifier Target { get; set; }

        public Expression Value { get; set; }

        public Assignment(string target, Expression value)
        {
            Target = new Identifier(target);
            Value = value;
        }

        public static Assignment Increment(string variable) =>
            new Assignment(variable, new BinaryOperation(variable, Operation.Add, 1));

        public override string ToString() => base.ToString() + $"{Target.Name} = {Value}";

        protected bool Equals(Assignment other)
        {
            return base.Equals(other) && Equals(Target, other.Target) && Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Assignment) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ (Target != null ? Target.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Value != null ? Value.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
