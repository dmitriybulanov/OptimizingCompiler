# Построение графа потока управления CFG (Control Flow Graph)

### Выполнено командой:
*PiedPiper (Бергер Анна, Колесников Сергей)*

### От каких проектов зависит:
1. Базовые блоки 

### Зависимые проекты:
1. Алгоритм классификации ребер графа

### Теория

** Граф потока управления** -- множество всех возможных путей исполнения программы, представленное в виде графа. В графе потока управления каждый узел  графа соответствует базовому блоку. Ребро из блока В в блок С идет тогда и только тогда, когда первая команда блока С может следовать непосредственно за последней командой блока В.


### Входные данные:
 - Список базовых блоков (BasicBlocksList)

### Выходные данные:
 - Граф потока управления (Graph)

 ### Используемые структуры данных
 - BidirectionalGraph<BasicBlock, Edge<BasicBlock>> - граф потока управления (структура данных из пакета QuickGraph)
 - Dictionary<int, BasicBlock> blockMap - соответствие между узлами графа и уникальными идентификаторами базовых блоков

 ### Реализация алгоритма

 ``` C#
 // конструктор графа потока управления
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

	spanTree.AddVertexRange(listBlocks.Blocks);
	var visited = new Dictionary<BasicBlock, bool>();

	foreach (var v in spanTree.Vertices)
	{
		visited[v] = false;
	}
	int c = spanTree.Vertices.Count();

    dfs(blockMap[blockMap.Keys.First()], visited, ref c);
}
 ``` 
 Для удобства использования графа в нем реализованы следующие методы:
 - public BasicBlock getBlockById(int id) -- по идентификатору базового блока возвращается базовый блок - узел графа
 - 


### Пример использования