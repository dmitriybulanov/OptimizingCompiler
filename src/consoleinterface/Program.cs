using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GPPGParser;
using SyntaxTree.SyntaxNodes;
using SyntaxTree.Visitors;
using DataFlowAnalysis.RegionsAlgorithm;
using DataFlowAnalysis.SpecificIterativeAlgorithmParametrs.ReachingDefinitions.ExplicitTransferFunction;
using DataFlowAnalysis.IntermediateRepresentation.ControlFlowGraph;
using DataFlowAnalysis.IntermediateRepresentation.BasicBlockCode;
using DataFlowAnalysis.IntermediateRepresentation.ThreeAddressCode;

namespace ConsoleInterface
{
    class Program
    {
        static void Main(string[] args)
        {
            /*string FileName = @"../../../a.txt";
            try
            {
                string text = File.ReadAllText(FileName);
                SyntaxNode root = ParserWrap.Parse(text);
                Console.WriteLine(root == null ? "Ошибка" : "Программа распознана");
                if (root != null)
                {
                    var prettyPrintedProgram = PrettyPrinter.CreateAndVisit(root).FormattedCode;
                    Console.WriteLine(prettyPrintedProgram);
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Файл {0} не найден", FileName);
            }
            catch (LexException e)
            {
                Console.WriteLine("Лексическая ошибка. " + e.Message);
            }
            catch (SyntaxException e)
            {
                Console.WriteLine("Синтаксическая ошибка. " + e.Message);
            }*/
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
            Console.WriteLine(threeAddressCode);

            var basicBlocks = BasicBlocksGenerator.CreateBasicBlocks(threeAddressCode);
            Console.WriteLine(Environment.NewLine + "Базовые блоки");
            Console.WriteLine(basicBlocks);

            Console.WriteLine(Environment.NewLine + "Управляющий граф программы");
            Graph g = new Graph(basicBlocks);
            Console.WriteLine(g);

            Console.WriteLine(Environment.NewLine + "Достигающие определения");

            var reachingDefs = RegionsAlgorithm.Apply(g, new ExplicitTransferFunction(g));
            var outDefs = reachingDefs.Out.Select(
                pair => $"{pair.Key}: {string.Join(", ", pair.Value.Select(ex => ex.ToString()))}");

            foreach (var outInfo in outDefs)
            {
                Console.WriteLine(outInfo);
            }

            Console.ReadKey();
        }
    }
}
