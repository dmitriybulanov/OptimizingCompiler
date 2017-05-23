using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        public void FindReverseEdgesTest()
        {
            string programText_1 = @"
for i = 1 + 2 * 3 .. 10
  println(i);
";

            string programText_2 = @"
x = 5;
if x < 5 
1: x = x + 1;
else
x = x - 1;
println(x);
goto 1;
";

            string programText_3 = @"
for i = 1..10
  if i < 5
    println(i);
  else
    println(i-5);
";

            Trace.WriteLine("===============");
            Trace.WriteLine("Тест 1");
            Trace.WriteLine("===============");
            SyntaxNode root = ParserWrap.Parse(programText_1);
            var threeAddressCode = ThreeAddressCodeGenerator.CreateAndVisit(root).Program;
            Trace.WriteLine(threeAddressCode);

            var basicBlocks = BasicBlocksGenerator.CreateBasicBlocks(threeAddressCode);
            Trace.WriteLine(Environment.NewLine + "Базовые блоки");
            Trace.WriteLine(basicBlocks);

            Trace.WriteLine(Environment.NewLine + "Управляющий граф программы");
            Graph g = new Graph(basicBlocks);
            Trace.WriteLine(g);

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

            Trace.WriteLine("===============");
            Trace.WriteLine("Тест 2");
            Trace.WriteLine("===============");

            root = ParserWrap.Parse(programText_2);
            threeAddressCode = ThreeAddressCodeGenerator.CreateAndVisit(root).Program;
            Trace.WriteLine(threeAddressCode);

            basicBlocks = BasicBlocksGenerator.CreateBasicBlocks(threeAddressCode);
            Trace.WriteLine(Environment.NewLine + "Базовые блоки");
            Trace.WriteLine(basicBlocks);

            Trace.WriteLine(Environment.NewLine + "Управляющий граф программы");
            g = new Graph(basicBlocks);
            Trace.WriteLine(g);

            reverseEdges = FindReverseEdge.FindReverseEdges(g);
            tuples = new List<Tuple<int, int>>();
            foreach (var v in reverseEdges)
            {
                Trace.WriteLine(v.Source.BlockId + " -> " + v.Target.BlockId);
                tuples.Add(new Tuple<int, int>(v.Source.BlockId, v.Target.BlockId));
            }

            start = g.GetMinBlockId();

            Assert.IsTrue(tuples.SequenceEqual(new List<Tuple<int, int>>()));

            Trace.WriteLine("===============");
            Trace.WriteLine("Тест 3");
            Trace.WriteLine("===============");

            root = ParserWrap.Parse(programText_3);
            threeAddressCode = ThreeAddressCodeGenerator.CreateAndVisit(root).Program;
            Trace.WriteLine(threeAddressCode);

            basicBlocks = BasicBlocksGenerator.CreateBasicBlocks(threeAddressCode);
            Trace.WriteLine(Environment.NewLine + "Базовые блоки");
            Trace.WriteLine(basicBlocks);

            Trace.WriteLine(Environment.NewLine + "Управляющий граф программы");
            g = new Graph(basicBlocks);
            Trace.WriteLine(g);

            reverseEdges = FindReverseEdge.FindReverseEdges(g);
            tuples = new List<Tuple<int, int>>();
            foreach (var v in reverseEdges)
            {
                Trace.WriteLine(v.Source.BlockId + " -> " + v.Target.BlockId);
                tuples.Add(new Tuple<int, int>(v.Source.BlockId, v.Target.BlockId));
            }

            start = g.GetMinBlockId();

            Assert.IsTrue(tuples.SequenceEqual(new List<Tuple<int, int>>()
            { new Tuple<int, int>(start + 5, start + 1) }
            ));
        }
    }
}
