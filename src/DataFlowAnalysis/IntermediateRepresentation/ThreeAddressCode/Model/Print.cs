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
    }
}