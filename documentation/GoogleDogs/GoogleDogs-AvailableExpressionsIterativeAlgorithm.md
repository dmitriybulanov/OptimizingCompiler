# Итерационный алгоритм для доступных выражений

### Выполнено командой: 
*Google Dogs (Александр Василенко, Кирилл Куц)*

### От каких проектов зависит:
1. Построение базовых блоков
2. Построение трехадресного кода
3. Построение множеств *e_gen* и *e_kill*

### Зависимые проекты:
отсутствуют

### Постановка задачи
Написать класс-наследник общего итерационного алгоритма, переопределить методы для поиска доступных выражений

### Теория
Выражение *х* + *у* **доступно** (available) в точке *р*, если любой путь от входного узла к *р* вычисляет *х* + *у* и после последнего такого вычисления до достижения *р* нет последующих присваиваний переменным *х* и *у*. Когда речь идет о *доступных выражениях*, мы говорим, что блок уничтожает выражение *х* + *у*, если он присваивает (или может присваивать) *х* и *у* и после этого не вычисляет *х* + *у* заново. Блок генерирует выражение *х* + *у*, если он вычисляет *х* + *у* и не выполняет последующих переопределений *х* и *у*. 


Мы можем найти доступные выражения методом, напоминающим метод вычисления достигающих определений. Предположим, что *U* — "универсальное" множество всех выражений, появляющихся в правой части одной или нескольких инструкций программы. Пусть для каждого блока *В* множество IN[*В*] содержит выражения из *U*, доступные в точке непосредственно перед началом блока *B*, a OUT[*В*] — такое же множество для точки, следующей за концом блока *В*. Определим *e_gen*<sub>*B*</sub> как множество выражений, генерируемых *В*, a *e_kill*<sub>*B*</sub> — как множество выражений из *U*, уничтожаемых в *B*. Заметим, что множества IN, OUT, *e_gen* 
и *e_kill* могут быть представлены в виде битовых векторов. Неизвестные множества IN и OUT связаны друг с другом и с известными *e_gen* и *e_kill* следующими 
соотношениями: 

OUT[Вход] = <html>&#x2205;</html>

и для всех базовых блоков *В*, отличных от входного:

OUT[*В*] = *e_gen*<sub>*B*</sub> <html>&#8746;</html> (IN[*В*] - *e_kill*<sub>*B*</sub>)

IN[*В*] = <html>&#8745;</html> OUT[*P*], где *P* - предшественник *B*

### Реализация алгоритма
```cs
public AvailableExpressionsCalculator(Graph g)
{
    Graph = g;
}

public override ISet<Expression> GatherOperation
  (IEnumerable<ISet<Expression>> blocks)
{
    ISet<Expression> intersection = SetFactory.GetSet((IEnumerable<Expression>)blocks.First());
    foreach (var block in blocks.Skip(1))
      intersection.IntersectWith(block);
    return intersection;
}

public override ISet<Expression> GetGen(BasicBlock block)
{
    return SetFactory.GetSet(
        block.Commands
          .OfType<Assignment>()
          .Select(x => x.Value));
}

public override ISet<Expression> GetKill(BasicBlock block)
{
    return SetFactory.GetSet(
      block.Commands
        .OfType<Assignment>()
        .Select(x => x.Target as Expression));
}

public override ISet<Expression> TransferFunction
  (ISet<Expression> input, BasicBlock block)
{
    var kill = GetKill(block).Cast<Identifier>();
    var result = SetFactory.GetSet(
        input.Where(inputExpression =>
        !kill.Any(inputExpression.HasIdentifiedSubexpression)));
    result.UnionWith(GetGen(block));
    return result;
}

public override bool ForwardDirection => true;

public override ISet<Expression> FirstValue => SetFactory.GetSet();

public override ISet<Expression> StartingValue
{
    get
    {
        ISet<Expression> result = SetFactory.GetSet();
        foreach (BasicBlock b in Graph)
          foreach (ThreeAddressCommand c in b.Commands)
            if (c.GetType() == typeof(Assignment))
            {
                result.Add((c as Assignment).Value);
                result.Add((c as Assignment).Target);
            }
         return result;
    }
}
```

### Пример использования
```cs
var availableExprs = IterativeAlgorithm.Apply(g,
    new AvailableExpressionsCalculator(g));
var outExpressions = availableExprs.Out.Select(
    pair => $"{pair.Key}: {string.Join(", ", 
    pair.Value.Select(ex => ex.ToString()))}");
```

