# C# Lambda 運算式

`Lamada` 表達式寫法，主要用來省略繁瑣表達式或運算式
<https://learn.microsoft.com/zh-tw/dotnet/csharp/language-reference/operators/lambda-expressions>

## 基本

運算式，針對只有一行程式碼
```C#
//運算式 (input-parameters) => expression
x => x * x;
//範例
Func<int, int> square = x => x * x;
System.Linq.Expressions.Expression<Func<int, int>> e = x => x * x;
```

陳述式，{}當作程式區塊撰寫
```C#
//陳述式 (input-parameters) => { <sequence-of-statements> }
name => { ... };
//範例
Action<string> greet = name =>
{
    string greeting = $"Hello {name}!";
    Console.WriteLine(greeting);
};
greet("World");
```

運算式例子
```C#
//無參數使用()代替
Action line = () => Console.WriteLine();
//只有一個參數時，可以省略()
Func<double, double> cube = x => x * x * x;
//超過一個參數時，必須使用()
Func<int, int, bool> testForEquality = (x, y) => x == y;
//參數的型態如果不一致(無法判斷)，則需要明確定義
Func<int, string, bool> isTooLong = (int x, string s) => s.Length > x;
//C# 9.0，如果不使用參數可以 _ 省略參數
Func<int, int, int> constant = (_, _) => 42;
//C# 12.0，可以使用參數的預設值
var IncrementBy = (int source, int increment = 1) => source + increment;
//C# 12.0，可以使用 params 可變參數長度
var sum = (params int[] values) =>
{
    int sum = 0;
    foreach (var value in values) 
        sum += value;
    
    return sum;
};
var total = sum(1, 2, 3, 4, 5);
```

針對類別成員寫法
```C#
//變數指派
string age => "10";

//成員 get/set 方法
string? _Name;
string? Name
{
    get => _Name; //{get{return _Name;}}
    set => _Name = value; //{set{_Name = value;}}
}

//建構式略寫
LambdaClass(string name) => Name = name; //LambdaClass(string name){Name = name;}

//方法略寫
string NameTpye() => GetType().Name; // string NameTpye(){return GetType().Name;}

//運算子略寫
~LambdaClass() => Console.WriteLine($"The {ToString()} finalizer is executing.");

//自訂陣列索引略寫
private string[] types = { "Baseball", "Basketball", "Football",
                      "Hockey", "Soccer", "Tennis", "Volleyball" };
public string this[int i]
{
    get => types[i];
    set => types[i] = value;
}
//LambdaArray[i];
```

針對委派的寫法 `Func` `delegate`
```C#
//C# 9.0，捨棄參數 _
// lambda： (_, _) => 0 、 (int _, int _) => 0
// 匿名方法： delegate(int _, int _) { return 0; }

//委派略寫(參數唯一可以省略())
Func<int> tan = () => 10; // static int tan() { return 10; }
Func<int, int> square = x => x * x; // static int square(int x) { return x * x; }
Func<int, int, bool> equality = (x, y) => x == y;  //static bool equality(int x, int y) { return x == y; }
Func<int, string, bool> isLong = (int x, string s) => s.Length > x;  //static bool isLong(int x, string s){return s.Length > x;}

//委派解構元組
Func<(int, int, int), (int, int, int)> doubleThem = ns => (2 * ns.Item1, 2 * ns.Item2, 2 * ns.Item3);  //static (int, int, int) doubleThem(int a, int b, int c) { return (2 * a, 2 * b, 2 * c); }
// doubleThem((2, 3, 4));  //(4, 6, 8)

//委派參數回傳添加屬性
Func<string?, int?> parse =[Obsolete("abc")] (s) => (s is not null) ? int.Parse(s) : null;
//var concat = ([DisallowNull] string a, [DisallowNull] string b) => a + b;

//樹狀結構 委派略寫
Expression<Func<int, int>> exp = x => x * x;  //static int exp(int x) { return x * x; }

//方法參數是委派，則可以參數略寫
int[] numbers = { 2, 3, 4, 5 };
numbers.Select(x => x * x);
numbers.Count(n => n % 2 == 1);

//委派多行略寫
Action<string> greet = name => { Console.WriteLine($"Hello {name}!"); };

//委派可以使用外部變數，即使變數可能被回收
int j = 0;
Func<int, bool>? isEqualToCapturedLocalVariable = x => x == j;

//委派限制外部變數必須式 static
Func<double, double> square = static x => x * x;
```

## 進階

參數，視作 `Func` 傳入
```C#
//參數方法
int[] numbers = { 2, 3, 4, 5 };
var squaredNumbers = numbers.Select(x => x * x);
```

元祖，回傳或傳入的參數使用元組
```C#
//元祖的 Item1 Item2
Func<(int, int, int), (int, int, int)> doubleThem = ns => (2 * ns.Item1, 2 * ns.Item2, 2 * ns.Item3);
var numbers = (2, 3, 4);
var doubledNumbers = doubleThem(numbers);
Console.WriteLine($"The set {numbers} doubled: {doubledNumbers}");
// Output:
// The set (2, 3, 4) doubled: (4, 6, 8)

//元祖的 Item1 Item2 欄位命名
Func<(int n1, int n2, int n3), (int, int, int)> doubleThem = ns => (2 * ns.n1, 2 * ns.n2, 2 * ns.n3);
```

屬性的寫法
```C#
//C# 10，可以使用屬性
Func<string?, int?> parse = [ProvidesNullCheck] (s) => (s is not null) ? int.Parse(s) : null;

//屬性使用在輸入參數或傳回值
var concat = ([DisallowNull] string a, [DisallowNull] string b) => a + b;
var inc = [return: NotNullifNotNull(nameof(s))] (int? s) => s.HasValue ? s++ : null;
```

非同步 `async` `await`
```C#
public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
        button1.Click += async (sender, e) =>
        {
            await ExampleMethodAsync();
            textBox1.Text += "\r\nControl returned to Click event handler.\n";
        };
    }

    private async Task ExampleMethodAsync()
    {
        // The following line simulates a task-returning asynchronous process.
        await Task.Delay(1000);
    }
}
```

查詢運算子
```C#
//封裝套用，用在查詢時的寫法
//https://learn.microsoft.com/zh-tw/dotnet/api/system.func-2?view=net-7.0
public delegate TResult Func<in T, out TResult>(T arg);

//明確的比較方法
Func<int, bool> equalsFive = x => x == 5;
bool result = equalsFive(4);
Console.WriteLine(result);   // False

//查詢比較方法
int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };
int oddNumbers = numbers.Count(n => n % 2 == 1);
Console.WriteLine($"There are {oddNumbers} odd numbers in {string.Join(" ", numbers)}");

//查詢 n < 6
int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };
var firstNumbersLessThanSix = numbers.TakeWhile(n => n < 6);
Console.WriteLine(string.Join(" ", firstNumbersLessThanSix));
// Output:
// 5 4 1 3

//查詢 n >= index (index 索引)
int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };
var firstSmallNumbers = numbers.TakeWhile((n, index) => n >= index);
Console.WriteLine(string.Join(" ", firstSmallNumbers));
// Output:
// 5 4

//查詢運算式 from in select
var numberSets = new List<int[]>
{
    new[] { 1, 2, 3, 4, 5 },
    new[] { 0, 0, 0 },
    new[] { 9, 8 },
    new[] { 1, 0, 1, 0, 1, 0, 1, 0 }
};

var setsWithManyPositives = 
    from numberSet in numberSets
    where numberSet.Count(n => n > 0) > 3
    select numberSet;

foreach (var numberSet in setsWithManyPositives)
{
    Console.WriteLine(string.Join(" ", numberSet));
}
// Output:
// 1 2 3 4 5
// 1 0 1 0 1 0 1 0

```

## 補充

`Lambda` 運算式的判定類型
```C#
//C# 10，可能出現 Func<...> Action<...>，需要指定的場合
//一般任意型態
var parse = (string s) => int.Parse(s);

//父類別承接
object parse = (string s) => int.Parse(s);   // Func<string, int>
Delegate parse = (string s) => int.Parse(s); // Func<string, int>

//示範方法承接
var read = Console.Read; // Func<int>
var write = Console.Write; // ERROR: 多載錯誤

//指定型態
LambdaExpression parseExpr = (string s) => int.Parse(s); // Expression<Func<string, int>>
Expression parseExpr = (string s) => int.Parse(s);       // Expression<Func<string, int>>

//並非所有 Lambda 運算式都有自然類型
var parse = s => int.Parse(s); // ERROR: lambda中的類型不足
Func<string, int> parse = s => int.Parse(s);// 改為指定型態
```

針對傳回類型不明的處理
```C#
//通常傳回類型必須是明確的
var choose = (bool b) => b ? 1 : "two"; // ERROR: 無法推斷返回類型

//C# 10 ，可以指定明確的傳回型別
var choose = object (bool b) => b ? 1 : "two"; // Func<bool, object>
```

針對使用外部變數和變數範圍規範
```C#
//  Lambda 可以參考「外部變數」。 這些 外部變數是定義 Lambda 運算式之方法範圍中的變數 ，或包含在包含 Lambda 運算式之型別的範圍內。
//  以這種方式擷取的變數會加以儲存，以便在 Lambda 運算式中使用，即使這些變數可能會超出範圍而遭到記憶體回收。 
//  外部變數必須確實指派，才能用於 Lambda 運算式。 

// 下列規則適用於 Lambda 運算式中的變數範圍：

//     擷取的變數將不會進行垃圾收集，直到參考該變數的委派才有資格進行垃圾收集。
//     在 Lambda 運算式中引進的變數不會顯示在封入方法中。
//     Lambda 運算式無法直接從封入方法擷取 in 、 ref 或 out 參數。
//     Lambda 運算式中的 return 陳述式不會造成封入方法傳回。
//     如果跳躍語句的目標不在 Lambda 運算式區塊之外，Lambda 運算式就無法包含 goto 、 break 或 continue 語句。 
//     即使目標位於區塊內，跳躍陳述式出現在 Lambda 運算式區塊外部也一樣是錯誤。

public static class VariableScopeWithLambdas
{
    public class VariableCaptureGame
    {
        internal Action<int>? updateCapturedLocalVariable;
        internal Func<int, bool>? isEqualToCapturedLocalVariable;

        public void Run(int input)
        {
            int j = 0;

            updateCapturedLocalVariable = x =>
            {
                j = x;
                bool result = j > input;
                Console.WriteLine($"{j} is greater than {input}: {result}");
            };

            isEqualToCapturedLocalVariable = x => x == j;

            Console.WriteLine($"Local variable before lambda invocation: {j}");
            updateCapturedLocalVariable(10);
            Console.WriteLine($"Local variable after lambda invocation: {j}");
        }
    }

    public static void Main()
    {
        var game = new VariableCaptureGame();

        int gameInput = 5;
        game.Run(gameInput);

        int jTry = 10;
        bool result = game.isEqualToCapturedLocalVariable!(jTry);
        Console.WriteLine($"Captured local variable is equal to {jTry}: {result}");

        int anotherJ = 3;
        game.updateCapturedLocalVariable!(anotherJ);

        bool equalToAnother = game.isEqualToCapturedLocalVariable(anotherJ);
        Console.WriteLine($"Another lambda observes a new value of captured variable: {equalToAnother}");
    }
    // Output:
    // lambda調用之前的本地變量：0
    // 10大於5：正確
    // lambda調用後的本地變量：10
    // 捕獲的本地變量等於10：true
    // 3大於5：false
    // 另一個lambda觀察到捕獲變量的新值：true
}

// C# 9.0 ，可以用 static ，以防止 Lambda 意外擷取區域變數或實例狀態：
Func<double, double> square = static x => x * x;
//https://learn.microsoft.com/zh-tw/dotnet/csharp/language-reference/proposals/csharp-9.0/static-anonymous-functions

```

C 10.0 建議的寫法
<https://learn.microsoft.com/zh-tw/dotnet/csharp/language-reference/proposals/csharp-10.0/lambda-improvements>
```C#
//屬性
f = [A] () => { };        // [A] lambda
f = [return:A] x => x;    // 錯誤 syntax error at '=>'
f = [return:A] (x) => x;  // [A] lambda
f = [A] static x => x;    // 錯誤 syntax error at '=>'

f = ([A] x) => x;         // [A] x
f = ([A] ref int x) => x; // [A] x

//多個屬性
var f = [A1, A2][A3] () => { };    // ok
var g = ([A1][A2, A3] int x) => x; // ok

//delegate 不支援匿名方法
f = [A] delegate { return 1; };         // 錯誤 syntax error at 'delegate'
f = delegate ([A] int x) { return x; }; // 錯誤 syntax error at '['

//初始值
var y = new C { [A] = x };    // ok: y[A] = x
var z = new C { [A] x => x }; // ok: z[0] = [A] x => x

//?[ 條件判定
x = b ? [A];               // ok
y = b ? [A] () => { } : z; // 錯誤 syntax error at '('


//顯式返回類型，可以在帶括號的參數列表之前指定顯式返回類型。
f = T () => default;                    // ok
f = short x => 1;                       // 錯誤 syntax error at '=>'
f = ref int (ref int x) => ref x;       // ok
f = static void (_) => { };             // ok
f = async async (async async) => async; // ok?

//delegate 不支援匿名方法
f = delegate int { return 1; };         // 錯誤 syntax error
f = delegate int (int x) { return x; }; // 錯誤 syntax error

//返回類型進行精確推斷
static void F<T>(Func<T, T> f) { ... }
F(int (i) => i); // Func<int, int>

//不允許從 lambda 返回類型到委託返回類型進行方差轉換
Func<object> f1 = string () => null; // 錯誤 error
Func<object?> f2 = object () => x;   // 警告 warning

//ref在表達式中包含返回類型
d = ref int () => x; // d = (ref int () => x)
F(ref int () => x);  // F((ref int () => x))

//var不能用作 lambda 表達式的顯式返回類型。
class var { }

d = var (var v) => v;              // 錯誤 error: contextual keyword 'var' cannot be used as explicit lambda return type
d = @var (var v) => v;             // ok
d = ref var (ref var v) => ref v;  // 錯誤 error: contextual keyword 'var' cannot be used as explicit lambda return type
d = ref @var (ref var v) => ref v; // ok

//轉換 delegate
Delegate d = delegate (object obj) { }; // Action<object>
Expression e = () => "";                // Expression<Func<string>>
object o = "".Clone;                    // Func<object>

//轉換運算符
class C
{
    public static implicit operator C(Delegate d) { ... }
}

C c;
c = () => 1;      // error: cannot convert lambda expression to type 'C'
c = (C)(() => 2); // error: cannot convert lambda expression to type 'C'

//方法組隱式轉換，會有警告
Random r = new Random();
object obj;
obj = r.NextDouble;         // warning: Converting method group to 'object'. Did you intend to invoke the method?
obj = (object)r.NextDouble; // ok

//var 推斷
var f1 = () => default;           // error: cannot infer type
var f2 = x => x;                  // error: cannot infer type
var f3 = () => 1;                 // System.Func<int>
var f4 = string () => null;       // System.Func<string>
var f5 = delegate (object o) { }; // System.Action<object>

static void F1() { }
static void F1<T>(this T t) { }
static void F2(this string s) { }

var f6 = F1;    // error: multiple methods
var f7 = "".F1; // System.Action
var f8 = F2;    // System.Action<string> 

var fs = new[] { (string s) => s.Length; (string s) => int.Parse(s) } // Func<string, int>[]

//不適用 丟棄的賦值
d = () => 0; // ok
_ = () => 1; // error

```