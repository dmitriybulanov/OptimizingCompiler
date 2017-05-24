using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataFlowAnalysis.IntermediateRepresentation.CheckRetreatingIsReverse;
using SyntaxTree.SyntaxNodes;
using DataFlowAnalysis.IntermediateRepresentation.ControlFlowGraph;
using GPPGParser;
using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode.Model;
using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode;
using DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode;
using System.Diagnostics;

namespace UnitTests.IntermediateRepresentationTests
{
    [TestClass]
    public class RetreatingNotReverseEdgesTests
    {
        [TestMethod]
        public void RetreatingNotReverseEdges1()
        {
            var programText = File.ReadAllText("./../../RetreatingNotReverseEdgesEx1.txt");


            SyntaxNode root = ParserWrap.Parse(programText);
            Graph graph = new Graph(
                BasicBlocksGenerator.CreateBasicBlocks(
                    ThreeAddressCodeGenerator.CreateAndVisit(root).Program));

            Assert.IsFalse(CheckRetreatingIsReverse.CheckReverseEdges(graph));
        }

        [TestMethod]
        public void RetreatingNotReverseEdges2()
        {
            var programText = File.ReadAllText("./../../RetreatingNotReverseEdgesEx2.txt");

            SyntaxNode root = ParserWrap.Parse(programText);
            Graph graph = new Graph(
                BasicBlocksGenerator.CreateBasicBlocks(
                    ThreeAddressCodeGenerator.CreateAndVisit(root).Program));

            Assert.IsFalse(CheckRetreatingIsReverse.CheckReverseEdges(graph));
        }

        [TestMethod]
        public void RetreatingNotReverseEdges3()
        {
            var programText = File.ReadAllText("./../../RetreatingNotReverseEdgesEx3.txt");

            SyntaxNode root = ParserWrap.Parse(programText);
            Graph graph = new Graph(
                BasicBlocksGenerator.CreateBasicBlocks(
                    ThreeAddressCodeGenerator.CreateAndVisit(root).Program));

            Assert.IsFalse(CheckRetreatingIsReverse.CheckReverseEdges(graph));
        }
    }
}
