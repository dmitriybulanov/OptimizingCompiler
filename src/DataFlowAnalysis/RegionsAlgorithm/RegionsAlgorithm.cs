using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode.Model;
using DataFlowAnalysis.IntermediateRepresentation.ControlFlowGraph;
using DataFlowAnalysis.IntermediateRepresentation.Regions;
using DataFlowAnalysis.IntermediateRepresentation.Regions.Model;
using DataFlowAnalysis.IterativeAlgorithm;
using DataFlowAnalysis.IterativeAlgorithm.IterativeAlgorithmParameters;
using DataFlowAnalysis.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataFlowAnalysis.RegionsAlgorithm
{
    public static class RegionsAlgorithm
    {
        public static IterativeAlgorithmOutput<ISet<V>> Apply<V>(Graph graph, SetIterativeAlgorithmParameters<V> param)
        {
            List<Region> regions = new RegionSequence().CreateSequence(graph);

            return ApplyDescendingPart<V>(regions, ApplyAscendingPart<V>(graph, regions, param), param, graph);
        }
        
        static T Identity<T>(T input)
        {
            return input;
        }

        static TransferFunctionStorage<ISet<V>> ApplyAscendingPart<V>(Graph graph, List<Region> regions, SetIterativeAlgorithmParameters<V> param)
        {
            TransferFunctionStorage<ISet<V>> result = new TransferFunctionStorage<ISet<V>>();
            foreach (Region r in regions)
            {
                LeafRegion leaf = r as LeafRegion;
                TransferFunctionStorage<ISet<V>> clone = result.Clone();
                if (leaf != null)
                {
                    //////
                    for(int i = graph.Count(); i < regions.Count; ++i)
                        result[regions[i], RegionDirection.Out, leaf] = input => param.TransferFunction(input, leaf.Block);
                    /////   
                    result[leaf, RegionDirection.In, leaf] = Identity;
                    result[leaf, RegionDirection.Out, leaf] = input => param.TransferFunction(input, leaf.Block);
                }
                BodyRegion body = r as BodyRegion;
                if (body != null)
                {
                    foreach (Region s in body.Regions)
                    {
                        LeafRegion header = s as LeafRegion;
                        if (header != null)
                        {
                            result[body, RegionDirection.In, s] = Identity;
                        }
                        else
                        {
                            result[body, RegionDirection.In, s] = input => GatherFunctionsResults(input, clone, body, s.Header.InputBlocks, graph, param);
                        }
                        CalculateForOutputBlocks(result, body, s, s.OutputBlocks, graph);
                        
                    }
                }
                LoopRegion loop = r as LoopRegion;
                if(loop != null)
                {
                    result[loop, RegionDirection.In, loop.Body] = input => SetFactory.GetSet<V>(input.Union(GatherFunctionsResults(input, clone, loop.Body, loop.Header.InputBlocks, graph, param)));
                    CalculateForOutputBlocks(result, loop, loop.Body, loop.OutputBlocks, graph);
                }
            }
            return result;
        }

        static ISet<V> GatherFunctionsResults<V>(ISet<V> input, TransferFunctionStorage<ISet<V>> result, Region r, List<int> inputBlocks, Graph graph, SetIterativeAlgorithmParameters<V> param)
        {      
            return param.GatherOperation(inputBlocks.Select(i => result[r, RegionDirection.Out, new LeafRegion(graph.getBlockById(i))](input)));
        }

        static void CalculateForOutputBlocks<V>(TransferFunctionStorage<ISet<V>> result, Region r, Region s, List<int> outputBlocks, Graph graph)
        {
            foreach (BasicBlock bb in outputBlocks.Select(i => graph.getBlockById(i)))
            {
                LeafRegion b = new LeafRegion(bb);
                TransferFunctionStorage<ISet<V>> clone = result.Clone();
                result[r, RegionDirection.Out, b] = input => clone[s, RegionDirection.Out, b](clone[r, RegionDirection.In, s](input));
            }
        }

        static IterativeAlgorithmOutput<ISet<V>> ApplyDescendingPart<V>(List<Region> regions,
            TransferFunctionStorage<ISet<V>> functions, SetIterativeAlgorithmParameters<V> param, Graph graph)
        {
            Dictionary<int, ISet<V>> regionsInputs = new Dictionary<int, ISet<V>>();
            IterativeAlgorithmOutput<ISet<V>> result = new IterativeAlgorithmOutput<ISet<V>>();

            regionsInputs[regions.Count - 1] = param.FirstValue;
            
            Dictionary<Region, Region> parents = new Dictionary<Region, Region>();
            
            for (int i = regions.Count - 1; i >= 0; --i)
            {
                BodyRegion body = regions[i] as BodyRegion;
                if(body != null)
                    foreach (Region r in body.Regions)
                        parents[r] = body;
                
                LoopRegion loop = regions[i] as LoopRegion;
                if (loop != null)
                    parents[loop.Body] = loop;
                if (parents.ContainsKey(regions[i]))
                {
                    Region parent = parents[regions[i]];
                    regionsInputs[i] = functions[parent, RegionDirection.In, regions[i]](regionsInputs[regions.IndexOf(parent)]);
                }
            }

            int numOfBlocks = graph.Count();
            
            for(int i = 0; i < numOfBlocks; ++i)
            {
                var curBlock = regions[i].Header;
                int curBlockId =  curBlock.BlockId;
                
                result.In[curBlockId] = regionsInputs[i];
                result.Out[curBlockId] = param.TransferFunction(regionsInputs[i], curBlock);
            }

            return result;
        }
    }
}
