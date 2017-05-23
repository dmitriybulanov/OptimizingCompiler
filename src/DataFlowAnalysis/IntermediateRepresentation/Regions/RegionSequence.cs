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
        public Dictionary<Region, int> regionMapId = new Dictionary<Region, int>();
        public Dictionary<int, Region> idMapRegion = new Dictionary<int, Region>();
        private Dictionary<int, LeafRegion> regionMapBlockId = new Dictionary<int, LeafRegion>();
        public BidirectionalGraph<Region, Edge<Region>> regionCFG =
          new BidirectionalGraph<Region, Edge<Region>>();

        // FindAllNaturalLoops helpers private members
        private ISet<int> Loop;
        private Stack<int> Stack;
        private void Insert(int m)
        {
            if (!Loop.Contains(m))
            {
                Loop.Add(m);
                Stack.Push(m);
            }
        }
        public RegionSequence()
        {
            // проверка на приводимость графа
            // если неприводим, то выкидывать из конструктора
        }

        private bool checkLoopInclusion(KeyValuePair<Edge<Region>, ISet<int>> curLoop, Dictionary<Edge<Region>, ISet<int>> regionLoops) {
            foreach (var loop in regionLoops)
            {
                if (loop.Value.IsSubsetOf(curLoop.Value))
                {
                    return false;
                }
            }
            return true;
        }

        private BasicBlock getBasicBlockHead(Region r){
            if (r.GetType() == typeof(LeafRegion))
            {
                return (r as LeafRegion).Block;
            }
            else if (r.GetType() == typeof(LoopRegion))
            {
                return getBasicBlockHead((r as LoopRegion).Body);
            }
            else return (r as BodyRegion).Header;
        }

        public List<Region> CreateSequence(Graph g)
        {
            CreateRegionCFG(g);
            var regionLoops = FindAllNaturalLoops(g);
            while (regionLoops.Count != 0)
            {
                foreach (var loop in regionLoops)
                {
                    if (checkLoopInclusion(loop, regionLoops)) continue;

                    var curRegions = new List<Region>();
                    var curBasicBlocks = new List<BasicBlock>();
                    var outputBlocks = new List<int>();

                    var allOutputs = SetFactory.GetSet<int>();
                    foreach (var regionID in loop.Value)
                    {
                        curRegions.Add(idMapRegion[regionID]);

                        if (idMapRegion[regionID].GetType() == typeof(LeafRegion))
                        {
                            curBasicBlocks.Add((idMapRegion[regionID] as LeafRegion).Block);
                        }
                        // добавляем все аутпутблоки регионов - блоки, из которых есть дуги в другие регионы
                        allOutputs.UnionWith(idMapRegion[regionID].OutputBlocks);
                    }

                    foreach (var outBlockId in allOutputs)
                    {
                        if (!allOutputs.IsSupersetOf(g.getBlockById(outBlockId).OutputBlocks))
                        {
                            outputBlocks.Add(outBlockId);
                        }
                    }

                    var header = getBasicBlockHead(loop.Key.Target);
                    BodyRegion newReg = new BodyRegion(header, outputBlocks, curRegions);
                    LoopRegion newLoopReg = new LoopRegion(newReg);
                    regionList.Add(newReg);
                    regionList.Add(newLoopReg);

                    // add new vertex to regionCFG
                    regionCFG.AddVertex(newReg);

                    // старый алгоритм, возможно не будет работать, потому что неизвестно как поведет себя граф при уничтожении связи с подграфом
                    // сначала нужно найти вершину из которой дуга ведет в header
                    // затем, заменить таргет дуги на новый регион
                    //foreach (var edge in regionCFG.Edges)
                    //{
                    //    if (getBasicBlockHead(edge.Target) == header)
                    //    {
                    //        Edge<Region> newEdge = new Edge<Region>(edge.Source, newReg);
                    //        regionCFG.RemoveEdge(edge);
                    //        regionCFG.AddEdge(newEdge);
                    //    }
                    //}


                    // "вроде должно работать" алгоритм
                    foreach (var reg in regionCFG.Vertices)
                    {
                        if (newReg.Regions.Contains(reg))
                        {
                            foreach (var outReg in reg.OutputBlocks)
                            {
                                // regionMapBlockId - у нас только leaf, а может быть любая область. Надо шото сделать
                                regionCFG.RemoveEdge(new Edge<Region>(reg, regionMapBlockId[outReg]));

                                if (!newReg.Regions.Contains(regionMapBlockId[outReg]))
                                {
                                    regionCFG.AddEdge(new Edge<Region>(newReg, regionMapBlockId[outReg]));
                                }
                            }

                        } else {
                            foreach (var outReg in reg.OutputBlocks)
                            {
                                if (newReg.Regions.Contains(regionMapBlockId[outReg]))
                                {
                                    regionCFG.RemoveEdge(new Edge<Region>(reg, regionMapBlockId[outReg]));
                                    regionCFG.AddEdge(new Edge<Region>(reg, newReg));
                                }
                            }
                        }
                    }
                }
            }
            return regionList;
        }


        public Dictionary<Edge<Region>, ISet<int>> FindAllNaturalLoops(Graph g)
        {
            var classify = EdgeClassification.EdgeClassification.ClassifyEdge(g);
            var result = new Dictionary<Edge<Region>, ISet<int>>();
            foreach (var pair in classify)
            {
                if (pair.Value == EdgeClassification.Model.EdgeType.Retreating)
                {
                    Stack = new Stack<int>();
                    Loop = SetFactory.GetSet<int>(new int[] { regionMapId[regionMapBlockId[pair.Key.Target.BlockId]] });
                    Insert(regionMapId[regionMapBlockId[pair.Key.Source.BlockId]]);
                    while (Stack.Count > 0)
                    {
                        int m = Stack.Pop();
                        foreach (BasicBlock p in g.getParents(m))
                            Insert(regionMapId[regionMapBlockId[p.BlockId]]);
                    }
                    var edgeRegion = new Edge<Region>(regionMapBlockId[pair.Key.Source.BlockId], regionMapBlockId[pair.Key.Target.BlockId]);
                    result.Add(edgeRegion, Loop);
                }
            }
            return result;
        }

        public void CreateRegionCFG(Graph g) {
            
            var count = 0;
            foreach (var vertex in g.GetVertices())
            {
                LeafRegion block = new LeafRegion(vertex);
                regionList.Add(block);
                regionMapId.Add(block, count);
                idMapRegion.Add(count, block);
                count++;
                regionMapBlockId.Add(vertex.BlockId, block);
            }
            regionCFG.AddVertexRange(regionList);

            foreach (LeafRegion reg in regionList)
            {
                foreach (var outBlock in reg.Block.OutputBlocks)
                {
                    regionCFG.AddEdge(new Edge<Region>(reg, regionMapBlockId[outBlock]));
                }
            }
        }


    //    private Graph GetGraphFromLoop(KeyValuePair<Edge<BasicBlock>, ISet<int>> loop, Graph g)
    //    {
    //        BasicBlocksList blockList = new BasicBlocksList();
    //        foreach (var blockId in loop.Value)
    //        {
    //            BasicBlock curBlock = new BasicBlock(g.getBlockById(blockId));
    //            blockList.Add(curBlock);
    //        }

    //        foreach (var block in blockList)
    //        {
    //            foreach (var inBlockId in block.InputBlocks)
    //            {
    //                BasicBlock inBlock = g.getBlockById(inBlockId);

    //                if (!blockList.Blocks.Contains(inBlock))
    //                {
    //                    block.InputBlocks.Remove(inBlockId);
    //                }
    //            }

    //            foreach (var inBlockId in block.OutputBlocks)
                //{
                //    BasicBlock inBlock = g.getBlockById(inBlockId);

                //    if (!blockList.Blocks.Contains(inBlock))
                //    {
                //        block.OutputBlocks.Remove(inBlockId);
                //    }
                //}
        //    }

        //    return new Graph(blockList);
        //}

        //// Рекурсивная функция для добавления 
        //private void AddLoopRegions(Graph g)
   //     {
            //Dictionary<Edge<BasicBlock>, ISet<int>> loops = SearchNaturalLoops.FindAllNaturalLoops(g);

    //        if (loops.Count != 0)
    //        {
                //foreach (var loop in loops)
                //{
                    //Graph loopGraph = GetGraphFromLoop(loop, g);
                    //AddLoopRegions(loopGraph);
        //        }


        //    } else {
        //        BasicBlock headerBlock = g.getRoot();
        //        List<Region> regions = new List<Region>();
        //        List<int> outputBlocks = new List<int>();

        //        foreach (var vertex in g.GetVertices())
        //        {
        //            foreach (var outBlockId in vertex.OutputBlocks)
        //            {
        //                if (!g.Contains(g.getBlockById(outBlockId)))
        //                {
        //                    outputBlocks.Add(outBlockId);
        //                }
        //            }
        //            regions.Add(leafBlockMap[vertex.BlockId]);
        //        }

        //        BodyRegion bodyRegion = new BodyRegion(headerBlock, outputBlocks, regions);
        //        LoopRegion loopRegion = new LoopRegion(bodyRegion);
        //        regionList.Add(bodyRegion);
        //        regionList.Add(loopRegion);
        //    }
        //}

    }
}
