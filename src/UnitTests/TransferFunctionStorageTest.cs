using System;
using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode;
using DataFlowAnalysis.IntermediateRepresentation.ControlFlowGraph;
using DataFlowAnalysis.IntermediateRepresentation.Regions;
using DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode;
using DataFlowAnalysis.IterativeAlgorithm;
using DataFlowAnalysis.Utilities;
using GPPGParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SyntaxTree.SyntaxNodes;

namespace UnitTests
{
    [TestClass]
    public class TransferFunctionStorageTest
    {
        [TestMethod]
        public void TransferFunctionStorage()
        {
string text = @"
    i = 1;
    j = 4;
    a = 2;
    while i < 20
    {  
        i = i + 1;
        j = j + 1;
        if i > a
            a = a + 5;
        i = i + 1;
    }";
            SyntaxNode root = ParserWrap.Parse(text);
            var graph = new Graph(BasicBlocksGenerator.CreateBasicBlocks(ThreeAddressCodeGenerator.CreateAndVisit(root).Program));
            var regions = new RegionSequence().CreateSequence(graph);
            var storage = new AbstractTransferFunctionStorage<int>();
            for (var i = 0; i < regions.Count - 1; i++)
            {
                storage[regions[i], RegionDirection.Out, regions[i + 1]] = 2 * i;
                storage[regions[i], RegionDirection.In, regions[i + 1]] = 2 * i + 1;
            }

            for (var i = 0; i < regions.Count - 1; i++)
            {
                Assert.IsTrue(storage[regions[i], RegionDirection.Out, regions[i + 1]] == 2 * i);
                Assert.IsTrue(storage[regions[i], RegionDirection.In, regions[i + 1]] == 2 * i + 1);
            }
        }
    }
}
