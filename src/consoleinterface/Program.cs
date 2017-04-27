using System;
using System.Collections.Generic;
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
using DataFlowAnalysis.Dominators;

namespace ConsoleInterface
{
    class Program
    {
        static void Main(string[] args)
        {
            string FileName = @"..\..\..\a.txt";
            try
            {
                string text = File.ReadAllText(FileName);
                SyntaxNode root = ParserWrap.Parse(text);
                Console.WriteLine(root == null ? "Ошибка" : "Программа распознана");
                if (root != null)
                {
                    //var result = PrettyPrinter.CreateAndVisit(root).FormattedCode;
                    var result = ThreeAddressCodeGenerator.CreateAndVisit(root).Program;
                    Console.WriteLine(result);

                    Console.WriteLine("\nУправляющий граф программы");
                    Graph g = new Graph(BasicBlocksGenerator.CreateBasicBlocks(result));

                    Console.WriteLine(g);

                    Console.WriteLine("Дерево доминаторов");
                    var dTree = new DominatorsTree(g);
                    Console.WriteLine(dTree);
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
            }
            Console.ReadLine();
        }
    }
}
