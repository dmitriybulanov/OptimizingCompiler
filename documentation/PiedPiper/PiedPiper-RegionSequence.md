# Формирование восходящей последовательности областей

### Выполнено командой
*PiedPiper (Бергер Анна, Колесников Сергей)*

### От каких проектов зависит
1. Граф потока управления
2. Задача определения приводимости графа
3. Поиск естественных циклов
4. Разработка класса "область" 

### Зависимые проекты
1. Восходящая часть алгоритма анализа на основе областей

### Теория
В процессе анализа на основе областей программа рассматривается как иерархия областей(регионов), которые грубо говоря, можно считать 
частями графа потока, имеющими единственную точку входа.
Формально область графа представляет собой набор узолв *N* и ребер *E* таких, что

- существует заголовок *h <html>&#8712;<html> N*, доминирующий над всеми узлами в *N*
- Если некоторый узел *m* может достичь узла *n <html>&#8712;<html> N*, минуя *h*, то *m* также входит в *N*
- *E* - множество всех ребер потока управления между узлами *n1* и *n2* из *N*, за исключением, возможно, некоторых ребер, входящих в *h*

В процессе построения иерархии областей идентифицируем естественные циклы.
Процесс начинается с того, что каждый базовый блок рассматривается как область (LeafRegion).
Затем естественные циклы упорядочиваются изнутри наружу, т.е. начиная с наиболее внутренних цикла. 

Для каждого цикла выделяются две области

 - R -- область тела (BodyRegion). Тело цикла *L* (все узлы и ребра, за исключением обратных ребер к заголовку) замещаем узлом, представляющим область *R*. Ребра, ведущие к заголовку *L*, входят теперь в узел
R. Ребро из любого выхода из *L* замещается ребром от *R* в то же самое место назначения. Однако если ребро является обратным, то оно становится петлей у *R*.

 - LR -- область цикла (LoopRegion).
Единственное отличие области тела от области цикла заключается в том, что последняя включает обратные ребра к заголовку цикла L. 

### Входные данные
Граф потока управления ```Graph g```

### Выходные данные
Список областей в восходящем порядке (```List < Region > regionList```)

### Используемые структуры данных
 - ``` List<Region> regionList ``` -- список областей в восходящем порядке
 - ```Dictionary<BasicBlock, Region> basicBlockLastRegion``` -- словарь, хранящий соответствие базовый блок - самый внешний регион, в который он вошел
 - ```Dictionary<Edge<BasicBlock>, bool> regionMade```-- словарь, хранящий соответствие обратная дуга - верно ли, что цикл по ней уже сформирован

### Реализация алгоритма

Проверка циклов на вложенность осуществляется с помощью вспомогательного метода
```
private bool checkLoopInclusion(KeyValuePair<Edge<BasicBlock>, ISet<int>> curLoop,
                  KeyValuePair<Edge<BasicBlock>, ISet<int>> loopOther, bool regionMade)
{
  if (loopOther.Value.IsSubsetOf(curLoop.Value) && !regionMade)
    return true;
  else
    return false;
}
```

Основная работа по построению восходящей последовательности областей проходит в методе
```public List<Region> CreateSequence(Graph g)```

``` 
public List<Region> CreateSequence(Graph g)
{
```
Проверка приводимости графа
```
    if (!CheckRetreatingIsReverse.CheckRetreatingIsReverse.CheckReverseEdges(g)){
        Console.WriteLine("there are some retreating edges which aren't reverse");
        Environment.Exit(0);
    }
```
Добавление базовых блоков как LeafRegion в список областей
```
    var basicBlockLastRegion = new Dictionary<BasicBlock, Region>();

    foreach (var v in g.GetVertices())
    {
        var newReg = new LeafRegion(v);
        regionList.Add(newReg);
        basicBlockLastRegion[v] = newReg;
    }

    var loops = SearchNaturalLoops.FindAllNaturalLoops(g);

    var regionMade = new Dictionary<Edge<BasicBlock>, bool>();

    foreach (var loop in loops)
    {
        regionMade[loop.Key] = false;
    }
```
Пока остались незанесенные в список областей циклы, метод продолжает работу
```
    while (regionMade.ContainsValue(false))
    {
        foreach (var loop in loops)
        {
```
Проверка цикла на существование вложенных в него и еще не обработанных циклов
```
            bool anyInsideLoops = false;
            foreach (var loopOther in loops)
            {
                anyInsideLoops = anyInsideLoops || checkLoopInclusion(loop, loopOther, regionMade[loopOther.Key]);
            }
            if (!anyInsideLoops) continue;
```
Если все вложенные циклы уже обработаны или их нет, приступаем к формированию новой области
Для формирования BodyRegion требуется 
- BasicBlock header - им является Target обратной дуги, формирующей цикл
- List<Region> curRegions - формируется с помощью словаря basicBlockLastRegion
- List<int> outputBlocks (блоки, из которых есть дуги в другие регионы - формируется с помощью массива OutputBlocks каждого блока
```
            regionMade[loop.Key] = true;

            var header = loop.Key.Target;

            var curRegions = new List<Region>();
            var outputBlocks = new List<int>();
            foreach (var blockId in loop.Value)
            {
                var block = g.getBlockById(blockId);
                if (!curRegions.Contains(basicBlockLastRegion[block]))
                    curRegions.Add(basicBlockLastRegion[block]);

                foreach (var outputBlock in block.OutputBlocks)
                {
                    if (!loop.Value.Contains(outputBlock))
                    {
                        outputBlocks.Add(outputBlock);
                        break;
                    }
                }
            }

            var bodyReg = new BodyRegion(header, outputBlocks, curRegions);
            regionList.Add(bodyReg);

            var loopReg = new LoopRegion(bodyReg);
            regionList.Add(loopReg);

            foreach (var blockId in loop.Value)
            {
                var block = g.getBlockById(blockId);
                basicBlockLastRegion[block] = loopReg;
            }
        }
    }
```
Если программа не является циклом, то формируется последний регион, включающий в себя всю программу
```
    foreach (var block in basicBlockLastRegion)
    {
        if (block.Value.GetType() == typeof(LeafRegion))
        {
            var header = g.getRoot();
            var outputBlocks = new List<int>();
            var curRegions = new List<Region>();
            foreach (var curblock in basicBlockLastRegion)
            {
                if (!curRegions.Contains(curblock.Value))
                    curRegions.Add(curblock.Value);
            }
            var newReg = new BodyRegion(header, outputBlocks, curRegions);
            regionList.Add(newReg);
            break;
        }
    }
 ```
