using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataFlowAnalysis.IntermediateRepresentation.ControlFlowGraph;
using DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode.Model;
using QuickGraph;

namespace UnitTests
{
    [TestClass]
    public class MeetOverPathsTests
    {
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
            var blocks = CreateTestGraph1Blocks();
            var graph = new Graph(blocks);

            var paths = GraphAlgorithms.FindAllPaths(graph, blocks.Blocks[6].BlockId);
            foreach (var path in paths)
            {
                foreach (var block in path)
                {
                    Trace.Write(blocks.Blocks.FindIndex(basicBlock => basicBlock.BlockId == block.BlockId) , " ");
                }
                Trace.WriteLine("");
            }
        }
    }
}
