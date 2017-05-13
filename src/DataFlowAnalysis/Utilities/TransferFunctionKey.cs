using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataFlowAnalysis.IntermediateRepresentation.Regions.Model;

namespace DataFlowAnalysis.Utilities
{
    public enum RegionDirection { In, Out }

    public class TransferFunctionKey
    {
        public Region From { get; }

        public Region To { get; }

        public RegionDirection Direction { get; }

        public TransferFunctionKey(Region from, RegionDirection direction, Region to)
        {
            From = from;
            To = to;
            Direction = direction;
        }

        protected bool Equals(TransferFunctionKey other)
        {
            return Equals(From, other.From) && Equals(To, other.To) && Direction == other.Direction;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TransferFunctionKey) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (From != null ? From.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (To != null ? To.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int) Direction;
                return hashCode;
            }
        }
    }
}
