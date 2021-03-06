﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode;
using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataFlowAnalysis.IntermediateRepresentation.ControlFlowGraph;
using DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode;
using DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode.Model;
using DataFlowAnalysis.IterativeAlgorithm;
using QuickGraph;
using DataFlowAnalysis.MeetOverPaths;
using DataFlowAnalysis.SpecificIterativeAlgorithmParametrs.AvailableExpressions;
using DataFlowAnalysis.SpecificIterativeAlgorithmParametrs.ConstantsPropagation;
using DataFlowAnalysis.SpecificIterativeAlgorithmParametrs.DeadAliveVariables;
using GPPGParser;
using SyntaxTree;
using SyntaxTree.SyntaxNodes;
using Expression = DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode.Model.Expression;
using Identifier = DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode.Model.Identifier;
using Int32Const = DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode.Model.Int32Const;

namespace UnitTests
{
    [TestClass]
    public class MeetOverPathsTests
    {
        /// Generates example graph:
        ///        B0
        ///     /  |  \
        ///   /    |    \
        ///  B1    B4   B8
        ///   |   / \    |\
        ///  B2  B5 B6   | B9
        ///   | / \ / \  |/
        ///   B3   B7  B10
        ///     \  |  /
        ///       B11
        public BasicBlocksList CreateTestGraph2Blocks()
        {
            var blocks = new BasicBlocksList();
            for (int i = 0; i < 12; i++)
            {
                blocks.Add(new BasicBlock(new List<ThreeAddressCommand>(), new List<int>(), null));
            }
            blocks.Blocks[0].InputBlocks = new List<int>();
            blocks.Blocks[1].InputBlocks = new List<int> { blocks.Blocks[0].BlockId };
            blocks.Blocks[2].InputBlocks = new List<int> { blocks.Blocks[1].BlockId };
            blocks.Blocks[3].InputBlocks = new List<int> { blocks.Blocks[2].BlockId, blocks.Blocks[5].BlockId };
            blocks.Blocks[4].InputBlocks = new List<int> { blocks.Blocks[0].BlockId };
            blocks.Blocks[5].InputBlocks = new List<int> { blocks.Blocks[4].BlockId };
            blocks.Blocks[6].InputBlocks = new List<int> { blocks.Blocks[4].BlockId };
            blocks.Blocks[7].InputBlocks = new List<int> { blocks.Blocks[5].BlockId, blocks.Blocks[6].BlockId };
            blocks.Blocks[8].InputBlocks = new List<int> { blocks.Blocks[0].BlockId };
            blocks.Blocks[9].InputBlocks = new List<int> { blocks.Blocks[8].BlockId };
            blocks.Blocks[10].InputBlocks = new List<int> { blocks.Blocks[8].BlockId, blocks.Blocks[9].BlockId, blocks.Blocks[6].BlockId };
            blocks.Blocks[11].InputBlocks = new List<int> { blocks.Blocks[3].BlockId, blocks.Blocks[7].BlockId, blocks.Blocks[10].BlockId };

            return blocks;
        }
        public Graph CreateTestGraph2()
        {
            return new Graph(CreateTestGraph2Blocks());
        }
        
        [TestMethod]
        public void TestGraph2()
        {
            var blocks = CreateTestGraph2Blocks();
            var graph = new Graph(blocks);

            // Check block 0
            var block0Childrens = graph.getChildren(blocks.Blocks[0].BlockId);
            Assert.AreEqual(block0Childrens.Blocks.Count, 3);
            Assert.IsTrue(block0Childrens.Blocks.Exists(block => block.BlockId == blocks.Blocks[1].BlockId));
            Assert.IsTrue(block0Childrens.Blocks.Exists(block => block.BlockId == blocks.Blocks[4].BlockId));
            Assert.IsTrue(block0Childrens.Blocks.Exists(block => block.BlockId == blocks.Blocks[8].BlockId));

            // Check block 1
            var block1Childrens = graph.getChildren(blocks.Blocks[1].BlockId);
            Assert.AreEqual(block1Childrens.Blocks.Count, 1);
            Assert.IsTrue(block1Childrens.Blocks.Exists(block => block.BlockId == blocks.Blocks[2].BlockId));

            // Check block 2
            var block2Childrens = graph.getChildren(blocks.Blocks[2].BlockId);
            Assert.AreEqual(block2Childrens.Blocks.Count, 1);
            Assert.IsTrue(block2Childrens.Blocks.Exists(block => block.BlockId == blocks.Blocks[3].BlockId));

            // Check block 3
            var block3Childrens = graph.getChildren(blocks.Blocks[3].BlockId);
            Assert.AreEqual(block3Childrens.Blocks.Count, 1);
            Assert.IsTrue(block3Childrens.Blocks.Exists(block => block.BlockId == blocks.Blocks[11].BlockId));

            // Check block 4
            var block4Childrens = graph.getChildren(blocks.Blocks[4].BlockId);
            Assert.AreEqual(block4Childrens.Blocks.Count, 2);
            Assert.IsTrue(block4Childrens.Blocks.Exists(block => block.BlockId == blocks.Blocks[5].BlockId));
            Assert.IsTrue(block4Childrens.Blocks.Exists(block => block.BlockId == blocks.Blocks[6].BlockId));

            // Check block 5
            var block5Childrens = graph.getChildren(blocks.Blocks[5].BlockId);
            Assert.AreEqual(block5Childrens.Blocks.Count, 2);
            Assert.IsTrue(block5Childrens.Blocks.Exists(block => block.BlockId == blocks.Blocks[3].BlockId));
            Assert.IsTrue(block5Childrens.Blocks.Exists(block => block.BlockId == blocks.Blocks[7].BlockId));

            // Check block 6
            var block6Childrens = graph.getChildren(blocks.Blocks[6].BlockId);
            Assert.AreEqual(block6Childrens.Blocks.Count, 2);
            Assert.IsTrue(block6Childrens.Blocks.Exists(block => block.BlockId == blocks.Blocks[7].BlockId));
            Assert.IsTrue(block6Childrens.Blocks.Exists(block => block.BlockId == blocks.Blocks[10].BlockId));

            // Check block 7
            var block7Childrens = graph.getChildren(blocks.Blocks[7].BlockId);
            Assert.AreEqual(block7Childrens.Blocks.Count, 1);
            Assert.IsTrue(block7Childrens.Blocks.Exists(block => block.BlockId == blocks.Blocks[11].BlockId));

            // Check block 8
            var block8Childrens = graph.getChildren(blocks.Blocks[8].BlockId);
            Assert.AreEqual(block8Childrens.Blocks.Count, 2);
            Assert.IsTrue(block8Childrens.Blocks.Exists(block => block.BlockId == blocks.Blocks[9].BlockId));
            Assert.IsTrue(block8Childrens.Blocks.Exists(block => block.BlockId == blocks.Blocks[10].BlockId));

            // Check block 9
            var block9Childrens = graph.getChildren(blocks.Blocks[9].BlockId);
            Assert.AreEqual(block9Childrens.Blocks.Count, 1);
            Assert.IsTrue(block9Childrens.Blocks.Exists(block => block.BlockId == blocks.Blocks[10].BlockId));

            // Check block 10
            var block10Childrens = graph.getChildren(blocks.Blocks[10].BlockId);
            Assert.AreEqual(block10Childrens.Blocks.Count, 1);
            Assert.IsTrue(block10Childrens.Blocks.Exists(block => block.BlockId == blocks.Blocks[11].BlockId));

            // Check block 11
            var block11Childrens = graph.getChildren(blocks.Blocks[11].BlockId);
            Assert.AreEqual(block11Childrens.Blocks.Count, 0);

        }

        /// Generates example graph:
        ///    B0
        ///   /  \
        ///  B1  B2
        ///   \  /
        ///    B3
        ///   /  \
        ///  B4  B5
        ///   \  /
        ///    B6
        public BasicBlocksList CreateTestGraph1Blocks()
        {
            var blocks = new BasicBlocksList();
            for (int i = 0; i < 7; i++)
            {
                blocks.Add(new BasicBlock(new List<ThreeAddressCommand>(), new List<int>(), null));
            }
            blocks.Blocks[0].InputBlocks = new List<int>();
            blocks.Blocks[1].InputBlocks = new List<int> {blocks.Blocks[0].BlockId};
            blocks.Blocks[2].InputBlocks = new List<int> {blocks.Blocks[0].BlockId};
            blocks.Blocks[3].InputBlocks = new List<int> {blocks.Blocks[1].BlockId, blocks.Blocks[2].BlockId};
            blocks.Blocks[4].InputBlocks = new List<int> {blocks.Blocks[3].BlockId};
            blocks.Blocks[5].InputBlocks = new List<int> {blocks.Blocks[3].BlockId};
            blocks.Blocks[6].InputBlocks = new List<int> {blocks.Blocks[4].BlockId, blocks.Blocks[5].BlockId};

            return blocks;
        }

        public Graph CreateTestGraph1()
        {
            return new Graph(CreateTestGraph1Blocks());
        }

        [TestMethod]
        public void TestGraph1()
        {
            var blocks = CreateTestGraph1Blocks();
            var graph = new Graph(blocks);

            // Check block 0
            var block0Childrens = graph.getChildren(blocks.Blocks[0].BlockId);
            Assert.AreEqual(block0Childrens.Blocks.Count, 2);
            Assert.IsTrue(block0Childrens.Blocks.Exists(block => block.BlockId == blocks.Blocks[1].BlockId));
            Assert.IsTrue(block0Childrens.Blocks.Exists(block => block.BlockId == blocks.Blocks[2].BlockId));

            // Check block 1
            var block1Childrens = graph.getChildren(blocks.Blocks[1].BlockId);
            Assert.AreEqual(block1Childrens.Blocks.Count, 1);
            Assert.IsTrue(block1Childrens.Blocks.Exists(block => block.BlockId == blocks.Blocks[3].BlockId));

            // Check block 2
            var block2Childrens = graph.getChildren(blocks.Blocks[2].BlockId);
            Assert.AreEqual(block2Childrens.Blocks.Count, 1);
            Assert.IsTrue(block2Childrens.Blocks.Exists(block => block.BlockId == blocks.Blocks[3].BlockId));
            
            // Check block 3
            var block3Childrens = graph.getChildren(blocks.Blocks[3].BlockId);
            Assert.AreEqual(block3Childrens.Blocks.Count, 2);
            Assert.IsTrue(block3Childrens.Blocks.Exists(block => block.BlockId == blocks.Blocks[4].BlockId));
            Assert.IsTrue(block3Childrens.Blocks.Exists(block => block.BlockId == blocks.Blocks[5].BlockId));

            // Check block 4
            var block4Childrens = graph.getChildren(blocks.Blocks[4].BlockId);
            Assert.AreEqual(block4Childrens.Blocks.Count, 1);
            Assert.IsTrue(block4Childrens.Blocks.Exists(block => block.BlockId == blocks.Blocks[6].BlockId));

            // Check block 5
            var block5Childrens = graph.getChildren(blocks.Blocks[5].BlockId);
            Assert.AreEqual(block5Childrens.Blocks.Count, 1);
            Assert.IsTrue(block5Childrens.Blocks.Exists(block => block.BlockId == blocks.Blocks[6].BlockId));

            // Check block 6
            var block6Childrens = graph.getChildren(blocks.Blocks[6].BlockId);
            Assert.AreEqual(block6Childrens.Blocks.Count, 0);
        }

        [TestMethod]
        public void FindAllPathsTest()
        {
            // Test graph 1
            {
                var blocks = CreateTestGraph1Blocks();
                var graph = new Graph(blocks);

                var benchmarkPaths = new List<List<int>>
            {
                // 0 -> 1 -> 3 -> 4 -> 6
                new List<int>{ 0,1,3,4,6 },
                // 0 -> 1 -> 3 -> 5 -> 6
                new List<int>{ 0,1,3,5,6 },
                // 0 -> 2 -> 3 -> 4 -> 6
                new List<int>{ 0,2,3,4,6 },
                // 0 -> 2 -> 3 -> 5 -> 6
                new List<int>{ 0,2,3,5,6 }
            }.Select(x => x.Select(y => blocks.Blocks[y].BlockId)).ToList();

                var benchmarkPathsChecked = Enumerable.Repeat(false, benchmarkPaths.Count).ToList();

                var paths = GraphAlgorithms.FindAllPaths(graph, blocks.Blocks[6].BlockId).ToList();
                Assert.AreEqual(paths.Count, 4);
                foreach (var path in paths)
                {
                    foreach (var block in path)
                    {
                        Trace.Write(blocks.Blocks.FindIndex(basicBlock => basicBlock.BlockId == block.BlockId) + " ");
                    }
                    Trace.WriteLine("");

                    for (int i = 0; i < benchmarkPaths.Count; i++)
                    {
                        benchmarkPathsChecked[i] = benchmarkPathsChecked[i] || benchmarkPaths[i].SequenceEqual(path.Select(block => block.BlockId));
                    }
                }

                Assert.IsTrue(benchmarkPathsChecked.All(x => x));
            }

            {
                // Test graph 2
                var blocks = CreateTestGraph2Blocks();
                var graph = new Graph(blocks);

                var benchmarkPaths = new List<List<int>>
                {
                    new List<int>{ 0,1,2,3,11 },
                    new List<int>{ 0,4,5,3,11 },
                    new List<int>{ 0,4,5,7,11 },
                    new List<int>{ 0,4,6,7,11 },
                    new List<int>{ 0,4,6,10,11 },
                    new List<int>{ 0,8,10,11 },
                    new List<int>{ 0,8,9,10,11 }
                }.Select(x => x.Select(y => blocks.Blocks[y].BlockId)).ToList();

                var benchmarkPathsChecked = Enumerable.Repeat(false, benchmarkPaths.Count).ToList();

                var paths = GraphAlgorithms.FindAllPaths(graph, blocks.Blocks[11].BlockId).ToList();
                Assert.AreEqual(paths.Count, 7);
                foreach (var path in paths)
                {
                    foreach (var block in path)
                    {
                        Trace.Write(blocks.Blocks.FindIndex(basicBlock => basicBlock.BlockId == block.BlockId) + " ");
                    }
                    Trace.WriteLine("");

                    for (int i = 0; i < benchmarkPaths.Count; i++)
                    {
                        benchmarkPathsChecked[i] = benchmarkPathsChecked[i] || benchmarkPaths[i].SequenceEqual(path.Select(block => block.BlockId));
                    }
                }

                Assert.IsTrue(benchmarkPathsChecked.All(x => x));
            }
            
        }

        [TestMethod]
        public void AvailableExpression()
        {
            string programText = @"
a = 4;
b = 4;
c = a + b;
if 1 
  a = 3;
else
  b = 2;
print(c);
";
            SyntaxNode root = ParserWrap.Parse(programText);
            var threeAddressCode = ThreeAddressCodeGenerator.CreateAndVisit(root).Program;
            var basicBlocks = BasicBlocksGenerator.CreateBasicBlocks(threeAddressCode);
            Graph g = new Graph(basicBlocks);

            var availableExprsIterative = IterativeAlgorithm.Apply(g, new AvailableExpressionsCalculator(g));
            var availableExprsMOP = MeetOverPaths.Apply(g, new AvailableExpressionsCalculator(g));
            var it = availableExprsIterative.Out.Select(
                pair => $"{pair.Key}: {string.Join(", ", pair.Value.Select(ex => ex.ToString()))}");

            foreach (var outInfo in it)
            {
                Trace.WriteLine(outInfo);
            }

            var mop = availableExprsMOP.Select(
                pair => $"{pair.Key}: {string.Join(", ", pair.Value.Select(ex => ex.ToString()))}");
            Trace.WriteLine("====");
            foreach (var outInfo in mop)
            {
                Trace.WriteLine(outInfo);
            }
            
            Assert.IsTrue(availableExprsIterative.Out.OrderBy(kvp => kvp.Key).
                Zip(availableExprsMOP.OrderBy(kvp => kvp.Key), (v1, v2) => v1.Key == v2.Key && v1.Value.SetEquals(v2.Value)).All(x => x));
        }

        [TestMethod]
        public void DeadAliveVariables()
        {
            string text = @"
a = 2;
b = 3;

1: c = a + b;
2: a = 3; 
b = 4;
3: c = a;
";
            SyntaxNode root = ParserWrap.Parse(text);
            Graph graph = new Graph(
                BasicBlocksGenerator.CreateBasicBlocks(
                    ThreeAddressCodeGenerator.CreateAndVisit(root).Program));

            var deadAliveVarsIterative = IterativeAlgorithm.Apply(graph, new DeadAliveIterativeAlgorithmParameters());
            var deadAliveVarsMOP = MeetOverPaths.Apply(graph, new DeadAliveIterativeAlgorithmParameters());
            var it = deadAliveVarsIterative.In.Select(
                pair => $"{pair.Key}: {string.Join(", ", pair.Value.Select(ex => ex.ToString()))}");

            foreach (var outInfo in it)
            {
                Trace.WriteLine(outInfo);
            }

            var mop = deadAliveVarsMOP.Select(
                pair => $"{pair.Key}: {string.Join(", ", pair.Value.Select(ex => ex.ToString()))}");
            Trace.WriteLine("====");
            foreach (var outInfo in mop)
            {
                Trace.WriteLine(outInfo);
            }

            Assert.IsTrue(deadAliveVarsIterative.In.OrderBy(kvp => kvp.Key).
                Zip(deadAliveVarsMOP.OrderBy(kvp => kvp.Key), (v1, v2) => v1.Key == v2.Key && v1.Value.SetEquals(v2.Value)).All(x => x));
        }

        [TestMethod]
        public void ConstantPropagation()
        {
            string text = @"
if 1
{
    x = 2;
    y = 3;
}
else
{
    x = 3;
    y = 2;
}
z = x + y;
";
            SyntaxNode root = ParserWrap.Parse(text);
            Graph graph = new Graph(
                BasicBlocksGenerator.CreateBasicBlocks(
                    ThreeAddressCodeGenerator.CreateAndVisit(root).Program));

            var constantPropagationIterative = IterativeAlgorithm.Apply(graph, new ConstantsPropagationParameters());
            var constantPropagationMOP = MeetOverPaths.Apply(graph, new ConstantsPropagationParameters());
            var it = constantPropagationIterative.Out.Select(
                pair => $"{pair.Key}: {string.Join(", ", pair.Value.Select(ex => ex.ToString()))}");

            foreach (var outInfo in it)
            {
                Trace.WriteLine(outInfo);
            }

            var mop = constantPropagationMOP.Select(
                pair => $"{pair.Key}: {string.Join(", ", pair.Value.Select(ex => ex.ToString()))}");
            Trace.WriteLine("====");
            foreach (var outInfo in mop)
            {
                Trace.WriteLine(outInfo);
            }

            Assert.IsFalse(constantPropagationIterative.In.OrderBy(kvp => kvp.Key).
                Zip(constantPropagationMOP.OrderBy(kvp => kvp.Key), (v1, v2) => 
                    v1.Key == v2.Key && v1.Value.OrderBy(kvp => kvp.Key).SequenceEqual(v2.Value.OrderBy(kvp => kvp.Key))).All(x => x));
        }
    }
}
