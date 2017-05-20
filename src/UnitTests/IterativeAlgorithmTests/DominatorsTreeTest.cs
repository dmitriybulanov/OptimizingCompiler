using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode;
using DataFlowAnalysis.IntermediateRepresentation.ControlFlowGraph;
using DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode;
using DataFlowAnalysis.SpecificIterativeAlgorithmParametrs.Dominators;
using GPPGParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SyntaxTree.SyntaxNodes;

namespace UnitTests.IterativeAlgorithmTests
{
    [TestClass]
    public class DominatorsTreeTest
    {
        [TestMethod]
        public void DominatorsTreesTest()
        {
            string programText_1 = @"
b = 1;
if 1 
  b = 3;
else
  b = 2;
";

            string programText_2 = @"
b = 1;
if 1
  b = 3;
else
  for i = 1..5
    b = 6;
";

            string programText_3 = @"
i = 10;
1: i = i - 1;
if i > 0
  goto 1;
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

            var dominatorsTree = new DominatorsTree(g);
            foreach (var v in dominatorsTree.GetMap())
                Trace.WriteLine(v.Key + " " + v.Value);
            Assert.IsTrue(dominatorsTree.GetMap().OrderBy(x => x.Key).SequenceEqual(
                new Dictionary<int, int> { { 34, 34 }, { 35, 34 }, { 36, 34 }, { 37, 34 } }
                .OrderBy(x => x.Key)));

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

            dominatorsTree = new DominatorsTree(g);
            foreach (var v in dominatorsTree.GetMap())
                Trace.WriteLine(v.Key + " " + v.Value);
            Assert.IsTrue(dominatorsTree.GetMap().OrderBy(x => x.Key).SequenceEqual(
                new Dictionary<int, int> { { 38, 38 }, { 39, 38 }, { 40, 38 }, { 41, 40 }, { 42, 41 }, { 43, 41 }, { 44, 38 }  }
                .OrderBy(x => x.Key)));

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

            dominatorsTree = new DominatorsTree(g);
            foreach (var v in dominatorsTree.GetMap())
                Trace.WriteLine(v.Key + " " + v.Value);
            Assert.IsTrue(dominatorsTree.GetMap().OrderBy(x => x.Key).SequenceEqual(
                new Dictionary<int, int> { { 45, 45 }, { 46, 45 }, { 47, 46 }, { 48, 46 } }
                .OrderBy(x => x.Key)));
        }
    }
}
