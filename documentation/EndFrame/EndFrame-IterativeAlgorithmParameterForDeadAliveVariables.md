# Название задачи: Iterative Algorithm Parameter for DeadAliveVariables

### Выполнено командой: EndFrame

### Постановка задачи: 
Необходимо реализовать класс, являющийся параметром обобщенного 
итерационного алгоритма. Класс должен наследоваться от абстрактного класса BasicIterativeAlgorithmParameters<V>, 
и в нём необходимо задать поля ForwardDirection, отвечающее за порядок обхода блоков, FirstValue, хранящее значение первого(последнего)
блока, StartingValue, хранящее начальное приближение всех блоков, а также методы GatherOperation, являющийся оператором сбора, и 
TransferFunction, являющийся передаточной функцией.


### От каких проектов зависит:

  - BasicIterativeAlgorithmParameters
  - SetIterativeAlgorithmParameters
  - BasicBlock
  - DefUseBlockCalculator

### Зависимые проекты:

  - IterativeAlgorithm

### Теория

> Переменная x - активная в точке p, если существует путь, проходящий через p, начинающийся присваиванием и заканчивающийся
ее использованием, и на всем промежутке нет других присваиваний переменной x.
defB - множество переменных, определенных в B до любого их использования
useB - множество переменных, использующихся в B до любого их определения.
fB - передаточная функция для блока B.
fB(OUT[B]) = IN[B].
IN[B] = useB U (OUT[B] - defB).

### Входные данные:

### Выходные данные:
 - DeadAliveIterativeAlgorithmParameters : SetIterativeAlgorithmParameters<string>

### Используемые структуры данных

 - IEnumerable<ISet<string>> blocks - множество переменных
 - BasicBlock block - базовый блок
 - ISet<string> input - IN[B]

### Реализация алгоритма

    public class DeadAliveIterativeAlgorithmParameters : SetIterativeAlgorithmParameters<string>
    {
        public override ISet<string> GatherOperation(IEnumerable<ISet<string>> blocks)
        {
            ISet<string> union = SetFactory.GetSet((IEnumerable<string>)blocks.First());
            /* U(по всем потомкам) IN[B] */
            foreach (var block in blocks.Skip(1))
            {
                union.UnionWith(block);
            }
            return union;
        }
        public override ISet<string> GetGen(BasicBlock block)
        {
            /* Gen == Use */
            DefUseBlockCalculator DefUseCalc = new DefUseBlockCalculator();
            return DefUseCalc.GetDefUseSetsByBlock(block).Item2;
        }
        public override ISet<string> GetKill(BasicBlock block)
        {
            /* Kill = Def */
            DefUseBlockCalculator DefUseCalc = new DefUseBlockCalculator();
            return DefUseCalc.GetDefUseSetsByBlock(block).Item1;
        }
        public override ISet<string> TransferFunction(ISet<string> input, BasicBlock block)
        {
            /* useB U (input \ defB) */
            return SetFactory.GetSet(GetGen(block).Union(input.Except(GetKill(block))));
        }
        /* Направление вверх */
        public override bool ForwardDirection { get { return false; } } 
        /* OUT[Выход] := пустое множество */
        public override ISet<string> FirstValue { get { return SetFactory.GetSet<string>(); } }
        /* Начальное приближение - пустое множество */
        public override ISet<string> StartingValue { get { return SetFactory.GetSet<string>(); } }

### Пример использования
```
SyntaxNode root = ParserWrap.Parse(text);
Graph graph = new Graph(BasicBlocksGenerator.CreateBasicBlocks(ThreeAddressCodeGenerator.CreateAndVisit(root).Program));
var deadAliveVars = IterativeAlgorithm.Apply(graph, new DeadAliveIterativeAlgorithmParameters());
```
### Тест
```
   a = 2;
   b = 3;
1: c = a + b;
2: a = 3; 
   b = 4;
3: c = a;
```
```
OUT[0] = {a, b}
OUT[1] = {}
OUT[2] = {a}
OUT[3] = {}
```
