using System;

namespace SyntaxTree
{
    public enum Operation
    {
        // comparison
        Equal,
        NotEqual,
        Lesser, 
        Greater,
        LesserEqual,
        GreaterEqual,

        // binary operations
        Add,
        Subtract,
        Multiply,
        Divide,

        LogicalAnd,
        LogicalOr,

        UnaryNot,
        UnaryMinus
    }


    public static class OperationExtensions
    {
        /// <summary>
        /// Переводит операцию в соответствующий символ
        /// </summary>
        /// <param name="op"></param>
        /// <returns></returns>
        public static string ToText(this Operation op)
        {
            switch (op)
            {
                case Operation.Equal:
                    return "==";
                case Operation.NotEqual:
                    return "!=";
                case Operation.Lesser:
                    return "<";
                case Operation.Greater:
                    return ">";
                case Operation.LesserEqual:
                    return "<=";
                case Operation.GreaterEqual:
                    return ">=";
                case Operation.Add:
                    return "+";
                case Operation.Subtract:
                case Operation.UnaryMinus:
                    return "-";
                case Operation.Multiply:
                    return "*";
                case Operation.Divide:
                    return "/";
                case Operation.LogicalAnd:
                    return "&";
                case Operation.LogicalOr:
                    return "|";
                case Operation.UnaryNot:
                    return "!";
                default:
                    throw new ArgumentOutOfRangeException(nameof(op), op, null);
            }
        }
    }
}
