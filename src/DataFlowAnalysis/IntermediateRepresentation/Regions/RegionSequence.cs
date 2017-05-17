using System;
using System.Collections.Generic;
using DataFlowAnalysis.IntermediateRepresentation.Regions.Model;
using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode.Model;
using DataFlowAnalysis.IntermediateRepresentation.NaturalLoops;
using DataFlowAnalysis.Utilities;
using QuickGraph;
using DataFlowAnalysis.IntermediateRepresentation.ControlFlowGraph;

namespace DataFlowAnalysis.IntermediateRepresentation.Regions
{
    public class RegionSequence
    {

        private List<Region> regionList = new List<Region>();
        private Dictionary<int, LeafRegion> leafBlockMap = new Dictionary<int, LeafRegion>(); 

	    public RegionSequence()
	    {
	    }

	    public List<Region> CreateSequence(Graph g)
	    {
            // Здесь должна быть проверка графа
            // Отступающие ребра == обратные ребра
            foreach (var vertex in g.GetVertices())
            {
                LeafRegion block = new LeafRegion(vertex);
                regionList.Add(block);
                leafBlockMap.Add(vertex.BlockId, block);
            }


             return null;
        }

        private Graph GetGraphFromLoop(KeyValuePair<Edge<BasicBlock>, ISet<int>> loop, Graph g)
        {
            BasicBlocksList blockList = new BasicBlocksList();
            foreach (var blockId in loop.Value)
            {
                BasicBlock curBlock = new BasicBlock(g.getBlockById(blockId));
                blockList.Add(curBlock);
            }

            foreach (var block in blockList)
            {
                foreach (var inBlockId in block.InputBlocks)
                {
                    BasicBlock inBlock = g.getBlockById(inBlockId);

                    if (!blockList.Blocks.Contains(inBlock))
                    {
                        block.InputBlocks.Remove(inBlockId);
                    }
                }

                foreach (var inBlockId in block.OutputBlocks)
				{
					BasicBlock inBlock = g.getBlockById(inBlockId);

					if (!blockList.Blocks.Contains(inBlock))
					{
						block.OutputBlocks.Remove(inBlockId);
					}
				}
            }

            return new Graph(blockList);
        }

		// Рекурсивная функция для добавления 
		private void AddLoopRegions(Graph g)
        {
			Dictionary<Edge<BasicBlock>, ISet<int>> loops = SearchNaturalLoops.FindAllNaturalLoops(g);

            if (loops.Count != 0)
            {
				foreach (var loop in loops)
				{
					Graph loopGraph = GetGraphFromLoop(loop, g);
					AddLoopRegions(loopGraph);
                }


            } else {
                BasicBlock headerBlock = g.getRoot();
                List<Region> regions = new List<Region>();
                List<int> outputBlocks = new List<int>();

                foreach (var vertex in g.GetVertices())
                {
                    foreach (var outBlockId in vertex.OutputBlocks)
                    {
                        if (!g.Contains(g.getBlockById(outBlockId)))
                        {
                            outputBlocks.Add(outBlockId);
                        }
                    }
                    regions.Add(leafBlockMap[vertex.BlockId]);
                }

                BodyRegion bodyRegion = new BodyRegion(headerBlock, outputBlocks, regions);
                LoopRegion loopRegion = new LoopRegion(bodyRegion);
                regionList.Add(bodyRegion);
                regionList.Add(loopRegion);
            }
        }

    }
}
