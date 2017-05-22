# Поиск естественных циклов

### Выполнено командой: 
*Google Dogs (Александр Василенко, Кирилл Куц)*

### От каких проектов зависит:
1. Построение CFG
2. Алгоритм классификации рёбер CFG

### Зависимые проекты:
1. Формирование последовательности областей в восходящем порядке

### Постановка задачи: 
В данной задаче требуется найти на графе потока управления (CFG) *естественные циклы*. Эти циклы обладают двумя важными свойствами:
  1. Цикл имеет единственный входной узел - базовый блок, который называется *заголовком*. Данный узел доминирует над всеми узлами этого цикла, в противном случае узел не является единственной точкой входа в цикл.
  2. Обязано существовать обратное ребро, которое ведет в *заголовок*. Иначе данная структура не может считаться циклом.

Пример:

![](img/ExampleNL.png?raw=true)

Узлы 1, 2, 4 образуют естественный цикл.

### Теория
Пусть существует обратное ребро *n* <html>&rarr;</html> *d*. Для этого ребра естественный цикл определяется как множество узлов, в которое входят:
  1. Сам узел *d*
  2. Все узлы, которые могут достичь *n*, не проходя через *d*. В данном случае узел *d* является заголовком цикла.

На первом шаге алгоритма необходимо найти все обратные рёбра, далее для каждого ребра выполнить следующий алгоритм:

#### Алгоритм построения естественного цикла обратной дуги:

**Вход:** граф потока *G* и обратная дуга *n* <html>&rarr;</html> *d*.

**Выход:** множество *loop*, состоящее из всех узлов естественного цикла *n* <html>&rarr;</html> *d*.

**Метод:** вначале *loop* инициализируется множеством { *n*, *d* }. Узел *d* помечается как "посещеннный", чтобы поиск не проходил дальше *d*. Выполним поиск в глубину на обратном графе потока, начиная с *n*. Внесем все посещенные при этом поиске узлы в *loop*. Такая процедура позволяет найти все узлы, достигающие *n*, минуя *d*.

### Входные данные
- Граф потока управления

### Выходные данные
- Пары "обратное ребро - соответствующее этому ребру множество базовых блоков, входящих в естественный цикл"

### Используемые структуры данных
- `ISet<int> Loop` - множество номеров базовых блоков, входящих в цикл
- `Stack<int> Stack` - стек
- `Dictionary<Edge<BasicBlock>, EdgeType> classify` - ассоциативный массив, хранящий классификацию ребер
- `Dictionary<Edge<BasicBlock>, ISet<int>> result` - ассоциативный массив, хранящий результат

### Реализация алгоритма

```cs
// Вспомогательный алгоритм
private static void Insert(int m)
{
    if (!Loop.Contains(m))
    {
        Loop.Add(m); // Добавление узла в цикл
        Stack.Push(m); // Добавление узла в стек
    }
}

// Основной алгоритм
public static Dictionary<Edge<BasicBlock>, ISet<int>> FindAllNaturalLoops(ControlFlowGraph.Graph g)
{
    var classify = EdgeClassification.EdgeClassification.ClassifyEdge(g); // Выполнение классификации рёбер 
    var result = new Dictionary<Edge<BasicBlock>, ISet<int>>(); // Инициализация рёбер
    foreach (var pair in classify)
    {
       if (pair.Value == EdgeClassification.Model.EdgeType.Retreating) // Алгоритм выполняется только для отступающих рёбер (в данном случае, и обратных)
       {
          Stack = new Stack<int>(); // Инициализация стека
          Loop = SetFactory.GetSet(new int[] { pair.Key.Target.BlockId }); 
          Insert(pair.Key.Source.BlockId); // Добавление в цикл и стек узлов, входящих в обратное ребро 
          while (Stack.Count() > 0)
          {
             int m = Stack.Pop();
             foreach (BasicBlock p in g.getParents(m)) // Добавление в цикл и стек всех непосредственных предков узла
                Insert(p.BlockId);
          }
          result.Add(pair.Key, Loop);
       }
     }
     return result;
}
```

### Пример использования

```cs
var allNaturalLoops = SearchNaturalLoops.FindAllNaturalLoops(g);
foreach (var loop in allNaturalLoops)
{
    // В консоль выводятся вход и выход обратного ребра, а также номера блоков, входящих в естественный цикл
    Console.Write(loop.Key.Source.BlockId + " -> " + loop.Key.Target.BlockId + " : ");
    foreach (int node in loop.Value)
        Console.Write(node.ToString() + " ");
    Console.WriteLine("");
}
```

### Тест

*Программа*:
```
i = 10;
1: i = i - 1;
if i > 0
  goto 1;
```

*Граф потока управления*:

![](img/TestNL.png?raw=true)

Естественный цикл в данном случае один, в него входят узлы 1 и 2.

*Вывод программы*:

Формат вывода: "выход обратного ребра - вход обратного ребра - список узлов из цикла"

2 -> 1 : 1 2