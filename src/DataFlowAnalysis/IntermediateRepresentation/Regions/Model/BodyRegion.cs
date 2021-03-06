﻿using System.Collections.Generic;
using System.Linq;
using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode.Model;

namespace DataFlowAnalysis.IntermediateRepresentation.Regions.Model
{
    public class BodyRegion : IntermediateRegion
    {
        public List<Region> Regions { get; set; }
        private Region regionParent;

        public BodyRegion(BasicBlock header, List<int> outputBlocks, List<Region> regions) : base(header, outputBlocks)
        {
            Regions = regions;
        }

		public override Region RegionParent
		{
			get
			{
				return regionParent;
			}
			set
			{
				regionParent = value;
			}
		}

        protected bool Equals(BodyRegion other)
        {
            return base.Equals(other) && Regions.SequenceEqual(other.Regions);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BodyRegion) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ (Regions != null ? Regions.GetHashCode() : 0);
            }
        }

        public override string ToString()
        {
            var res = string.Format("BodyRegion: \n");
            foreach (var reg in Regions)
            {
                res += reg.ToString();
            }
            return res;
        }
    }
}
