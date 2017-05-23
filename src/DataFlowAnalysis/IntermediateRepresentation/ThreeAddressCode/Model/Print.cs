namespace DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode.Model
{
    public class Print : ThreeAddressCommand
    {
        public Expression Argument { get;set; }

        public bool WithNewLine { get; set; }

        public Print() { }

        public Print(Expression expression, bool withNewLine)
        {
            Argument = expression;
            WithNewLine = withNewLine;
        }

        public override string ToString()
        {
            var funcName = WithNewLine ? "println" : "print";
            return base.ToString() + $"{funcName} {Argument}";
        }

        protected bool Equals(Print other)
        {
            return base.Equals(other) && Equals(Argument, other.Argument) && WithNewLine == other.WithNewLine;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Print) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ (Argument != null ? Argument.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ WithNewLine.GetHashCode();
                return hashCode;
            }
        }
    }
}