��������
�������� ������ ����������������
	�������� ������� ����� ���������������� (Parser-YACT)
�����������
	� �������� �������� �����
		��������� ������������� ���� (ThreeAddressCode-YACT)
		Def-Use ���������� � ���������� (DefUseCalculation-PiedPiper)
		���������� �������� ���� (IterativeAlgorithmParameterForDeadAliveVariables-EndFrame)
		������������ ������� ��� ������ ��������������� �������� (TransferFuctionForPropagationParameters-EndFrame)
		�������� �������� (ConstantPropagation-Ampersand)
		��������� ������� ������ (BasicBlock-EndFrame)
	���������� ����� ������ ���������� (ControlFlowGraph-PiedPiper)
		���������� ������������� ���� �� CFG (CreateCodeCFG-YetYetAnotherTeam)
	������������ ���������
		����������� �����������
			���������� Gen � Kill ��� ����� ������� (GenKillForOneCommandDocumentation-NoName)
			���������� �������� Gen � Kill (ExplicitTransferFunction-Ampersand)
			������������ ������� (TransferFunction-GoogleDogs)
		��������� ���������
			������������ �������� ��� ��������� ��������� (AvailableExpressionsIterativeAlgorithm-GoogleDogs)
			��������� Gen � Kill ��� ��������� ��������� (GenAndKillSetsForAvailableExpressions-YACT)
		��������������� �������� (ConstantPropagation-Ampersand)
		������������ �������� (IterativeAlgorithm-Ampersand)
		��������� �������������
			���������� ������ ����������� (BuildingDominationTree-NoName)
	������ ����� ��� ��������� ��������� ��������
		���������� ��������� ������ (DFN-PiedPiper)
		������������� ����� (EdgeClassification-GoogleDogs)
		���������� �������� ����� � CFG (FindReverseEdges-EndFrame)
		�������� ����������� ����� = �������� ����� � CFG (CheckReverseEdges-Ampersand)
		����������� ���� ������������ ������ (SearchNaturalLoops-GoogleDogs)
	�������� ��������� ��������
		��������� �������� (RegionClassDocumentation-NoName)
		���������� ����� ��������� ������� �� ������ �������� (RegionsAlgorithmAscendingPart-Ampersand)
		������������ ���������� ������������������ �������� (RegionSequence-PiedPiper)
		���������� ����� ��������� ������� �� ������ �������� (DescendingPartOfIterativeAlgoritmByRegionsAlgorithm-EndFrame)
		�������� ������������ ������� (TransferFunctionStorage-YACT)
		MeetOverAllPaths (MeetOverPaths-YACT)

# 01: �������� ������� ����� ����������������

### ��������� ��������: YACT

### ���������� ������: 

���������� ������� ������ ����� ����������������, ������������� ��� ���������� � ������������ ��������� �����������. � �������� ���������� ������� ��� ����������� GPPG. �� ���������� bottom-up ������, ������������ LALR(1) ����� � ������������ �������� ���������� ��������������� yacc. 

������������� ����������� ����� ���������������� ������������� � ��������� [language-syntax.md](https://github.com/wisestump/OptimizingCompiler/blob/master/docs/language-syntax.md)

### �� ����� �������� �������:

  - GPPG

### ��������� �������:

  - ��������� ������� ������� ������

### ������

� ������ ������� ��������������� ��� ������������ ��������� ��������: ������ � ������.

#### ������

������ ������������ ��� ��������� �������� ������ �������� �� *�������* - ���������, ����������� ������� ���������. � ������ ���������� ������ ��������� ��������� �������:

- ��������� ��������������� � ����� �����
- ��������� ���������� ������� (>, <=, & � �.�.)
- ��������� �������� ����
- ������������ ������ �������������� ������

�������� ��� ������� ��������� � ����� [SimpleLex.lex](https://github.com/wisestump/OptimizingCompiler/blob/master/src/gppgparser/SimpleLex.lex)

#### ������

������ ��������� �� ���� ����� ������ � ��������� *����������� �������������� ������* (AST). � ������ ���������� ���� ������������ ��������� ���� ��� ������:

- *SyntaxNode* � �������� �������� ���� ��������������� ������
- *Statement* - ��� �����, �������������� ��������
- *Expression* - ��� �����, �������������� ���������
- *String* - ��� ����� ���������

�������� ��� ������� ��������� � ����� [SimpleYacc.y](https://github.com/wisestump/OptimizingCompiler/blob/master/src/gppgparser/SimpleYacc.y)
### ������� ������:
 - ����� ��������� �� �����, �������� � [language-syntax.md](https://github.com/wisestump/OptimizingCompiler/blob/master/docs/language-syntax.md)

### �������� ������:
 - �������������� ������ 

### ������������ ��������� ������

- `List<T>` - ��� �������� ������ �������� ���� ��������������� ������

### ���������� ���������

������������ ��������������� ������ ���������� � ���������, ����������� �������� � ����� [SimpleYacc.y](https://github.com/wisestump/OptimizingCompiler/blob/master/src/gppgparser/SimpleYacc.y). ��������, ������������ ������ ���������� �������� ��������� �������:
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

�������� �������������� �����:
![SyntaxTypesGraph.png](https://github.com/wisestump/OptimizingCompiler/raw/master/documentation/YACT/img/SyntaxTypesGraph.png "Syntax Types")

### ������ �������������

``` C#
string text = @"
a = 40;
b = a + 2;
print(b);";
SyntaxNode root = ParserWrap.Parse(text);
Console.WriteLine(root == null ? "������" : "��������� ����������");
if (root != null)
{
    var prettyPrintedProgram = PrettyPrinter.CreateAndVisit(root).FormattedCode;
    Console.WriteLine(prettyPrintedProgram);
}
```
�����:

```
a = 40;
b = a + 2;
print(b);
```

### ����
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

# 02: ��������� ������������� ���� �� ��������������� ������

### ��������� ��������: YACT

### ���������� ������: 

���������� ���������� ��������� ������������� ���� �� ��������������� ������. ������������ ��� ������� �� ���� ����� ����������:

- `x = y op z`
- `x = y`
- `x = op z`
- `goto L`
- `if x goto  L`
- `no-op`
- `print`

### �� ����� �������� �������:

  - �������������� ������

### ��������� �������:

  - ������� �����

### ������

������� �������������� ����������� � ������������ ��� ����� ���� �������� ��������� �������:

- �������� ��������� ����������� � ���������, ������������ ���������� (`t0`, `t1`, ...)
- ������������ ������� ��������� ����������� ��������
- ������������ �������� ��������� ������������� � ������������ ������� � �������������� ������ �������������� �������� ���������
- ����� � �������� �������� ����� ���� ������������� � �������������� ��������/����������� ���������� �������� � ������ �������� � �������

### ������� ������:
 - �������������� ������

### �������� ������:
 - ������ ������ ������������� ����

### ������������ ��������� ������

- `List<T>` - ��� �������� ������ ������
- `Stack<T>` - ��� �������� ��������� ��� �������������� �������� ���������

### ���������� ���������

��������� ������������� ���� ��� �������� � �������������� �������� *�������*. ��� ������� ���� ��������������� ������ ���� ���������� ������ ��� ��������� � � ��� �������������� ������������ ������������ ������.

�������� ��� ���������� ���������� � [ThreeAddressCodeGenerator.cs](https://github.com/wisestump/OptimizingCompiler/blob/master/src/DataFlowAnalysis/IntermediateRepresentation/ThreeAddressCode/ThreeAddressCodeGenerator.cs)

�������� ������������ ������
![ThreeAddrCode.png](https://github.com/wisestump/OptimizingCompiler/raw/master/documentation/YACT/img/ThreeAddressCodeTypeGraph.png "Three-Address Code Hierarcy")



### ������ �������������

``` C#
text = @"
a = 1;
b = 1;
c = a + b;
for i = 1 .. 3
    c = c - 1;
";
SyntaxNode root = ParserWrap.Parse(text);
if (root != null)
{
    var tac = ThreeAddressCodeGenerator.CreateAndVisit(root);
    Console.WriteLine(string.Join(Environment.NewLine, tac.Program.Commands.Select(x => x.ToString())));
}
```

�����:
```
a = 1
b = 1
t0 = a + b
c = t0
i = 1
$GL_1: goto $GL_2 if i > 3
t1 = c - 1
c = t1
i = i + 1
goto $GL_1
$GL_2: <no-op>
```

### ����
``` C#
string text = @"
a = 2;
while a > 1
a = a - 1;";
SyntaxTree.SyntaxNodes.SyntaxNode root = ParserWrap.Parse(text);
var threeAddressCode = ThreeAddressCodeGenerator.CreateAndVisit(root).Program;

var expectedCommands = new List<ThreeAddressCommand>
{
    new Assignment("a", new Int32Const(2)),
    new Assignment("t0", new BinaryOperation("a", Operation.Greater, 1)),
    new ConditionalGoto("$GL_2", new BinaryOperation("t0", Operation.Equal, 0)) { Label = "$GL_1" },
    new Assignment("t1", new BinaryOperation("a", Operation.Subtract, 1)),
    new Assignment("a", new Identifier("t1")),
    new Goto("$GL_1"),
    new NoOperation("$GL_2")
};

CollectionAssert.AreEqual(threeAddressCode.Commands, expectedCommands);
```

# 03: ���������� �������� Use(B) � Def(B) ��� �������� ����������

### ��������� ��������:
*PiedPiper (������ ����, ���������� ������)*

### �� ����� �������� �������:
1. ������� �����

### ��������� �������:
1. ������������ �������� ��� �������� ����������

### ������
� ����� p **x** �������� �������� ����������, ���� ���������� ����, ���������� ����� p, ������������ �������������, �������������
�� �������������� � �� ���� ���������� ��� ������ ������������ ���������� **x**.
 - Def(B) -- ��������� ����������, ������������ � ������� ����� �� ������ �� �������������. 
 
 ����� ������������ ����� ������� ����������� Def(B) -- ��������� ����������, ������������ � ������� �����. ��� ����� ��������� �����������
 ��������� ������ ������������� ��������� �� ���������.
 - Use(B) -- ��������� ����������, ������������ � ������� ����� �� ������ �� �����������.

### ������� ������:
 - ������� ���� (BasicBlock)

### �������� ������:
 - ���� �������� Use(B) � Def(B)
 
 ### ������������ ��������� ������
Dictionary<int, Tuple<ISet<string>, ISet<string>>> SetStorage - �������, �������������� ������ �������� ����� ��� ���� ��� �������� Use(B) � Def(B)

 ### ���������� ���������
 
 ����� ��������� �������� Use(B) � Def(B) �� �������� �����. 
 �� ��������� ���������� ��������� � SetStorage ��� ��������� ��������� ���������� ��� ��������� ������ ��� �����.
 ```C#
 public Tuple<ISet<string>, ISet<string>> GetDefUseSetsByBlock(BasicBlock block)
  {
    if (!SetStorage.Keys.Contains(block.BlockId))
    {
      SetStorage.Add(block.BlockId, CreateDefUseSets(block));
    }
    return SetStorage[block.BlockId];
  }
 ```
 �����, ���������� �� ������ ������ �������� ����� � ����������� ��������� Use(B) � Def(B). 
 ��������� Def(B) �����������, ���� ����������� ������� ���� Assignment. 
 ��� ���������� ��������� Use(B) ���������� ��������������� ������� ExpressionParser.
 ``` C#
 private Tuple<ISet<string>, ISet<string>> CreateDefUseSets(BasicBlock block)
  {
    ISet<string> Def = new HashSet<string>();
    ISet<string> Use = new HashSet<string>();

    foreach (var command in block.Commands)
    {
      if (command.GetType() == typeof(Assignment)) 
      {
        Def.Add(((Assignment)command).Target.Name);
        ExpressionParser(((Assignment)command).Value, Def, Use);
      }
      if (command.GetType() == typeof(ConditionalGoto))
      {
        ExpressionParser(((ConditionalGoto)command).Condition, Def, Use);
      }
      if (command.GetType() == typeof(Print))
      {
        ExpressionParser(((Print)command).Argument, Def, Use);
      }
    }
    return new Tuple<ISet<string>, ISet<string>>(Def, Use);
  }
 ```
 
 ����������� �������, ����������� ���������� ����� ��������� ���� Expression � ��������� ��������� Use(B)
 ``` C#
 private void ExpressionParser(Expression expr, ISet<string> Def, ISet<string> Use)
  {
    if (expr.GetType() == typeof(BinaryOperation))
    {
      ExpressionParser(((BinaryOperation)expr).Left, Def, Use);
      ExpressionParser(((BinaryOperation)expr).Right, Def, Use);
    }
    if (expr.GetType() == typeof(Identifier))
    {
      if (!Def.Contains(((Identifier)expr).Name))
      {
        Use.Add(((Identifier)expr).Name);
      }
    }
    if (expr.GetType() == typeof(UnaryOperation))
    {
      ExpressionParser(((UnaryOperation)expr).Operand, Def, Use);
    }
  }
 ``` 
 ### ������ �������������
 ```
 DefUseBlockCalculator DefUseCalc = new DefUseBlockCalculator();
 var UseDefTuple = DefUseCalc.GetDefUseSetsByBlock(block);
 var Def = UseDefTuple.Item1;
 var Use = UseDefTuple.Item2;
 ```
 
 ### ���� 
���������:
```
a = 4;
b = 4;
c = a + b;
if 1
    a = 3;
else
    b = 2;
print(c);
```
������� ���� BlockId = 0
```
Commands:
  a = 4
  b = 4
  t0 = a + b
  c = t0

Def(B) = {a, b, t0, c}
Use(B) = {}
```
������� ���� BlockId = 3
```
Commands:
  $GL_1: <no-op>
  print c

Def(B) = {}
Use(B) = {c}
```

# 04: Iterative Algorithm Parameter for DeadAliveVariables

### ��������� ��������: EndFrame (������ ����, ������� ���������)

### ���������� ������: 
���������� ����������� �����, ���������� ���������� ����������� 
������������� ���������. ����� ������ ������������� �� ������������ ������ BasicIterativeAlgorithmParameters<V>, 
� � �� ���������� ������ ���� ForwardDirection, ���������� �� ������� ������ ������, FirstValue, �������� �������� �������(����������)
�����, StartingValue, �������� ��������� ����������� ���� ������, � ����� ������ GatherOperation, ���������� ���������� �����, � 
TransferFunction, ���������� ������������ ��������.


### �� ����� �������� �������:

  - BasicIterativeAlgorithmParameters
  - SetIterativeAlgorithmParameters
  - BasicBlock
  - DefUseBlockCalculator

### ��������� �������:

  - IterativeAlgorithm

### ������

> ���������� x - �������� � ����� p, ���� ���������� ����, ���������� ����� p, ������������ ������������� � ���������������
�� ��������������, � �� ���� ���������� ��� ������ ������������ ���������� x.
defB - ��������� ����������, ������������ � B �� ������ �� �������������
useB - ��������� ����������, �������������� � B �� ������ �� �����������.
fB - ������������ ������� ��� ����� B.
fB(OUT[B]) = IN[B].
IN[B] = useB U (OUT[B] - defB).

### ������� ������:

### �������� ������:
 - DeadAliveIterativeAlgorithmParameters : SetIterativeAlgorithmParameters<string>

### ������������ ��������� ������

 - IEnumerable<ISet<string>> blocks - ��������� ����������
 - BasicBlock block - ������� ����
 - ISet<string> input - IN[B]

### ���������� ���������
```C#
    public class DeadAliveIterativeAlgorithmParameters : SetIterativeAlgorithmParameters<string>
    {
        public override ISet<string> GatherOperation(IEnumerable<ISet<string>> blocks)
        {
            ISet<string> union = SetFactory.GetSet((IEnumerable<string>)blocks.First());
            /* U(�� ���� ��������) IN[B] */
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
        /* ����������� ����� */
        public override bool ForwardDirection { get { return false; } } 
        /* OUT[�����] := ������ ��������� */
        public override ISet<string> FirstValue { get { return SetFactory.GetSet<string>(); } }
        /* ��������� ����������� - ������ ��������� */
        public override ISet<string> StartingValue { get { return SetFactory.GetSet<string>(); } }
```
### ������ �������������
```
SyntaxNode root = ParserWrap.Parse(text);
Graph graph = new Graph(BasicBlocksGenerator.CreateBasicBlocks(ThreeAddressCodeGenerator.CreateAndVisit(root).Program));
var deadAliveVars = IterativeAlgorithm.Apply(graph, new DeadAliveIterativeAlgorithmParameters());
```
### ����
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
# 05: ������������ ������� ��� ������ ��������������� ��������

### ��������� ��������:
*EndFrame (������ ����,  ������� ���������)*

### ���������� ������:
���������� ����������� ������� ��� ������ ��������������� ��������.

### �� ����� �������� �������:
1. ������������� ����
2. �������� �����

### ��������� �������:
1. ������������ ��������

### ������
```
����� ���������� ������������ ������� � �������� ������ ������ ���������� �����.

����� ������ - m
����� �������� ������ ������ - m'
�������� �������� ������ ������ �  ������� ���������� ������������ �������:
1. m1 ^ m2 = m <=> m1(v) ^ m2(v) = m(v), ��� ����� v
2. m1 < m2 <=> m1(v) < m2(v) , ��� ����� v
  fs(m) = m', ��� s - ������� ������������� ����
  2.1. ���� s - ��  ������������, �� m' = m
  2.2. ���� s - ������������, �� ��� ������ v != x : m'(v) = m(v)
3. ���� v = x
  3.1. x := c
      m'(x) = c
  3.2. x := y + z
      m'(x) = m(y) + m(z), ���� m(y) � m(z) - const
      m'(x) = NAC, ���� m(y) ��� m(z) - NAC
      m'(x) = UNDEF - � ��������� �������
  3.3. x := f(...), ��� f - �������� ������ �������
      m'(x) = NAC
```

### ������� ������:
 - Dictionary<string, string> input - �������� ������ ������
 - BasicBlock block - ������� ����(���)
 - int commandNumber - ����� ������� � ���

### �������� ������:
 - Dictionary<string, string> - ����� �������� ������ ������

 ### ���������� ���������:

 ``` C#
 /* ���������� ������������ ������� � ������
 public class ConstantsPropagationParameters : CompositionIterativeAlgorithmParameters<Dictionary<string, string>> */
 
 public override Dictionary<string, string> CommandTransferFunction(Dictionary<string, string> input, BasicBlock block, int commandNumber)
        {
            //��������� ������� �� ������ �� ���
            ThreeAddressCommand command = block.Commands[commandNumber];
            //���� ������������
            if (command.GetType() == typeof(Assignment))
            {
                string newValue = NAC;
                Expression expr = (command as Assignment).Value;
                if (expr.GetType() == typeof(Int32Const) || expr.GetType() == typeof(Identifier))
                    newValue = getConstantFromSimpleExpression(input, (expr as SimpleExpression));
                //���� ������� ��������
                else if (expr.GetType() == typeof(UnaryOperation))
                {
                    UnaryOperation operation = (expr as UnaryOperation);
                    newValue = calculateVal(getConstantFromSimpleExpression(input, operation.Operand), operation.Operation);
                }
                //���� �������� ��������
                else if (expr.GetType() == typeof(BinaryOperation))
                {
                    BinaryOperation operation = (expr as BinaryOperation);
                    newValue = calculateVal(getConstantFromSimpleExpression(input, operation.Left), getConstantFromSimpleExpression(input, operation.Right), operation.Operation);
                }
                string leftOperand = (command as Assignment).Target.Name;
                input[leftOperand] = newValue;
            }
            //���� �� ������������, �������� ������� ����� ������
            return input;
        }
        
        //��������������� �������, ������� ���������� ��������� �� �������� �������� ������ ������ � ���������
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

        ...
}
 ``` 
 
 //��������������� ������� calculateVal ����������� ������ ��������

### ������ �������������
```C#
   Dictionary<string, string> m = new Dictionary(/* ... */);
   BasicBlock block = new BasicBlock(/*...*/);
   Dictionary<string, string> m2 = CommandTransferFunction(m, block, 0)
```

### ����
���������
```cs 
x = y + z;
```

```
m: x = UNDEF; y = "2"; z = "3";
m': x = "5"; y = "2"; z = "3";
```

# 06: ������� ����� � �������� �� ���������.

### ��������� ��������:
*EndFrame (������ ����,  ������� ���������)*

### ���������� ������:
���������� ����������� ����� ������� ����(���).
���������� ����������� ����� ������ ������� ������.
���������� ����������� �������� ���������� ������ ��� �� ���������� ������ ������������ ������.

### �� ����� �������� �������:
1. ������������� ���� 

### ��������� �������:
1. ���� ���������� ������

### ������
������� ����(���) - ������������ ������������������, ������ ���� �� ������ ������, ��������������� ��������� ���������:
1. ����� ���������� ����� ������� � ���� ������ ����� ������ �������;
2. ���������� �������� ���� ��� �������� ��� ��������� �� �����������, ��������, ��������� �������.

�������� ����������:
1. ���������� �������-������:
        - ������ ������� ���������;
        - ����� �������, �� ������� ���� �������;
        - ����� �������, ��������� �� �������� ��������;
2. ������������ ��� - ����� ������ �� ������ �� ������.(������ ����������, ������ ���)

### ������� ������:
 - ������ ������ Program �� ThreeAddressCode.Model, � ������� �������� ������ ������������ ������ (program)

### �������� ������:
 - ������ ������� ������ (BasicBlocksList)

 ### ������������ ��������� ������:
 - BasicBlock - ����� ��� (��������� ���� �����)
 - BasicBlocksList - ����� ������ ��� (��������� ���� �����)
 
 - BasicBlocksList basicBlocks - ������ ���
 - List<ThreeAddressCommand> commands - ������ ������������ ������ (ThreeAddressCommand ��������� �� ThreeAddressCode.Model)
 - Dictionary<string, int> labels - ������� ������������ ������� � ������������ ���� � �����, ����������� ��
 - List<int> firstCommandsOfBlocks  - ������ ��� ������� �������, ����������� ������� � ���
 - List<int> lastCommandsOfBlocks - ������ ��� ������� �������, ����������� ���������� � ���
 - int[] BlockByFirstCommand - ������������ ������ ����� ������ ������� � ������������ ����, ������� �������� ������ �������� � ��� 
 - int[] BlockByLastCommand - ������������ ������ ����� ������ ������� � ������������ ����, ������� �������� ��������� �������� � ���
 - List<int>[] PreviousBlocksByBlockID - ������ ������� ������, �������� � ������ ���
 - List<int>[] NextBlocksByBlockID - ������ ������� ������, ��������� �� ������� ���
            
 ### ���������� ���������:

 ``` C#
 // ���������� ������ ��� �� ������ ����������� ������
public static BasicBlocksList CreateBasicBlocks(Program program)
        {
            BasicBlocksList basicBlocks = new BasicBlocksList();
            List<ThreeAddressCommand> commands = program.Commands;
            Dictionary<string, int> labels = new Dictionary<string, int>();
            List<int> firstCommandsOfBlocks = new List<int>(); 
            List<int> lastCommandsOfBlocks = new List<int>();

            //����� ������-������� � ������������ ����� ���� ���������(labels) 
            
            //������ �������� � ���������
            firstCommandsOfBlocks.Add(0);
            for (int i = 0; i < commands.Count; ++i)
            {
                ThreeAddressCommand currentCommand = commands[i];
                //���� ���� ����� � ������������ ����
                if (currentCommand.Label != null)
                {
                    labels[currentCommand.Label] = i;
                    //���� �� ������ ������� � ���������
                    if (i > 0)
                    {
                        //������� � ������
                        firstCommandsOfBlocks.Add(i);
                        //������� �������������� ������� � ������
                        lastCommandsOfBlocks.Add(i - 1);
                    }
                }
                //���� ���� �������
                if (currentCommand is Goto && i < commands.Count - 1)
                {
                    //��������� ������� �� �������� ��������
                    firstCommandsOfBlocks.Add(i + 1);
                    //���� ������� ��������
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

            //������������ ������� ������� ������, �������� � ��������� �� ������� ���
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
            
            //���������� ������ � ������ ���
            for (int i = 0; i < firstCommandsOfBlocks.Count; ++i)
            {
                BasicBlock block = new BasicBlock();
                block.Commands = new List<ThreeAddressCommand>(commands.Take(lastCommandsOfBlocks[i] + 1).Skip(firstCommandsOfBlocks[i]).ToList());
                basicBlocks.Blocks.Add(block);
                basicBlocks.BlockByID[block.BlockId] = block;
            }

            //���������� ���������� ������� �������� � ��������� ��� � ������ ����
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

### ������ �������������
```
...
var b = BasicBlocksGenerator.CreateBasicBlocks(threeAddressCode);
...
```
### ����
���������
```cs 
b = 1;
if 1
  b = 3;
else
  b = 2;
```

������ ������� ������ 
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

# 07: ���������� ����� ������ ���������� CFG (Control Flow Graph)

### ��������� ��������:
*PiedPiper (������ ����, ���������� ������)*

### �� ����� �������� �������:
1. ������� ����� 

### ��������� �������:
1. ���������� ������������� ���� �� ����� ������ ����������
2. ������������ �������� (� ��������������� ��������� ��� ��� ������)
3. ���������� ������ �����������

### ������

**���� ������ ����������** -- ��������� ���� ��������� ����� ���������� ���������, �������������� � ���� �����. � ����� ������ ���������� ������ ����  ����� ������������� �������� �����. ����� �� ����� � � ���� � ���� ����� � ������ �����, ����� ������ ������� ����� � ����� ��������� ��������������� �� ��������� �������� ����� �. ����� �������, ��� � -- �������������� �, � � -- �������� �.

�������� � ����� ����������� ��� ����, ��������� ������ � ������� � �� ��������������� ���������� ������������� ��������. ���������� ����� �� ����� � ������� ����������� ���� �����, �.�. � �������� �����, ������� ���������� � ������ ������� �������������� ����. ���� ��������� ������� ��������� �� �������� ����������� ���������, �� ������ ������������ ����, ���������� ��� ��������� ������� ���������. � ��������� ������ ������� �������������� ������ �������� ����� ������� ����, ������� ������� � ����, �� ����������� ������ ���������. 

### ������� ������:
 - ������ ������� ������ (BasicBlocksList)

### �������� ������:
 - ���� ������ ���������� (Graph)

 ### ������������ ��������� ������
 - BidirectionalGraph<BasicBlock, Edge<BasicBlock>> - ���� ������ ���������� (��������� ������ �� ������ QuickGraph)
 - Dictionary<int, BasicBlock> blockMap - ������������ ����� ������ ����� � ����������� ���������������� ������� ������ (blockId)

 ### ���������� ���������

 ``` C#
 // ���������� ����� ������ ���������� �� ������ ������� ������
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
 ��� �������� ������������� ����� � ��� ����������� ��������� ������:
 - public BasicBlock getBlockById(int id) -- �� �������������� �������� ����� ������������ ������� ���� - ���� �����
 - public BasicBlocksList getChildren(int id) -- �� �������������� �������� ����� ������������ ������ ������� ������-����������
 - public BasicBlocksList getParents(int id) -- �� �������������� �������� ����� ������������ ������ ������� ������-����������������
 - BasicBlock getRoot() -- ����� ����� � ���������
 - public int GetCount() -- ���������� ������ � �����
 - public IEnumerable<Edge<BasicBlock>> GetEdges() -- ������ ����� � �����
 - public IEnumerable<BasicBlock> GetVertices() -- ������ ������ � �����
 - public bool Contains(BasicBlock block) -- ��������, ���������� �� ������� ���� � �����
 - public bool IsAncestor(int id1, int id2) -- �������� �������� �� ���� � blockId = id1


### ������ �������������
```
...
var b = BasicBlocksGenerator.CreateBasicBlocks(threeAddressCode);
Graph g = new Graph(b);
...
```
### ����
���������
```cs 
b = 1;
if 1
  b = 3;
else
  b = 2;
```

������ ������� ������ 
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

���� ������ ����������
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

# 08: �������� ������: ���������� ������������� ���� �� CFG

### ��������� ��������: 
Yet yet another team(������� �����, ������� ������)

### ���������� ������: 
��������� ����������� ��������, ������� �� ������� ����� ������ ���������� ��������������� ����� ��������� � ������������ ����.

### �� ����� �������� �������:

  - ���� ���������� ������(CFG)

### ��������� �������:

  - �����������

### ������

**������������ ���** - ��� ������������������ ���������� ������ �� ��������� �����.
x = y op z
x = y //������� �����������
x = op y //x, y, z - �����, ��������� ��� ��������� �������.
goto L
if x goto L
if x goto L
if False x goto L
**������������ ���** ������������ ����� ��������������� ������������� ��������������� ������ ��� ���������������� ������������� �����, � ������� ����� ����� ������������� ���������� ����� �����. 

![](img/CFG3Code.png?raw=true)

��������������� ������������ ���� �  ��������������� ��� ������������ ���.


### ������� ������:
 - ���� ������ ����������

### �������� ������:
 - ������������ ���.

### ������������ ��������� ������

 - BidirectionalGraph<BasicBlock, Edge> - ���� ������ ���������� (��������� ������ �� ������ QuickGraph)
 - Dictionary<int, BasicBlock> blockMap - ������������ ����� ������ ����� � ����������� ���������������� ������� ������ (blockId)
 - List<ThreeAddressCommand> - ��� �������� ����������
 - Stack<BasicBlock> - ��� �������� ������� ��������� ���.
 - HashSet<BasicBlock> - ��� �������� ����������� ���.

### ���������� ���������

```C#
public List<ThreeAddressCommand>  transformToThreeAddressCode()
         {
             var res = new List<ThreeAddressCommand>();
             var done = new HashSet<BasicBlock>();
             var stack = new Stack<BasicBlock>();
             stack.Push(getBlockById(0));
             BasicBlock cur = null;
 
             while (done.Count < this.GetCount())
             {
                 cur = stack.Pop();
                 done.Add(cur);
                 
                 foreach (var c in cur.Commands)  { res.Add(c); }
                 
                 switch (cur.OutputBlocks.Count)
                 {
                     case 0:
                         continue;
                      case 1:
                          //�������� � ���� �������� ���
                      case 2:
                          //���� � else ��������� �������� goto, �� �������� � ���� ���, �� ������� ��������� goto
                          //�������� � ���� ����� else
                          //�������� � ���� ����� if
                      default:
                          throw new Exception("There cannot be more than two output blocks!");
                }
            }
```

### ������ �������������
```C#
SyntaxTree.SyntaxNodes.SyntaxNode root = ParserWrap.Parse(sourseText);
                var sourceThreeAddressCode = ThreeAddressCodeGenerator.CreateAndVisit(root).Program;
                BasicBlocksList bbl = BasicBlocksGenerator.CreateBasicBlocks(sourceThreeAddressCode);
                Graph cfg = new Graph(bbl);
                List<ThreeAddressCommand> resultThreeAddressCode = cfg.transformToThreeAddressCode();
```
### ����

```C#
if 1
    if 1
    {
        a = 5;
        a = 1;
    }
    else
    {   
        for i = 1..5 
            a = 1;
    }
    
while 1
    a = 1;
a = 2;

for i = 1 .. 5
    for j = 1 .. 5
        a = 1;

for i = 1 .. 5
    for j = 1 .. 5
        if 1
            a = 1;
        else
        {   
            a = 1;
            a = 2;
        }
```

# 09: ���������� ��������� gen S � kill S ��� ����� �������

### ��������� ��������:
*NoName (����� ������, ������� ������)*

### �� ����� �������� �������:
1. ���� ���������� ������

### ��������� �������:
1. ���������� ������������ �������, ��� ���������� ������������ ������� ��� ����� �������
2. ��������� gen B � kill B(�� ������ ������������ ������� �� ����� ��������)

### ���������� ������:
���������� ����������� ��������, ����������� ��������� *gen* � *kill* ��� ����� �������

### ������
���������� *gen S* ���������� ��������� �����������, ������������ � ������� ����� *B*. ���������� *kill S* ���������� ��������� ���� ������ ����������� ��� �� ���������� �� ���� ��������� ���������.

��� ��������� ������ � ������������ ������� ��� ������ � ����������� ������������, ������� ����� �������� ������������� ��������� ��� ��� ���������� ��������� ��� ��������� ��� ������� �����,
� ��� ����� ����� ��������� ��� ��������� ��� ������ ������� � ���� �����.

### ������� ������
- ������� ���� � ������� ���������� �������
- ID �������

### �������� ������
- ��������� gen
- ��������� kill


### ������������ ��������� ������
- `CommandNumber Gen` - ��������� gen (��� ��� ������� ����, �� ��� ����� ���� �������)
- `ISet<CommandNumber>` - ��������� kill
- `Dictionary<string, ISet<CommandNumber>>` - ��������� ������
- `Graph graph` - ���� ������ ����������


### ����������

� ���������� ������� ������ ��� ����������� ����� NuGet QuickGraph.

```cs
// ����� ���������� ��������� gen � kill.
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

// �������� ���������� �������� gen � kill
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

### ������ �������������
```cs
calculator = new GenKillOneCommandCalculator(g);
GenKillOneCommand genKill = calculator.CalculateGenAndKill(block, commandNumber);
```

# 10: ���������� �������� gen B � kill B (�� ������ ������������ ������� �� ����� ��������)

### ��������� ��������:
*Ampersand (�������� �����, �������� ����)*

### �� ����� �������� �������:
1. ������� ����� � �������� ���������

### ��������� �������:
1. ������������ ������������������ �������� � ���������� �������
2. ���������� ������ ��� �������� ������������ ������� ��� ��������� ������� �� ������ ��������
3. ���������� ����� ��������� ������� �� ������ ��������
4. ���������� ����� ��������� ������� �� ������ ��������

### ���������� ������:
� ������ ������ ��������� ����������� �����, ������� ��������� ������� ���������� �������� Gen � Kill �� �������� ����� ������. ���������� �������� �������������� �� ������ ������������ ������� �� ����� ��������.

### ������
� �������� ������� �� ������ �������� ��������� ��������������� ��� ��������  ������� ����� ����� ������� ������� ����� ������, �������� ������������ ����� �����.
������ ���������� � ������ ����������������� ��������� �������� ��������, ��������� ����� ���������� ����� ������� ���������� ������ ����� �� ������. ������ ������� �����������
���������� ������������� ������ � �������� ��������.

��������� ������� ����� ������ ������������ ����� ����� ����� *N* � ����� *E*, �����, ���
1. ���������� ��������� *h <html>&#8712;<html> N*, ������������ ��� ����� ������ � *N*;
2. ���� ��������� ���� *m* ����� ������� ���� *n <html>&#8712;<html> N*, ����� *h*, �� *m* ����� ������ � *N*;
3. *E* - ��������� ���� ����� ������ ���������� ����� ������ *n1* � *n2* �� *N*, �� �����������, ��������, ��������� �����, �������� � *h*.
	
*����* ����� *L* (��� ���� � �����, �� ����������� �������� ����� � ���������) �������� �����, �������������� ������� *R*. �����, ������� � ��������� *L*, ������ ������ � ����
R. ����� �� ������ ������ �� *L* ���������� ������ �� *R* � �� �� ����� ����� ����������. ������ ���� ����� �������� ��������, �� ��� ���������� ������ � *R*.
������� *R �������� ����*.

������������ ������� ������� ���� �� ������� ����� ����������� � ���, ��� ��������� �������� �������� ����� � ��������� ����� L. 

### ������������ ��������� ������
- `List<int> OutputBlocks` - ������ �������� ������� ������

### ����������

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

### ������ �������������

```cs

```

# 11: ���������� ������������ �������, ��� ���������� ������������ ������� ��� ����� �������

### ��������� ��������: 
*Google Dogs (��������� ���������, ������ ���)*

### �� ����� �������� �������:
1. ��������� ������� ������
2. ��������� �������� *gen* � *kill* ��� ����� �������

### ��������� �������:

1. ������������ �������� ��� ����������� �����������

### ���������� ������:
��������� ��������� *gen* � *kill* ��� ����� �������, ���������� ��������� ��������� *gen* � *kill* ��� ������� �����.

### ������

�����������, ��� ���� *�* ������� �� ���������� *s*<sub>*1*</sub>, ..., *s*<sub>*n*</sub> � ��������� �������. ���� *s*<sub>*1*</sub> � ������ ���������� �������� ����� *�*, �� IN[*�*] = IN[*s*<sub>*1*</sub>]. ����������, ���� *s*<sub>*n*</sub>� ��������� ���������� �������� ����� *B*, �� OUT[*B*] = OUT[*s*<sub>*n*</sub>]. ������������ ������� �������� ����� *B*, ������� �� ��������� ��� *f*<sub>*B*</sub>, ����� ���� �������� ��� ���������� ������������ ������� ���������� �������� �����. ����� *f*<sub>*s*<sub>*i*</sub></sub> � ������������ ������� ��� ���������� *s*<sub>*i*</sub>. ����� *f*<sub>*B*</sub> = *f*<sub>*s*<sub>*n*</sub></sub> <font size="3" color="black" face="Arial">&#9675;</font> ...
<font size="3" color="black" face="Arial">&#9675;</font>
*f*<sub>*s*<sub>*2*</sub></sub><font size="3" color="black" face="Arial"> &#9675; </font>*f*<sub>*s*<sub>*1*</sub></sub>.

### ������� ������
- ������� �����

### �������� ������
- ��������� *gen* � *kill* ��� �����

### ������������ ��������� ������
- ```GenKillOneCommandCalculator commandCalc``` - ����������� 
 �������� *gen* � *kill* ��� ����� �������
 
### ���������� ���������
```cs
public override ISet<CommandNumber> CommandTransferFunction
    (ISet<CommandNumber> input, BasicBlock block,
      int commandNumber)
{
    GenKillOneCommand genKill =
      calculator.CalculateGenAndKill(block, commandNumber);
    var result = SetFactory.GetSet<CommandNumber>(input);
    result.ExceptWith(genKill.Kill);
    result.UnionWith(genKill.Gen == null
      ? SetFactory.GetSet<CommandNumber>()
      : SetFactory.GetSet(genKill.Gen));
    return result;
}
```

### ������ �������������
```cs
public override T TransferFunction
  (T input, BasicBlock block)
{
    return Enumerable.Range(0, block.Commands.Count)
      .Aggregate(input, (result, c) => 
         CommandTransferFunction(result, block, c));
}
```

# 12: ������������ �������� ��� ��������� ���������

### ��������� ��������: 
*Google Dogs (��������� ���������, ������ ���)*

### �� ����� �������� �������:
1. ���������� ������� ������
2. ���������� ������������� ����
3. ���������� �������� *e_gen* � *e_kill*

### ��������� �������:
�����������

### ���������� ������
�������� �����-��������� ������ ������������� ���������, �������������� ������ ��� ������ ��������� ���������

### ������
��������� *�* + *�* **��������** (available) � ����� *�*, ���� ����� ���� �� �������� ���� � *�* ��������� *�* + *�* � ����� ���������� ������ ���������� �� ���������� *�* ��� ����������� ������������ ���������� *�* � *�*. ����� ���� ���� � *��������� ����������*, �� �������, ��� ���� ���������� ��������� *�* + *�*, ���� �� ����������� (��� ����� �����������) *�* � *�* � ����� ����� �� ��������� *�* + *�* ������. ���� ���������� ��������� *�* + *�*, ���� �� ��������� *�* + *�* � �� ��������� ����������� ��������������� *�* � *�*. 


�� ����� ����� ��������� ��������� �������, ������������ ����� ���������� ����������� �����������. �����������, ��� *U* � "�������������" ��������� ���� ���������, ������������ � ������ ����� ����� ��� ���������� ���������� ���������. ����� ��� ������� ����� *�* ��������� IN[*�*] �������� ��������� �� *U*, ��������� � ����� ��������������� ����� ������� ����� *B*, a OUT[*�*] � ����� �� ��������� ��� �����, ��������� �� ������ ����� *�*. ��������� *e_gen*<sub>*B*</sub> ��� ��������� ���������, ������������ *�*, a *e_kill*<sub>*B*</sub> � ��� ��������� ��������� �� *U*, ������������ � *B*. �������, ��� ��������� IN, OUT, *e_gen* 
� *e_kill* ����� ���� ������������ � ���� ������� ��������. ����������� ��������� IN � OUT ������� ���� � ������ � � ���������� *e_gen* � *e_kill* ���������� 
�������������: 

OUT[����] = <html>&#x2205;</html>

� ��� ���� ������� ������ *�*, �������� �� ��������:

OUT[*�*] = *e_gen*<sub>*B*</sub> <html>&#8746;</html> (IN[*�*] - *e_kill*<sub>*B*</sub>)

IN[*�*] = <html>&#8745;</html> OUT[*P*], ��� *P* - �������������� *B*

### ���������� ���������
```cs
public AvailableExpressionsCalculator(Graph g)
{
    Graph = g;
}

public override ISet<Expression> GatherOperation
  (IEnumerable<ISet<Expression>> blocks)
{
    ISet<Expression> intersection = SetFactory.GetSet((IEnumerable<Expression>)blocks.First());
    foreach (var block in blocks.Skip(1))
      intersection.IntersectWith(block);
    return intersection;
}

public override ISet<Expression> GetGen(BasicBlock block)
{
    return SetFactory.GetSet(
        block.Commands
          .OfType<Assignment>()
          .Select(x => x.Value));
}

public override ISet<Expression> GetKill(BasicBlock block)
{
    return SetFactory.GetSet(
      block.Commands
        .OfType<Assignment>()
        .Select(x => x.Target as Expression));
}

public override ISet<Expression> TransferFunction
  (ISet<Expression> input, BasicBlock block)
{
    var kill = GetKill(block).Cast<Identifier>();
    var result = SetFactory.GetSet(
        input.Where(inputExpression =>
        !kill.Any(inputExpression.HasIdentifiedSubexpression)));
    result.UnionWith(GetGen(block));
    return result;
}

public override bool ForwardDirection => true;

public override ISet<Expression> FirstValue => SetFactory.GetSet();

public override ISet<Expression> StartingValue
{
    get
    {
        ISet<Expression> result = SetFactory.GetSet();
        foreach (BasicBlock b in Graph)
          foreach (ThreeAddressCommand c in b.Commands)
            if (c.GetType() == typeof(Assignment))
            {
                result.Add((c as Assignment).Value);
                result.Add((c as Assignment).Target);
            }
         return result;
    }
}
```

### ������ �������������
```cs
var availableExprs = IterativeAlgorithm.Apply(g,
    new AvailableExpressionsCalculator(g));
var outExpressions = availableExprs.Out.Select(
    pair => $"{pair.Key}: {string.Join(", ", 
    pair.Value.Select(ex => ex.ToString()))}");
```

# 13: �������� ������: ���������� `e_gen(b)` � `e_kill(b)` ��� ��������� ���������

### ��������� ��������: YACT

### ���������� ������: 

���������� �� ������� ����� ������ ������ ����� ��������� `e_gen` � `e_kill` ��� ������� �������� �����. ��� ������ ������������ ���������� ������� ��������� ���������. 

### �� ����� �������� �������:

  - CFG
  - ������� �����
  - ���������� ����������� ��������
  - ������������ ���

### ��������� �������:

  - ���������� ��������� ���������

### ������

**��������� ���������** -- �������� �������, ������������ ��� ������ ����� � ��������� ��������� ���������, ������� �� ��������� �������������. ����� ��������� ���������� *����������* � ������ �����. ��� ����, ����� ���� ���������� � ����� ���������, �������� ��������� �� ������ ���� �������� �� ����� ���� �� ��������� ����� ��������� �� ����� ���������.

![alt text](https://github.com/wisestump/OptimizingCompiler/raw/master/documentation/YACT/img/GenKillExample.png "Gen-Kill Example")

### ������� ������:
 - ������� ����

### �������� ������:
 - ��������� ���������

### ������������ ��������� ������

 - `HashSet<Expression>` -- ��������� ���������

### ���������� ���������

``` C#
// ����� ��������� ��������� Gen
public override ISet<Expression> GetGen(BasicBlock block)
{
    return SetFactory.GetSet( 
        block.Commands
            .OfType<Assignment>() // ��������� ������ ������������ 
            .Select(x => x.Value)); // �� ��� ����� ������ �����
}

// ����� ��������� ��������� Kill
public override ISet<Expression> GetKill(BasicBlock block)
{
    return SetFactory.GetSet(
        block.Commands
            .OfType<Assignment>() // ��������� ������ ������������ 
            .Select(x => x.Target as Expression)); // �� ��� ����� ����� �����
}
```

### ������ �������������

������������ ��� ���������:
``` C#
1: <no-op>
t0 = a + b
x = t0
2: <no-op>
t1 = a * b
y = t1
3: <no-op>
t2 = a + 1
a = t2
```

����� ���������

``` C#
// ----
// ����������� ����� g
// ----
...
var availableExprs = IterativeAlgorithm.Apply(g, new AvailableExpressionsCalculator(g));
var outExpressions = availableExprs.Out.Select(
    pair => $"{pair.Key}: {string.Join(", ", pair.Value.Select(ex => ex.ToString()))}");
```
����� `outExpressions`:
```
// ��������� ���������
0: a + b, t0
1: a + b, t0, a * b, t1
2: t0, t1, a + 1, t2
```
### ����

``` C#
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

// ������������ CFG � ����� ���������
SyntaxNode root = ParserWrap.Parse(programText);
var threeAddressCode = ThreeAddressCodeGenerator.CreateAndVisit(root).Program;
var basicBlocks = BasicBlocksGenerator.CreateBasicBlocks(threeAddressCode);
Graph g = new Graph(basicBlocks);

var availableExprs = IterativeAlgorithm.Apply(g, new AvailableExpressionsCalculator(g));
var outExpressions = availableExprs.Out.Select(
    pair => $"{pair.Key}: {string.Join(", ", pair.Value.Select(ex => ex.ToString()))}");

// ��������� � ��������
int startIndex = availableExprs.Out.Keys.Min();
Assert.IsTrue(availableExprs.Out[startIndex]
    .SetEquals(
        new HashSet<Expression>(new Expression[]
        {
            new Int32Const(4),
            new BinaryOperation(new identifier("a"), Operation.Add, new identifier("b")),
            new identifier("t0")
        })));

Assert.IsTrue(availableExprs.Out[startIndex + 1]
    .SetEquals(
        new HashSet<Expression>(new Expression[]
        {
            new Int32Const(4),
            new Int32Const(3),
            new identifier("t0")
        })));

Assert.IsTrue(availableExprs.Out[startIndex + 2]
    .SetEquals(
        new HashSet<Expression>(new Expression[]
        {
            new Int32Const(4),
            new Int32Const(2),
            new identifier("t0")
        })));

Assert.IsTrue(availableExprs.Out[startIndex + 3]
    .SetEquals(
        new HashSet<Expression>(new Expression[]
        {
                new Int32Const(4),
                new identifier("t0")
        })));
```

# 14: ������������ �������� ��� ������ ��������������� ��������

### ��������� ��������:
*Ampersand (�������� �����, �������� ����)*

### �� ����� �������� �������:
1. ����� ������������ ��������
2. ������������ ������� ��� ������ �������������� ��������

### ��������� �������:
-

### ���������� ������:

### ������

### ������������ ��������� ������
- ` Dictionary<Edge<BasicBlock>, EdgeType>` - ������� ������������������ �����

### ����������

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

### ������ �������������

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
            Trace.WriteLine(Environment.NewLine + "������� �����");
            Trace.WriteLine(basicBlocks);

            Trace.WriteLine(Environment.NewLine + "����������� ���� ���������");
            Graph g = new Graph(basicBlocks);
            Trace.WriteLine(g);



            Trace.WriteLine(Environment.NewLine + "��������������� ��������");
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
            Trace.WriteLine(Environment.NewLine + "������� �����");
            Trace.WriteLine(basicBlocks);

            Trace.WriteLine(Environment.NewLine + "����������� ���� ���������");
            Graph g = new Graph(basicBlocks);
            Trace.WriteLine(g);
            
            Trace.WriteLine(Environment.NewLine + "��������������� ��������");
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

# 15: ������������ ��������

### ��������� ��������:
*Ampersand (�������� �����, �������� ����)*

### �� ����� �������� �������:
1. ���� ���������� ������

### ��������� �������:
1. ������������ �������� ��� ������ ��������������� ��������
2. ������������ �������� ��� ��������� ��������� 
3. ������������ �������� ��� �������� ���������� 

### ���������� ������:
� ������ ������ ��������� ����������� �����, ������� ��������� ������� �������� 

### ������

### ������������ ��������� ������
- ` ` - ������� ������������������ �����

### ����������

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

### ������ �������������

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
            Trace.WriteLine(Environment.NewLine + "������� �����");
            Trace.WriteLine(basicBlocks);

            Trace.WriteLine(Environment.NewLine + "����������� ���� ���������");
            Graph g = new Graph(basicBlocks);
            Trace.WriteLine(g);


            
            Trace.WriteLine(Environment.NewLine + "��������� ���������");
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

# 16: ���������� ������ �����������

### ��������� ��������:
*NoName (����� ������, ������� ������)*

### �� ����� �������� �������:
1. ������������ ��������
2. ���� ���������� ������

### ��������� �������:
1. �������� ������������� ����� �����

### ���������� ������:
���������� ������� ����� ��� ������ ����������� � ��������� ��� ������, ��������� ����������� ��������.

### ������
���� *d* ����� ������ ���������� ��� ����� *n*, ���� ����� ���� �� �������� ���� ����� ������ � *n* �������� ����� *d*.
��� ����� ����������� ������ ���� ���������� ��� ����� �����.

������ ����������� - ��� ������, � ������� ������� ���� �������� ������, � ������ ���� *d* ���������� ������ ��� ������ ��������� � ������.

������������� �������� ����������� ������� �� �������� �����������: ������ ���� *n* ����� ������������ ���������������� ��������� *m*, ������� ��������
��������� ����������� *n* �� ����� ���� �� �������� ���� �� *n*.

### ������� ������
- ���� ������ ����������

### �������� ������
- ������ �����������


### ������������ ��������� ������
- `AdjacencyGraph<int, Edge<int>> Tree` - ������ ����������� (��������� ������ �� ������ NuGet QuickGraph)
- `Dictionary<int, int> Map` - �������: ����� �������� ����� - ��� ���������������� ���������

### ����������

� ���������� ������� ������ ��� ����������� ����� NuGet QuickGraph.

```cs
// ����� "������ �����������"
public class DominatorsTree : IEnumerable
{
    private AdjacencyGraph<int, Edge<int>> Tree = new AdjacencyGraph<int, Edge<int>>();
    private Dictionary<int, int> Map;

    public DominatorsTree(Graph g)
    {
        Map = ImmediateDominator.FindImmediateDominator(g);

        Tree.AddVertexRange(Map.Keys);

        foreach (int key in Map.Keys.Skip(1))
            Tree.AddEdge(new Edge<int>(Map[key], key));
    }

    public int GetParent(int id)
    {
        return Map[id];
    }

    public List<int> GetAncestors(int id)
    {
        return Map.Where(x => x.Value == id).Select(x => x.Key).ToList();
    }

    public override string ToString()
    {
        string res = "";
        foreach (var v in Tree.Vertices)
            if (Tree.OutEdges(v).Count() > 0)
                foreach (var e in Tree.OutEdges(v))
                    res += v + " --> " + e.Target + "\n";
        return res;
    }

    public IEnumerator GetEnumerator()
    {
        return Map.Values.GetEnumerator();
    }

    public Dictionary<int, int> GetMap()
    {
        return Map;
    }
}

// ����� "���������������� ���������"
public static class ImmediateDominator
{
    public static Dictionary<int, int> FindImmediateDominator(Graph g)
    {
        var _out = IterativeAlgorithm.IterativeAlgorithm.Apply(g, new DominatorsIterativeAlgorithmParametrs(g)).Out;

        int min = _out.Keys.Min();

        return _out.Select(x => new KeyValuePair<int, int>(x.Key,
                                        x.Key > min ? _out[x.Key].Take(_out[x.Key].Count - 1).Last() : min))
                                      .ToDictionary(x => x.Key, x => x.Value);
    }
}

// ��������� ������ ������ "��������� ������������� ���������"
public class DominatorsIterativeAlgorithmParametrs : BasicIterativeAlgorithmParameters<ISet<int>>
{
    private Graph graph;

    public DominatorsIterativeAlgorithmParametrs(Graph g)
    {
        graph = g;
    }

    public override ISet<int> GatherOperation(IEnumerable<ISet<int>> blocks)
    {
        ISet<int> intersection = SetFactory.GetSet((IEnumerable<int>)blocks.First());
        foreach (var block in blocks.Skip(1))
            intersection.IntersectWith(block);

        return intersection;
    }

    public override ISet<int> TransferFunction(ISet<int> input, BasicBlock block)
    {
        return SetFactory.GetSet<int>(input.Union(new int[] { block.BlockId }));
    }

    public override bool AreEqual(ISet<int> t1, ISet<int> t2)
    {
        return t1.IsSubsetOf(t2) && t2.IsSubsetOf(t1);
    }

    public override ISet<int> StartingValue { get { return SetFactory.GetSet<int>(Enumerable.Range(graph.GetMinBlockId(), graph.Count())); } }

    public override ISet<int> FirstValue { get { return SetFactory.GetSet<int>(Enumerable.Repeat(graph.GetMinBlockId(), 1)); } }

    public override bool ForwardDirection { get { return true; } }

}

// ����������� ��������, ������������ ��� ���������� ������ �����������
public static IterativeAlgorithmOutput<V> Apply<V>(Graph graph, BasicIterativeAlgorithmParameters<V> param, int[] order = null)
{
    IterativeAlgorithmOutput<V> result = new IterativeAlgorithmOutput<V>();

    foreach (BasicBlock bb in graph)
        result.Out[bb.BlockId] = param.StartingValue;
    IEnumerable<BasicBlock> g = order == null ? graph : order.Select(i => graph.getBlockById(i));
    bool changed = true;
    while (changed)
    {
        changed = false;
        foreach (BasicBlock bb in g)
        {
            BasicBlocksList parents = param.ForwardDirection ? graph.getParents(bb.BlockId) : graph.getChildren(bb.BlockId);
            if (parents.Blocks.Count > 0)
                result.In[bb.BlockId] = param.GatherOperation(parents.Blocks.Select(b => result.Out[b.BlockId]));
            else
                result.In[bb.BlockId] = param.FirstValue;
            V newOut = param.TransferFunction(result.In[bb.BlockId], bb);
            changed = changed || !param.AreEqual(result.Out[bb.BlockId], newOut);
            result.Out[bb.BlockId] = param.TransferFunction(result.In[bb.BlockId], bb);
        }
    }
    if (!param.ForwardDirection)
        result = new IterativeAlgorithmOutput<V> { In = result.Out, Out = result.In };
    return result;
}
```

### ������ �������������
```cs
DominatorsTree tree = new DominatorsTree(g);
foreach (Edge<BasicBlock> e in g.GetEdges())
{
    if (dfn[e.Source.BlockId] >= dfn[e.Target.BlockId])
        edgeTypes.Add(e, EdgeType.Retreating);
    else if (g.IsAncestor(e.Target.BlockId, e.Source.BlockId) && tree.GetParent(e.Target.BlockId) == e.Source.BlockId)
        edgeTypes.Add(e, EdgeType.Advancing);
    else
        edgeTypes.Add(e, EdgeType.Cross);
}
```

### ����
```cs
*���������*:
b = 1;
if 1 
  b = 3;
else
  b = 2;
```

*���� ������ ����������*:

![](../GoogleDogs/img/TestEC.png?raw=true)

*����� ���������*:

������ ������: "����� �������� ����� - ��� ���������������� ���������"

0 -> 0                                                        
1 -> 0                                                          
2 -> 0                                                                
3 -> 0

# 17: ���������� ���������� ��������� ������

### ��������� ��������:
*PiedPiper (������ ����, ���������� ������)*

### �� ����� �������� �������:
1. ���� ���������� ������

### ��������� �������:
1. ���������� ������������ ������

### ������
�������� ������ ����� ������� �� ������������ ������������ ���� �����, 
�����, ��� �� ����� ������� ����� ����� ������� � ����� ������ �������, �������� �� ���� �����.
�������� ������ ����� ���� ��������� ����������� ����� ���������� ������ �����, �������� ������� 
� ������� ��� ������� � ������. ��� ������� �� ���� ��� ���� ```(u,v)```, �����, 
��� ��������, ������������ ������� ```u```, ������������ � � ������ ��������� �����, 
�� ������������ ����� ������� ```v```. 

����� � ����� � ������� (depth-first search) ���������� �������� ��� ���� �����, ������� � �������� ���� � �������, � ������ 
�������, ��������� ��� ��������, ����, ����������� ��������� �� ��������. ���� ������ � ������� �������� ��������� �������� ������ (������������ ������ ������) (depth-first spanning tree � DFST). ����� � ������ ������ �������� ���� ����� ���������� ������ �� ��� �������� �����, ������� ����� ���������� ���������� � ������� ����� �������. ����� � �������� ������� ������� ���������� ����� ������� �������� ����, �������� �� ��������� � ��������, � ����� �������� ��� ������� ����. 

### ������� ������:
 - �� ���������, �������� ������ �������� � �������� ���������� CFG

### �������� ������:
 - ������������� ������ <int, int>

### ���������� ���������
���� ����� ������ ���������� ��������� � ������ ������� � � �������� ������ ����������.
```cs
private void dfs(BasicBlock block, Dictionary<BasicBlock, bool> visited, ref int c)
{
  visited[block] = true;
  foreach (var node in getChildren(block.BlockId).Blocks)
  {
    if (!visited[node])
    {
      spanTree.AddEdge(new Edge<BasicBlock>(block, node));
      dfs(node, visited, ref c);
    }
  }
  spanTreeOrder[block.BlockId] = c;
  c--;
}
```
#### ������ �������������
```cs
var b = BasicBlocksGenerator.CreateBasicBlocks(threeAddressCode);
Graph g = new Graph(b);
Dictionary<int, int> dfn = g.GetDFN();
```

### ����
���������:

```cs
b = 1;
if 1 
  b = 3;
else
  b = 2;
```

����������� ���� ���������
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
<--  1 2
--> 
```

����� �����������
```
3-->4
1-->3
2-->2
0-->1
```

# 18: ������������� ���� �����

### ��������� ��������: 
*Google Dogs (��������� ���������, ������ ���)*

### �� ����� �������� �������:
1. ���������� CFG
2. ���������� ������ �����������
3. ���������� ���������� ��������� ������

### ��������� �������:
1. ����������, ��� �� ����������� ����� �������� ���������
2. ������������ ������������������ �������� � ���������� �������

### ���������� ������:
���������� ���������������� ����� ����� ������ ���������� �� ��� ����:

1. *�����������* (advancing) ����� ���� �� ���� *m* � �������� ���������� *m* � ������.
2. *�����������* (retreating) ����� ���� �� ���� *m* � ������ *m* � ������ (��������, � ������ *m*).
3. *����������* (cross) - ��� ��������� �����.

������:

![](img/ExampleEC.png?raw=true)

и��� 1 <html>&rarr;</html> 3, 6 <html>&rarr;</html> 7, 9 <html>&rarr;</html> 10 �������� ������������, ���� 4 <html>&rarr;</html> 3, 7 <html>&rarr;</html> 4, 8 <html>&rarr;</html> 3 �������� ������������, ���� 2 <html>&rarr;</html> 3, 5 <html>&rarr;</html> 7 - �����������.

### ������

��� �����, ���������� � ��������� �������� ������, �������� *������������*. � ������� ���� ����� ����� ���������� �������� ��������. ������, ���� �� �� ����� ������ ���������� ������������ �����, ��������, 1 <html>&rarr;</html> 4, ��� �� ���� ��������� *�����������*, �������� �� ��, ��� �� ������� �� � DFST.

����� *m <html>&rarr;</html> n* �������� *�����������* ����� � ������ �����, ����� *dfn*[*m*] <html>&ge;</html> *dfn*[*n*]. ���� *m* �������� �������� *n* � ��������� �������� ������, �� *search*(*m*) ����������� �� *search*(*n*), ������� *dfn*[*m*] <html>&ge;</html> *dfn*[*n*]. � ��������, ���� *dfn*[*m*] <html>&ge;</html> *dfn*[*n*], �� *search(*m*) ����������� �� *search*(*n*), ��� *m* = *n*. �� ����� *search*(*n*) ������ ���������� �� *search*(*m*), ���� ���������� ����� *m <html>&rarr;</html> n*, ����� ��� ����, ��� *n* �������� ���������� *m*, ������ ������� *n* �������� *m* � DFST. ����� �������, ����� ���������� *search*(*m*) 
������������ ����� ����������� ������� ���������� *search*(*�*), ������ �������, ��� *n* �������� ������� *�* � DFST.

������ ��������� *����������* ����� �������� ��, ��� ���� ���������� DFST ���, ����� �������� ���� ���������� ���� ������������� ����� ������� � �������, � ������� ��� ����������� � ������, �� ��� *����������* ����� ����� ���� ������ ������. � ����� ����� *����������* ����� ����� ���� �������� ������ � ������, ���� �� ����� ������ ���������� ����� ����� ��� ���������� "����": �� ���� �� ������ �������� ����� ����� ������� � ������ (�� ���������������� ������� �������) �� ���� � ����� �����.

### ������� ������
- ���� ������ ����������

### �������� ������
- ���� "����� - ��� �����"

### ������������ ��������� ������
- `Dictionary<Edge<BasicBlock>, EdgeType> edgeTypes` - �������� ����������
- `Dictionary<int, int> dfn` - ������� �����
- `DominatorsTree tree` - ������ �����������

### ���������� ���������
```cs
public static Dictionary<Edge<BasicBlock>, EdgeType>
    ClassifyEdge(ControlFlowGraph.Graph g)
{
    // ������������� ������ ��������
    Dictionary<Edge<BasicBlock>, EdgeType> edgeTypes 
    = new Dictionary<Edge<BasicBlock>, EdgeType>();
    // ��������� ������� �����
    Dictionary<int, int> dfn = g.GetDFN();
    // ��������� ������ �����������
    DominatorsTree tree = new DominatorsTree(g); 
    
    // ���������� ��� ���� ����� �����
    foreach (Edge<BasicBlock> e in g.GetEdges())
    {
        /* ���� ������� ������ ������ ������� �����,
           �� ����� �����������*/
        if (dfn[e.Source.BlockId] >= dfn[e.Target.BlockId])
            edgeTypes.Add(e, EdgeType.Retreating);
        // ���� ������ � ������, �� �����������
        else if (g.IsAncestor(e.Target.BlockId, e.Source.BlockId) 
        && tree.GetParent(e.Target.BlockId) == e.Source.BlockId)
            edgeTypes.Add(e, EdgeType.Advancing);
        // ����� ����������
        else
            edgeTypes.Add(e, EdgeType.Cross);
    }

    return edgeTypes;
}
```

### ������ �������������
```cs
var edgeClassify = EdgeClassification.ClassifyEdge(g);
foreach (var v in edgeClassify)
    // � ������� ��������� ���� � ����� ��������� �����, � ����� ��� �����
    Console.WriteLine(v.Key.Source.BlockId + " -> "
    + v.Key.Target.BlockId + " : " + v.Value);
```

### ����
*���������*:
```
b = 1;
if 1 
  b = 3;
else
  b = 2;
```

*���� ������ ����������*:

![](img/TestEC.png?raw=true)

����� *0 <html>&rarr;</html> 1* � *0 <html>&rarr;</html> 2* �������� ������������, � ����� *1 <html>&rarr;</html> 3* � *2 <html>&rarr;</html> 3* �������� �����������.

*����� ���������*:

������ ������: "����� ��������� ����� - ���� ��������� ����� - ��� �����"

0 -> 1 : Advancing                                                        
0 -> 2 : Advancing                                                              
1 -> 3 : Cross                                                                     
2 -> 3 : Cross

# 19: FindReverseEdges

### ��������� ��������: EndFrame (������ ����, ������� ���������)

### ���������� ������: ���������� ����������� �������, ������� ������� � �������� ����� ������ ���������� �������� �����.

### �� ����� �������� �������:

  - ControlFlowGraph
  - EdgeClassification
  - ImmediateDominator

### ��������� �������:

  - CheckRetreatingIsReverse

### ������

> ����������� ����� � ����� ������ ���������� - �����, ������ �� ������� � ������.
������� ����� d ���������� ��� a, ���� ����� ���� �� ��������� ������� ����� �� ������� �, �������� ����� d. �������� �������������: d ���������� ��� d; ���� d ���������� ��� b, � b ���������� ��� a, �� d ���������� ��� a. ���������� �����, ������������ �� a � b, ���������� ��������, ���� b ���������� ��� a

### ������� ������:
 - Graph - ���� ������ ����������

### �������� ������:
 - ISet<Edge<BasicBlock>> - ��������� ��� ������� ������

### ������������ ��������� ������

 - Dictionary<Edge<BasicBlock>, EdgeType> ClassifiedEdges - ������������ � ����� ��� ����
 - Dictionary<int, int> Dominators - ����������� � ���� ����� ��� ����������������� ����������
 - Dictionary<Edge<BasicBlock>, EdgeType> RetreatingEdges - ����������� ����� �� ����� �����

### ���������� ���������
```C#
        public static ISet<Edge<BasicBlock>> FindReverseEdges(ControlFlowGraph.Graph g)
        {
            ISet<Edge<BasicBlock>> res = SetFactory.GetEdgesSet();//new SortedSet<Edge <BasicBlock>>();
            Dictionary<Edge<BasicBlock>, EdgeType> ClassifiedEdges = EdgeClassification.EdgeClassification.ClassifyEdge(g);
            Dictionary<int, int> Dominators = ImmediateDominator.FindImmediateDominator(g);
            var RetreatingEdges = ClassifiedEdges.Where(x => x.Value == EdgeType.Retreating);
            foreach (var edg in RetreatingEdges)
            {
                var edge = edg.Key;               // ������� ����������� �����
                int key = edge.Source.BlockId;    // ������� ���� �������� �� ������ ������������ �����
                int value = edge.Target.BlockId;  // ����� ������������ �����
                bool isReverse = false;           // �������� �� ������� ���� ������ ������������ �����
                /* ����, ���� � �������� ���� ���� ���������������� ���������, �� �� ������ ������ �����������, 
                � ������� ���� �� ����� �� ����� ������������ ����� */
                while (Dominators.ContainsKey(key) && Dominators[key] != key && !isReverse)
                {
                    key = Dominators[key];      // ������� ���� ������������ �� ����������������� ����������
                    isReverse = (key == value); // ����� ������������ ���������� ��� �������, �� ���� ����� - ��������
                }
                if (isReverse)        // ���� ����� �������� - ���������
                    res.Add(edge); 
            }
            return res;
        }
```
### ������ �������������

var ReverseEdges = FindReverseEdge.FindReverseEdges(g);

### ����
```
 for i = 1 + 2 * 3 .. 10
   println(i);
```
```
���������� CFG: 
0: 1
1: 2, 3
2: 1
3: 
```
```  
�������� �����: 2 --> 1

# 20: ����������, ��� �� ����������� ����� �������� ���������

### ��������� ��������:
*Ampersand (�������� �����, �������� ����)*

### �� ����� �������� �������:
1. �������� ������������� ����� �����
2. �������� ���������� �������� ����� �����

### ���������� ������:
� ������ ������ ��������� ����������� �����, ������� ��������� ������� �������� ���� �����, ��� ��� ����������� ����� ����� �������� ���������.

### ������
����������� ����� - ����� �� �������� � ������ ��������� �����.

����� �� A � B ���������� ��������, ���� B ���������� ��� A.

### ������������ ��������� ������
- ` Dictionary<Edge<BasicBlock>, EdgeType>` - ������� ������������������ �����

### ����������

```cs
namespace DataFlowAnalysis.IntermediateRepresentation.CheckRetreatingIsReverse
{
    class CheckRetreatingIsReverse
    {
        public static bool CheckReverseEdges(ControlFlowGraph.Graph g)
        {
            Dictionary<Edge<BasicBlock>, EdgeType> ClassifiedEdges = EdgeClassification.EdgeClassification.ClassifyEdge(g);
            var RetreatingEdges = ClassifiedEdges.Where(x => x.Value == EdgeType.Retreating).Select(x => x.Key);

            var ReverseEdges = FindReverseEdge.FindReverseEdges(g);

            return ReverseEdges.IsSubsetOf(RetreatingEdges);
        }
    }
}
```

### ������ �������������

```cs
[TestClass]
    public class RetreatingNotReverseEdgesTests
    {
        [TestMethod]
        public void RetreatingNotReverseEdges1()
        {
            var programText = File.ReadAllText("./../../RetreatingNotReverseEdgesEx1.txt");


            SyntaxNode root = ParserWrap.Parse(programText);
            Graph graph = new Graph(
                BasicBlocksGenerator.CreateBasicBlocks(
                    ThreeAddressCodeGenerator.CreateAndVisit(root).Program));

            Assert.IsFalse(CheckRetreatingIsReverse.CheckReverseEdges(graph));
        }

        [TestMethod]
        public void RetreatingNotReverseEdges2()
        {
            var programText = File.ReadAllText("./../../RetreatingNotReverseEdgesEx2.txt");

            SyntaxNode root = ParserWrap.Parse(programText);
            Graph graph = new Graph(
                BasicBlocksGenerator.CreateBasicBlocks(
                    ThreeAddressCodeGenerator.CreateAndVisit(root).Program));

            Assert.IsFalse(CheckRetreatingIsReverse.CheckReverseEdges(graph));
        }

        [TestMethod]
        public void RetreatingNotReverseEdges3()
        {
            var programText = File.ReadAllText("./../../RetreatingNotReverseEdgesEx3.txt");

            SyntaxNode root = ParserWrap.Parse(programText);
            Graph graph = new Graph(
                BasicBlocksGenerator.CreateBasicBlocks(
                    ThreeAddressCodeGenerator.CreateAndVisit(root).Program));

            Assert.IsFalse(CheckRetreatingIsReverse.CheckReverseEdges(graph));
        }
    }
```

# 21: ����� ������������ ������

### ��������� ��������: 
*Google Dogs (��������� ���������, ������ ���)*

### �� ����� �������� �������:
1. ���������� CFG
2. �������� ������������� ���� CFG

### ��������� �������:
1. ������������ ������������������ �������� � ���������� �������

### ���������� ������: 
� ������ ������ ��������� ����� �� ����� ������ ���������� (CFG) *������������ �����*. ��� ����� �������� ����� ������� ����������:
  1. ���� ����� ������������ ������� ���� - ������� ����, ������� ���������� *����������*. ������ ���� ���������� ��� ����� ������ ����� �����, � ��������� ������ ���� �� �������� ������������ ������ ����� � ����.
  2. ������� ������������ �������� �����, ������� ����� � *���������*. ����� ������ ��������� �� ����� ��������� ������.

������:

![](img/ExampleNL.png?raw=true)

���� 1, 2, 4 �������� ������������ ����.

### ������
����� ���������� �������� ����� *n* <html>&rarr;</html> *d*. ��� ����� ����� ������������ ���� ������������ ��� ��������� �����, � ������� ������:
  1. ��� ���� *d*
  2. ��� ����, ������� ����� ������� *n*, �� ������� ����� *d*. � ������ ������ ���� *d* �������� ���������� �����.

�� ������ ���� ��������� ���������� ����� ��� �������� ����, ����� ��� ������� ����� ��������� ��������� ��������:

#### �������� ���������� ������������� ����� �������� ����:

**����:** ���� ������ *G* � �������� ���� *n* <html>&rarr;</html> *d*.

**�����:** ��������� *loop*, ��������� �� ���� ����� ������������� ����� *n* <html>&rarr;</html> *d*.

**�����:** ������� *loop* ���������������� ���������� { *n*, *d* }. ���� *d* ���������� ��� "�����������", ����� ����� �� �������� ������ *d*. �������� ����� � ������� �� �������� ����� ������, ������� � *n*. ������ ��� ���������� ��� ���� ������ ���� � *loop*. ����� ��������� ��������� ����� ��� ����, ����������� *n*, ����� *d*.

### ������� ������
- ���� ������ ����������

### �������� ������
- ���� "�������� ����� - ��������������� ����� ����� ��������� ������� ������, �������� � ������������ ����"

### ������������ ��������� ������
- `ISet<int> Loop` - ��������� ������� ������� ������, �������� � ����
- `Stack<int> Stack` - ����
- `Dictionary<Edge<BasicBlock>, EdgeType> classify` - ������������� ������, �������� ������������� �����
- `Dictionary<Edge<BasicBlock>, ISet<int>> result` - ������������� ������, �������� ���������

### ���������� ���������

```cs
// ��������������� ��������
private static void Insert(int m)
{
    if (!Loop.Contains(m))
    {
        Loop.Add(m); // ���������� ���� � ����
        Stack.Push(m); // ���������� ���� � ����
    }
}

// �������� ��������
public static Dictionary<Edge<BasicBlock>, ISet<int>> FindAllNaturalLoops(ControlFlowGraph.Graph g)
{
    var classify = EdgeClassification.EdgeClassification.ClassifyEdge(g); // ���������� ������������� ���� 
    var result = new Dictionary<Edge<BasicBlock>, ISet<int>>(); // ������������� ����
    foreach (var pair in classify)
    {
       if (pair.Value == EdgeClassification.Model.EdgeType.Retreating) // �������� ����������� ������ ��� ����������� ���� (� ������ ������, � ��������)
       {
          Stack = new Stack<int>(); // ������������� �����
          Loop = SetFactory.GetSet(new int[] { pair.Key.Target.BlockId }); 
          Insert(pair.Key.Source.BlockId); // ���������� � ���� � ���� �����, �������� � �������� ����� 
          while (Stack.Count() > 0)
          {
             int m = Stack.Pop();
             foreach (BasicBlock p in g.getParents(m)) // ���������� � ���� � ���� ���� ���������������� ������� ����
                Insert(p.BlockId);
          }
          result.Add(pair.Key, Loop);
       }
     }
     return result;
}
```

### ������ �������������

```cs
var allNaturalLoops = SearchNaturalLoops.FindAllNaturalLoops(g);
foreach (var loop in allNaturalLoops)
{
    // � ������� ��������� ���� � ����� ��������� �����, � ����� ������ ������, �������� � ������������ ����
    Console.Write(loop.Key.Source.BlockId + " -> " + loop.Key.Target.BlockId + " : ");
    foreach (int node in loop.Value)
        Console.Write(node.ToString() + " ");
    Console.WriteLine("");
}
```

### ����

*���������*:
```
i = 10;
1: i = i - 1;
if i > 0
  goto 1;
```

*���� ������ ����������*:

![](img/TestNL.png?raw=true)

������������ ���� � ������ ������ ����, � ���� ������ ���� 1 � 2.

*����� ���������*:

������ ������: "����� ��������� ����� - ���� ��������� ����� - ������ ����� �� �����"

2 -> 1 : 1 2

# 22: ���������� ������ "�������" � �������������: ����� "������� ����" � ����� "������� �����"

### ��������� ��������:
*NoName (����� ������, ������� ������)*

### �� ����� �������� �������:
1. ������� ����� � �������� ���������

### ��������� �������:
1. ������������ ������������������ �������� � ���������� �������
2. ���������� ������ ��� �������� ������������ ������� ��� ��������� ������� �� ������ ��������
3. ���������� ����� ��������� ������� �� ������ ��������
4. ���������� ����� ��������� ������� �� ������ ��������

### ���������� ������:
� ������ ������ ��������� ����������� ����� "�������". ����� ���������� ����������� ������������� ������ "�������": ����� "������� ����" � ����� "������� �����". ��� ��� ������� ���� ��������������� ��� �������, �� ���������� ����� ����������� ����� "�������-����"

### ������
� �������� ������� �� ������ �������� ��������� ��������������� ��� ��������  ������� ����� ����� ������� ������� ����� ������, �������� ������������ ����� �����.
������ ���������� � ������ ����������������� ��������� �������� ��������, ��������� ����� ���������� ����� ������� ���������� ������ ����� �� ������. ������ ������� �����������
���������� ������������� ������ � �������� ��������.

��������� ������� ����� ������ ������������ ����� ����� ����� *N* � ����� *E*, �����, ���
1. ���������� ��������� *h <html>&#8712;<html> N*, ������������ ��� ����� ������ � *N*;
2. ���� ��������� ���� *m* ����� ������� ���� *n <html>&#8712;<html> N*, ����� *h*, �� *m* ����� ������ � *N*;
3. *E* - ��������� ���� ����� ������ ���������� ����� ������ *n1* � *n2* �� *N*, �� �����������, ��������, ��������� �����, �������� � *h*.
	
*����* ����� *L* (��� ���� � �����, �� ����������� �������� ����� � ���������) �������� �����, �������������� ������� *R*. �����, ������� � ��������� *L*, ������ ������ � ����
R. ����� �� ������ ������ �� *L* ���������� ������ �� *R* � �� �� ����� ����� ����������. ������ ���� ����� �������� ��������, �� ��� ���������� ������ � *R*.
������� *R �������� ����*.

������������ ������� ������� ���� �� ������� ����� ����������� � ���, ��� ��������� �������� �������� ����� � ��������� ����� L. 

### ������������ ��������� ������
- `List<int> OutputBlocks` - ������ �������� ������� ������

### ����������

```cs
// ����������� ����� "�������"
public abstract class Region
{
    public abstract List<int> OutputBlocks { get; }
}

// ��������������� ����� IntermediateRegion
public class IntermediateRegion : Region
{
    public BasicBlock Header { get; set; }

    List<int> outputBlocks;
    public override List<int> OutputBlocks { get { return outputBlocks; } }
    public IntermediateRegion(BasicBlock header, List<int> outputBlocks)
    {
        Header = header;
        this.outputBlocks = outputBlocks;
    }

    protected bool Equals(IntermediateRegion other)
    {
        return Equals(Header, other.Header) && OutputBlocks.SequenceEqual(other.OutputBlocks);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((IntermediateRegion) obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return ((Header != null ? Header.GetHashCode() : 0) * 397) ^ (OutputBlocks != null ? OutputBlocks.GetHashCode() : 0);
        }
    }
}

// ����� "������� ����"
public class BodyRegion : IntermediateRegion
{
    public List<Region> Regions { get; set; }

    public BodyRegion(BasicBlock header, List<int> outputBlocks, List<Region> regions) : base(header, outputBlocks)
    {
        Regions = regions;
    }

    protected bool Equals(BodyRegion other)
    {
        return Regions.SequenceEqual(other.Regions);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((BodyRegion) obj);
    }

    public override int GetHashCode()
    {
        return (Regions != null ? Regions.GetHashCode() : 0);
    }
}

// ����� "������� �����"
public class LoopRegion : IntermediateRegion
{
    public BodyRegion Body { get; set; }

    public LoopRegion(BasicBlock header, List<int> outputBlocks, BodyRegion body) : base(header, outputBlocks)
    {
        Body = body;
    }

    public LoopRegion(BodyRegion body) : base(body.Header, body.OutputBlocks)
    {
        Body = body;
    }

    protected bool Equals(LoopRegion other)
    {
        return base.Equals(other) && Equals(Body, other.Body);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((LoopRegion) obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (base.GetHashCode() * 397) ^ (Body != null ? Body.GetHashCode() : 0);
        }
    }
}

// ����� "������� ����". ������ ������� ���� ��������������� ��� �������-����
public class LeafRegion : Region
{
    public BasicBlock Block { get; set; }

    public override List<int> OutputBlocks
    {
        get
        {
            return Block.OutputBlocks.Count > 0 ? new List<int> { Block.BlockId } : new List<int>() ;
        }
    }
    
    public LeafRegion(BasicBlock block)
    {
        Block = block;
    }

    protected bool Equals(LeafRegion other)
    {
        return Equals(Block, other.Block);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((LeafRegion) obj);
    }

    public override int GetHashCode()
    {
        return (Block != null ? Block.GetHashCode() : 0);
    }
}
```

### ������ �������������

```cs
foreach (Region r in regions)
{
    LeafRegion leaf = r as LeafRegion;
    if(leaf != null)
    {
        result[r, RegionDirection.In, r] = Identity;
        result[r, RegionDirection.Out, r] = input => param.TransferFunction(input, leaf.Block);
    }
    BodyRegion body = r as BodyRegion;
    if(body != null)
        foreach(Region s in body.Regions)
        {
            result[r, RegionDirection.In, s] = input => GatherFunctionsResults(input, result, r, body.Header.InputBlocks, graph, param);
            CalculateForOutputBlocks(result, r, s, s.OutputBlocks, graph);
        }
    LoopRegion loop = r as LoopRegion;
    if(loop != null)
    {
        result[r, RegionDirection.In, loop.Body] = input => SetFactory.GetSet<V>(input.Union(GatherFunctionsResults(input, result, loop.Body, loop.Header.InputBlocks, graph, param)));
        CalculateForOutputBlocks(result, r, loop.Body, loop.OutputBlocks, graph);
    }
}
```

# 23: ���������� ����� ��������� ������� �� ������ ��������

### ��������� ��������:
*Ampersand (�������� �����, �������� ����)*

### �� ����� �������� �������:
1. 

### ��������� �������:


### ���������� ������:
� ������ ������ ��������� ����������� �����, ������� ��������� ������� �������� 

### ������

### ������������ ��������� ������
- ` Dictionary<Edge<BasicBlock>, EdgeType>` - ������� ������������������ �����

### ����������

```cs

```

### ������ �������������

```cs

```

# 24: ������������ ���������� ������������������ ��������

### ��������� ��������
*PiedPiper (������ ����, ���������� ������)*

### �� ����� �������� �������
1. ���� ������ ����������
2. ������ ����������� ������������ �����
3. ����� ������������ ������
4. ���������� ������ "�������" 

### ��������� �������
1. ���������� ����� ��������� ������� �� ������ ��������

### ������
� �������� ������� �� ������ �������� ��������� ��������������� ��� �������� ��������(��������), ������� ����� ������, ����� ������� 
������� ����� ������, �������� ������������ ����� �����.
��������� ������� ����� ������������ ����� ����� ����� *N* � ����� *E* �����, ���

- ���������� ��������� *h <html>&#8712;<html> N*, ������������ ��� ����� ������ � *N*
- ���� ��������� ���� *m* ����� ������� ���� *n <html>&#8712;<html> N*, ����� *h*, �� *m* ����� ������ � *N*
- *E* - ��������� ���� ����� ������ ���������� ����� ������ *n1* � *n2* �� *N*, �� �����������, ��������, ��������� �����, �������� � *h*

� �������� ���������� �������� �������� �������������� ������������ �����.
������� ���������� � ����, ��� ������ ������� ���� ��������������� ��� ������� (LeafRegion).
����� ������������ ����� ��������������� ������� ������, �.�. ������� � �������� ���������� �����. 

��� ������� ����� ���������� ��� �������

 - R -- ������� ���� (BodyRegion). ���� ����� *L* (��� ���� � �����, �� ����������� �������� ����� � ���������) �������� �����, �������������� ������� *R*. �����, ������� � ��������� *L*, ������ ������ � ����
R. ����� �� ������ ������ �� *L* ���������� ������ �� *R* � �� �� ����� ����� ����������. ������ ���� ����� �������� ��������, �� ��� ���������� ������ � *R*.

 - LR -- ������� ����� (LoopRegion).
������������ ������� ������� ���� �� ������� ����� ����������� � ���, ��� ��������� �������� �������� ����� � ��������� ����� L. 

### ������� ������
���� ������ ���������� ```Graph g```

### �������� ������
������ �������� � ���������� ������� (```List < Region > regionList```)

### ������������ ��������� ������
 - ``` List<Region> regionList ``` -- ������ �������� � ���������� �������
 - ```Dictionary<BasicBlock, Region> basicBlockLastRegion``` -- �������, �������� ������������ ������� ���� - ����� ������� ������, � ������� �� �����
 - ```Dictionary<Edge<BasicBlock>, bool> regionMade```-- �������, �������� ������������ �������� ���� - ����� ��, ��� ���� �� ��� ��� �����������

### ���������� ���������

�������� ������ �� ����������� �������������� � ������� ���������������� ������
```
private bool checkLoopInclusion(KeyValuePair<Edge<BasicBlock>, ISet<int>> curLoop,
                  KeyValuePair<Edge<BasicBlock>, ISet<int>> loopOther, bool regionMade)
{
  if (loopOther.Value.IsSubsetOf(curLoop.Value) && !regionMade)
    return true;
  else
    return false;
}
```

�������� ������ �� ���������� ���������� ������������������ �������� �������� � ������
```public List<Region> CreateSequence(Graph g)```

``` 
public List<Region> CreateSequence(Graph g)
{
```
�������� ������������ �����
```
    if (!CheckRetreatingIsReverse.CheckRetreatingIsReverse.CheckReverseEdges(g)){
        Console.WriteLine("there are some retreating edges which aren't reverse");
        Environment.Exit(0);
    }
```
���������� ������� ������ ��� LeafRegion � ������ ��������
```
    var basicBlockLastRegion = new Dictionary<BasicBlock, Region>();

    foreach (var v in g.GetVertices())
    {
        var newReg = new LeafRegion(v);
        regionList.Add(newReg);
        basicBlockLastRegion[v] = newReg;
    }

    var loops = SearchNaturalLoops.FindAllNaturalLoops(g);

    var regionMade = new Dictionary<Edge<BasicBlock>, bool>();

    foreach (var loop in loops)
    {
        regionMade[loop.Key] = false;
    }
```
���� �������� ������������ � ������ �������� �����, ����� ���������� ������
```
    while (regionMade.ContainsValue(false))
    {
        foreach (var loop in loops)
        {
```
�������� ����� �� ������������� ��������� � ���� � ��� �� ������������ ������
```
            bool anyInsideLoops = false;
            foreach (var loopOther in loops)
            {
                anyInsideLoops = anyInsideLoops || checkLoopInclusion(loop, loopOther, regionMade[loopOther.Key]);
            }
            if (!anyInsideLoops) continue;
```
���� ��� ��������� ����� ��� ���������� ��� �� ���, ���������� � ������������ ����� �������
��� ������������ BodyRegion ��������� 
- BasicBlock header - �� �������� Target �������� ����, ����������� ����
- List<Region> curRegions - ����������� � ������� ������� basicBlockLastRegion
- List<int> outputBlocks (�����, �� ������� ���� ���� � ������ ������� - ����������� � ������� ������� OutputBlocks ������� �����
```
            regionMade[loop.Key] = true;

            var header = loop.Key.Target;

            var curRegions = new List<Region>();
            var outputBlocks = new List<int>();
            foreach (var blockId in loop.Value)
            {
                var block = g.getBlockById(blockId);
                if (!curRegions.Contains(basicBlockLastRegion[block]))
                    curRegions.Add(basicBlockLastRegion[block]);

                foreach (var outputBlock in block.OutputBlocks)
                {
                    if (!loop.Value.Contains(outputBlock))
                    {
                        outputBlocks.Add(outputBlock);
                        break;
                    }
                }
            }

            var bodyReg = new BodyRegion(header, outputBlocks, curRegions);
            regionList.Add(bodyReg);

            var loopReg = new LoopRegion(bodyReg);
            regionList.Add(loopReg);

            foreach (var blockId in loop.Value)
            {
                var block = g.getBlockById(blockId);
                basicBlockLastRegion[block] = loopReg;
            }
        }
    }
```
���� ��������� �� �������� ������, �� ����������� ��������� ������, ���������� � ���� ��� ���������
```
    foreach (var block in basicBlockLastRegion)
    {
        if (block.Value.GetType() == typeof(LeafRegion))
        {
            var header = g.getRoot();
            var outputBlocks = new List<int>();
            var curRegions = new List<Region>();
            foreach (var curblock in basicBlockLastRegion)
            {
                if (!curRegions.Contains(curblock.Value))
                    curRegions.Add(curblock.Value);
            }
            var newReg = new BodyRegion(header, outputBlocks, curRegions);
            regionList.Add(newReg);
            break;
        }
    }
 ```
 
 ### ����
 ���������
 ```
 i = 1; 
 j = 4; 
 a = 2; 
 while i < 20 
 { 
 i = i + 1; 
 j = j + 1; 
 if i > a 
 a = a + 5; 
 while j < 5 
 { 
 a = 4; 
 } 
 i = i + 1; 
 }
```

���� ������ ����������, ��������������� ���������
```
0:
<-- 
-->  1
1:
<--  0 7
-->  2 8
2:
<--  1
-->  3 4
3:
<--  2
-->  4
4:
<--  2 3
-->  5
5:
<--  4 6
-->  6 7
6:
<--  5
-->  5
7:
<--  5
-->  1
8:
<--  1
--> 
```

������ �������� � ���������� ������������������
``` 
R0 Leaf - 0
R1 Leaf - 1
R2 Leaf - 2
R3 Leaf - 3
R4 Leaf - 4
R5 Leaf - 5
R6 Leaf - 6
R7 Leaf - 7
R8 Leaf - 8
R9 Body - R5, R6
R10 Loop - R9
R11 Body - R1 R2 R3 R4 R10 R7
R12 Loop - R11
R13 Body - R0 R11 R8
```

# 25: ���������� ����� ��������� ������� �� ������ ��������.

### ��������� ��������:
*EndFrame (������ ����,  ������� ���������)*

### ���������� ������:
���������� ����������� ���������� ����� ��������� ������� �� ������ �������� � ������ RegionsAlgorithm.

### �� ����� �������� �������:
1. Region
2. Graph
3. IterativeAlgorithm
4. IterativeAlgorithmParameters

### ��������� �������:
1. IterativeAlgorithm

### ������
```
���������� ����� ���������:
IN[Rn] = IN[�����]
��� ������� ������� Ri � ���������� �������
  {
    IN[Ri] = fR(i-1), IN[Ri](IN[R(i-1)])
    OUT[Ri] = transferfunction(IN[Ri])
  }
```

### ������� ������:
 - List<Region> regions - ������ ��������
 - TransferFunctionStorage<ISet<V>> functions - ������������ �������
 - SetIterativeAlgorithmParameters<V> param - ��������� ������������� ���������
 - Graph graph - ���� ���������
 
### �������� ������:
 - IterativeAlgorithmOutput<ISet<V>> - ��������� ������ � ������� ��� ������� �����

 ### ���������� ���������:

 ``` C#
 /* ���������� ���������� ����� ��������� ������� �� ������ ��������. � ������ RegionsAlgorithm */
 
 static IterativeAlgorithmOutput<ISet<V>> ApplyDescendingPart<V>(List<Region> regions,
            TransferFunctionStorage<ISet<V>> functions, SetIterativeAlgorithmParameters<V> param, Graph graph)
        {
            Dictionary<int, ISet<V>> regionsInputs = new Dictionary<int, ISet<V>>();
            IterativeAlgorithmOutput<ISet<V>> result = new IterativeAlgorithmOutput<ISet<V>>();

            Dictionary<Region, int> RegionIndexes = new Dictionary<Region, int>();
            for (int i = 0; i < regions.Count; ++i)
            {
                RegionIndexes[regions[i]] = i;
            }

            int lastIndex = RegionIndexes[regions.Last()];
            int prevIndex = lastIndex;
            regionsInputs[lastIndex] = param.FirstValue;

            foreach (var r in regions.Reverse<Region>())
            {
                int curIndex = RegionIndexes[r];
                if (curIndex != lastIndex)
                {
                    regionsInputs[curIndex] = functions[regions[prevIndex], RegionDirection.In, regions[curIndex]](regionsInputs[prevIndex]);
                }
            }

            int numOfBlocks = graph.Count();
            for(int i = 0; i < numOfBlocks; ++i)
            {
                int curBlockId =  regions[i].OutputBlocks.First();
                var curBlock = graph.getBlockById(curBlockId);

                result.In[curBlockId] = regionsInputs[i];
                result.Out[curBlockId] = param.TransferFunction(result.In[curBlockId], curBlock);
            }

            return result;
        }
        ...
 ``` 
 
### ������ �������������

### ����
���������
```cs 
 i = 1;
    j = 4;
    a = 2;
    while i < 20
    {  
        i = i + 1;
        j = j + 1;
        if i > a
            a = a + 5;
        i = i + 1;
    }
```

```

```

# 26: �������� ������: �������� ������������ �������

### ��������� ��������: YACT

### ���������� ������: 

���������� ���������� ��������� ��� ������������ �������, ����������� ��������� � �������� ������������ ������� �� ����� `Region1, Direction, Region2`. 

### �� ����� �������� �������:

  - ������� �����
  - �������

### ��������� �������:

  - �������� ����������� � ������� ��������

### ������

��� ���������� ������� � �� ��������� ���������� ������� ����, �� �������� ����� ������������������ ������������ �������. ���� ������� �� ���� �������� � �����������. ��� ���������� ��������� ������ ������������ ������ `Equals(obj)` � `GetHashCode()`. �������� ������ ������ ���� ����������� � ������� �������� � � ����� �����. 

### ������� ������:
 - ������������ �������

### �������� ������:
 - ������������ �������

### ������������ ��������� ������

- `Dictionary<TKey, TValue>` - ��� �������� ������������ �������

### ���������� ���������

��� ������� `BodyRegion`, `IntermediateRegion`, `LeafRegion`, `LoopRegion` ���� ����������� ������ `Equals` � `GetHashCode`. ��� ���������� ����������� ��������� �������:

- �������, ��������� �� ������ ����� ������������ �� �����
- �������, ���������� � ���� ������ ������, ������������ � ������� ������ `SequenceEqual`
- ��� �������-����������� `IntermediateRegion` ���������� �������� ����� `base.Equals`
- `GetHashCode` ��������������� � ������ �����, ������������� ��� ��������� �� ��������� � �������� ����������� `int`, ����� �������� �������� ���������� ���.

### ������ �������������

``` C#
// setting functions
foreach (Region r in regions)
{
    LeafRegion leaf = r as LeafRegion;
    if(leaf != null)
    {
        result[r, RegionDirection.In, r] = Identity;
        result[r, RegionDirection.Out, r] = input => param.TransferFunction(input, leaf.Block);
    }
...
}

// getting functions
foreach (var r in regions.Reverse<Region>())
{
    int curIndex = RegionIndexes[r];
    if (curIndex != lastIndex)
    {
        regionsInputs[curIndex] = functions[regions[prevIndex], RegionDirection.In, regions[curIndex]](regionsInputs[prevIndex]);
    }
}
```

### ����
``` C#
string text = @"
    i = 1;
    j = 4;
    a = 2;
    while i < 20
    {  
        i = i + 1;
        j = j + 1;
        if i > a
            a = a + 5;
        i = i + 1;
    }";

SyntaxNode root = ParserWrap.Parse(text);
var graph = new Graph(BasicBlocksGenerator.CreateBasicBlocks(ThreeAddressCodeGenerator.CreateAndVisit(root).Program));
var regions = new RegionSequence().CreateSequence(graph);
var storage = new AbstractTransferFunctionStorage<int>();
for (var i = 0; i < regions.Count - 1; i++)
{
    storage[regions[i], RegionDirection.Out, regions[i + 1]] = 2 * i;
    storage[regions[i], RegionDirection.In, regions[i + 1]] = 2 * i + 1;
}

for (var i = 0; i < regions.Count - 1; i++)
{
    Assert.IsTrue(storage[regions[i], RegionDirection.Out, regions[i + 1]] == 2 * i);
    Assert.IsTrue(storage[regions[i], RegionDirection.In, regions[i + 1]] == 2 * i + 1);
}
```

# 27: �������� ������: Meet Over All Paths

### ��������� ��������: YACT

### ���������� ������: 

���������� ����������� �������� Meet Over All Paths �� ������������ ����� ������ ������ ��� ���������� �����������. � ����� ���������� ���������� ������ ����� ��������� � ������������ ������������� ���������� ��� ���� ������������.

������ ������ ������ ������, ������� ������ ���� ����� �� ������� ����� �� ������� �������� �����. �� �������� ��������, ��� ��� �������������� ���� �������� ������ ������ ��������� ������ Meet Over All Paths ��������� � ����������� ������ ������������� ���������, � ��� �� ��������������(��������������� ��������) �� ���������. 

### �� ����� �������� �������:

  - Control Flow Graph
  - Basic Block Model
  - Iterative Algorithm Parameters

### ��������� �������:

���

### ������

**�����������**: ��� ������� ���� *p*, ���������� �� ������� ������ *B<sub>i</sub>* ���������� *f<sub>p</sub>(V<sub>����</sub>)*, ��� *f<sub>p</sub>* ��� ���������� ������������ ������� ������� ������ �� ���� *p*. ��������� �������� ������

IDEAL[B] = ? f<sub>p</sub>(V<sub>����</sub>), ��� ? ��� �������� ����� �� ��������� ����� �� ����� � B

���������� ������ �1

![](img/MOP_Example1.png)

�� ����� ������� �����, ��� �� ���� ������ � sqr ������ ������� ����� ���� ��������, � ����� ���. ������� ����� ������������� Meet Over All Paths(MOP)

MOP[B] = ? f<sub>p</sub>(V<sub>����</sub>), ��� ? ��� �������� ����� �� ���� ����� �� ����� � B

������� Meet Over All Paths � ������������ ����������(Maximum Fix Point) �� ������� ���������� �����

![](img/MOP_Example2.png)

MOP[B] = f<sub>B3</sub>(f<sub>B1</sub>(V<sub>����</sub>)) ? f<sub>B3</sub>(f<sub>B2</sub>(V<sub>����</sub>))

MFP[B] = f<sub>B3</sub>(f<sub>B1</sub>(V<sub>����</sub>) ? f<sub>B2</sub>(V<sub>����</sub>))

**�����������**: ���� ����� �������� ������ ������ �������������, �� **MFP[B] = MOP[B]** 

**��������� 1**: ����� �������� ������ ������������� <=> f - �������������

**��������� 2**: ������ ����� ����� **MFP[B]?MOP[B]?IDEAL[B]**

### ������� ������:
 - ������������ ���� ������ ������ ���������
 - ��������� ���������� �����������

### �������� ������:
 - ������� ������ ������ ������ ��� ���������� �����������

### ������������ ��������� ������

 - ������� �� �������������� ����� � ��������� ������ ������������ �������
 - ���� ������ ������
 - ����������� ����� ���������� ���������

### ���������� ���������

```C#
var MOP = new Dictionary<int, V>(); // ���������

foreach (BasicBlock blockTo in graph) // ��� ������� ����� ���������������� ������������
{
  MOP[blockTo.BlockId] = param.StartingValue;
  foreach (var path in GraphAlgorithms.FindAllPaths(graph, blockTo.BlockId)) // ��� ������� ���� �� ������ �� ����������� ����� �������� ���������� ������������ ������� � �������� ���������� ��������� �����
  {
    var value = path.Aggregate(param.FirstValue, param.TransferFunction);
    MOP[blockTo.BlockId] = param.GatherOperation(new List<V> { MOP[blockTo.BlockId], value});
  }
}
```

### ������ �������������

��������� �� �����:

```C#
if 1
{
    x = 2;
    y = 3;
}
else
{
    x = 3;
    y = 2;
}
z = x + y;
```

����� ���������:

```C#
// ������� ��������� � ��������� CFG
...
var constantPropagationMOP = MeetOverPaths.Apply(graph, new ConstantsPropagationParameters());
var it = constantPropagationMOP.Out.Select(
  pair => $"{pair.Key}: {string.Join(", ", pair.Value.Select(ex => ex.ToString()))}");
```

�����:

```C#
50: 
51: [x, 2], [y, 3]
52: [x, 3], [y, 2]
53: [x, NAC], [y, NAC], [t0, 5], [z, 5]
```

### ����

```C#
// ��������� ����� �� ������ ����������� AvailableExpression
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

// ��������� CFG
SyntaxNode root = ParserWrap.Parse(programText);
var threeAddressCode = ThreeAddressCodeGenerator.CreateAndVisit(root).Program;
var basicBlocks = BasicBlocksGenerator.CreateBasicBlocks(threeAddressCode);
Graph g = new Graph(basicBlocks);

// �������� ���������� ������� �����������
var availableExprsIterative = IterativeAlgorithm.Apply(g, new AvailableExpressionsCalculator(g));
var availableExprsMOP = MeetOverPaths.Apply(g, new AvailableExpressionsCalculator(g));

// ����� ���������� �����������
var it = availableExprsIterative.Out.Select(
  pair => $"{pair.Key}: {string.Join(", ", pair.Value.Select(ex => ex.ToString()))}");
foreach (var outInfo in it)
{
  Trace.WriteLine(outInfo);
}
var mop = availableExprsMOP.Select(
  pair => $"{pair.Key}: {string.Join(", ", pair.Value.Select(ex => ex.ToString()))}");
Trace.WriteLine("====");
foreach (var outInfo in mop)
{
  Trace.WriteLine(outInfo);
}

// ���������� �� ���������� ���������� ��������� ������
Assert.IsTrue(availableExprsIterative.Out.OrderBy(kvp => kvp.Key).
  Zip(availableExprsMOP.OrderBy(kvp => kvp.Key), (v1, v2) => v1.Key == v2.Key && v1.Value.SetEquals(v2.Value)).All(x => x));
```