# Восходящая часть алгоритма анализа на основе областей

### Выполнено командой:
*Ampersand (Золотарёв Федор, Маросеев Олег)*

### От каких проектов зависит:
1. Получение областей

### Зависимые проекты:
1. Нисходящая часть алгоритма

### Постановка задачи:
В данной задаче требуется написать восходящую часть алгоритма на основе областей

### Теория
Алгоритм на основе областей условно можно разделить на 2 части: восходящую и нисходящую. Восходящая часть проходит по всем областям в восходящем порядке, заполняя при этом передаточные функции для всех регионов.

### Реализация

```cs
static TransferFunctionStorage<ISet<V>> ApplyAscendingPart<V>(Graph graph, List<Region> regions, SetIterativeAlgorithmParameters<V> param)
        {
            TransferFunctionStorage<ISet<V>> result = new TransferFunctionStorage<ISet<V>>();
            foreach (Region r in regions)
            {
                LeafRegion leaf = r as LeafRegion;
                TransferFunctionStorage<ISet<V>> clone = result.Clone();
                if (leaf != null)
                {
                    //////
                    for(int i = graph.Count(); i < regions.Count; ++i)
                        result[regions[i], RegionDirection.Out, leaf] = input => param.TransferFunction(input, leaf.Block);
                    /////   
                    result[leaf, RegionDirection.In, leaf] = Identity;
                    result[leaf, RegionDirection.Out, leaf] = input => param.TransferFunction(input, leaf.Block);
                }
                BodyRegion body = r as BodyRegion;
                if (body != null)
                {
                    foreach (Region s in body.Regions)
                    {
                        LeafRegion header = s as LeafRegion;
                        if (header != null)
                        {
                            result[body, RegionDirection.In, s] = Identity;
                        }
                        else
                        {
                            result[body, RegionDirection.In, s] = input => GatherFunctionsResults(input, clone, body, s.Header.InputBlocks, graph, param);
                        }
                        CalculateForOutputBlocks(result, body, s, s.OutputBlocks, graph);
                        
                    }
                }
                LoopRegion loop = r as LoopRegion;
                if(loop != null)
                {
                    result[loop, RegionDirection.In, loop.Body] = input => SetFactory.GetSet<V>(input.Union(GatherFunctionsResults(input, clone, loop.Body, loop.Header.InputBlocks, graph, param)));
                    CalculateForOutputBlocks(result, loop, loop.Body, loop.OutputBlocks, graph);
                }
            }
            return result;
        }

        static ISet<V> GatherFunctionsResults<V>(ISet<V> input, TransferFunctionStorage<ISet<V>> result, Region r, List<int> inputBlocks, Graph graph, SetIterativeAlgorithmParameters<V> param)
        {      
            return param.GatherOperation(inputBlocks.Select(i => result[r, RegionDirection.Out, new LeafRegion(graph.getBlockById(i))](input)));
        }

        static void CalculateForOutputBlocks<V>(TransferFunctionStorage<ISet<V>> result, Region r, Region s, List<int> outputBlocks, Graph graph)
        {
            foreach (BasicBlock bb in outputBlocks.Select(i => graph.getBlockById(i)))
            {
                LeafRegion b = new LeafRegion(bb);
                TransferFunctionStorage<ISet<V>> clone = result.Clone();
                result[r, RegionDirection.Out, b] = input => clone[s, RegionDirection.Out, b](clone[r, RegionDirection.In, s](input));
            }
        }
```

### Пример использования

```cs
public static IterativeAlgorithmOutput<ISet<V>> Apply<V>(Graph graph, SetIterativeAlgorithmParameters<V> param)
        {
            List<Region> regions = new RegionSequence().CreateSequence(graph);

            return ApplyDescendingPart<V>(regions, ApplyAscendingPart<V>(graph, regions, param), param, graph);
        }
```
