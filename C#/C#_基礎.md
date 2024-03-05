# Csharp 基礎

紀錄 `C#` 基礎類別

## Hello World

基礎類別命名予宣告
```C#
//引用命名空間，可以呼叫類別
using System;
//引用類別，可以呼叫方法
using static MyClass;

// 輸入
string s = Console.ReadLine();
// 顯示
Console.WriteLine(s);
// 讀取key
Console.ReadKey();

//定義變數
int x, y;
x = y = 0;

// #region Name
// 區塊
// #endregion

```

## switch

依據參數比較執行內容
<https://learn.microsoft.com/zh-tw/dotnet/csharp/language-reference/statements/selection-statements>
```C#
switch (measurement)
{
    //小於
    case < 0.0:
        Console.WriteLine($"Measured value is {measurement}; too low.");
        break;
    //大於
    case > 15.0:
        Console.WriteLine($"Measured value is {measurement}; too high.");
        break;
    //等於
    case double.NaN:
        Console.WriteLine("Failed measurement.");
        break;
    //其他
    default:
        Console.WriteLine($"Measured value is {measurement}.");
        break;
}

switch (measurement)
{
    //小於 與 大於
    case < 0:
    case > 100:
        Console.WriteLine($"Measured value is {measurement}; out of an acceptable range.");
        break;
}

//多參數
switch ((a, b))
{
    // a > 0 且 B > 0 同時 a == b
    case (> 0, > 0) when a == b:
        Console.WriteLine($"Both measurements are valid and equal to {a}.");
        break;
    // a > 0 且 B > 0 
    case (> 0, > 0):
        Console.WriteLine($"First measurement is {a}, second measurement is {b}.");
        break;

    default:
        Console.WriteLine("One or both measurements are not valid.");
        break;
}
```