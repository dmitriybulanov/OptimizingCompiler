# Название задачи: генерация трехадресного кода по синтаксическому дереву

### Выполнено командой: YACT

### Постановка задачи: 

Необходимо релизовать генератор трехадресного кода по синтаксическому дереву. Трехадресный код состоит из семи видов инструкций:

- `x = y op z`
- `x = y`
- `x = op z`
- `goto L`
- `if x goto  L`
- `no-op`
- `print`

### От каких проектов зависит:

  - Синтаксическое дерево

### Зависимые проекты:

  - Базовые блоки

### Теория

Перевод синтаксических конструкций в трехадресный код может быть выполнен следующим образом:

- Бинарные выражения сохраняются в отдельных, генерируемых переменных (`t0`, `t1`, ...)
- Присваивания унарных выражения переводятся напрямую
- Присваивания бинарных выражений преобразуются в присваивания унарных с использованием правил преобразования бинарных выражений
- Циклы и условный оператор могут быть преобразованы с использованием условных/безусловных операторов перехода и пустых операций с метками

### Входные данные:
 - Синтаксическое дерево

### Выходные данные:
 - Список команд трехадресного кода

### Используемые структуры данных

- `List<T>` - для хранения списка команд
- `Stack<T>` - для хранения выражений при преобразовании бинарных выражений

### Реализация алгоритма

Генератор трехадресного кода был выполнен с использованием паттерна *визитор*. Для каждого узла синтаксического дерева были определены методы его обработки и в них осуществлялось формирование трехадресных команд.

Исходный код генератора расположен в [ThreeAddressCodeGenerator.cs](https://github.com/wisestump/OptimizingCompiler/blob/master/src/DataFlowAnalysis/IntermediateRepresentation/ThreeAddressCode/ThreeAddressCodeGenerator.cs)

Иерархия трехадресных команд
![ThreeAddrCode.png](https://github.com/wisestump/OptimizingCompiler/raw/master/documentation/YACT/img/ThreeAddressCodeTypeGraph.png "Three-Address Code Hierarcy")



### Пример использования

``` C#
text = @"
a = 1;
b = 1;
c = a + b;
for i = 1 .. 3
    c = c - 1;
";
SyntaxNode root = ParserWrap.Parse(text);
if (root != null)
{
    var tac = ThreeAddressCodeGenerator.CreateAndVisit(root);
    Console.WriteLine(string.Join(Environment.NewLine, tac.Program.Commands.Select(x => x.ToString())));
}
```

Вывод:
```
a = 1
b = 1
t0 = a + b
c = t0
i = 1
$GL_1: goto $GL_2 if i > 3
t1 = c - 1
c = t1
i = i + 1
goto $GL_1
$GL_2: <no-op>
```

### Тест
``` C#
string text = @"
a = 2;
while a > 1
a = a - 1;";
SyntaxTree.SyntaxNodes.SyntaxNode root = ParserWrap.Parse(text);
var threeAddressCode = ThreeAddressCodeGenerator.CreateAndVisit(root).Program;

var expectedCommands = new List<ThreeAddressCommand>
{
    new Assignment("a", new Int32Const(2)),
    new Assignment("t0", new BinaryOperation("a", Operation.Greater, 1)),
    new ConditionalGoto("$GL_2", new BinaryOperation("t0", Operation.Equal, 0)) { Label = "$GL_1" },
    new Assignment("t1", new BinaryOperation("a", Operation.Subtract, 1)),
    new Assignment("a", new Identifier("t1")),
    new Goto("$GL_1"),
    new NoOperation("$GL_2")
};

CollectionAssert.AreEqual(threeAddressCode.Commands, expectedCommands);
```