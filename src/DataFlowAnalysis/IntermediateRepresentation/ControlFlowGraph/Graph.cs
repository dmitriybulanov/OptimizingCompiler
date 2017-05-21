using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QuickGraph;
using QuickGraph.Algorithms;

using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode.Model;
using DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode.Model;
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

            dfs(blockMap[blockMap.Keys.First()], visited, ref c);
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

        public BasicBlock getRoot()
		{
            return CFG.Roots().First();
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

		public IEnumerable<BasicBlock> GetVertices()
		{
            return CFG.Vertices;
		}

        public bool Contains(BasicBlock block)
        {
            return CFG.Vertices.Contains(block);
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

        public int GetMinBlockId()
        {
            return blockMap.Keys.Min();
        }

        public List<ThreeAddressCommand>  transformToThreeAddressCode()
        {
            var res = new List<ThreeAddressCommand>();

            var done = new HashSet<BasicBlock>();
            var stack = new Stack<BasicBlock>();
            stack.Push(getBlockById(0));

            BasicBlock cur = null;

            while (done.Count < this.GetCount())
            {
                cur = stack.Pop();
                done.Add(cur);

                foreach (var c in cur.Commands)
                {
                    res.Add(c);
                }

                switch (cur.OutputBlocks.Count)
                {
                    case 0:
                        continue;
                    case 1:
                        BasicBlock outBlock = getBlockById(cur.OutputBlocks[0]);
                        if (!done.Contains(outBlock) && !stack.Contains(outBlock))
                        {
                            stack.Push(outBlock);
                        }
                        break;
                    case 2:
                        int lastStatementIndex = cur.Commands.Count - 1;
                        ConditionalGoto lastStatement = (ConditionalGoto)cur.Commands[lastStatementIndex];

                        BasicBlock outFirst = getBlockById(cur.OutputBlocks[0]);
                        BasicBlock outSecond = getBlockById(cur.OutputBlocks[1]);

                        string labelFirst = outFirst.Commands[0].Label;
                        string labelSecond = outSecond.Commands[0].Label;
                        string gotoLabel = lastStatement.GotoLabel;

                        if (gotoLabel == labelSecond)
                        {
                            BasicBlock t = outFirst;
                            outFirst = outSecond;
                            outSecond = t;
                        }

                        int secondLastIndex = outSecond.Commands.Count - 1;
                        ThreeAddressCommand secondLast = outSecond.Commands[secondLastIndex];
                        if ((secondLast is Goto) && !(secondLast is ConditionalGoto))
                        {
                            BasicBlock afterIfStatement = getBlockById(outSecond.OutputBlocks[0]);
                            if (!stack.Contains(afterIfStatement) && !done.Contains(afterIfStatement))
                            {
                                stack.Push(afterIfStatement);
                            }
                        }

                        if (!done.Contains(outFirst) && !stack.Contains(outFirst))
                        {
                            stack.Push(outFirst);
                        }

                        if (!done.Contains(outSecond) && !stack.Contains(outSecond))
                        {
                            stack.Push(outSecond);
                        }
                        break;
                    default:
                        throw new Exception("There cannot be more than two output blocks!");
                }
            }

            return res;
        }
    }
}
