using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode.Model
{
    public class BasicBlocksList : IEnumerable<BasicBlock>
    {
        public List<BasicBlock> Blocks { get; set; } = new List<BasicBlock>();
        public Dictionary<int, BasicBlock> BlockByID { get; set; } = new Dictionary<int, BasicBlock>();

        public BasicBlocksList() { }

        public BasicBlocksList(IEnumerable<BasicBlock> blocks)
        {
            Blocks.AddRange(blocks);
            foreach (var block in Blocks)
                BlockByID[block.BlockId] = block;
        }

        public void Add(BasicBlock block)
        {
            Blocks.Add(block);
            BlockByID[block.BlockId] = block;
        }

        public void Remove(int BlockId)
        {
            foreach(BasicBlock bl in Blocks)
                if (bl.BlockId == BlockId)
                {
                    Blocks.Remove(bl);
                    BlockByID.Remove(BlockId);
                }
        }

        public IEnumerator<BasicBlock> GetEnumerator()
        {
            return Blocks.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, Blocks.Select(x => x.ToString()));
        }
    }
}
