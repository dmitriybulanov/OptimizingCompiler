# Название задачи: вычисление `e_gen(b)` и `e_kill(b)` для доступных выражений

### Выполнено командой: YACT

### Постановка задачи: 

Необходимо по данному графу потока данных найти множества `e_gen` и `e_kill` для каждого базового блока. Эти данные используются алгоритмом анализа доступных выражений. 

### От каких проектов зависит:

  - CFG
  - Базовые блоки
  - Обобщенный итеративный алгоритм
  - Трехадресный код

### Зависимые проекты:

  - Анализатор доступных выражений

### Теория

**Доступные выражения** -- алгоритм анализа, определяющий для каждой точки в программе множество выражений, которые не требуется перевычислять. Такие выражения называются *доступными* в данной точке. Для того, чтобы быть доступными в точке программы, операнды выражения не должны быть изменены на любом пути от вхождения этого выражения до точки программы.

![alt text](https://github.com/wisestump/OptimizingCompiler/raw/master/documentation/YACT/img/GenKillExample.png "Gen-Kill Example")

### Входные данные:
 - Базовый блок

### Выходные данные:
 - Множество выражений

### Используемые структуры данных

 - `HashSet<Expression>` -- множество выражений

### Реализация алгоритма

```
// Метод получения множества Gen
public override ISet<Expression> GetGen(BasicBlock block)
{
    return SetFactory.GetSet( 
        block.Commands
            .OfType<Assignment>() // оставляем только присваивания 
            .Select(x => x.Value)); // из них берем правые части
}

// Метод получения множества Kill
public override ISet<Expression> GetKill(BasicBlock block)
{
    return SetFactory.GetSet(
        block.Commands
            .OfType<Assignment>() // оставляем только присваивания 
            .Select(x => x.Target as Expression)); // из них берем левые части
}
```

### Пример использования

Трехадресный код программы:
```
1: <no-op>
t0 = a + b
x = t0
2: <no-op>
t1 = a * b
y = t1
3: <no-op>
t2 = a + 1
a = t2
```

Вызов алгоритма

```
// ----
// фомирование графа g
// ----
...
var availableExprs = IterativeAlgorithm.Apply(g, new AvailableExpressionsCalculator(g));
var outExpressions = availableExprs.Out.Select(
    pair => $"{pair.Key}: {string.Join(", ", pair.Value.Select(ex => ex.ToString()))}");
```
Вывод `outExpressions`:
```
// Доступные выражения
0: a + b, t0
1: a + b, t0, a * b, t1
2: t0, t1, a + 1, t2
```
### Тест

```
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

// Формирование CFG и вызов алгоритма
SyntaxNode root = ParserWrap.Parse(programText);
var threeAddressCode = ThreeAddressCodeGenerator.CreateAndVisit(root).Program;
var basicBlocks = BasicBlocksGenerator.CreateBasicBlocks(threeAddressCode);
Graph g = new Graph(basicBlocks);

var availableExprs = IterativeAlgorithm.Apply(g, new AvailableExpressionsCalculator(g));
var outExpressions = availableExprs.Out.Select(
    pair => $"{pair.Key}: {string.Join(", ", pair.Value.Select(ex => ex.ToString()))}");

// Сравнение с эталоном
int startIndex = availableExprs.Out.Keys.Min();
Assert.IsTrue(availableExprs.Out[startIndex]
    .SetEquals(
        new HashSet<Expression>(new Expression[]
        {
            new Int32Const(4),
            new BinaryOperation(new identifier("a"), Operation.Add, new identifier("b")),
            new identifier("t0")
        })));

Assert.IsTrue(availableExprs.Out[startIndex + 1]
    .SetEquals(
        new HashSet<Expression>(new Expression[]
        {
            new Int32Const(4),
            new Int32Const(3),
            new identifier("t0")
        })));

Assert.IsTrue(availableExprs.Out[startIndex + 2]
    .SetEquals(
        new HashSet<Expression>(new Expression[]
        {
            new Int32Const(4),
            new Int32Const(2),
            new identifier("t0")
        })));

Assert.IsTrue(availableExprs.Out[startIndex + 3]
    .SetEquals(
        new HashSet<Expression>(new Expression[]
        {
                new Int32Const(4),
                new identifier("t0")
        })));
```