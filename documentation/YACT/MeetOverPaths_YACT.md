# Название задачи: Meet Over All Paths

### Выполнено командой: YACT

### Постановка задачи: 

Необходимо реализовать алгоритм Meet Over All Paths на ациклическом графе потока данных для реализации оптимизаций. А также сравненить результаты работы этого алгоритма с результатами итерационного алгоритмом для двух конфигураций.

Решить задачу потока данных, методом обхода всех путей от входной точки до каждого базового блока. На примерах показать, что для дистрибутивных схем передачи потока данных результат работы Meet Over All Paths совпадает с результатом работы итерационного алгоритма, а для не дистрибутивных(распространение констант) не совпадает. 

### От каких проектов зависит:

  - Control Flow Graph
  - Basic Block Model
  - Iterative Algorithm Parameters

### Зависимые проекты:

нет

### Теория

**Определение**: Для каждого пути *p*, состоящего из базовых блоков *B<sub>i</sub>* рассмотрим *f<sub>p</sub>(V<sub>Вход</sub>)*, где *f<sub>p</sub>* это композиция передаточных функций базовых блоков на пути *p*. Идеальным решением назовём

IDEAL[B] = ⋀ f<sub>p</sub>(V<sub>Вход</sub>), где ⋀ это оператор сбора по возможным путям от входа к B

Рассмотрим пример №1

![](YACT/img/MOP_Example1.png?raw=true)

Из этого примера видно, что не зная ничего о sqr нельзя сказать какие пути возможны, а какие нет. Поэтому будем рассматривать Meet Over All Paths(MOP)

MOP[B] = ⋀ f<sub>p</sub>(V<sub>Вход</sub>), где ⋀ это оператор сбора по всем путям от входа к B

Сравним Meet Over All Paths с итерационным алгоритмом(Maximum Fix Point) на примере следующего графа

![](YACT/img/MOP_Example2.png?raw=true)

MOP[B] = f<sub>B3</sub>(f<sub>B1</sub>(V<sub>Вход</sub>)) ⋀ f<sub>B3</sub>(f<sub>B2</sub>(V<sub>Вход</sub>))

MFP[B] = f<sub>B3</sub>(f<sub>B1</sub>(V<sub>Вход</sub>) ⋀ f<sub>B2</sub>(V<sub>Вход</sub>))

**Утверждение**: Если схема передачи потока данных дистрибутивна, то **MFP[B] = MOP[B]** 

**Замечание 1**: Схема передачи данных дистрибутивна <=> f - дистрибутивна

**Замечание 2**: Всегда имеет место **MFP[B]≤MOP[B]≤IDEAL[B]**

### Входные данные:
 - Ациклический граф потока данных программы
 - Параметры конкретной оптимизации

### Выходные данные:
 - Решение задачи потока данных для конкретной оптимизации

### Используемые структуры данных

 - Словарь из идентификатора блока в результат работы передаточной функции
 - Граф потока данных
 - Абстрактный класс параметров алгоритма

### Реализация алгоритма

```C#
var MOP = new Dictionary<int, V>(); // Результат

foreach (BasicBlock blockTo in graph) // Для каждого блока получаемзначение потокаданных
{
  MOP[blockTo.BlockId] = param.StartingValue;
  foreach (var path in GraphAlgorithms.FindAllPaths(graph, blockTo.BlockId)) // Для каждого пути от начала до конкретного блока получаем композицию передаточных функций и собираем результаты опертором сбора
  {
    var value = path.Aggregate(param.FirstValue, param.TransferFunction);
    MOP[blockTo.BlockId] = param.GatherOperation(new List<V> { MOP[blockTo.BlockId], value});
  }
}
```

### Пример использования

Программа на языке:

```C#
if 1
{
    x = 2;
    y = 3;
}
else
{
    x = 3;
    y = 2;
}
z = x + y;
```

Вызов алгоритма:

```C#
// Парсинг программы и получение CFG
...
var constantPropagationMOP = MeetOverPaths.Apply(graph, new ConstantsPropagationParameters());
var it = constantPropagationMOP.Out.Select(
  pair => $"{pair.Key}: {string.Join(", ", pair.Value.Select(ex => ex.ToString()))}");
```

Вывод:

```C#
50: 
51: [x, 2], [y, 3]
52: [x, 3], [y, 2]
53: [x, NAC], [y, NAC], [t0, 5], [z, 5]
```

### Тест

```C#
// Программа взята из тестов оптимизации AvailableExpression
string programText = @"
a = 4;
b = 4;
c = a + b;
if 1 
  a = 3;
else
  b = 2;
print(c);
";

// Получение CFG
SyntaxNode root = ParserWrap.Parse(programText);
var threeAddressCode = ThreeAddressCodeGenerator.CreateAndVisit(root).Program;
var basicBlocks = BasicBlocksGenerator.CreateBasicBlocks(threeAddressCode);
Graph g = new Graph(basicBlocks);

// Получаем результаты разными алгоритмами
var availableExprsIterative = IterativeAlgorithm.Apply(g, new AvailableExpressionsCalculator(g));
var availableExprsMOP = MeetOverPaths.Apply(g, new AvailableExpressionsCalculator(g));

// Вывод полученных результатов
var it = availableExprsIterative.Out.Select(
  pair => $"{pair.Key}: {string.Join(", ", pair.Value.Select(ex => ex.ToString()))}");
foreach (var outInfo in it)
{
  Trace.WriteLine(outInfo);
}
var mop = availableExprsMOP.Select(
  pair => $"{pair.Key}: {string.Join(", ", pair.Value.Select(ex => ex.ToString()))}");
Trace.WriteLine("====");
foreach (var outInfo in mop)
{
  Trace.WriteLine(outInfo);
}

// Сравниваем на совпадение полученные структуры данных
Assert.IsTrue(availableExprsIterative.Out.OrderBy(kvp => kvp.Key).
  Zip(availableExprsMOP.OrderBy(kvp => kvp.Key), (v1, v2) => v1.Key == v2.Key && v1.Value.SetEquals(v2.Value)).All(x => x));
```
