using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode;
using DataFlowAnalysis.IntermediateRepresentation.ControlFlowGraph;
using DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode;
using DataFlowAnalysis.IterativeAlgorithm;
using DataFlowAnalysis.SpecificIterativeAlgorithmParametrs.DeadAliveVariables;
using GPPGParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SyntaxTree.SyntaxNodes;

namespace UnitTests.IterativeAlgorithmTests
{
    [TestClass]
    public class DeadAliveVariablesTest
    {
        [TestMethod]
        public void DeadAliveVariables1()
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

            var deadAliveVars = IterativeAlgorithm.Apply(graph, new DeadAliveIterativeAlgorithmParameters());

            var startIndex = graph.GetMinBlockId();
            // check outs
            Assert.IsTrue(deadAliveVars.Out[startIndex]
                .SetEquals(new[]
                {
                    "a", "b"
                }));

            Assert.IsTrue(deadAliveVars.Out[startIndex + 1].Count == 0);

            Assert.IsTrue(deadAliveVars.Out[startIndex + 2]
                .SetEquals(new[]
                {
                    "a"
                }));

            Assert.IsTrue(deadAliveVars.Out[startIndex + 3].Count == 0);

            // check ins
            Assert.IsTrue(deadAliveVars.In[startIndex].Count == 0);
            Assert.IsTrue(deadAliveVars.In[startIndex + 1].SetEquals(deadAliveVars.Out[startIndex]));
            Assert.IsTrue(deadAliveVars.In[startIndex + 2].SetEquals(deadAliveVars.Out[startIndex + 1]));
            Assert.IsTrue(deadAliveVars.In[startIndex + 3].SetEquals(deadAliveVars.Out[startIndex + 2]));
        }

        [TestMethod]
        public void DeadAliveVariables2()
        {
            string text = @"
x = 1;
y = z + 2;

1: z = x - 1;
2: x = y * z;
";
            SyntaxNode root = ParserWrap.Parse(text);
            Graph graph = new Graph(
                BasicBlocksGenerator.CreateBasicBlocks(
                    ThreeAddressCodeGenerator.CreateAndVisit(root).Program));

            var deadAliveVars = IterativeAlgorithm.Apply(graph, new DeadAliveIterativeAlgorithmParameters());

            var startIndex = graph.GetMinBlockId();
            // check outs
            Assert.IsTrue(deadAliveVars.Out[startIndex]
                .SetEquals(new[]
                {
                    "x", "y"
                }));

            Assert.IsTrue(deadAliveVars.Out[startIndex + 1]
                .SetEquals(new[]
                {
                    "y", "z"
                }));

            Assert.IsTrue(deadAliveVars.Out[startIndex + 2].Count == 0);

            // check ins
            Assert.IsTrue(deadAliveVars.In[startIndex].Count == 1);
            Assert.IsTrue(deadAliveVars.In[startIndex + 1].SetEquals(deadAliveVars.Out[startIndex]));
            Assert.IsTrue(deadAliveVars.In[startIndex + 2].SetEquals(deadAliveVars.Out[startIndex + 1]));
        }
    }
}
