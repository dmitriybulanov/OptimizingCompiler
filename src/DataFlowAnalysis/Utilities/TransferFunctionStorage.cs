using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode.Model;
using DataFlowAnalysis.IntermediateRepresentation.Regions.Model;

namespace DataFlowAnalysis.Utilities
{
    public delegate T TransferFunction<T>(T input);

    public class TransferFunctionStorage<T> : AbstractTransferFunctionStorage<TransferFunction<T>>
    {
        public TransferFunctionStorage<T> Clone()
        {
            var cloned = new TransferFunctionStorage<T>();

            foreach (var transferFunction in Functions)
            {
                cloned.Functions.Add(transferFunction.Key, (TransferFunction<T>)transferFunction.Value.Clone());
            }

            return cloned;
        }
    }

    public class AbstractTransferFunctionStorage<TFunc>
    {
        public Dictionary<TransferFunctionKey, TFunc> Functions { get; }

        public AbstractTransferFunctionStorage()
        {
            Functions = new Dictionary<TransferFunctionKey, TFunc>();
        }
        
        public TFunc this[Region from, RegionDirection direction, Region to]
        {
            get
            {
                TFunc result;
                if (Functions.TryGetValue(new TransferFunctionKey(from, direction, to), out result))
                    return result;
                return default(TFunc);
            }
            set
            {
                var key = new TransferFunctionKey(from, direction, to);
                if (Functions.ContainsKey(key))
                    Functions[key] = value;
                else
                    Functions.Add(new TransferFunctionKey(from, direction, to), value);
            }
        }
    }
}
