using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode;
using DataFlowAnalysis.IntermediateRepresentation.ControlFlowGraph;
using DataFlowAnalysis.IntermediateRepresentation.NaturalLoops;
using DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode;
using GPPGParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SyntaxTree.SyntaxNodes;

namespace UnitTests.IntermediateRepresentationTests
{
    [TestClass]
    public class NaturalLoopTest
    {
        [TestMethod]
        public void NaturalLoopsTest()
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

            var allNaturalLoops = SearchNaturalLoops.FindAllNaturalLoops(g);
            var tuples = new List<Tuple<int, int, SortedSet<int>>>();
            foreach (var v in allNaturalLoops)
            {
                Trace.Write(v.Key.Source.BlockId + " -> " + v.Key.Target.BlockId + " : ");
                foreach (int k in v.Value)
                    Trace.Write(k.ToString() + " ");
                Trace.WriteLine("");
                tuples.Add(new Tuple<int, int, SortedSet<int>>(v.Key.Source.BlockId, v.Key.Target.BlockId, new SortedSet<int>(v.Value)));
            }

            int start = g.GetMinBlockId();

            Assert.IsTrue(tuples.SequenceEqual(new List<Tuple<int, int, SortedSet<int>>>()));

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

            allNaturalLoops = SearchNaturalLoops.FindAllNaturalLoops(g);
            tuples = new List<Tuple<int, int, SortedSet<int>>>();
            foreach (var v in allNaturalLoops)
            {
                Trace.Write(v.Key.Source.BlockId + " -> " + v.Key.Target.BlockId + " : ");
                foreach (int k in v.Value)
                    Trace.Write(k.ToString() + " ");
                Trace.WriteLine("");
                tuples.Add(new Tuple<int, int, SortedSet<int>>(v.Key.Source.BlockId, v.Key.Target.BlockId, new SortedSet<int>(v.Value)));
            }

            start = g.GetMinBlockId();

            var check = new List<Tuple<int, int, SortedSet<int>>>()
            {
               new Tuple<int, int, SortedSet<int>>(start + 4, start + 3, new SortedSet<int>() { start + 3, start + 4 })
            };

            Assert.IsTrue(tuples.Count == 1);
            Assert.IsTrue(tuples[0].Item1 == check[0].Item1);
            Assert.IsTrue(tuples[0].Item2 == check[0].Item2);
            Assert.IsTrue(tuples[0].Item3.SequenceEqual(check[0].Item3));

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

            allNaturalLoops = SearchNaturalLoops.FindAllNaturalLoops(g);
            tuples = new List<Tuple<int, int, SortedSet<int>>>();
            foreach (var v in allNaturalLoops)
            {
                Trace.Write(v.Key.Source.BlockId + " -> " + v.Key.Target.BlockId + " : ");
                foreach (int k in v.Value)
                    Trace.Write(k.ToString() + " ");
                Trace.WriteLine("");
                tuples.Add(new Tuple<int, int, SortedSet<int>>(v.Key.Source.BlockId, v.Key.Target.BlockId, new SortedSet<int>(v.Value)));
            }

            start = g.GetMinBlockId();

            check = new List<Tuple<int, int, SortedSet<int>>>()
            {
               new Tuple<int, int, SortedSet<int>>(start + 2, start + 1, new SortedSet<int>() { start + 1, start + 2 })
            };

            Assert.IsTrue(tuples.Count == 1);
            Assert.IsTrue(tuples[0].Item1 == check[0].Item1);
            Assert.IsTrue(tuples[0].Item2 == check[0].Item2);
            Assert.IsTrue(tuples[0].Item3.SequenceEqual(check[0].Item3));
        }
    }
}
