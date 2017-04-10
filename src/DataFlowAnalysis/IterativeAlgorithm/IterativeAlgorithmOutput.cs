using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFlowAnalysis.IterativeAlgorithm
{
    public class IterativeAlgorithmOutput<T>
    {
        public Dictionary<int, T> In { get; set; } = new Dictionary<int, T>();
        public Dictionary<int, T> Out { get; set; } = new Dictionary<int, T>();
    }
}
