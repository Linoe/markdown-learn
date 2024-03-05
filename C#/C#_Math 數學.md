# C# Math 數學

紀錄 `Math` 常用的方法

## 方法

```C#
using System;
//Math 函式
Math.Pow(2, 2); // 次方
Math.Sqrt(4); //開根號
Math.Round(0.1348f, 2); // 四捨五入至指定位數
Math.Abs(-456); // 絕對值


//字串轉型
a = int.Parse("123"); //int float double 皆是，會報錯
int.TryParse("123", out a);  //int float double 皆是，不會報錯
Convert.ToInt32("123"); // 各種類型轉換 會報錯
//處理字元內出現非數字
System.Globalization.NumberFormatInfo provider = new System.Globalization.NumberFormatInfo();
provider.NumberDecimalSeparator = ".";
provider.NumberGroupSeparator = ",";
a = int.Parse("15,123.123", provider);


//亂數產生
Random rand = new Random(99); //參數為亂數種子
rand.Next(1, 101); // 產生 1~ 101 整數
```