using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QuickGraph;
using QuickGraph.Algorithms.Search;

using DataFlowAnalysis.BasicBlockCode.Model;
using System.Collections;

namespace DataFlowAnalysis.ControlFlowGraph
{
	public class Graph : IEnumerable
	{
		private BidirectionalGraph<BasicBlock, Edge<BasicBlock>> CFG =
		  new BidirectionalGraph<BasicBlock, Edge<BasicBlock>>();

		private Dictionary<int, BasicBlock> blockMap = new Dictionary<int, BasicBlock>();

		public Graph() { }

		// constructor from recently prepared BasicBlocksList
		public Graph(BasicBlocksList listBlocks)
		{
			CFG.AddVertexRange(listBlocks.Blocks);

			foreach (BasicBlock block in listBlocks.Blocks)
			{
				blockMap.Add(block.BlockId, block);
			}

			foreach (var block in listBlocks.Blocks)
			{
				foreach (var numIn in block.InputBlocks)
				{
					CFG.AddEdge(new Edge<BasicBlock>(this.getBlockById(numIn), block));
				}
			}
		}

		// returns null if such id doesn't exist
		public BasicBlock getBlockById(int id)
		{
			return blockMap[id];
		}

		// get BasicBlocksList of all ancestors of the block
		public BasicBlocksList getAncestors(int id)
		{
			var result = new BasicBlocksList();
			var v = getBlockById(id);

			foreach (var edge in CFG.OutEdges(v))
			{
				result.Add(edge.Target);
			}

			return result;
		}

		// get BasicBlocksList of all predecessors of the block
		public BasicBlocksList getParents(int id)
		{
			BasicBlocksList blockList = new BasicBlocksList();
			var v = getBlockById(id);
			foreach (var edge in CFG.InEdges(v))
			{
				blockList.Add(edge.Source);
			}

			return blockList;
		}

		public override string ToString()
		{
			string res = "";
			foreach (var v in CFG.Vertices)
			{
				res += v.BlockId;
				foreach (var e in CFG.OutEdges(v))
				{
					res += " " + e.Target.BlockId;
				}
				foreach (var e in CFG.InEdges(v))
				{
					res += " " + e.Source.BlockId;
				}
				res += "\n";
			}
			return res;
		}

		public IEnumerator GetEnumerator()
		{
			return blockMap.Values.GetEnumerator();
		}

        public int Count()
        {
            return CFG.Vertices.Count();
        }
    }
}
