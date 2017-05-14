using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode.Model;
using DataFlowAnalysis.IntermediateRepresentation.ControlFlowGraph;
using DataFlowAnalysis.IntermediateRepresentation.Regions;
using DataFlowAnalysis.IntermediateRepresentation.Regions.Model;
using DataFlowAnalysis.IterativeAlgorithm;
using DataFlowAnalysis.IterativeAlgorithm.IterativeAlgorithmParameters;
using DataFlowAnalysis.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace DataFlowAnalysis.RegionsAlgorithm
{
    public static class RegionsAlgorithm
    {
        public static IterativeAlgorithmOutput<ISet<V>> Apply<V>(Graph graph, SetIterativeAlgorithmParameters<V> param)
        {
            List<Region> regions = new RegionSequence().CreateSequence(graph);
            return ApplyDescendingPart(regions, ApplyAscendingPart<V>(graph, regions, param));
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
                if(leaf != null)
                {
                    result[r, RegionDirection.In, r] = Identity;
                    result[r, RegionDirection.Out, r] = input => param.TransferFunction(input, leaf.Block);
                }
                BodyRegion body = r as BodyRegion;
                if(body != null)
                    foreach(Region s in body.Regions)
                    {
                        result[r, RegionDirection.In, s] = input => GatherFunctionsResults(input, result, r, body.Header.InputBlocks, graph, param);
                        CalculateForOutputBlocks(result, r, s, s.OutputBlocks, graph);
                    }
                LoopRegion loop = r as LoopRegion;
                if(loop != null)
                {
                    result[r, RegionDirection.In, loop.Body] = input => SetFactory.GetSet<V>(input.Union(GatherFunctionsResults(input, result, loop.Body, loop.Header.InputBlocks, graph, param)));
                    CalculateForOutputBlocks(result, r, loop.Body, loop.OutputBlocks, graph);
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
                result[r, RegionDirection.Out, b] = input => result[s, RegionDirection.Out, b](result[r, RegionDirection.In, s](input));
            }
        }

        static IterativeAlgorithmOutput<V> ApplyDescendingPart<V>(List<Region> regions, TransferFunctionStorage<V> functions)
        {
            throw new System.NotImplementedException();
        }
    }
}
