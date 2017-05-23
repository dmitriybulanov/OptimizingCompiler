# Вычисление множеств Use(B) и Def(B) для активных переменных

### Выполнено командой:
*PiedPiper (Бергер Анна, Колесников Сергей)*

### От каких проектов зависит:
1. Базовые блоки

### Зависимые проекты:
1. Итерационный алгоритм для активных переменных

### Теория
В точке p **x** является активной переменной, если существует путь, проходящий через p, начинающийся присваиванием, заканчивается
ее использованием и на всем промежутке нет других присваиваний переменной **x**.
 - Def(B) -- множество переменных, определенных в базовом блоке до любого их использования. 
 
 Можно использовать более простое определение Def(B) -- множество переменных, определенных в базовом блоке. При таком изменении определения
 результат работы итерационного алгоритма не изменится.
 - Use(B) -- множество переменных, определенных в базовом блоке до любого их определения.

### Входные данные:
 - Базовый блок (BasicBlock)

### Выходные данные:
 - Пара множеств Use(B) и Def(B)
 
 ### Используемые структуры данных
Dictionary<int, Tuple<ISet<string>, ISet<string>>> SetStorage - словарь, сопоставляющий номеру базового блока его пару его множеств Use(B) и Def(B)

 ### Реализация алгоритма
 
 Метод получения множеств Use(B) и Def(B) по базовому блоку. 
 Он сохраняет полученные множества в SetStorage для избежания повторных вычислений при повторном вызове для блока.
 ```C#
 public Tuple<ISet<string>, ISet<string>> GetDefUseSetsByBlock(BasicBlock block)
  {
    if (!SetStorage.Keys.Contains(block.BlockId))
    {
      SetStorage.Add(block.BlockId, CreateDefUseSets(block));
    }
    return SetStorage[block.BlockId];
  }
 ```
 Метод, проходящий по списку команд базового блока и заполняющий множества Use(B) и Def(B). 
 Множество Def(B) пополняется, если встретилась команда типа Assignment. 
 Для пополнения множества Use(B) вызывается вспомогательная функция ExpressionParser.
 ``` C#
 private Tuple<ISet<string>, ISet<string>> CreateDefUseSets(BasicBlock block)
  {
    ISet<string> Def = new HashSet<string>();
    ISet<string> Use = new HashSet<string>();

    foreach (var command in block.Commands)
    {
      if (command.GetType() == typeof(Assignment)) 
      {
        Def.Add(((Assignment)command).Target.Name);
        ExpressionParser(((Assignment)command).Value, Def, Use);
      }
      if (command.GetType() == typeof(ConditionalGoto))
      {
        ExpressionParser(((ConditionalGoto)command).Condition, Def, Use);
      }
      if (command.GetType() == typeof(Print))
      {
        ExpressionParser(((Print)command).Argument, Def, Use);
      }
    }
    return new Tuple<ISet<string>, ISet<string>>(Def, Use);
  }
 ```
 
 Рекурсивная функция, позволяющая обработать части выражений типа Expression и пополнить множество Use(B)
 ``` C#
 private void ExpressionParser(Expression expr, ISet<string> Def, ISet<string> Use)
  {
    if (expr.GetType() == typeof(BinaryOperation))
    {
      ExpressionParser(((BinaryOperation)expr).Left, Def, Use);
      ExpressionParser(((BinaryOperation)expr).Right, Def, Use);
    }
    if (expr.GetType() == typeof(Identifier))
    {
      if (!Def.Contains(((Identifier)expr).Name))
      {
        Use.Add(((Identifier)expr).Name);
      }
    }
    if (expr.GetType() == typeof(UnaryOperation))
    {
      ExpressionParser(((UnaryOperation)expr).Operand, Def, Use);
    }
  }
 ``` 
 ### Пример использования
 ```
 DefUseBlockCalculator DefUseCalc = new DefUseBlockCalculator();
 var UseDefTuple = DefUseCalc.GetDefUseSetsByBlock(block);
 var Def = UseDefTuple.Item1;
 var Use = UseDefTuple.Item2;
 ```
 
 ### Тест 
Программа:
```
a = 4;
b = 4;
c = a + b;
if 1
    a = 3;
else
    b = 2;
print(c);
```
Базовый блок BlockId = 0
```
Commands:
  a = 4
  b = 4
  t0 = a + b
  c = t0

Def(B) = {a, b, t0, c}
Use(B) = {}
```
Базовый блок BlockId = 3
```
Commands:
  $GL_1: <no-op>
  print c

Def(B) = {}
Use(B) = {c}
```
