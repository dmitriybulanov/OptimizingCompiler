using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QuickGraph;
using QuickGraph.Algorithms;

using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode.Model;
using System.Collections;

namespace DataFlowAnalysis.IntermediateRepresentation.ControlFlowGraph
{
	public class Graph : IEnumerable<BasicBlock>
	{
		private BidirectionalGraph<BasicBlock, Edge<BasicBlock>> CFG =
		  new BidirectionalGraph<BasicBlock, Edge<BasicBlock>>();

		private BidirectionalGraph<BasicBlock, Edge<BasicBlock>> spanTree =
		  new BidirectionalGraph<BasicBlock, Edge<BasicBlock>>();

		private Dictionary<int, int> spanTreeOrder = new Dictionary<int, int>();

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

			spanTree.AddVertexRange(listBlocks.Blocks);
			var visited = new Dictionary<BasicBlock, bool>();

			foreach (var v in spanTree.Vertices)
			{
				visited[v] = false;
			}
			int c = spanTree.Vertices.Count();

            dfs(blockMap[0], visited, ref c);
		}

		private void dfs(BasicBlock block, Dictionary<BasicBlock, bool> visited, ref int c)
		{
			visited[block] = true;
			foreach (var node in getChildren(block.BlockId).Blocks)
			{
				if (!visited[node])
				{
					spanTree.AddEdge(new Edge<BasicBlock>(block, node));
					dfs(node, visited, ref c);
				}
			}
			spanTreeOrder[block.BlockId] = c;
			c--;
		}

		// returns null if such id doesn't exist
		public BasicBlock getBlockById(int id)
		{
			return blockMap[id];
		}
			                                 
		// get BasicBlocksList of all ancestors of the block
		public BasicBlocksList getChildren(int id)
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
                res += ":\n<-- ";
				foreach (var e in CFG.InEdges(v))
				{
					res += " " + e.Source.BlockId;
				}
                res += "\n--> ";
                foreach (var e in CFG.OutEdges(v))
				{
					res += " " + e.Target.BlockId;
				}

				res += "\n";
			}
			return res;
		}

		public IEnumerator GetEnumerator()
		{
			return blockMap.Values.GetEnumerator();
		}

        public int GetCount()
        {
            return CFG.Vertices.Count();
        }

        public Dictionary<int, int> GetDFN()
        {
            return spanTreeOrder;
        }

        public IEnumerable<Edge<BasicBlock>> GetEdges()
        {
            return CFG.Edges;
        }

        public bool IsAncestor(int id1, int id2)
        {
            var b = getBlockById(id1);
            if (b.InputBlocks.Count() == 0)
                return false;
            else if (b.InputBlocks[0] == id2)
                return true;
            else
                return IsAncestor(b.InputBlocks[0], id2);
        }

        IEnumerator<BasicBlock> IEnumerable<BasicBlock>.GetEnumerator()
        {
            return blockMap.Values.GetEnumerator();
        }
    }
}
