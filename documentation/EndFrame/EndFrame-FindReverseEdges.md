# Название задачи: FindReverseEdges

### Выполнено командой: EndFrame (Аммаев Саид, Пирумян Маргарита)

### Постановка задачи: Необходимо реализовать функцию, которая находит в заданном графе потока управления обратные ребра.

### От каких проектов зависит:

  - ControlFlowGraph
  - EdgeClassification
  - ImmediateDominator

### Зависимые проекты:

  - CheckRetreatingIsReverse

### Теория

> Отступающее ребро в графе потока управления - ребро, идущее от потомка к предку.
Вершина графа d доминирует над a, если любой путь от начальной вершины графа до вершины а, проходит через d. Свойства доминирования: d доминирует над d; если d доминирует над b, а b доминирует над a, то d доминирует над a. Отступющее ребро, направленное из a в b, называется обратным, если b доминирует над a

### Входные данные:
 - Graph - граф потока управления

### Выходные данные:
 - ISet<Edge<BasicBlock>> - множество дуг базовых блоков

### Используемые структуры данных

 - Dictionary<Edge<BasicBlock>, EdgeType> ClassifiedEdges - соответствие к ребру его типа
 - Dictionary<int, int> Dominators - соответсвие к узлу графа его непосредственного доминатора
 - Dictionary<Edge<BasicBlock>, EdgeType> RetreatingEdges - отступающие ребра со своим типом

### Реализация алгоритма
```C#
        public static ISet<Edge<BasicBlock>> FindReverseEdges(ControlFlowGraph.Graph g)
        {
            ISet<Edge<BasicBlock>> res = SetFactory.GetEdgesSet();//new SortedSet<Edge <BasicBlock>>();
            Dictionary<Edge<BasicBlock>, EdgeType> ClassifiedEdges = EdgeClassification.EdgeClassification.ClassifyEdge(g);
            Dictionary<int, int> Dominators = ImmediateDominator.FindImmediateDominator(g);
            var RetreatingEdges = ClassifiedEdges.Where(x => x.Value == EdgeType.Retreating);
            foreach (var edg in RetreatingEdges)
            {
                var edge = edg.Key;               // текущее отступающее ребро
                int key = edge.Source.BlockId;    // текущий узел ставится на начало отступающего ребра
                int value = edge.Target.BlockId;  // конец отступающего ребра
                bool isReverse = false;           // является ли текущий узел концом отступающего ребра
                /* цикл, пока у текущего узла есть непосредственный доминатор, он не начало дерева доминаторов, 
                и текущий узел не попал на конец отступающего ребра */
                while (Dominators.ContainsKey(key) && Dominators[key] != key && !isReverse)
                {
                    key = Dominators[key];      // текущий узел перемещается на непосредственного доминатора
                    isReverse = (key == value); // конец отступающего доминирует над началом, то есть ребро - обратное
                }
                if (isReverse)        // если ребро обратное - добавляем
                    res.Add(edge); 
            }
            return res;
        }
```
### Пример использования

var ReverseEdges = FindReverseEdge.FindReverseEdges(g);

### Тест
```
 for i = 1 + 2 * 3 .. 10
   println(i);
```
```
Полученный CFG: 
0: 1
1: 2, 3
2: 1
3: 
```
```  
Обратное ребро: 2 --> 1
