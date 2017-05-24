# Построение множества gen S и kill S для одной команды

### Выполнено командой:
*NoName (Скиба Кирилл, Борисов Сергей)*

### От каких проектов зависит:
1. Граф управления потока

### Зависимые проекты:
1. Вычисление передаточной функции, как композиции передаточных функций для одной команды
2. Множества gen B и kill B(на основе передаточной функции по явным формулам)

### Постановка задачи:
Необходимо реализовать алгоритм, вычисляющий множества *gen* и *kill* для одной команды

### Теория
Множеством *gen S* называется множество определений, генерируемых в базовом блоке *B*. Множеством *kill S* называется множество всех прочих определений той же переменной во всей остальной программе.

Эти множества входят в передаточную функцию для задачи о достигающих определениях, поэтому перед запуском итерационного алгоритма для нее необходимо вычислить эти множества для каждого блока,
а для этого нужно вычислить эти множества для каждой команды в этом блоке.

### Входные данные
- Базовый блок в котором содержится команда
- ID команды

### Выходные данные
- Множество gen
- Множество kill


### Используемые структуры данных
- `CommandNumber Gen` - множество gen (так как команда одна, то это номер этой команды)
- `ISet<CommandNumber>` - множество kill
- `Dictionary<string, ISet<CommandNumber>>` - хранилище команд
- `Graph graph` - граф потока управления


### Реализация

В реализации данного модуля был использован пакет NuGet QuickGraph.

```cs
// класс содержащий множества gen и kill.
public class GenKillOneCommand
{
    public CommandNumber Gen { get; set; }

    public ISet<CommandNumber> Kill { get; set; }

    public GenKillOneCommand(CommandNumber gen, ISet<CommandNumber> kill)
    {
        Gen = gen;
        Kill = kill;
    }
}

// Алгоритм построения множеств gen и kill
public class GenKillOneCommandCalculator
{
    private CommandNumber Gen;
    private ISet<CommandNumber> Kill;
    private Dictionary<string, ISet<CommandNumber>> CommandStorage;
    private Graph graph;

    public GenKillOneCommandCalculator(Graph g)
    {
        graph = g;
        CommandStorage = new Dictionary<string, ISet<CommandNumber>>();
        foreach (BasicBlock block in graph)
            for (int i = 0; i < block.Commands.Count(); i++)
            {
                var command = block.Commands[i];
                if (command.GetType() == typeof(Assignment))
                    if (CommandStorage.ContainsKey((command as Assignment).Target.Name))
                        CommandStorage[(command as Assignment).Target.Name].Add(new CommandNumber(block.BlockId, i));
                    else
                        CommandStorage.Add((command as Assignment).Target.Name,
                            SetFactory.GetSet(new CommandNumber(block.BlockId, i)));
            }
        Kill = SetFactory.GetSet<CommandNumber>();
    }

    public GenKillOneCommand CalculateGenAndKill(BasicBlock block, ThreeAddressCommand command)
    {
        Kill.Clear();
        if (command.GetType() == typeof(Assignment))
        {
            Gen = new CommandNumber(block.BlockId, block.Commands.IndexOf(command));
            string target = (command as Assignment).Target.Name;
            foreach (var c in CommandStorage[target])
                if (c.BlockId != block.BlockId && c.CommandId != block.Commands.IndexOf(command))
                    Kill.Add(new CommandNumber(c.BlockId, c.CommandId));
        }
        return new GenKillOneCommand(Gen, Kill);
    }

    public GenKillOneCommand CalculateGenAndKill(int blockId, int commandId)
    {
        var block = graph.getBlockById(blockId);
        return CalculateGenAndKill(block, block.Commands[commandId]);
    }

    public GenKillOneCommand CalculateGenAndKill(BasicBlock block, int commandId)
    {
        return CalculateGenAndKill(block, block.Commands[commandId]);
    }
}
```

### Пример использования
```cs
calculator = new GenKillOneCommandCalculator(g);
GenKillOneCommand genKill = calculator.CalculateGenAndKill(block, commandNumber);
```
