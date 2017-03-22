using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
