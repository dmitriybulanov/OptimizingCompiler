# Установить, все ли отступающие ребра являются обратными

### Выполнено командой:
*Ampersand (Золотарёв Федор, Маросеев Олег)*

### От каких проектов зависит:
1. 

### Зависимые проекты:


### Постановка задачи:
В данной задаче требуется разработать класс, который реализует функцию проверки 

### Теория

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

```
