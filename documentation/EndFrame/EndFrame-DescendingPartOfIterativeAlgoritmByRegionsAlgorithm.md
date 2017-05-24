# Нисходящая часть алгоритма анализа на основе областей.

### Выполнено командой:
*EndFrame (Аммаев Саид,  Пирумян Маргарита)*

### Постановка задачи:
Необходимо реализовать нисходящую часть алгоритма анализа на основе областей в классе RegionsAlgorithm.

### От каких проектов зависит:
1. Region
2. Graph
3. IterativeAlgorithm
4. IterativeAlgorithmParameters

### Зависимые проекты:
1. IterativeAlgorithm

### Теория
```
Нисходящая часть алгоритма:
IN[Rn] = IN[Входа]
для каждого региона Ri в нисходящем порядке
  {
    IN[Ri] = fR(i-1), IN[Ri](IN[R(i-1)])
    OUT[Ri] = transferfunction(IN[Ri])
  }
```

### Входные данные:
 - List<Region> regions - список регионов
 - TransferFunctionStorage<ISet<V>> functions - передаточные функции
 - SetIterativeAlgorithmParameters<V> param - параметры итерационного алгоритма
 - Graph graph - граф программы
 
### Выходные данные:
 - IterativeAlgorithmOutput<ISet<V>> - множества входов и выходов для каждого блока

 ### Реализация алгоритма:

 ``` C#
 /* реализация нисходящей части алгоритма анализа на основе областей. в классе RegionsAlgorithm */
 
 static IterativeAlgorithmOutput<ISet<V>> ApplyDescendingPart<V>(List<Region> regions,
            TransferFunctionStorage<ISet<V>> functions, SetIterativeAlgorithmParameters<V> param, Graph graph)
        {
            Dictionary<int, ISet<V>> regionsInputs = new Dictionary<int, ISet<V>>();
            IterativeAlgorithmOutput<ISet<V>> result = new IterativeAlgorithmOutput<ISet<V>>();

            Dictionary<Region, int> RegionIndexes = new Dictionary<Region, int>();
            for (int i = 0; i < regions.Count; ++i)
            {
                RegionIndexes[regions[i]] = i;
            }

            int lastIndex = RegionIndexes[regions.Last()];
            int prevIndex = lastIndex;
            regionsInputs[lastIndex] = param.FirstValue;

            foreach (var r in regions.Reverse<Region>())
            {
                int curIndex = RegionIndexes[r];
                if (curIndex != lastIndex)
                {
                    regionsInputs[curIndex] = functions[regions[prevIndex], RegionDirection.In, regions[curIndex]](regionsInputs[prevIndex]);
                }
            }

            int numOfBlocks = graph.Count();
            for(int i = 0; i < numOfBlocks; ++i)
            {
                int curBlockId =  regions[i].OutputBlocks.First();
                var curBlock = graph.getBlockById(curBlockId);

                result.In[curBlockId] = regionsInputs[i];
                result.Out[curBlockId] = param.TransferFunction(result.In[curBlockId], curBlock);
            }

            return result;
        }
        ...
 ``` 
 
### Пример использования

### Тест
Программа
```cs 
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
```

```

```

