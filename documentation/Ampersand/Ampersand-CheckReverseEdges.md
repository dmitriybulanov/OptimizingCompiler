# Установить, все ли отступающие ребра являются обратными

### Выполнено командой:
*Ampersand (Золотарёв Федор, Маросеев Олег)*

### От каких проектов зависит:
1. Алгоритм классификации ребер графа
2. Алгоритм нахождения обратных ребер графа

### Зависимые проекты:
-

### Постановка задачи:
В данной задаче требуется разработать класс, который реализует функцию проверки того факта, что все отступающие ребра графа являются обратными.

### Теория
Отступающее ребро - ребро от потомков к предку остовного графа.

Ребро от A к B называется обратным, если B доминирует над A.

### Используемые структуры данных
- ` Dictionary<Edge<BasicBlock>, EdgeType>` - словарь классифицированных ребер

### Реализация

```cs
namespace DataFlowAnalysis.IntermediateRepresentation.CheckRetreatingIsReverse
{
    class CheckRetreatingIsReverse
    {
        public static bool CheckReverseEdges(ControlFlowGraph.Graph g)
        {
            Dictionary<Edge<BasicBlock>, EdgeType> ClassifiedEdges = EdgeClassification.EdgeClassification.ClassifyEdge(g);
            var RetreatingEdges = ClassifiedEdges.Where(x => x.Value == EdgeType.Retreating).Select(x => x.Key);

            var ReverseEdges = FindReverseEdge.FindReverseEdges(g);

            return ReverseEdges.IsSubsetOf(RetreatingEdges);
        }
    }
}
```

### Пример использования

```cs
[TestClass]
    public class RetreatingNotReverseEdgesTests
    {
        [TestMethod]
        public void RetreatingNotReverseEdges1()
        {
            var programText = File.ReadAllText("./../../RetreatingNotReverseEdgesEx1.txt");


            SyntaxNode root = ParserWrap.Parse(programText);
            Graph graph = new Graph(
                BasicBlocksGenerator.CreateBasicBlocks(
                    ThreeAddressCodeGenerator.CreateAndVisit(root).Program));

            Assert.IsFalse(CheckRetreatingIsReverse.CheckReverseEdges(graph));
        }

        [TestMethod]
        public void RetreatingNotReverseEdges2()
        {
            var programText = File.ReadAllText("./../../RetreatingNotReverseEdgesEx2.txt");

            SyntaxNode root = ParserWrap.Parse(programText);
            Graph graph = new Graph(
                BasicBlocksGenerator.CreateBasicBlocks(
                    ThreeAddressCodeGenerator.CreateAndVisit(root).Program));

            Assert.IsFalse(CheckRetreatingIsReverse.CheckReverseEdges(graph));
        }

        [TestMethod]
        public void RetreatingNotReverseEdges3()
        {
            var programText = File.ReadAllText("./../../RetreatingNotReverseEdgesEx3.txt");

            SyntaxNode root = ParserWrap.Parse(programText);
            Graph graph = new Graph(
                BasicBlocksGenerator.CreateBasicBlocks(
                    ThreeAddressCodeGenerator.CreateAndVisit(root).Program));

            Assert.IsFalse(CheckRetreatingIsReverse.CheckReverseEdges(graph));
        }
    }
```
