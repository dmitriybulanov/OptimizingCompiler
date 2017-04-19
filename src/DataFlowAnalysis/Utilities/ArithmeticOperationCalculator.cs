using SyntaxTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFlowAnalysis.Utilities
{
    public class ArithmeticOperationCalculator
    {
        public static int Calculate(Operation op, int lx, int rx)
        {
            switch (op)
            {
                case Operation.Add:
                    return lx + rx;
                case Operation.Divide:
                    return lx / rx;
                case Operation.Equal:
                    return lx == rx ? 1 : 0;
                case Operation.Greater:
                    return lx > rx ? 1 : 0;
                case Operation.GreaterEqual:
                    return lx >= rx ? 1 : 0;
                case Operation.Lesser:
                    return lx < rx ? 1 : 0;
                case Operation.LesserEqual:
                    return lx <= rx ? 1 : 0;
                case Operation.LogicalAnd:
                    return lx != 0 && rx != 0 ? 1 : 0;
                case Operation.LogicalOr:
                    return lx != 0 || rx != 0 ? 1 : 0;
                case Operation.Multiply:
                    return lx * rx;
                case Operation.NotEqual:
                    return lx != rx ? 1 : 0;
                case Operation.Subtract:
                    return lx - rx;
                case Operation.UnaryMinus:
                    return -lx;
                case Operation.UnaryNot:
                    return lx == 0 ? 1 : 0;
            }
            return 0;
        }
        
    }
}
