using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode.Model;

namespace DataFlowAnalysis.Utilities
{
    public class SetFactory
    {
        public static ISet<T> GetSet<T>(IEnumerable<T> data)
        {
            return new SortedSet<T>(data);
        }

        public static ISet<T> GetSet<T>(params T[] data)
        {
            return new SortedSet<T>(data);
        }

        public static ISet<Expression> GetSet(IEnumerable<Expression> data)
        {
            return new HashSet<Expression>(data);
        }

        public static ISet<Expression> GetSet(params Expression[] data)
        {
            return new HashSet<Expression>(data);;
        }
    }
}
