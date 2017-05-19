using System;
using System.Collections.Generic;
using System.Diagnostics;
using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataFlowAnalysis.IntermediateRepresentation.ControlFlowGraph;
using DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode.Model;

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
        [TestMethod]
        public void CreateTestGraph1()
        {
            var blocks = new BasicBlocksList();
            for (int i = 0; i < 7; i++)
            {
                blocks.Add(new BasicBlock(new List<ThreeAddressCommand>(), new List<int>(), null));
            }
            blocks.Blocks[0].OutputBlocks = new List<int>{ blocks.Blocks[1].BlockId, blocks.Blocks[2].BlockId };
            blocks.Blocks[1].OutputBlocks = new List<int>{ blocks.Blocks[3].BlockId };
            blocks.Blocks[2].OutputBlocks = new List<int>{ blocks.Blocks[3].BlockId };
            blocks.Blocks[3].OutputBlocks = new List<int>{ blocks.Blocks[4].BlockId, blocks.Blocks[5].BlockId };
            blocks.Blocks[4].OutputBlocks = new List<int>{ blocks.Blocks[6].BlockId };
            blocks.Blocks[5].OutputBlocks = new List<int>{ blocks.Blocks[6].BlockId };
            blocks.Blocks[6].OutputBlocks = new List<int>();
            
            var graph = new Graph(blocks);
            Trace.WriteLine(graph.ToString());
        }

        [TestMethod]
        public void FindAllPathsTest()
        {
            
        }
    }
}
