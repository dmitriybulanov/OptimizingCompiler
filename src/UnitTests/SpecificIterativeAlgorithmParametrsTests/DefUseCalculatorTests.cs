using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode.Model;
using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode;
using DataFlowAnalysis.IntermediateRepresentation.ControlFlowGraph;
using DataFlowAnalysis.IntermediateRepresentation.EdgeClassification;
using DataFlowAnalysis.IntermediateRepresentation.EdgeClassification.Model;
using DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode;
using DataFlowAnalysis.SpecificIterativeAlgorithmParametrs.DeadAliveVariables.DefUseCalculator;
using GPPGParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SyntaxTree.SyntaxNodes;


namespace UnitTests.SpecificIterativeAlgorithmParametrsTests
{
    [TestClass]
    public class DefUseCalculatorTests
    {
        [TestMethod]
        public void TestDefUse()
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
            BasicBlocksList basicBlocks = BasicBlocksGenerator.CreateBasicBlocks(threeAddressCode);

            DefUseBlockCalculator defUse = new DefUseBlockCalculator();

            var defUseList = new List<KeyValuePair<int, Tuple<ISet<string>, ISet<string>>>>();
            foreach (var block in basicBlocks)
            {
                defUseList.Add(new KeyValuePair<int, Tuple<ISet<string>, ISet<string>>>(block.BlockId, defUse.GetDefUseSetsByBlock(block)));
            }

            Assert.IsTrue(defUseList[0].Value.Item1                
                .SetEquals(
                    new HashSet<string>(new string[]
                    {
                        "a",
                        "b",
                        "t0",
                        "c"
                    })));
            Assert.IsTrue(defUseList[0].Value.Item2.Count == 0);

            Assert.IsTrue(defUseList[1].Value.Item1
                .SetEquals(
                    new HashSet<string>(new string[]
                    {
                        "a"
                    })));
            Assert.IsTrue(defUseList[1].Value.Item2.Count == 0);

            Assert.IsTrue(defUseList[2].Value.Item1
                .SetEquals(
                    new HashSet<string>(new string[]
                    {
                        "b"
                    })));
            Assert.IsTrue(defUseList[2].Value.Item2.Count == 0);

            Assert.IsTrue(defUseList[3].Value.Item2
                .SetEquals(
                    new HashSet<string>(new string[]
                    {
                        "c"
                    })));
            Assert.IsTrue(defUseList[3].Value.Item1.Count == 0);
        }
    }
}
