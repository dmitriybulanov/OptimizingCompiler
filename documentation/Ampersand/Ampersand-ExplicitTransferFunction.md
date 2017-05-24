# Построение множеств gen B и kill B (на основе передаточной функции по явным формулам)

### Выполнено командой:
*Ampersand (Золотарёв Федор, Маросеев Олег)*

### От каких проектов зависит:
1. Множества gen S и kill S для одной команды

### Зависимые проекты:
1. Итерационный алгоритм
2. Достигающие определения

### Постановка задачи:
В данной задаче требуется разработать класс, который реализует функции построения множеств Gen и Kill по входному графу потока. Вычисления множеств осуществляются на основе передаточной функции по явным формулам.

### Теория
Множество gen(B) - множество операций в блоке B. Множество kill(B) - множество остальных определений этих переменных в программе.

Формулы для построения множеств:
Gen(B) = gen[n] + ( gen[n-1] - kill[n] ) + ( gen[n-2] - kill[n-1] - kill[n]) + ... + (gen[1] - kill[2] - ... - kill[n])
Kill(B) = kill[1] + ... + kill[n]


### Используемые структуры данных
- `List<int> OutputBlocks` - номера выходных базовых блоков

### Реализация

```cs
class ExplicitTransferFunction : SetIterativeAlgorithmParameters<CommandNumber>
    {
        private GenKillOneCommandCalculator commandCalc;

        public ExplicitTransferFunction(Graph g)
        {
            commandCalc = new GenKillOneCommandCalculator(g);
        }

        public override ISet<CommandNumber> GatherOperation(IEnumerable<ISet<CommandNumber>> blocks)
        {
            ISet<CommandNumber> res = SetFactory.GetSet<CommandNumber>();
            foreach (var command in blocks)
                res.UnionWith(command);
            return res;
        }

        public override ISet<CommandNumber> GetGen(BasicBlock block)
        {
            //list of kill-sets for every command in block
            List<ISet<CommandNumber>> listKill = new List<ISet<CommandNumber>>(block.Commands.Count);
            foreach (var command in block.Commands)
                listKill.Add(commandCalc.CalculateGenAndKill(block, command).Kill);

            ISet<CommandNumber> genB = SetFactory.GetSet<CommandNumber>();

            for (int i = block.Commands.Count - 1; i >= 0; i--)
            {
                ISet<CommandNumber> genS = SetFactory.GetSet<CommandNumber>();
                genS.Add(commandCalc.CalculateGenAndKill(block, block.Commands[i]).Gen);
                for (int j = i; j < block.Commands.Count - 1; j++)
                {
                    genS.IntersectWith(listKill[i]);
                }
                genB.UnionWith(genS);
            }

            return genB;
        }

        public override ISet<CommandNumber> GetKill(BasicBlock block)
        {
            ISet<CommandNumber> killB = SetFactory.GetSet<CommandNumber>();
            foreach (var command in block.Commands)
                killB.UnionWith(commandCalc.CalculateGenAndKill(block, command).Kill);
            return killB;
        }
    }
```

### Пример использования

```cs
 public override ISet<Expression> TransferFunction(ISet<Expression> input, BasicBlock block)
        {
            var kill = GetKill(block).Cast<Identifier>();
            var result = SetFactory.GetSet(
                input.Where(inputExpression =>
                !kill.Any(inputExpression.HasIdentifiedSubexpression)));
            result.UnionWith(GetGen(block));

            return result;
            /*
             * Non-LINQ realization
            var difference = SetFactory.GetSet();
            var foundInKill = false;
            foreach (var inputExpression in input)
            {
                foreach (var killExpression in kill)
                {
                    if (!inputExpression.HasIdentifiedSubexpression(killExpression as Identifier))
                        continue;
                    foundInKill = true;
                    break;
                }
                if (!foundInKill)
                    difference.Add(inputExpression);
                foundInKill = false;
            }
            return SetFactory.GetSet(GetGen(block).Union(difference));
            */
        }
```
