using System;
using System.Collections.Generic;
using DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode;
using DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode.Model;
using GPPGParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SyntaxTree;

namespace UnitTests.IntermediateRepresentationTests
{
    [TestClass]
    public class ThreeAddressCodeTests
    {
        [TestMethod]
        public void ThreeeAddressCodeTest()
        {
            string text = @"
            a = 2;
            while a > 1
                a = a - 1;";
            SyntaxTree.SyntaxNodes.SyntaxNode root = ParserWrap.Parse(text);
            var threeAddressCode = ThreeAddressCodeGenerator.CreateAndVisit(root).Program;
            
            var expectedCommands = new List<ThreeAddressCommand>
            {
                new Assignment("a", new Int32Const(2)),
                new Assignment("t0", new BinaryOperation("a", Operation.Greater, 1)),
                new ConditionalGoto("$GL_2", new BinaryOperation("t0", Operation.Equal, 0)) { Label = "$GL_1" },
                new Assignment("t1", new BinaryOperation("a", Operation.Subtract, 1)),
                new Assignment("a", new Identifier("t1")),
                new Goto("$GL_1"),
                new NoOperation("$GL_2")
            };

            CollectionAssert.AreEqual(threeAddressCode.Commands, expectedCommands);
        }
    }
}
