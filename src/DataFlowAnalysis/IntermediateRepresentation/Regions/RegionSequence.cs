using System;
using System.Collections.Generic;
using DataFlowAnalysis.IntermediateRepresentation.Regions.Model;
using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode.Model;
using DataFlowAnalysis.IntermediateRepresentation.NaturalLoops;
using DataFlowAnalysis.Utilities;
using QuickGraph;
using DataFlowAnalysis.IntermediateRepresentation.ControlFlowGraph;
using DataFlowAnalysis.IntermediateRepresentation.CheckRetreatingIsReverse;

namespace DataFlowAnalysis.IntermediateRepresentation.Regions
{
    public class RegionSequence
    {

        private List<Region> regionList = new List<Region>();

        private bool checkLoopInclusion(KeyValuePair<Edge<BasicBlock>, ISet<int>> curLoop, KeyValuePair<Edge<BasicBlock>, ISet<int>> loopOther,
                                       bool regionMade)
        {
            if (loopOther.Value.IsSubsetOf(curLoop.Value) && !regionMade)
                return true;
            else
                return false;
        }

        public List<Region> CreateSequence(Graph g)
        {
            //if (!CheckRetreatingIsReverse.CheckRetreatingIsReverse.CheckReverseEdges(g)){
            //    Console.WriteLine("there are some retreating edges which aren't reverse");
            //    Environment.Exit(0);
            //}
            var basicBlockLastRegion = new Dictionary<BasicBlock, Region>();

            foreach (var v in g.GetVertices())
            {
                var newReg = new LeafRegion(v);
                regionList.Add(newReg);
                basicBlockLastRegion[v] = newReg;
            }

            var loops = SearchNaturalLoops.FindAllNaturalLoops(g);
            Console.WriteLine(loops.Count);

            var regionMade = new Dictionary<Edge<BasicBlock>, bool>();

            foreach (var loop in loops)
            {
                regionMade[loop.Key] = false;
            }

            while (regionMade.ContainsValue(false))
            {
                foreach (var loop in loops)
                {
                    bool anyInsideLoops = false;
                    foreach (var loopOther in loops)
                    {
                        anyInsideLoops = anyInsideLoops || checkLoopInclusion(loop, loopOther, regionMade[loopOther.Key]);
                    }
                    if (!anyInsideLoops) continue;

                    regionMade[loop.Key] = true;

                    var header = loop.Key.Target;

                    var curRegions = new List<Region>();
                    var outputBlocks = new List<int>();
                    foreach (var blockId in loop.Value)
                    {
                        var block = g.getBlockById(blockId);
                        if (!curRegions.Contains(basicBlockLastRegion[block]))
                            curRegions.Add(basicBlockLastRegion[block]);

                        foreach (var outputBlock in block.OutputBlocks)
                        {
                            if (!loop.Value.Contains(outputBlock))
                            {
                                outputBlocks.Add(outputBlock);
                                break;
                            }
                        }
                    }

                    var bodyReg = new BodyRegion(header, outputBlocks, curRegions);
                    regionList.Add(bodyReg);

                    var loopReg = new LoopRegion(bodyReg);
                    regionList.Add(loopReg);

                    foreach (var blockId in loop.Value)
                    {
                        var block = g.getBlockById(blockId);
                        basicBlockLastRegion[block] = loopReg;
                    }
                }
            }
            Console.WriteLine(regionList.Count);


            // check if program has become one region
            foreach (var block in basicBlockLastRegion)
            {
                // if there are leaves not included in loops
                if (block.Value.GetType() == typeof(LeafRegion))
                {
                    var header = g.getRoot();
                    var outputBlocks = new List<int>();
                    var curRegions = new List<Region>();
                    foreach (var curblock in basicBlockLastRegion)
                    {
                        if (!curRegions.Contains(curblock.Value))
                            curRegions.Add(curblock.Value);
                    }
                    var newReg = new BodyRegion(header, outputBlocks, curRegions);
                    regionList.Add(newReg);
                    break;
                }
            }

            return regionList;
        }
    }
}
