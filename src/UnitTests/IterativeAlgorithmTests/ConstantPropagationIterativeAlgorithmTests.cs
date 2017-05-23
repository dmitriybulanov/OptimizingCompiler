using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode;
using DataFlowAnalysis.IntermediateRepresentation.ControlFlowGraph;
using DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode;
using DataFlowAnalysis.IterativeAlgorithm;
using DataFlowAnalysis.SpecificIterativeAlgorithmParametrs.ConstantsPropagation;
using GPPGParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SyntaxTree.SyntaxNodes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.IterativeAlgorithmTests
{
    [TestClass]
    public class ConstantPropagationIterativeAlgorithmTests
    {
        [TestMethod]
        public void ConstantsPropagation1()
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



            Trace.WriteLine(Environment.NewLine + "Распространение констант");
            var consts = IterativeAlgorithm.Apply(g, new ConstantsPropagationParameters());
            var outConsts = consts.Out.Select(
                pair => $"{pair.Key}: {string.Join(", ", pair.Value.Select(ex => ex.ToString()))}");

            foreach (var outInfo in outConsts)
            {
                Trace.WriteLine(outInfo);
            }

            int startIndex = consts.Out.Keys.Min();
            Assert.IsTrue(consts.Out[startIndex]["a"]=="4" &&
                consts.Out[startIndex]["b"] == "4" &&
                consts.Out[startIndex]["t0"] == "8" &&
                consts.Out[startIndex]["c"] == "8");

            Assert.IsTrue(consts.Out[startIndex + 1]["a"] == "3" &&
                consts.Out[startIndex + 1]["b"] == "4" &&
                consts.Out[startIndex + 1]["t0"] == "8" &&
                consts.Out[startIndex + 1]["c"] == "8");

            Assert.IsTrue(consts.Out[startIndex + 2]["a"] == "4" &&
                consts.Out[startIndex + 2]["b"] == "2" &&
                consts.Out[startIndex + 2]["t0"] == "8" &&
                consts.Out[startIndex + 2]["c"] == "8");

            Assert.IsTrue(consts.Out[startIndex + 3]["a"] == "NAC" &&
                consts.Out[startIndex + 3]["b"] == "NAC" &&
                consts.Out[startIndex + 3]["t0"] == "8" &&
                consts.Out[startIndex + 3]["c"] == "8");
        }

        [TestMethod]
        public void ConstantsPropagation2()
        {
            string programText = @"
    e=10;
    c=4;
    d=2;
    a=4;
    if 0
        goto 2;
    a=c+d;
    e=a;
    goto 3;
2:  a=e;
3:  t=0;
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
            
            Trace.WriteLine(Environment.NewLine + "Распространение констант");
            var consts = IterativeAlgorithm.Apply(g, new ConstantsPropagationParameters());
            var outConsts = consts.Out.Select(
                pair => $"{pair.Key}: {string.Join(", ", pair.Value.Select(ex => ex.ToString()))}");

            foreach (var outInfo in outConsts)
            {
                Trace.WriteLine(outInfo);
            }

            int startIndex = consts.Out.Keys.Min();
            Assert.IsTrue(consts.Out[startIndex]["a"] == "4" &&
                consts.Out[startIndex]["e"] == "10" &&
                consts.Out[startIndex]["d"] == "2" &&
                consts.Out[startIndex]["c"] == "4");

            Assert.IsTrue(consts.Out[startIndex + 1]["a"] == "4" &&
                consts.Out[startIndex + 1]["e"] == "10" &&
                consts.Out[startIndex + 1]["d"] == "2" &&
                consts.Out[startIndex + 1]["c"] == "4");

            Assert.IsTrue(consts.Out[startIndex + 2]["e"] == "6" &&
                consts.Out[startIndex + 2]["c"] == "4" &&
                consts.Out[startIndex + 2]["d"] == "2" &&
                consts.Out[startIndex + 2]["a"] == "6" &&
                consts.Out[startIndex + 2]["t0"] == "6");

            Assert.IsTrue(consts.Out[startIndex + 3]["a"] == "10" &&
                consts.Out[startIndex + 3]["e"] == "10" &&
                consts.Out[startIndex + 3]["d"] == "2" &&
                consts.Out[startIndex + 3]["c"] == "4");

            Assert.IsTrue(consts.Out[startIndex + 4]["e"] == "NAC" &&
                consts.Out[startIndex + 4]["c"] == "4" &&
                consts.Out[startIndex + 4]["d"] == "2" &&
                consts.Out[startIndex + 4]["t"] == "0" &&
                consts.Out[startIndex + 4]["t0"] == "6");
        }
    }
}
