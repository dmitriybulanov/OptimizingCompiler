# Построение графа потока управления CFG (Control Flow Graph)

### Выполнено командой:
*PiedPiper (Бергер Анна, Колесников Сергей)*

### От каких проектов зависит:
1. Базовые блоки 

### Зависимые проекты:
1. Построение трехадресного кода по графу потока управления
2. Итерационный алгоритм (и вспомогательные алгоритмы для его работы)
3. Построение дерева доминаторов

### Теория

**Граф потока управления** -- множество всех возможных путей исполнения программы, представленное в виде графа. В графе потока управления каждый узел  графа соответствует базовому блоку. Ребро из блока В в блок С идет тогда и только тогда, когда первая команда блока С может следовать непосредственно за последней командой блока В. Тогда говорим, что В -- предшественник С, а С -- преемник В.


### Входные данные:
 - Список базовых блоков (BasicBlocksList)

### Выходные данные:
 - Граф потока управления (Graph)

 ### Используемые структуры данных
 - BidirectionalGraph<BasicBlock, Edge<BasicBlock>> - граф потока управления (структура данных из пакета QuickGraph)
 - Dictionary<int, BasicBlock> blockMap - соответствие между узлами графа и уникальными идентификаторами базовых блоков (blockId)

 ### Реализация алгоритма

 ``` C#
 // построение графа потока управления по списку базовых блоков
public Graph(BasicBlocksList listBlocks)
{
	CFG.AddVertexRange(listBlocks.Blocks);

	foreach (BasicBlock block in listBlocks.Blocks)
	{
		blockMap.Add(block.BlockId, block);
	}

	foreach (var block in listBlocks.Blocks)
	{
		foreach (var numIn in block.InputBlocks)
		{
			CFG.AddEdge(new Edge<BasicBlock>(this.getBlockById(numIn), block));
		}
	}
	...
}
 ``` 
 Для удобства использования графа в нем реализованы следующие методы:
 - public BasicBlock getBlockById(int id) -- по идентификатору базового блока возвращается базовый блок - узел графа
 - public BasicBlocksList getChildren(int id) -- по идентификатору базового блока возвращается список базовых блоков-преемников
 - public BasicBlocksList getParents(int id) -- по идентификатору базового блока возвращается список базовых блоков-предшественников
 - BasicBlock getRoot() -- точка входа в программу
 - public int GetCount() -- количество вершин в графе
 - public IEnumerable<Edge<BasicBlock>> GetEdges() -- список ребер в графе
 - public IEnumerable<BasicBlock> GetVertices() -- список вершин в графе
 - public bool Contains(BasicBlock block) -- проверка, содержится ли базовый блок в графе
 - public bool IsAncestor(int id1, int id2) -- проверка является ли блок с blockId = id1


### Пример использования
```
...
var b = BasicBlocksGenerator.CreateBasicBlocks(threeAddressCode);
Graph g = new Graph(b);
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

Граф потока управления
```
0:
<-- 
-->  1 2
1:
<--  0
-->  3
2:
<--  0
-->  3
3:
<-- 1 2
--> 
```
