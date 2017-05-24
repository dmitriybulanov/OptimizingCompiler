# Передаточная функция для задачи распространения констант

### Выполнено командой:
*EndFrame (Аммаев Саид,  Пирумян Маргарита)*

### Постановка задачи:
Необходимо реализовать функцию для задачи распространения констант.

### От каких проектов зависит:
1. Трехадресного кода
2. Базового блока

### Зависимые проекты:
1. Итерационный алгоритм

### Теория
```
После применения передаточной функции к значению потока данных получаются новые.

Поток данных - m
Новое значение потока данных - m'
Свойства значения потока данных и  правила вычисления передаточной функции:
1. m1 ^ m2 = m <=> m1(v) ^ m2(v) = m(v), для любых v
2. m1 < m2 <=> m1(v) < m2(v) , для любых v
  fs(m) = m', где s - команда трехадресного кода
  2.1. если s - не  присваивание, то m' = m
  2.2. если s - присваивание, то для любого v != x : m'(v) = m(v)
3. если v = x
  3.1. x := c
      m'(x) = c
  3.2. x := y + z
      m'(x) = m(y) + m(z), если m(y) и m(z) - const
      m'(x) = NAC, если m(y) или m(z) - NAC
      m'(x) = UNDEF - в остальных случаях
  3.3. x := f(...), где f - оператор вызова функции
      m'(x) = NAC
```

### Входные данные:
 - Dictionary<string, string> input - значение потока данных
 - BasicBlock block - базовый блок(ББЛ)
 - int commandNumber - номер команды в ББЛ

### Выходные данные:
 - Dictionary<string, string> - новое значение потока данных

 ### Реализация алгоритма:

 ``` C#
 /* реализация передаточной функции в классе
 public class ConstantsPropagationParameters : CompositionIterativeAlgorithmParameters<Dictionary<string, string>> */
 
 public override Dictionary<string, string> CommandTransferFunction(Dictionary<string, string> input, BasicBlock block, int commandNumber)
        {
            //получение команды по номеру из ББЛ
            ThreeAddressCommand command = block.Commands[commandNumber];
            //если присваивание
            if (command.GetType() == typeof(Assignment))
            {
                string newValue = NAC;
                Expression expr = (command as Assignment).Value;
                if (expr.GetType() == typeof(Int32Const) || expr.GetType() == typeof(Identifier))
                    newValue = getConstantFromSimpleExpression(input, (expr as SimpleExpression));
                //если унарная операция
                else if (expr.GetType() == typeof(UnaryOperation))
                {
                    UnaryOperation operation = (expr as UnaryOperation);
                    newValue = calculateVal(getConstantFromSimpleExpression(input, operation.Operand), operation.Operation);
                }
                //если бинарная операция
                else if (expr.GetType() == typeof(BinaryOperation))
                {
                    BinaryOperation operation = (expr as BinaryOperation);
                    newValue = calculateVal(getConstantFromSimpleExpression(input, operation.Left), getConstantFromSimpleExpression(input, operation.Right), operation.Operation);
                }
                string leftOperand = (command as Assignment).Target.Name;
                input[leftOperand] = newValue;
            }
            //если не присваивание, вернется входной поток данных
            return input;
        }
        
        //вспомогательная функция, которая возвращает константу по значению входного потока данных и выражению
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
 
 //вспомогательные функции calculateVal реализованы другой командой

### Пример использования

### Тест
Программа
```cs 
a = 4;
b = 4;
c = a + b;
if 1 
  a = 3;
else
  b = 2;
print(c);
```

```

```

