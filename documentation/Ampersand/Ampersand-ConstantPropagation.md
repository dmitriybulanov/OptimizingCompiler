# Итерационный алгоритм для задачи распространения констант

### Выполнено командой:
*Ampersand (Золотарёв Федор, Маросеев Олег)*

### От каких проектов зависит:
1. Общий итерационный алгоритм
2. Передаточная функция для задачи распостронения констант

### Зависимые проекты:
-

### Постановка задачи:

### Теория

### Используемые структуры данных
- ` Dictionary<Edge<BasicBlock>, EdgeType>` - словарь классифицированных ребер

### Реализация

```cs
 public class ConstantPropagation : IOptimization
    {
        public Graph Apply(Graph graph)
        {
            var constants = IterativeAlgorithm.Apply(graph, new ConstantsPropagationParameters());

            return graph;
        }
    }
    
public class ConstantsPropagationParameters : CompositionIterativeAlgorithmParameters<Dictionary<string, string>>
    {
        public const string NAC = "NAC";
        public const string UNDEF = "UNDEF";

        public override bool ForwardDirection { get { return true; } }

        public override Dictionary<string, string> FirstValue { get { return new Dictionary<string, string>(); } }

        public override Dictionary<string, string> StartingValue { get { return new Dictionary<string, string>(); } }

        public override Dictionary<string, string> CommandTransferFunction(Dictionary<string, string> input, BasicBlock block, int commandNumber)
        {
            ThreeAddressCommand command = block.Commands[commandNumber];
            if (command.GetType() == typeof(Assignment))
            {
                string newValue = NAC;
                Expression expr = (command as Assignment).Value;
                if (expr.GetType() == typeof(Int32Const) || expr.GetType() == typeof(Identifier))
                    newValue = getConstantFromSimpleExpression(input, (expr as SimpleExpression));
                else if (expr.GetType() == typeof(UnaryOperation))
                {
                    UnaryOperation operation = (expr as UnaryOperation);
                    newValue = calculateVal(getConstantFromSimpleExpression(input, operation.Operand), operation.Operation);
                }
                else if (expr.GetType() == typeof(BinaryOperation))
                {
                    BinaryOperation operation = (expr as BinaryOperation);
                    newValue = calculateVal(getConstantFromSimpleExpression(input, operation.Left), getConstantFromSimpleExpression(input, operation.Right), operation.Operation);
                }
                string leftOperand = (command as Assignment).Target.Name;
                input[leftOperand] = newValue;
            }
            return input;
        }

        string getConstantFromSimpleExpression(Dictionary<string, string> input, SimpleExpression expr)
        {
            string result = NAC;
            if (expr.GetType() == typeof(Int32Const))
                result = (expr as Int32Const).ToString();
            else if (expr.GetType() == typeof(Identifier))
            {
                string var = (expr as Identifier).ToString();
                if (!input.ContainsKey(var))
                    input[var] = UNDEF;
                result = input[var];
            }
            return result;
        }

        public override bool AreEqual(Dictionary<string, string> t1, Dictionary<string, string> t2)
        {
            return t1.Count == t2.Count && t1.Keys.All(key => t2.ContainsKey(key) && t1[key] == t2[key]);
        }
        string calculateVal(string x1, Operation op)
        {
            return calculateVal(x1, "0", op);
        }
        
        string calculateVal(string x1, string x2, Operation op)
        {
            if (x1 == NAC || x2 == NAC)
                return NAC;
            else if (x1 == UNDEF || x2 == UNDEF)
                return UNDEF;
            else
            {
                int lx = int.Parse(x1);
                int rx = int.Parse(x2);
                return ArithmeticOperationCalculator.Calculate(op, lx, rx).ToString();
            }
        }
        string gatherVal(string x1, string x2)
        {
            if (x1 == x2 || x2 == UNDEF || x1 == NAC)
                return x1;
            else if (x1 == UNDEF || x2 == NAC)
                return x2;
            else
                return NAC;
        }
        public override Dictionary<string, string> GatherOperation(IEnumerable<Dictionary<string, string>> blocks)
        {
            return blocks.Aggregate(new Dictionary<string, string>(), (result, x) =>
            {
                foreach(KeyValuePair<string, string> pair in x)
                    result[pair.Key] = result.ContainsKey(pair.Key) ? gatherVal(result[pair.Key], pair.Value) : pair.Value;
                return result;
            });
        }
    }
```

### Пример использования

```cs
 [TestClass]
    public class ConstantPropagationIterativeAlgorithmTests
    {
        [TestMethod]
        public void ConstantsPropagation1()
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



            Trace.WriteLine(Environment.NewLine + "Распространение констант");
            var consts = IterativeAlgorithm.Apply(g, new ConstantsPropagationParameters());
            var outConsts = consts.Out.Select(
                pair => $"{pair.Key}: {string.Join(", ", pair.Value.Select(ex => ex.ToString()))}");

            foreach (var outInfo in outConsts)
            {
                Trace.WriteLine(outInfo);
            }

            int startIndex = consts.Out.Keys.Min();
            Assert.IsTrue(consts.Out[startIndex]["a"]=="4" &&
                consts.Out[startIndex]["b"] == "4" &&
                consts.Out[startIndex]["t0"] == "8" &&
                consts.Out[startIndex]["c"] == "8");

            Assert.IsTrue(consts.Out[startIndex + 1]["a"] == "3" &&
                consts.Out[startIndex + 1]["b"] == "4" &&
                consts.Out[startIndex + 1]["t0"] == "8" &&
                consts.Out[startIndex + 1]["c"] == "8");

            Assert.IsTrue(consts.Out[startIndex + 2]["a"] == "4" &&
                consts.Out[startIndex + 2]["b"] == "2" &&
                consts.Out[startIndex + 2]["t0"] == "8" &&
                consts.Out[startIndex + 2]["c"] == "8");

            Assert.IsTrue(consts.Out[startIndex + 3]["a"] == "NAC" &&
                consts.Out[startIndex + 3]["b"] == "NAC" &&
                consts.Out[startIndex + 3]["t0"] == "8" &&
                consts.Out[startIndex + 3]["c"] == "8");
        }

        [TestMethod]
        public void ConstantsPropagation2()
        {
            string programText = @"
    e=10;
    c=4;
    d=2;
    a=4;
    if 0
        goto 2;
    a=c+d;
    e=a;
    goto 3;
2:  a=e;
3:  t=0;
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
            
            Trace.WriteLine(Environment.NewLine + "Распространение констант");
            var consts = IterativeAlgorithm.Apply(g, new ConstantsPropagationParameters());
            var outConsts = consts.Out.Select(
                pair => $"{pair.Key}: {string.Join(", ", pair.Value.Select(ex => ex.ToString()))}");

            foreach (var outInfo in outConsts)
            {
                Trace.WriteLine(outInfo);
            }

            int startIndex = consts.Out.Keys.Min();
            Assert.IsTrue(consts.Out[startIndex]["a"] == "4" &&
                consts.Out[startIndex]["e"] == "10" &&
                consts.Out[startIndex]["d"] == "2" &&
                consts.Out[startIndex]["c"] == "4");

            Assert.IsTrue(consts.Out[startIndex + 1]["a"] == "4" &&
                consts.Out[startIndex + 1]["e"] == "10" &&
                consts.Out[startIndex + 1]["d"] == "2" &&
                consts.Out[startIndex + 1]["c"] == "4");

            Assert.IsTrue(consts.Out[startIndex + 2]["e"] == "6" &&
                consts.Out[startIndex + 2]["c"] == "4" &&
                consts.Out[startIndex + 2]["d"] == "2" &&
                consts.Out[startIndex + 2]["a"] == "6" &&
                consts.Out[startIndex + 2]["t0"] == "6");

            Assert.IsTrue(consts.Out[startIndex + 3]["a"] == "10" &&
                consts.Out[startIndex + 3]["e"] == "10" &&
                consts.Out[startIndex + 3]["d"] == "2" &&
                consts.Out[startIndex + 3]["c"] == "4");

            Assert.IsTrue(consts.Out[startIndex + 4]["e"] == "NAC" &&
                consts.Out[startIndex + 4]["c"] == "4" &&
                consts.Out[startIndex + 4]["d"] == "2" &&
                consts.Out[startIndex + 4]["t"] == "0" &&
                consts.Out[startIndex + 4]["t0"] == "6");
        }
    }
```
