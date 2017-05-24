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

            regionsInputs[regions.Count - 1] = param.FirstValue;
            
            Dictionary<Region, Region> parents = new Dictionary<Region, Region>();
            
            for (int i = regions.Count - 1; i >= 0; --i)
            {
                BodyRegion body = regions[i] as BodyRegion;
                if(body != null)
                    foreach (Region r in body.Regions)
                        parents[r] = body;
                
                LoopRegion loop = regions[i] as LoopRegion;
                if (loop != null)
                    parents[loop.Body] = loop;
                if (parents.ContainsKey(regions[i]))
                {
                    Region parent = parents[regions[i]];
                    regionsInputs[i] = functions[parent, RegionDirection.In, regions[i]](regionsInputs[regions.IndexOf(parent)]);
                }
            }

            int numOfBlocks = graph.Count();
            
            for(int i = 0; i < numOfBlocks; ++i)
            {
                var curBlock = regions[i].Header;
                int curBlockId =  curBlock.BlockId;
                
                result.In[curBlockId] = regionsInputs[i];
                result.Out[curBlockId] = param.TransferFunction(regionsInputs[i], curBlock);
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

