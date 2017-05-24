# Базовые блоки и алгоритм их получения.

### Выполнено командой:
*EndFrame (Аммаев Саид,  Пирумян Маргарита)*

### Постановка задачи:
Необходимо реализовать класс Базовый блок(ББЛ).
Необходимо реализовать класс Список Базовых блоков.
Необходимо реализовать алгоритм построения списка ББЛ по имеющемуся списку трехадресных команд.

### От каких проектов зависит:
1. Трехадресного кода 

### Зависимые проекты:
1. Граф управления потока

### Теория
Базовый блок(ББЛ) - максимальная последовательность, идущих друг за другом команд, удовлетворяющий мледующим свойствам:
1. Поток управления может входить в блок только через первую команду;
2. Управление покидает блок без останова или ветвления за исключением, возможно, последней команды.

Алгоритм нахождения:
1. Определить команды-лидеры:
        - первая команда программы;
        - любая команда, на которую есть переход;
        - любая команда, следующая за командой перехода;
2. Сформировать ББЛ - набор команд от лидера до лидера.(первый включается, второй нет)

### Входные данные:
 - Объект класса Program из ThreeAddressCode.Model, в котором хранится список трехадресных команд (program)

### Выходные данные:
 - Список базовых блоков (BasicBlocksList)

 ### Используемые структуры данных:
 - BasicBlock - класс ББЛ (созданный нами класс)
 - BasicBlocksList - класс списка ББЛ (созданный нами класс)
 
 - BasicBlocksList basicBlocks - список ББЛ
 - List<ThreeAddressCommand> commands - список трехадресных команд (ThreeAddressCommand структура из ThreeAddressCode.Model)
 - Dictionary<string, int> labels - словарь соответствия команды в трехадресном коде и метки, присвоенной ей
 - List<int> firstCommandsOfBlocks  - список для номеров комманд, являющимися первыми в ББЛ
 - List<int> lastCommandsOfBlocks - список для номеров комманд, являющимися последними в ББЛ
 - int[] BlockByFirstCommand - соответствие номера блока номеру команды в трехадресном коде, которая является первой командой в ББЛ 
 - int[] BlockByLastCommand - соответствие номера блока номеру команды в трехадресном коде, которая является последней командой в ББЛ
 - List<int>[] PreviousBlocksByBlockID - список номеров блоков, входящих в каждый ББЛ
 - List<int>[] NextBlocksByBlockID - списко номеров блоков, исходящих из каждого ББЛ
            
 ### Реализация алгоритма:

 ``` C#
 // построение списка ББЛ по списку трхадресных команд
public static BasicBlocksList CreateBasicBlocks(Program program)
        {
            BasicBlocksList basicBlocks = new BasicBlocksList();
            List<ThreeAddressCommand> commands = program.Commands;
            Dictionary<string, int> labels = new Dictionary<string, int>();
            List<int> firstCommandsOfBlocks = new List<int>(); 
            List<int> lastCommandsOfBlocks = new List<int>();

            //поиск команд-лидеров и присваивание меток всем коммандам(labels) 
            
            //первая комманда в программе
            firstCommandsOfBlocks.Add(0);
            for (int i = 0; i < commands.Count; ++i)
            {
                ThreeAddressCommand currentCommand = commands[i];
                //если есть метка в трехадресном коде
                if (currentCommand.Label != null)
                {
                    labels[currentCommand.Label] = i;
                    //если не первая команда в программе
                    if (i > 0)
                    {
                        //команда с меткой
                        firstCommandsOfBlocks.Add(i);
                        //команда предшествующая команде с меткой
                        lastCommandsOfBlocks.Add(i - 1);
                    }
                }
                //если есть переход
                if (currentCommand is Goto && i < commands.Count - 1)
                {
                    //следующая команда за командой перехода
                    firstCommandsOfBlocks.Add(i + 1);
                    //сама команда перехода
                    lastCommandsOfBlocks.Add(i);
                }
            }
            lastCommandsOfBlocks.Add(commands.Count - 1);
            firstCommandsOfBlocks = firstCommandsOfBlocks.Distinct().ToList();
            lastCommandsOfBlocks = lastCommandsOfBlocks.Distinct().ToList();

            int[] BlockByFirstCommand = new int[commands.Count];
            int[] BlockByLastCommand = new int[commands.Count];
            for (int i = 0; i < firstCommandsOfBlocks.Count; ++i)
                BlockByFirstCommand[firstCommandsOfBlocks[i]] = i;
            for (int i = 0; i < lastCommandsOfBlocks.Count; ++i)
                BlockByLastCommand[lastCommandsOfBlocks[i]] = i;

            List<int>[] PreviousBlocksByBlockID = new List<int>[commands.Count];
            List<int>[] NextBlocksByBlockID = new List<int>[commands.Count];
            for (int i = 0; i < commands.Count; ++i)
            {
                PreviousBlocksByBlockID[i] = new List<int>();
                NextBlocksByBlockID[i] = new List<int>();
            }

            //формирование списков номеров блоков, входящих и исходящих из каждого ББЛ
            foreach (var currentNumOfLastCommand in lastCommandsOfBlocks)
            {
                ThreeAddressCommand currentCommand = commands[currentNumOfLastCommand];
                int numOfCurrentBlock = BlockByLastCommand[currentNumOfLastCommand];
                if ((currentCommand.GetType() != typeof(Goto)) && (currentNumOfLastCommand < commands.Count - 1))
                {
                    int numOfNextBlock = numOfCurrentBlock + 1;
                    NextBlocksByBlockID[numOfCurrentBlock].Add(numOfNextBlock);
                    PreviousBlocksByBlockID[numOfNextBlock].Add(numOfCurrentBlock);
                }
                if (currentCommand is Goto)
                {
                    int numOfNextBlock = BlockByFirstCommand[labels[(currentCommand as Goto).GotoLabel]];
                    NextBlocksByBlockID[numOfCurrentBlock].Add(numOfNextBlock);
                    PreviousBlocksByBlockID[numOfNextBlock].Add(numOfCurrentBlock);
                }
            }
            
            //добавление команд в каждый ББЛ
            for (int i = 0; i < firstCommandsOfBlocks.Count; ++i)
            {
                BasicBlock block = new BasicBlock();
                block.Commands = new List<ThreeAddressCommand>(commands.Take(lastCommandsOfBlocks[i] + 1).Skip(firstCommandsOfBlocks[i]).ToList());
                basicBlocks.Blocks.Add(block);
                basicBlocks.BlockByID[block.BlockId] = block;
            }

            //добавление полученных списков входящих и исходящих ББЛ в каждый блок
            for (int i = 0; i < basicBlocks.Count(); ++i)
            {
                for (int j = 0; j < PreviousBlocksByBlockID[i].Count; ++j)
                    PreviousBlocksByBlockID[i][j] = basicBlocks.Blocks[PreviousBlocksByBlockID[i][j]].BlockId;
                for (int j = 0; j < NextBlocksByBlockID[i].Count; ++j)
                    NextBlocksByBlockID[i][j] = basicBlocks.Blocks[NextBlocksByBlockID[i][j]].BlockId;
                basicBlocks.Blocks[i].InputBlocks.AddRange(PreviousBlocksByBlockID[i]);
                basicBlocks.Blocks[i].OutputBlocks.AddRange(NextBlocksByBlockID[i]);
            }

            return basicBlocks;
        }	
        ...
}
 ``` 

### Пример использования
```
...
var b = BasicBlocksGenerator.CreateBasicBlocks(threeAddressCode);
...
```
### Тест
Программа
```cs 
b = 1;
if 1
  b = 3;
else
  b = 2;
```

Список базовых блоков 
```
BlockId = 0
Commands:
   b = 1
   goto $GL_2 if 1 == 0
InputBlocksNumbers = {}
OutputBlocksNumbers = {1; 2}
BlockId = 1
Commands:
   b = 3
   goto $GL_1
InputBlocksNumbers = {0}
OutputBlocksNumbers = {3}
BlockId = 2
Commands:
   $GL_2: <no-op>
   b = 2
InputBlocksNumbers = {0}
OutputBlocksNumbers = {3}
BlockId = 3
Commands:
   $GL_1: <no-op>
InputBlocksNumbers = {1; 2}
OutputBlocksNumbers = {}
```

