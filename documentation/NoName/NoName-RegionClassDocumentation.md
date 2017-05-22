# Разработка класса "область" и разновидности: класс "область тела" и класс "область цикла"

### Выполнено командой:
*NoName (Скиба Кирилл, Борисов Сергей)*

### От каких проектов зависит:
1. Базовые блоки и алгоритм получения

### Зависимые проекты:
1. Формирование последовательности областей в восходящем порядке
2. Разработка класса для хранения передаточных функций для алгоритма анализа на основе областей
3. Восходящая часть алгоритма анализа на основе областей
4. Нисходящая часть алгоритма анализа на основе областей

### Постановка задачи:
В данной задаче требуется разработать класс "область". Также необходимо разработать разновидности класса "область": класс "область тела" и класс "область цикла"

### Теория
В процессе анализа на основе областей программа рассматривается как иерархия  которые грубо можно считать частями графа потока, имеющими единственную точку входа.
Каждая инструкция в блочно структурированной программе является областью, поскольку поток управления может достичь инструкции только через ее начало. Каждый уровень вложенности
инструкций соответствует уровню в иерархии областей.

Формально область графа потока представляет собой набор узлов *N* и ребер *E*, таких, что
1. Существует заголовок *h <html>&#8712;<html> N*, доминирующий над всеми узлами в *N*;
2. Если некоторый узел *m* может достичь узла *n <html>&#8712;<html> N*, минуя *h*, то *m* также входит в *N*;
3. *E* - множество всех ребер потока управления между узлами *n1* и *n2* из *N*, за исключением, возможно, некоторых ребер, входящих в *h*.
	
*Тело* цикла *L* (все узлы и ребра, за исключением обратных ребер к заголовку) замещаем узлом, представляющим область *R*. Ребра, ведущие к заголовку *L*, входят теперь в узел
R. Ребро из любого выхода из *L* замещается ребром от *R* в то же самое место назначения. Однако если ребро является обратным, то оно становится петлей у *R*.
Назовем *R областью тела*.

Единственное отличие области тела от области цикла заключается в том, что последняя включает обратные ребра к заголовку цикла L. 

### Используемые структуры данных
- `List<int> OutputBlocks` - номера выходных базовых блоков

### Реализация

```cs
// Абстрактный класс "область"
public abstract class Region
{
    public abstract List<int> OutputBlocks { get; }
}

// Вспомогательный класс IntermediateRegion
public class IntermediateRegion : Region
{
    public BasicBlock Header { get; set; }

    List<int> outputBlocks;
    public override List<int> OutputBlocks { get { return outputBlocks; } }
    public IntermediateRegion(BasicBlock header, List<int> outputBlocks)
    {
        Header = header;
        this.outputBlocks = outputBlocks;
    }

    protected bool Equals(IntermediateRegion other)
    {
        return Equals(Header, other.Header) && OutputBlocks.SequenceEqual(other.OutputBlocks);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((IntermediateRegion) obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return ((Header != null ? Header.GetHashCode() : 0) * 397) ^ (OutputBlocks != null ? OutputBlocks.GetHashCode() : 0);
        }
    }
}

// Класс "область тела"
public class BodyRegion : IntermediateRegion
{
    public List<Region> Regions { get; set; }

    public BodyRegion(BasicBlock header, List<int> outputBlocks, List<Region> regions) : base(header, outputBlocks)
    {
        Regions = regions;
    }

    protected bool Equals(BodyRegion other)
    {
        return Regions.SequenceEqual(other.Regions);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((BodyRegion) obj);
    }

    public override int GetHashCode()
    {
        return (Regions != null ? Regions.GetHashCode() : 0);
    }
}

// Класс "область цикла"
public class LoopRegion : IntermediateRegion
{
    public BodyRegion Body { get; set; }

    public LoopRegion(BasicBlock header, List<int> outputBlocks, BodyRegion body) : base(header, outputBlocks)
    {
        Body = body;
    }

    public LoopRegion(BodyRegion body) : base(body.Header, body.OutputBlocks)
    {
        Body = body;
    }

    protected bool Equals(LoopRegion other)
    {
        return base.Equals(other) && Equals(Body, other.Body);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((LoopRegion) obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (base.GetHashCode() * 397) ^ (Body != null ? Body.GetHashCode() : 0);
        }
    }
}

// Класс "область листа". Каждый базовый блок рассматривается как область-лист
public class LeafRegion : Region
{
    public BasicBlock Block { get; set; }

    public override List<int> OutputBlocks
    {
        get
        {
            return Block.OutputBlocks.Count > 0 ? new List<int> { Block.BlockId } : new List<int>() ;
        }
    }
    
    public LeafRegion(BasicBlock block)
    {
        Block = block;
    }

    protected bool Equals(LeafRegion other)
    {
        return Equals(Block, other.Block);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((LeafRegion) obj);
    }

    public override int GetHashCode()
    {
        return (Block != null ? Block.GetHashCode() : 0);
    }
}
```

### Пример использования

```cs
foreach (Region r in regions)
{
    LeafRegion leaf = r as LeafRegion;
    if(leaf != null)
    {
        result[r, RegionDirection.In, r] = Identity;
        result[r, RegionDirection.Out, r] = input => param.TransferFunction(input, leaf.Block);
    }
    BodyRegion body = r as BodyRegion;
    if(body != null)
        foreach(Region s in body.Regions)
        {
            result[r, RegionDirection.In, s] = input => GatherFunctionsResults(input, result, r, body.Header.InputBlocks, graph, param);
            CalculateForOutputBlocks(result, r, s, s.OutputBlocks, graph);
        }
    LoopRegion loop = r as LoopRegion;
    if(loop != null)
    {
        result[r, RegionDirection.In, loop.Body] = input => SetFactory.GetSet<V>(input.Union(GatherFunctionsResults(input, result, loop.Body, loop.Header.InputBlocks, graph, param)));
        CalculateForOutputBlocks(result, r, loop.Body, loop.OutputBlocks, graph);
    }
}
```
