# Название задачи: создание парсера языка программирования

### Выполнено командой: YACT

### Постановка задачи: 

Необходимо создать парсер языка программирования, используемого для реализации и демонстрации различных оптимизаций. В качестве генератора парсера был использован GPPG. Он генерирует bottom-up парсер, распознающий LALR(1) языки с традиционной системой устранения неоднозначности yacc. 

Синтасические конструкции языка программирования зафиксированы в документе [language-syntax.md](https://github.com/wisestump/OptimizingCompiler/blob/master/docs/language-syntax.md)

### От каких проектов зависит:

  - GPPG

### Зависимые проекты:

  - Алгоритмы анализа потоков данных

### Теория

В данном разделе рассматриваются две составляющие алгоритма парсинга: лексер и парсер.

#### Лексер

Лексер предназначен для разбиения входного потока символов на *лексемы* - отдельные, осмысленные единицы программы. В данной реализации лексер выполняет следующие функции:

- Выделение идентификаторов и целых чисел
- Выделение символьных токенов (>, <=, & и т.п.)
- Выделение ключевых слов
- Формирование текста синтаксической ошибки

Исходный код лексера назодится в файле [SimpleLex.lex](https://github.com/wisestump/OptimizingCompiler/blob/master/src/gppgparser/SimpleLex.lex)

#### Парсер

Парсер принимает на вход поток лексем и формирует *абстрактное синтаксическое дерево* (AST). В данной реализации были использованы следующие типы для термов:

- *SyntaxNode* в качестве базового узла синтаксического дерева
- *Statement* - для узлов, представляющих оператор
- *Expression* - для узлов, представляющих выражение
- *String* - для метки оператора

Исходный код парсера находится в файле [SimpleYacc.y](https://github.com/wisestump/OptimizingCompiler/blob/master/src/gppgparser/SimpleYacc.y)
### Входные данные:
 - Текст программы на языке, описаном в [language-syntax.md](https://github.com/wisestump/OptimizingCompiler/blob/master/docs/language-syntax.md)

### Выходные данные:
 - Синтаксическое дерево 

### Используемые структуры данных

- `List<T>` - для хранения списка потомков узла синтаксического дерева

### Реализация алгоритма

Формирование синтаксического дерева происходит в действиях, совершаемых парсером в файле [SimpleYacc.y](https://github.com/wisestump/OptimizingCompiler/blob/master/src/gppgparser/SimpleYacc.y). Например, формирование списка операторов выглядит следующим образом:
``` 
stlist	: statement
	{ 
		$$ = new StatementList($1); 
	}
	| stlist statement
	{
		$$ = ($1 as StatementList).Add($2); 
	}
	;
```

Иерархия синтаксических узлов:
![SyntaxTypesGraph.png](https://github.com/wisestump/OptimizingCompiler/raw/master/documentation/YACT/img/SyntaxTypesGraph.png "Syntax Types")

### Пример использования

``` C#
string text = @"
a = 40;
b = a + 2;
print(b);";
SyntaxNode root = ParserWrap.Parse(text);
Console.WriteLine(root == null ? "Ошибка" : "Программа распознана");
if (root != null)
{
    var prettyPrintedProgram = PrettyPrinter.CreateAndVisit(root).FormattedCode;
    Console.WriteLine(prettyPrintedProgram);
}
```
Вывод:

```
a = 40;
b = a + 2;
print(b);
```

### Тест
``` C#
string text = @"
a = 40;
b = a + 2;
print(b);";
SyntaxNode root = ParserWrap.Parse(text);

var statements = new StatementList();
statements.Add(new AssignmentStatement(new Identifier("a"), new Int32Const(40)));
statements.Add(new AssignmentStatement(
    new Identifier("b"),
    new BinaryExpression(
        new Identifier("a"),
        Operation.Add,
        new Int32Const(2))));
statements.Add(new PrintStatement(new Identifier("b"), false));
Assert.AreEqual(root, new Program(statements));
```
