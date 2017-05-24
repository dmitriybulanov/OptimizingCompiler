using System;
using System.Collections.Generic;
using DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode;
using DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode.Model;
using DataFlowAnalysis.IntermediateRepresentation.ControlFlowGraph;
using GPPGParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SyntaxTree;
using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode;
using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode.Model;

namespace UnitTests.IntermediateRepresentationTests
{
    [TestClass]
    public class CFG2TACodeTransformationTests
    {
        [TestMethod]
        public void CFG2TACodeTransformationTest()
        {
            foreach (string sourseText in sources)
            {
                SyntaxTree.SyntaxNodes.SyntaxNode root = ParserWrap.Parse(sourseText);
                var sourceThreeAddressCode = ThreeAddressCodeGenerator.CreateAndVisit(root).Program;
                BasicBlocksList bbl = BasicBlocksGenerator.CreateBasicBlocks(sourceThreeAddressCode);
                Graph cfg = new Graph(bbl);
                List<ThreeAddressCommand> resultThreeAddressCode = cfg.transformToThreeAddressCode();

                CollectionAssert.AreEqual(sourceThreeAddressCode.Commands, resultThreeAddressCode);
            }
        }

        public string[] sources =
        {
            @"
            a = 5;
            a = 1;",

            @"
            if 1
            {
                a = 5;
                a = 1;
            }
            else
            {   
                for i = 1..5 
                    a = 1;
            }",

            @"for i = 1 .. 5
                for j = 1 .. 5
                    if 1
                        a = 1;
                    else
                    {   
                        a = 1;
                        a = 2;
                    }",

            @"
            if 1
                if 1
                {
                    a = 5;
                    a = 1;
                }
                else
                {   
                    for i = 1..5 
                        a = 1;
                }
            while 1
                a = 1;
            a = 2;

            for i = 1 .. 5
                for j = 1 .. 5
                    a = 1;
        
            for i = 1 .. 5
                for j = 1 .. 5
                    if 1
                        a = 1;
                    else
                    {   
                        a = 1;
                        a = 2;
                    }"
        };
    }
}
