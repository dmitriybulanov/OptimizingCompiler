using System;
using System.Collections.Generic;
using DataFlowAnalysis.IntermediateRepresentation.Regions.Model;
using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode.Model;
using DataFlowAnalysis.IntermediateRepresentation.FindNaturalLoops;
using DataFlowAnalysis.Utilities;
using QuickGraph;
using DataFlowAnalysis.IntermediateRepresentation.ControlFlowGraph;

namespace DataFlowAnalysis.IntermediateRepresentation.Regions
{
    public class RegionSequence
    {

        private List<Region> regionList = new List<Region>();

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
            }

            Dictionary<Edge<BasicBlock>, ISet<int>> loops = FindNaturalLoops.FindNaturalLoops.FindAllNaturalLoops(g);

            if (loops.Count != 0) {
                foreach (var loop in loops)
                {
                    //getGraphFromLoop();
                    //AddLoopRegions();
                }

            }

            return null;
        }

        private Graph getGraphFromLoop(KeyValuePair<Edge<BasicBlock>, ISet<int>> loop)
        {
            // нужно добавлять дуги, которые ведут только к вершинам, котоыре находятся в цикле
            // или написать фукнцию такую в Graph или использовать BidirectionalGraph здесь
            return null;
        }

		// Рекурсивная функция для добавления 
		private void AddLoopRegions(Graph g)
        {
            //CFG.AddVertexRange(listBlocks.Blocks);
        }

    }
}
