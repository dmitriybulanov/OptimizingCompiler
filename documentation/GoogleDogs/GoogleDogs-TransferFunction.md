# Вычисление передаточной функции, как композиции передаточных функций для одной команды

### Выполнено командой: 
*Google Dogs (Александр Василенко, Кирилл Куц)*

### От каких проектов зависит:
1. Получение базовых блоков
2. Получение множеств *gen* и *kill* для одной команды

### Зависимые проекты:

1. Итерационный алгоритм для достигающих определений

### Постановка задачи:
Используя множества *gen* и *kill* для одной команды, необходимо вычислить множества *gen* и *kill* для каждого блока.

### Теория

Предположим, что блок *В* состоит из инструкций *s*<sub>*1*</sub>, ..., *s*<sub>*n*</sub> в указанном порядке. Если *s*<sub>*1*</sub> — первая инструкция базового блока *В*, то IN[*В*] = IN[*s*<sub>*1*</sub>]. Аналогично, если *s*<sub>*n*</sub>— последняя инструкция базового блока *B*, то OUT[*B*] = OUT[*s*<sub>*n*</sub>]. Передаточная функция базового блока *B*, которую мы обозначим как *f*<sub>*B*</sub>, может быть получена как композиция передаточных функций инструкций базового блока. Пусть *f*<sub>*s*<sub>*i*</sub></sub> — передаточная функция для инструкции *s*<sub>*i*</sub>. Тогда *f*<sub>*B*</sub> = *f*<sub>*s*<sub>*n*</sub></sub> <font size="3" color="black" face="Arial">&#9675;</font> ...
<font size="3" color="black" face="Arial">&#9675;</font>
*f*<sub>*s*<sub>*2*</sub></sub><font size="3" color="black" face="Arial"> &#9675; </font>*f*<sub>*s*<sub>*1*</sub></sub>.

### Входные данные
- Базовые блоки

### Выходные данные
- Множества *gen* и *kill* для блока

### Используемые структуры данных
- ```GenKillOneCommandCalculator commandCalc``` - вычислитель 
 множеств *gen* и *kill* для одной команды
 
### Реализация алгоритма
```cs
public override ISet<CommandNumber> CommandTransferFunction
    (ISet<CommandNumber> input, BasicBlock block,
      int commandNumber)
{
    GenKillOneCommand genKill =
      calculator.CalculateGenAndKill(block, commandNumber);
    var result = SetFactory.GetSet<CommandNumber>(input);
    result.ExceptWith(genKill.Kill);
    result.UnionWith(genKill.Gen == null
      ? SetFactory.GetSet<CommandNumber>()
      : SetFactory.GetSet(genKill.Gen));
    return result;
}
```

### Пример использования
```cs
public override T TransferFunction
  (T input, BasicBlock block)
{
    return Enumerable.Range(0, block.Commands.Count)
      .Aggregate(input, (result, c) => 
         CommandTransferFunction(result, block, c));
}
```