using System;
using SyntaxTree.SyntaxNodes;
using GPPGParser;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode;
using DataFlowAnalysis.IntermediateRepresentation.ControlFlowGraph;
using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode;
using DataFlowAnalysis.IntermediateRepresentation.Regions;
using DataFlowAnalysis.IntermediateRepresentation.Regions.Model;

namespace UnitTests.IntermediateRepresentationTests
{
    [TestClass]
    public class RegionSequenceTest
    {
        [TestMethod]
        public void CreateSequenceTest()
        {
            string programText1 = @"
                                i = 1; 
                                j = 4; 
                                a = 2; 
                                while i < 20 
                                { 
                                i = i + 1; 
                                j = j + 1; 
                                if i > a 
                                a = a + 5; 
                                while j < 5
                                {
                                a = 4;
                                }
                                i = i + 1; 
                                }";

            SyntaxNode root = ParserWrap.Parse(programText1);
            var threeAddressCode = ThreeAddressCodeGenerator.CreateAndVisit(root).Program;
            Graph g = new Graph(BasicBlocksGenerator.CreateBasicBlocks(threeAddressCode));
            RegionSequence seq = new RegionSequence();

            List<Region> seqList = seq.CreateSequence(g);
            Assert.IsTrue(seqList.Count == 14);

            string programText2 = @"
                                i = 1; 
                                j = 4; 
                                a = 2; 
                                while i < 20 
                                { 
                                j = j + 1;  
                                i = i + 1; 
                                }";

            SyntaxNode root2 = ParserWrap.Parse(programText2);
            var threeAddressCode2 = ThreeAddressCodeGenerator.CreateAndVisit(root2).Program;
            Graph g2 = new Graph(BasicBlocksGenerator.CreateBasicBlocks(threeAddressCode2));
            RegionSequence seq2 = new RegionSequence();

            List<Region> seqList2 = seq2.CreateSequence(g2);

            Assert.IsTrue(seqList2.Count == 7);
        }
    }
}
