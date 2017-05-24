# Название задачи: Построение трехадресного кода по CFG

### Выполнено командой: 
Yet yet another team(Горелов Антон, Кочерга Михаил)

### Постановка задачи: 
Требуется реализовать алгоритм, который по данному графу потока управления восстанавливает текст программы в трехадресном коде.

### От каких проектов зависит:

  - Граф управления потока(CFG)

### Зависимые проекты:

  - Отсутствует

### Теория

**Трехадресный код** - это последовательность операторов одного из следующих видов.
x = y op z
x = y //команды копирования
x = op y //x, y, z - имена, константы или временные объекты.
goto L
if x goto L
if x goto L
if False x goto L
**Трехадресный код** представляет собой линеаризованное представление синтаксического дерева или ориентированного ациклического графа, в котором явные имена соответствуют внутренним узлам графа. 

![](/img/CFG3Code.png?raw=true)
Ориентированный ациклический граф и  соответствующий ему трехадресный код.


### Входные данные:
 - Граф потока управления

### Выходные данные:
 - Трехадресный код.

### Используемые структуры данных

 - BidirectionalGraph<BasicBlock, Edge> - граф потока управления (структура данных из пакета QuickGraph)
 - Dictionary<int, BasicBlock> blockMap - соответствие между узлами графа и уникальными идентификаторами базовых блоков (blockId)
 - List<ThreeAddressCommand> - для хранения результата
 - Stack<BasicBlock> - для хранения порядка обработки ББл.
 - HashSet<BasicBlock> - для хранения обрабтанных ББл.

### Реализация алгоритма

```C#
public List<ThreeAddressCommand>  transformToThreeAddressCode()
         {
             var res = new List<ThreeAddressCommand>();
             var done = new HashSet<BasicBlock>();
             var stack = new Stack<BasicBlock>();
             stack.Push(getBlockById(0));
             BasicBlock cur = null;
 
             while (done.Count < this.GetCount())
             {
                 cur = stack.Pop();
                 done.Add(cur);
                 
                 foreach (var c in cur.Commands)  { res.Add(c); }
                 
                 switch (cur.OutputBlocks.Count)
                 {
                     case 0:
                         continue;
                      case 1:
                          //добавить в стек выходной ББЛ
                      case 2:
                          //если в else последний оператор goto, то добавить в стек ББл, на который указывает goto
                          //добавить в стек ветку else
                          //добавить в стек ветку if
                      default:
                          throw new Exception("There cannot be more than two output blocks!");
                }
            }
```

### Пример использования
```C#
SyntaxTree.SyntaxNodes.SyntaxNode root = ParserWrap.Parse(sourseText);
                var sourceThreeAddressCode = ThreeAddressCodeGenerator.CreateAndVisit(root).Program;
                BasicBlocksList bbl = BasicBlocksGenerator.CreateBasicBlocks(sourceThreeAddressCode);
                Graph cfg = new Graph(bbl);
                List<ThreeAddressCommand> resultThreeAddressCode = cfg.transformToThreeAddressCode();
```
### Тест

```C#
if 1
    if 1
    {
        a = 5;
        a = 1;
    }
    else
    {   
        for i = 1..5 
            a = 1;
    }
    
while 1
    a = 1;
a = 2;

for i = 1 .. 5
    for j = 1 .. 5
        a = 1;

for i = 1 .. 5
    for j = 1 .. 5
        if 1
            a = 1;
        else
        {   
            a = 1;
            a = 2;
        }
```
