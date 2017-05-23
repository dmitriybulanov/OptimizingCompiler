using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode;
using DataFlowAnalysis.IntermediateRepresentation.ControlFlowGraph;
using DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode;
using DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode.Model;
using DataFlowAnalysis.IterativeAlgorithm;
using DataFlowAnalysis.SpecificIterativeAlgorithmParametrs.AvailableExpressions;
using GPPGParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SyntaxTree;
using SyntaxTree.SyntaxNodes;
using Expression = DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode.Model.Expression;
using Int32Const = DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode.Model.Int32Const;
using identifier = DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode.Model.Identifier;
using BinaryOperation = DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode.Model.BinaryOperation;

namespace UnitTests.IterativeAlgorithmTests
{
    [TestClass]
    public class AvailableExpressionTest
    {
        [TestMethod]
        public void AvailableExpressionsTest()
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
            Trace.WriteLine(threeAddressCode);

            var basicBlocks = BasicBlocksGenerator.CreateBasicBlocks(threeAddressCode);
            Trace.WriteLine(Environment.NewLine + "Базовые блоки");
            Trace.WriteLine(basicBlocks);

            Trace.WriteLine(Environment.NewLine + "Управляющий граф программы");
            Graph g = new Graph(basicBlocks);
            Trace.WriteLine(g);


            
            Trace.WriteLine(Environment.NewLine + "Доступные выражения");
            var availableExprs = IterativeAlgorithm.Apply(g, new AvailableExpressionsCalculator(g));
            var outExpressions = availableExprs.Out.Select(
                pair => $"{pair.Key}: {string.Join(", ", pair.Value.Select(ex => ex.ToString()))}");

            foreach (var outInfo in outExpressions)
            {
                Trace.WriteLine(outInfo);
            }

            int startIndex = availableExprs.Out.Keys.Min();
            Assert.IsTrue(availableExprs.Out[startIndex]
                .SetEquals(new Expression[]
                    {
                        new Int32Const(4),
                        new BinaryOperation(new identifier("a"), Operation.Add, new identifier("b")),
                        new identifier("t0")
                    }));

            Assert.IsTrue(availableExprs.Out[startIndex + 1]
                .SetEquals(new Expression[]
                    {
                        new Int32Const(4),
                        new Int32Const(3),
                        new identifier("t0")
                    }));

            Assert.IsTrue(availableExprs.Out[startIndex + 2]
                .SetEquals(
                    new Expression[]
                    {
                        new Int32Const(4),
                        new Int32Const(2),
                        new identifier("t0")
                    }));

            Assert.IsTrue(availableExprs.Out[startIndex + 3]
                .SetEquals(
                   new Expression[]
                   {
                       new Int32Const(4),
                       new identifier("t0")
                   }));
        }
    }
}
