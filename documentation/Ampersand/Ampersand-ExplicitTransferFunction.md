# Построение множеств gen B и kill B (на основе передаточной функции по явным формулам)

### Выполнено командой:
*Ampersand (Золотарёв Федор, Маросеев Олег)*

### От каких проектов зависит:
1. Базовые блоки и алгоритм получения

### Зависимые проекты:
1. Формирование последовательности областей в восходящем порядке
2. Разработка класса для хранения передаточных функций для алгоритма анализа на основе областей
3. Восходящая часть алгоритма анализа на основе областей
4. Нисходящая часть алгоритма анализа на основе областей

### Постановка задачи:
В данной задаче требуется разработать класс, который реализует функции построения множеств Gen и Kill по входному графу потока. Вычисления множеств осуществляются на основе передаточной функции по явным формулам.

### Теория
В процессе анализа на основе областей программа рассматривается как иерархия  которые грубо можно считать частями графа потока, имеющими единственную точку входа.
Каждая инструкция в блочно структурированной программе является областью, поскольку поток управления может достичь инструкции только через ее начало. Каждый уровень вложенности
инструкций соответствует уровню в иерархии областей.

Формально область графа потока представляет собой набор узлов *N* и ребер *E*, таких, что
1. Существует заголовок *h <html>&#8712;<html> N*, доминирующий над всеми узлами в *N*;
2. Если некоторый узел *m* может достичь узла *n <html>&#8712;<html> N*, минуя *h*, то *m* также входит в *N*;
3. *E* - множество всех ребер потока управления между узлами *n1* и *n2* из *N*, за исключением, возможно, некоторых ребер, входящих в *h*.
	
*Тело* цикла *L* (все узлы и ребра, за исключением обратных ребер к заголовку) замещаем узлом, представляющим область *R*. Ребра, ведущие к заголовку *L*, входят теперь в узел
R. Ребро из любого выхода из *L* замещается ребром от *R* в то же самое место назначения. Однако если ребро является обратным, то оно становится петлей у *R*.
Назовем *R областью тела*.

Единственное отличие области тела от области цикла заключается в том, что последняя включает обратные ребра к заголовку цикла L. 

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

```
