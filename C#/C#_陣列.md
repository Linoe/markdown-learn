# C# 陣列

紀錄 `C#` 陣列

1. [Array](#array) - 基礎型態一維陣列
2. [Tuple](#tuple) - 一組參數
3. [List](#list) - 可變長度的一維陣列
4. [ArrayList](#arraylist) - 任意型態長度的一維陣列
5. [HashSet](#hashset) - 不重複的陣列
6. [Dictionary](#dictionary) - 鍵值組的陣列

>集合
>https://learn.microsoft.com/zh-tw/dotnet/csharp/programming-guide/concepts/collections
>陣列
>https://learn.microsoft.com/zh-tw/dotnet/csharp/programming-guide/arrays/
>事件events
>https://learn.microsoft.com/zh-tw/dotnet/csharp/programming-guide/events/
>Queue與Stack
>
>Convert Parse 浮點數轉換整數時 4捨5入差異
>https://ithelp.ithome.com.tw/articles/10213221

## Array

宣告類別的一維陣列，宣告後不可更改型態與長度<br>
```C#
// 數字陣列
int[] intArr = { 1, 2, 3 };
intArr[0] = 1;
intArr[1] = 2;
intArr[2] = 3;
intArr = new int[3];
intArr = new int[] { 1, 2, 3 };

Console.WriteLine(intArr.Max()); //最大
Console.WriteLine(intArr.Min()); //最小

//多重陣列
int[,] a = { { 1, 2, 3 }, { 4, 5, 6 } };
int[,] b = new int[2, 3];

//Array 用法
Array.Sort(intArr); // 陣列值排序
Array.Reverse(intArr); // 陣列反轉
```

## Tuple

元組是變數組合，很像陣列，但用法完全不同<br>
```C#
//元祖
Tuple<int> tp = new Tuple<int>(10);
tp = (10);
// (int t1, int t2, int t3) ToString(DateTime self) //參數
```

## List

儲存指定類別的變數陣列，宣告後可以更改長度，不可更改型態<br>
```C#
//List 建立
List<string> list = new List<string> { "asdf", "zxcv", "qwer" };

//List 二維建立
var numberSets = new List<int[]>
{
    new[] { 1, 2, 3, 4, 5 },
    new[] { 0, 0, 0 },
    new[] { 9, 8 },
    new[] { 1, 0, 1, 0, 1, 0, 1, 0 }
};

//List 方法
list.Count(); //總數，Linq 條件判斷符合的數量
list.ToArray(); //內容轉換為 string[] 型態
list.Sort((x, y) => x - y;)//排序， x y 分別代表當前值跟下個值

//List 歷遍
list.ForEach(str => Console.Write(str));

//List T 型態
public class Galaxy
{
    public string Name { get; set; }
    public int MegaLightYears { get; set; }
}
var theGalaxies = new List<Galaxy>
{
    new (){ Name="Tadpole", MegaLightYears=400},
    new (){ Name="Pinwheel", MegaLightYears=25},
    new (){ Name="Milky Way", MegaLightYears=0},
    new (){ Name="Andromeda", MegaLightYears=3}
};
```

## ArrayList

儲存任意型態的變數陣列<br>
```C#
//Array 建立特定長度
Array.CreateInstance(typeof(object), 2);

//ArrayList 物件陣列
//內容可以塞任何物件，存取時的依順序讀取
ArrayList list = new ArrayList();
list.Add(1);
list.Add("Hello");
list.Add(true);
list.Add(3.14);
list.IndexOf(0);
list[0];

//ArrayList 方法
list.Contains((object)1); //回傳是否存在，部分基礎值類別需要轉型成object

//yield return
//回傳後再次呼叫時會重該位置繼續
//主要用來持續查詢資料用


//IComparer<type>, IEnumerable<type>, IEnumerator<type>
//排序, GetEnumerator , foreach
```

## HashSet

儲存指定型態且內容不重陣列，宣告後不可更改型態<br>
```C#
//HashSet 雜湊 內容不重複
HashSet<int> numbers = new HashSet<int>();

//HashSet 方法
numbers.Contains(10); //是否包含
```

## Dictionary

儲存 `Kye-Value` 的陣列<br>
```C#
//宣告
Dictionary<string, string> names = new Dictionary<string, string>() { };

//加入
names.Add("Tim", "Clerk");
names.Add("John", "Manager");
names.Add("Mary", "Boss");

//索引
names["Tim"]

//檢查存在
if(names.ContainsKey("Tim"));
  Console.WriteLine("found: " + names["Tim"]);

if (names.TryGetValue("symbol", out string? str))
  Console.WriteLine("found: " + str);

//修改
names["Tim"] = "Cooker";

//移除
names.Remove("Tim");

//歷遍
//foreach (var kvp in names) //任意
//foreach ((var k, var v) in names)  //元組解構
foreach (KeyValuePair<string, string> kvp in names)
    Console.WriteLine(kvp.Key + " " + kvp.Value);

//轉List
List<string> keys = names.Keys.ToList<string>();
List<string> values = names.Values.ToList<string>();

//T 型態
public class Element
{
    public required string Symbol { get; init; }
    public required string Name { get; init; }
    public required int AtomicNumber { get; init; }
}
Dictionary<string, Element> BuildDictionary() =>
    new ()
    {
        {"K",
            new (){ Symbol="K", Name="Potassium", AtomicNumber=19}},
        {"Ca",
            new (){ Symbol="Ca", Name="Calcium", AtomicNumber=20}},
        {"Sc",
            new (){ Symbol="Sc", Name="Scandium", AtomicNumber=21}},
        {"Ti",
            new (){ Symbol="Ti", Name="Titanium", AtomicNumber=22}}
    };
```

## yield 迭代器

`yield` 並不是集合，大多是用來他配集合使用，故寫在此

```C#
// 方法中回傳集合內容
private static IEnumerable<int> EvenSequence(
    int firstNumber, int lastNumber)
{
    // Yield even numbers in the range.
    for (var number = firstNumber; number <= lastNumber; number++)
    {
        if (number % 2 == 0)
        {
            yield return number;
        }
    }
}

//外部使用該方法，每次呼叫時會從上次地方繼續
foreach (int number in EvenSequence(5, 18)); // Output: 6 8 10 12 14 16 18
```

## LINQ 查詢

`LINQ` 提供給習慣於查詢語言使用，可以它配集合歷遍，故寫在此<br>
`List` 的 `LINQ` 是擴展在 `System.Linq` ，必須額外引用

```C#
using System.Linq;

private static void ShowLINQ()
{
    List<Element> elements = BuildList();

    // LINQ Query.
    var subset = from theElement in elements
                 where theElement.AtomicNumber < 22
                 orderby theElement.Name
                 select theElement;

    foreach (Element theElement in subset)
    {
        Console.WriteLine(theElement.Name + " " + theElement.AtomicNumber);
    }

    // Output:
    //  Calcium 20
    //  Potassium 19
    //  Scandium 21
}

private static List<Element> BuildList() => new()
    {
        { new(){ Symbol="K", Name="Potassium", AtomicNumber=19}},
        { new(){ Symbol="Ca", Name="Calcium", AtomicNumber=20}},
        { new(){ Symbol="Sc", Name="Scandium", AtomicNumber=21}},
        { new(){ Symbol="Ti", Name="Titanium", AtomicNumber=22}}
    };
```

## string 搭配

`string` 部分方法可以使用集合達到回傳，故寫在此

```C#
string.Join("|", myList); // myList 中插入 "|"
string.Join("|", myList.Select(x => x.ToString()).ToArray()); //利用 LINQ 查詢回傳，插入 "|"

```