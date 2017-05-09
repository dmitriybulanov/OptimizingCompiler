using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GPPGParser;
using SyntaxTree.SyntaxNodes;
using SyntaxTree.Visitors;
using DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode;
using DataFlowAnalysis.IterativeAlgorithm;
using DataFlowAnalysis.IntermediateRepresentation.ControlFlowGraph;
using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode;
using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode.Model;
using DataFlowAnalysis.SpecificIterativeAlgorithmParametrs.Dominators;
using DataFlowAnalysis.IntermediateRepresentation.EdgeClassification;
using DataFlowAnalysis.IntermediateRepresentation.FindNaturalLoops;
using Microsoft.VisualStudio.TestTools.UnitTesting;



namespace UnitTests
{
    [TestClass]
    public class OtherTests
    {
        [TestMethod]
        public void ThisIsAwfulTestDontMakeItWorse()
        {
            string FileName = @"../../../a.txt";
            try
            {
                string text = File.ReadAllText(FileName);
                SyntaxNode root = ParserWrap.Parse(text);
                Trace.WriteLine(root == null ? "Ошибка" : "Программа распознана");
                if (root != null)
                {
                    var prettyPrintedProgram = PrettyPrinter.CreateAndVisit(root).FormattedCode;
                    Trace.WriteLine(prettyPrintedProgram);

                    var threeAddressCode = ThreeAddressCodeGenerator.CreateAndVisit(root).Program;
                    Trace.WriteLine(threeAddressCode);

                    Trace.WriteLine("\nУправляющий граф программы");
                    Graph g = new Graph(BasicBlocksGenerator.CreateBasicBlocks(threeAddressCode));

                    Trace.WriteLine(g);

                    Trace.WriteLine("Дерево доминаторов");
                    var dTree = new DominatorsTree(g);
                    Trace.WriteLine(dTree);

                    Trace.WriteLine("Классификация рёбер");
                    var classEdge = EdgeClassification.ClassifyEdge(g);
                    foreach (var p in classEdge)
                        Trace.WriteLine(p.Key.Source.BlockId + "->" + p.Key.Target.BlockId + "; type = " + p.Value);

                    Trace.WriteLine("\nНахождение естественных циклов");
                    var findNL = FindNaturalLoops.FindAllNaturalLoops(g);
                    foreach (var nl in findNL)
                    {
                        Trace.Write(nl.Key.Source.BlockId + "->" + nl.Key.Target.BlockId + ": ");
                        foreach (var node in nl.Value)
                            Trace.Write(node.ToString() + " ");
                        Trace.WriteLine("");
                    }

                    Trace.WriteLine(("\nПостроение глубинного остовного дерева"));
                    var DFN = g.GetDFN();
                    foreach (var node in DFN)
                    {
                        Trace.WriteLine("key: " + node.Key + " " + "value: " + node.Value);
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Trace.WriteLine("Файл {0} не найден", FileName);
            }
            catch (LexException e)
            {
                Trace.WriteLine("Лексическая ошибка. " + e.Message);
            }
            catch (SyntaxException e)
            {
                Trace.WriteLine("Синтаксическая ошибка. " + e.Message);
            }
        }
    }
}
