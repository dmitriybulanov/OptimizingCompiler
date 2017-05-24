# Название задачи: хранение передаточных функций

### Выполнено командой: YACT

### Постановка задачи: 

Необходимо релизовать хранилище для передаточных функций, позволяющее сохранять и получать передаточные функции по ключу `Region1, Direction, Region2`. 

### От каких проектов зависит:

  - Базовые блоки
  - Регионы

### Зависимые проекты:

  - Алгоритм оптимизации с помощью областей

### Теория

Для сохранения функций и их получения необходимо создать ключ, по которому будут идентифицироваться передаточные функции. Ключ состоит из двух регионов и направления. Для реализации сравнения ключей используются методы `Equals(obj)` и `GetHashCode()`. Подобные методы должны быть реализованы в классах регионов и в самом ключе. 

### Входные данные:
 - Передаточные функции

### Выходные данные:
 - Передаточные функции

### Используемые структуры данных

- `Dictionary<TKey, TValue>` - для хранения передаточных функций

### Реализация алгоритма

Для классов `BodyRegion`, `IntermediateRegion`, `LeafRegion`, `LoopRegion` были перегружены методы `Equals` и `GetHashCode`. При реализации учитывались следующие факторы:

- Регионы, состоящие из одного блока сравниваются по блоку
- Регионы, включающие в себя список блоков, сравниваются с помощью метода `SequenceEqual`
- Для классов-наследников `IntermediateRegion` необходимо вызывать также `base.Equals`
- `GetHashCode` реализовывались с учетом полей, учитывающихся при сравнении на равенство с попыткой переполнить `int`, чтобы получить наиболее уникальный хеш.

### Пример использования

``` C#
// setting functions
foreach (Region r in regions)
{
    LeafRegion leaf = r as LeafRegion;
    if(leaf != null)
    {
        result[r, RegionDirection.In, r] = Identity;
        result[r, RegionDirection.Out, r] = input => param.TransferFunction(input, leaf.Block);
    }
...
}

// getting functions
foreach (var r in regions.Reverse<Region>())
{
    int curIndex = RegionIndexes[r];
    if (curIndex != lastIndex)
    {
        regionsInputs[curIndex] = functions[regions[prevIndex], RegionDirection.In, regions[curIndex]](regionsInputs[prevIndex]);
    }
}
```

### Тест
``` C#
string text = @"
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
    }";

SyntaxNode root = ParserWrap.Parse(text);
var graph = new Graph(BasicBlocksGenerator.CreateBasicBlocks(ThreeAddressCodeGenerator.CreateAndVisit(root).Program));
var regions = new RegionSequence().CreateSequence(graph);
var storage = new AbstractTransferFunctionStorage<int>();
for (var i = 0; i < regions.Count - 1; i++)
{
    storage[regions[i], RegionDirection.Out, regions[i + 1]] = 2 * i;
    storage[regions[i], RegionDirection.In, regions[i + 1]] = 2 * i + 1;
}

for (var i = 0; i < regions.Count - 1; i++)
{
    Assert.IsTrue(storage[regions[i], RegionDirection.Out, regions[i + 1]] == 2 * i);
    Assert.IsTrue(storage[regions[i], RegionDirection.In, regions[i + 1]] == 2 * i + 1);
}
```