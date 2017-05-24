using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using GPPGParser;
using SyntaxTree.SyntaxNodes;
using DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode.Model;
using DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode;
using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode.Model;
using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.IntermediateRepresentationTests
{
    [TestClass]
    public class BasicBlockListTests
    {
        [TestMethod]
        public void BasicBlockList1()
        {
            string programText = @"
for i = 1 + 2 * 3 .. 10
  println(i);
";

            SyntaxNode root = ParserWrap.Parse(programText);
            var threeAddressCode = ThreeAddressCodeGenerator.CreateAndVisit(root).Program;
            var basicBlocks = BasicBlocksGenerator.CreateBasicBlocks(threeAddressCode);

            BasicBlocksList BBL = new BasicBlocksList();
            List<ThreeAddressCommand> commands = new List<ThreeAddressCommand>();

            commands.Add(new Assignment("t0",
                                        new BinaryOperation(new DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode.Model.Int32Const(2),
                                                            SyntaxTree.Operation.Multiply,
                                                            new DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode.Model.Int32Const(3))));
            commands.Add(new Assignment("t1",
                                        new BinaryOperation(new DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode.Model.Int32Const(1),
                                                            SyntaxTree.Operation.Add,
                                                            new DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode.Model.Identifier("t0"))));
            commands.Add(new Assignment("i",
                                        new DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode.Model.Identifier("t1")));
            BBL.Add(new BasicBlock(commands, new List<int>(), new List<int>() { 1 }));

            commands.Clear();
            commands.Add(new ConditionalGoto("$GL_2",
                                             new BinaryOperation(new DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode.Model.Identifier("i"),
                                                                 SyntaxTree.Operation.Greater,
                                                                 new DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode.Model.Int32Const(10))));
            commands[0].Label = "$GL_1";
            BBL.Add(new BasicBlock(commands, new List<int>() { 0, 2 }, new List<int>() { 2, 3 }));

            commands.Clear();
            commands.Add(new Print(new DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode.Model.Identifier("i"), true));
            commands.Add(new Assignment("i",
                                        new BinaryOperation(new DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode.Model.Identifier("i"),
                                                            SyntaxTree.Operation.Add,
                                                            new DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode.Model.Int32Const(1))));
            commands.Add(new Goto("$GL_1"));
            BBL.Add(new BasicBlock(commands, new List<int>() { 1 }, new List<int>() { 1 }));

            commands.Clear();
            commands.Add(new NoOperation("$GL_2"));
            BBL.Add(new BasicBlock(commands, new List<int>() { 1 }, new List<int>()));

            int start = basicBlocks.Blocks[0].BlockId;

            Assert.IsTrue(basicBlocks.Blocks[0].Commands.Select(x => x.ToString()).SequenceEqual(BBL.Blocks[0].Commands.Select(x => x.ToString())));
            Assert.IsTrue(basicBlocks.Blocks[0].InputBlocks.Select(x => x - start).SequenceEqual(BBL.Blocks[0].InputBlocks));
            Assert.IsTrue(basicBlocks.Blocks[0].OutputBlocks.Select(x => x - start).SequenceEqual(BBL.Blocks[0].OutputBlocks));

            Assert.IsTrue(basicBlocks.Blocks[1].Commands.Select(x => x.ToString()).SequenceEqual(BBL.Blocks[1].Commands.Select(x => x.ToString())));
            Assert.IsTrue(basicBlocks.Blocks[1].InputBlocks.Select(x => x - start).SequenceEqual(BBL.Blocks[1].InputBlocks));
            Assert.IsTrue(basicBlocks.Blocks[1].OutputBlocks.Select(x => x - start).SequenceEqual(BBL.Blocks[1].OutputBlocks));

            Assert.IsTrue(basicBlocks.Blocks[2].Commands.Select(x => x.ToString()).SequenceEqual(BBL.Blocks[2].Commands.Select(x => x.ToString())));
            Assert.IsTrue(basicBlocks.Blocks[2].InputBlocks.Select(x => x - start).SequenceEqual(BBL.Blocks[2].InputBlocks));
            Assert.IsTrue(basicBlocks.Blocks[2].OutputBlocks.Select(x => x - start).SequenceEqual(BBL.Blocks[2].OutputBlocks));

            Assert.IsTrue(basicBlocks.Blocks[3].Commands.Select(x => x.ToString()).SequenceEqual(BBL.Blocks[3].Commands.Select(x => x.ToString())));
            Assert.IsTrue(basicBlocks.Blocks[3].InputBlocks.Select(x => x - start).SequenceEqual(BBL.Blocks[3].InputBlocks));
            Assert.IsTrue(basicBlocks.Blocks[3].OutputBlocks.Select(x => x - start).SequenceEqual(BBL.Blocks[3].OutputBlocks));
        }

        [TestMethod]
        public void BasicBlockList2()
        {
            string programText = @"
x = 5;
if x < 5 
  x = x + 1;
else
  x = x - 1;
println(x);
";

            SyntaxNode root = ParserWrap.Parse(programText);
            var threeAddressCode = ThreeAddressCodeGenerator.CreateAndVisit(root).Program;
            var basicBlocks = BasicBlocksGenerator.CreateBasicBlocks(threeAddressCode);

            BasicBlocksList BBL = new BasicBlocksList();
            List<ThreeAddressCommand> commands = new List<ThreeAddressCommand>();

            commands.Add(new Assignment("x",
                                        new DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode.Model.Int32Const(5)));
            commands.Add(new Assignment("t0",
                                        new BinaryOperation(new DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode.Model.Identifier("x"),
                                                            SyntaxTree.Operation.Lesser,
                                                            new DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode.Model.Int32Const(5))));
            commands.Add(new ConditionalGoto("$GL_2",
                                             new BinaryOperation(new DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode.Model.Identifier("t0"),
                                                                SyntaxTree.Operation.Equal,
                                                                new DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode.Model.Int32Const(0))));
            BBL.Add(new BasicBlock(commands, new List<int>(), new List<int>() { 1, 2 }));

            commands.Clear();
            commands.Add(new Assignment("t1",
                                        new BinaryOperation(new DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode.Model.Identifier("x"),
                                                            SyntaxTree.Operation.Add,
                                                            new DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode.Model.Int32Const(1))));
            commands.Add(new Assignment("x",
                                        new DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode.Model.Identifier("t1")));
            commands.Add(new Goto("$GL_1"));
            BBL.Add(new BasicBlock(commands, new List<int>() { 0 }, new List<int>() { 3 }));

            commands.Clear();
            commands.Add(new NoOperation("$GL_2"));
            commands.Add(new Assignment("t2",
                            new BinaryOperation(new DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode.Model.Identifier("x"),
                                                SyntaxTree.Operation.Subtract,
                                                new DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode.Model.Int32Const(1))));
            commands.Add(new Assignment("x",
                                        new DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode.Model.Identifier("t2")));
            BBL.Add(new BasicBlock(commands, new List<int>() { 0 }, new List<int>() { 3 }));

            commands.Clear();
            commands.Add(new NoOperation("$GL_1"));
            commands.Add(new Print(new DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode.Model.Identifier("x"), true));
            BBL.Add(new BasicBlock(commands, new List<int>() { 1, 2 }, new List<int>()));

            int start = basicBlocks.Blocks[0].BlockId;

            Assert.IsTrue(basicBlocks.Blocks[0].Commands.Select(x => x.ToString()).SequenceEqual(BBL.Blocks[0].Commands.Select(x => x.ToString())));
            Assert.IsTrue(basicBlocks.Blocks[0].InputBlocks.Select(x => x - start).SequenceEqual(BBL.Blocks[0].InputBlocks));
            Assert.IsTrue(basicBlocks.Blocks[0].OutputBlocks.Select(x => x - start).SequenceEqual(BBL.Blocks[0].OutputBlocks));

            Assert.IsTrue(basicBlocks.Blocks[1].Commands.Select(x => x.ToString()).SequenceEqual(BBL.Blocks[1].Commands.Select(x => x.ToString())));
            Assert.IsTrue(basicBlocks.Blocks[1].InputBlocks.Select(x => x - start).SequenceEqual(BBL.Blocks[1].InputBlocks));
            Assert.IsTrue(basicBlocks.Blocks[1].OutputBlocks.Select(x => x - start).SequenceEqual(BBL.Blocks[1].OutputBlocks));

            Assert.IsTrue(basicBlocks.Blocks[2].Commands.Select(x => x.ToString()).SequenceEqual(BBL.Blocks[2].Commands.Select(x => x.ToString())));
            Assert.IsTrue(basicBlocks.Blocks[2].InputBlocks.Select(x => x - start).SequenceEqual(BBL.Blocks[2].InputBlocks));
            Assert.IsTrue(basicBlocks.Blocks[2].OutputBlocks.Select(x => x - start).SequenceEqual(BBL.Blocks[2].OutputBlocks));

            Assert.IsTrue(basicBlocks.Blocks[3].Commands.Select(x => x.ToString()).SequenceEqual(BBL.Blocks[3].Commands.Select(x => x.ToString())));
            Assert.IsTrue(basicBlocks.Blocks[3].InputBlocks.Select(x => x - start).SequenceEqual(BBL.Blocks[3].InputBlocks));
            Assert.IsTrue(basicBlocks.Blocks[3].OutputBlocks.Select(x => x - start).SequenceEqual(BBL.Blocks[3].OutputBlocks));
            
        }
    }
}
