# Построение дерева доминаторов

### Выполнено командой:
*NoName (Скиба Кирилл, Борисов Сергей)*

### От каких проектов зависит:
1. Итерационный алгоритм
2. Граф управления потока

### Зависимые проекты:
1. Алгоритм классификации ребер графа

### Постановка задачи:
Необходимо создать класс для дерева доминаторов и построить это дерево, используя итеративный алгоритм.

### Теория
Узел *d* графа потока доминирует над узлом *n*, если любой путь от входного узла графа потока к *n* проходит через *d*.
При таком определении каждый узел доминирует над самим собой.

Дерево доминаторов - это дерево, в котором входной узел является корнем, а каждый узел *d* доминирует только над своими потомками в дереве.

Существование деревьев доминаторов следует из свойства доминаторов: каждый узел *n* имеет единственный непосредственный доминатор *m*, который является
последним доминатором *n* на любом пути от входного узла до *n*.

### Входные данные
- Граф потока управления

### Выходные данные
- Дерево доминаторов


### Используемые структуры данных
- `AdjacencyGraph<int, Edge<int>> Tree` - дерево доминаторов (структура данных из пакета NuGet QuickGraph)
- `Dictionary<int, int> Map` - словарь: номер базового блока - его непосредственный доминатор

### Реализация

В реализации данного модуля был использован пакет NuGet QuickGraph.

```cs
// класс "дерево доминаторов"
public class DominatorsTree : IEnumerable
{
    private AdjacencyGraph<int, Edge<int>> Tree = new AdjacencyGraph<int, Edge<int>>();
    private Dictionary<int, int> Map;

    public DominatorsTree(Graph g)
    {
        Map = ImmediateDominator.FindImmediateDominator(g);

        Tree.AddVertexRange(Map.Keys);

        foreach (int key in Map.Keys.Skip(1))
            Tree.AddEdge(new Edge<int>(Map[key], key));
    }

    public int GetParent(int id)
    {
        return Map[id];
    }

    public List<int> GetAncestors(int id)
    {
        return Map.Where(x => x.Value == id).Select(x => x.Key).ToList();
    }

    public override string ToString()
    {
        string res = "";
        foreach (var v in Tree.Vertices)
            if (Tree.OutEdges(v).Count() > 0)
                foreach (var e in Tree.OutEdges(v))
                    res += v + " --> " + e.Target + "\n";
        return res;
    }

    public IEnumerator GetEnumerator()
    {
        return Map.Values.GetEnumerator();
    }

    public Dictionary<int, int> GetMap()
    {
        return Map;
    }
}

// Класс "непосредственный доминатор"
public static class ImmediateDominator
{
    public static Dictionary<int, int> FindImmediateDominator(Graph g)
    {
        var _out = IterativeAlgorithm.IterativeAlgorithm.Apply(g, new DominatorsIterativeAlgorithmParametrs(g)).Out;

        int min = _out.Keys.Min();

        return _out.Select(x => new KeyValuePair<int, int>(x.Key,
                                        x.Key > min ? _out[x.Key].Take(_out[x.Key].Count - 1).Last() : min))
                                      .ToDictionary(x => x.Key, x => x.Value);
    }
}

// наследник общего класса "параметры итерационного алгоритма"
public class DominatorsIterativeAlgorithmParametrs : BasicIterativeAlgorithmParameters<ISet<int>>
{
    private Graph graph;

    public DominatorsIterativeAlgorithmParametrs(Graph g)
    {
        graph = g;
    }

    public override ISet<int> GatherOperation(IEnumerable<ISet<int>> blocks)
    {
        ISet<int> intersection = SetFactory.GetSet((IEnumerable<int>)blocks.First());
        foreach (var block in blocks.Skip(1))
            intersection.IntersectWith(block);

        return intersection;
    }

    public override ISet<int> TransferFunction(ISet<int> input, BasicBlock block)
    {
        return SetFactory.GetSet<int>(input.Union(new int[] { block.BlockId }));
    }

    public override bool AreEqual(ISet<int> t1, ISet<int> t2)
    {
        return t1.IsSubsetOf(t2) && t2.IsSubsetOf(t1);
    }

    public override ISet<int> StartingValue { get { return SetFactory.GetSet<int>(Enumerable.Range(graph.GetMinBlockId(), graph.Count())); } }

    public override ISet<int> FirstValue { get { return SetFactory.GetSet<int>(Enumerable.Repeat(graph.GetMinBlockId(), 1)); } }

    public override bool ForwardDirection { get { return true; } }

}

// Итеративный алгоритм, использующий для построения дерева доминаторов
public static IterativeAlgorithmOutput<V> Apply<V>(Graph graph, BasicIterativeAlgorithmParameters<V> param, int[] order = null)
{
    IterativeAlgorithmOutput<V> result = new IterativeAlgorithmOutput<V>();

    foreach (BasicBlock bb in graph)
        result.Out[bb.BlockId] = param.StartingValue;
    IEnumerable<BasicBlock> g = order == null ? graph : order.Select(i => graph.getBlockById(i));
    bool changed = true;
    while (changed)
    {
        changed = false;
        foreach (BasicBlock bb in g)
        {
            BasicBlocksList parents = param.ForwardDirection ? graph.getParents(bb.BlockId) : graph.getChildren(bb.BlockId);
            if (parents.Blocks.Count > 0)
                result.In[bb.BlockId] = param.GatherOperation(parents.Blocks.Select(b => result.Out[b.BlockId]));
            else
                result.In[bb.BlockId] = param.FirstValue;
            V newOut = param.TransferFunction(result.In[bb.BlockId], bb);
            changed = changed || !param.AreEqual(result.Out[bb.BlockId], newOut);
            result.Out[bb.BlockId] = param.TransferFunction(result.In[bb.BlockId], bb);
        }
    }
    if (!param.ForwardDirection)
        result = new IterativeAlgorithmOutput<V> { In = result.Out, Out = result.In };
    return result;
}
```

### Пример использования
```cs
DominatorsTree tree = new DominatorsTree(g);
foreach (Edge<BasicBlock> e in g.GetEdges())
{
    if (dfn[e.Source.BlockId] >= dfn[e.Target.BlockId])
        edgeTypes.Add(e, EdgeType.Retreating);
    else if (g.IsAncestor(e.Target.BlockId, e.Source.BlockId) && tree.GetParent(e.Target.BlockId) == e.Source.BlockId)
        edgeTypes.Add(e, EdgeType.Advancing);
    else
        edgeTypes.Add(e, EdgeType.Cross);
}
```

### Тест
```cs
*Программа*:
b = 1;
if 1 
  b = 3;
else
  b = 2;
```

*Граф потока управления*:

![](../documentation/GoogleDogs/img/TestEC.png?raw=true)

*Вывод программы*:

Формат вывода: "номер базового блока - его непосредственный доминатор"

0 -> 0                                                        
1 -> 0                                                          
2 -> 0                                                                
3 -> 0
