using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode;
using DataFlowAnalysis.IntermediateRepresentation.ControlFlowGraph;
using DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode;
using DataFlowAnalysis.IterativeAlgorithm.IterativeAlgorithmParameters.Model;
using DataFlowAnalysis.RegionsAlgorithm;
using DataFlowAnalysis.SpecificIterativeAlgorithmParametrs.ReachingDefinitions.ExplicitTransferFunction;
using GPPGParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SyntaxTree.SyntaxNodes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.RegionsAlgorithmTest
{
   // [TestClass]
    public class RegionsAlgorithmTest
    {
        //[TestMethod]
        public void RegionsAlgorithm1()
        {
            string programText = @"
    i = 1;
    j = 4;
    a = 2;
    while i < 20
    {  
        i = i + 1;
        j = j + 1;
        if i > a
            a = a + 5;
        i = i + 1;
    }
";
            SyntaxNode root = ParserWrap.Parse(programText);
            var threeAddressCode = ThreeAddressCodeGenerator.CreateAndVisit(root).Program;
            Trace.WriteLine(threeAddressCode);

            var basicBlocks = BasicBlocksGenerator.CreateBasicBlocks(threeAddressCode);
            Trace.WriteLine(Environment.NewLine + "Базовые блоки");
            Trace.WriteLine(basicBlocks);

            Trace.WriteLine(Environment.NewLine + "Управляющий граф программы");
            Graph g = new Graph(basicBlocks);
            Trace.WriteLine(g);

            Trace.WriteLine(Environment.NewLine + "Достигающие определения");

            var reachingDefs = RegionsAlgorithm.Apply(g, new ExplicitTransferFunction(g));
            var outDefs = reachingDefs.Out.Select(
                pair => $"{pair.Key}: {string.Join(", ", pair.Value.Select(ex => ex.ToString()))}");

            foreach (var outInfo in outDefs)
            {
                Trace.WriteLine(outInfo);
            }

            int startIndex = reachingDefs.Out.Keys.Min();
            Assert.IsTrue(reachingDefs.Out[startIndex]
                .SetEquals(
                    new SortedSet<CommandNumber>(new CommandNumber[]
                    {
                        new CommandNumber(startIndex, 0),
                        new CommandNumber(startIndex, 1),
                        new CommandNumber(startIndex, 2),
                        new CommandNumber(startIndex, 3)
                    })));

            Assert.IsTrue(reachingDefs.Out[startIndex + 1]
                .SetEquals(
                    new SortedSet<CommandNumber>(new CommandNumber[]
                    {
                        new CommandNumber(startIndex, 0),
                        new CommandNumber(startIndex, 1),
                        new CommandNumber(startIndex, 2),
                        new CommandNumber(startIndex, 3),
                        new CommandNumber(startIndex+2, 0),
                        new CommandNumber(startIndex+2, 2),
                        new CommandNumber(startIndex+2, 3),
                        new CommandNumber(startIndex+2, 4),
                        new CommandNumber(startIndex+3, 0),
                        new CommandNumber(startIndex+3, 1),
                        new CommandNumber(startIndex+4, 1),
                        new CommandNumber(startIndex+4, 2)
                    })));

            Assert.IsTrue(reachingDefs.Out[startIndex + 2]
                .SetEquals(
                    new SortedSet<CommandNumber>(new CommandNumber[]
                    {
                        new CommandNumber(startIndex, 2),
                        new CommandNumber(startIndex, 3),
                        new CommandNumber(startIndex+2, 0),
                        new CommandNumber(startIndex+2, 1),
                        new CommandNumber(startIndex+2, 2),
                        new CommandNumber(startIndex+2, 3),
                        new CommandNumber(startIndex+2, 4),
                        new CommandNumber(startIndex+3, 0),
                        new CommandNumber(startIndex+3, 1),
                        new CommandNumber(startIndex+4, 1)
                    })));

            Assert.IsTrue(reachingDefs.Out[startIndex + 3]
                .SetEquals(
                    new SortedSet<CommandNumber>(new CommandNumber[]
                    {
                        new CommandNumber(startIndex, 3),
                        new CommandNumber(startIndex+2, 0),
                        new CommandNumber(startIndex+2, 1),
                        new CommandNumber(startIndex+2, 2),
                        new CommandNumber(startIndex+2, 3),
                        new CommandNumber(startIndex+2, 4),
                        new CommandNumber(startIndex+3, 0),
                        new CommandNumber(startIndex+3, 1),
                        new CommandNumber(startIndex+4, 1)
                    })));
            Assert.IsTrue(reachingDefs.Out[startIndex + 4]
                .SetEquals(
                    new SortedSet<CommandNumber>(new CommandNumber[]
                    {
                        new CommandNumber(startIndex, 2),
                        new CommandNumber(startIndex, 3),
                        new CommandNumber(startIndex+2, 0),
                        new CommandNumber(startIndex+2, 2),
                        new CommandNumber(startIndex+2, 3),
                        new CommandNumber(startIndex+2, 4),
                        new CommandNumber(startIndex+3, 0),
                        new CommandNumber(startIndex+3, 1),
                        new CommandNumber(startIndex+4, 1),
                        new CommandNumber(startIndex+4, 2)
                    })));
            Assert.IsTrue(reachingDefs.Out[startIndex + 5]
                .SetEquals(
                    new SortedSet<CommandNumber>(new CommandNumber[]
                    {
                        new CommandNumber(startIndex, 0),
                        new CommandNumber(startIndex, 1),
                        new CommandNumber(startIndex, 2),
                        new CommandNumber(startIndex, 3),
                        new CommandNumber(startIndex+2, 0),
                        new CommandNumber(startIndex+2, 2),
                        new CommandNumber(startIndex+2, 3),
                        new CommandNumber(startIndex+2, 4),
                        new CommandNumber(startIndex+3, 0),
                        new CommandNumber(startIndex+3, 1),
                        new CommandNumber(startIndex+4, 1),
                        new CommandNumber(startIndex+4, 2)
                    })));
        }
    }
}
