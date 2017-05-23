using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode.Model;
using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode;
using DataFlowAnalysis.IntermediateRepresentation.ControlFlowGraph;
using DataFlowAnalysis.IntermediateRepresentation.FindReverseEdges;
using DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode;
using GPPGParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SyntaxTree.SyntaxNodes;

namespace UnitTests.IntermediateRepresentationTests
{
    [TestClass]
    public class FindReverseEdgeTest
    {
        [TestMethod]
        public void FindReverseEdgesTest1()
        {
            string programText = @"
            for i = 1 + 2 * 3 .. 10
              println(i);
            ";

            SyntaxNode root = ParserWrap.Parse(programText);
            var threeAddressCode = ThreeAddressCodeGenerator.CreateAndVisit(root).Program;
            BasicBlocksList basicBlocks = BasicBlocksGenerator.CreateBasicBlocks(threeAddressCode);
            Graph g = new Graph(basicBlocks);
            var reverseEdges = FindReverseEdge.FindReverseEdges(g);

            List<Tuple<int, int>> tuples = new List<Tuple<int, int>>();
            foreach (var v in reverseEdges)
            {
                Trace.WriteLine(v.Source.BlockId + " -> " + v.Target.BlockId);
                tuples.Add(new Tuple<int, int>(v.Source.BlockId, v.Target.BlockId));
            }

            int start = g.GetMinBlockId();

            Assert.IsTrue(tuples.SequenceEqual(new List<Tuple<int, int>>()
            { new Tuple<int, int>(start + 2, start + 1) }
            ));
        }

        [TestMethod]
        public void FindReverseEdgesTest2()
        {
            string programText = @"
            x = 5;
            if x < 5 
            1: x = x + 1;
            else
            x = x - 1;
            println(x);
            goto 1;
            ";

            SyntaxNode root = ParserWrap.Parse(programText);
            var threeAddressCode = ThreeAddressCodeGenerator.CreateAndVisit(root).Program;
            BasicBlocksList basicBlocks = BasicBlocksGenerator.CreateBasicBlocks(threeAddressCode);
            Graph g = new Graph(basicBlocks);
            var reverseEdges = FindReverseEdge.FindReverseEdges(g);

            List<Tuple<int, int>> tuples = new List<Tuple<int, int>>();
            foreach (var v in reverseEdges)
            {
                Trace.WriteLine(v.Source.BlockId + " -> " + v.Target.BlockId);
                tuples.Add(new Tuple<int, int>(v.Source.BlockId, v.Target.BlockId));
            }

            int start = g.GetMinBlockId();

            Assert.IsTrue(tuples.SequenceEqual(new List<Tuple<int, int>>()));
        }

        [TestMethod]
        public void FindReverseEdgesTest3()
        {
            string programText = @"
            for i = 1..10
              if i < 5
                println(i);
              else
                println(i-5);
            ";

            SyntaxNode root = ParserWrap.Parse(programText);
            var threeAddressCode = ThreeAddressCodeGenerator.CreateAndVisit(root).Program;
            BasicBlocksList basicBlocks = BasicBlocksGenerator.CreateBasicBlocks(threeAddressCode);
            Graph g = new Graph(basicBlocks);
            var reverseEdges = FindReverseEdge.FindReverseEdges(g);
            
            List<Tuple<int, int>> tuples = new List<Tuple<int, int>>();
            foreach (var v in reverseEdges)
            {
                Trace.WriteLine(v.Source.BlockId + " -> " + v.Target.BlockId);
                tuples.Add(new Tuple<int, int>(v.Source.BlockId, v.Target.BlockId));
            }

            int start = g.GetMinBlockId();

            Assert.IsTrue(tuples.SequenceEqual(new List<Tuple<int, int>>()
            { new Tuple<int, int>(start + 5, start + 1) }
            ));
        }
    }
}
