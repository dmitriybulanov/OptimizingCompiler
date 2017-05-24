# Итерационный алгоритм

### Выполнено командой:
*Ampersand (Золотарёв Федор, Маросеев Олег)*

### От каких проектов зависит:
1. 

### Зависимые проекты:


### Постановка задачи:
В данной задаче требуется разработать класс, который реализует функцию проверки 

### Теория

### Используемые структуры данных
- ` ` - словарь классифицированных ребер

### Реализация

```cs
public static class IterativeAlgorithm
    {
        public static IterativeAlgorithmOutput<V> Apply<T, V>(Graph graph, BasicIterativeAlgorithmParameters<V> param) where T : BasicIterativeAlgorithmParameters<V>
        {
            IterativeAlgorithmOutput<V> result = new IterativeAlgorithmOutput<V>();

            foreach (BasicBlock bb in graph)
                result.Out[bb.BlockId] = param.StartingValue;

            bool changed = true;
            while (changed)
            {
                changed = false;
                foreach (BasicBlock bb in graph)
                {
                    result.In[bb.BlockId] = param.GatherOperation((param.ForwardDirection ? graph.getParents(bb.BlockId) : graph.getAncestors(bb.BlockId)).Blocks.Select(b => result.Out[b.BlockId]));
                    V newOut = param.TransferFunction(result.In[bb.BlockId], bb);
                    changed = changed || !param.Compare(result.Out[bb.BlockId], newOut);
                    result.Out[bb.BlockId] = param.TransferFunction(result.In[bb.BlockId], bb);
                }
            }
            if (!param.ForwardDirection)
                result = new IterativeAlgorithmOutput<V> { In = result.Out, Out = result.In };
            return result;
        }
    }
```

### Пример использования

```cs
 [TestClass]
    public class AvailableExpressionTest
    {
        [TestMethod]
        public void AvailableExpressionsTest()
        {
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
            SyntaxNode root = ParserWrap.Parse(programText);
            var threeAddressCode = ThreeAddressCodeGenerator.CreateAndVisit(root).Program;
            Trace.WriteLine(threeAddressCode);

            var basicBlocks = BasicBlocksGenerator.CreateBasicBlocks(threeAddressCode);
            Trace.WriteLine(Environment.NewLine + "Базовые блоки");
            Trace.WriteLine(basicBlocks);

            Trace.WriteLine(Environment.NewLine + "Управляющий граф программы");
            Graph g = new Graph(basicBlocks);
            Trace.WriteLine(g);


            
            Trace.WriteLine(Environment.NewLine + "Доступные выражения");
            var availableExprs = IterativeAlgorithm.Apply(g, new AvailableExpressionsCalculator(g));
            var outExpressions = availableExprs.Out.Select(
                pair => $"{pair.Key}: {string.Join(", ", pair.Value.Select(ex => ex.ToString()))}");

            foreach (var outInfo in outExpressions)
            {
                Trace.WriteLine(outInfo);
            }

            int startIndex = availableExprs.Out.Keys.Min();
            Assert.IsTrue(availableExprs.Out[startIndex]
                .SetEquals(new Expression[]
                    {
                        new Int32Const(4),
                        new BinaryOperation(new identifier("a"), Operation.Add, new identifier("b")),
                        new identifier("t0")
                    }));

            Assert.IsTrue(availableExprs.Out[startIndex + 1]
                .SetEquals(new Expression[]
                    {
                        new Int32Const(4),
                        new Int32Const(3),
                        new identifier("t0")
                    }));

            Assert.IsTrue(availableExprs.Out[startIndex + 2]
                .SetEquals(
                    new Expression[]
                    {
                        new Int32Const(4),
                        new Int32Const(2),
                        new identifier("t0")
                    }));

            Assert.IsTrue(availableExprs.Out[startIndex + 3]
                .SetEquals(
                   new Expression[]
                   {
                       new Int32Const(4),
                       new identifier("t0")
                   }));
        }
    }
```
