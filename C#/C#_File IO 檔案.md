# C# File IO 檔案

紀錄讀寫檔案的方法

## 方法

```C#
using System.IO;
// 使用 StreamReader 讀取檔案
using (StreamReader sr = new StreamReader("file.txt"))
{
    string content = sr.ReadToEnd();
}

// 使用 StreamWriter 寫入檔案
using (StreamWriter sw = new StreamWriter("file.txt"))
{
    sw.Write("Hello World!"); // 寫入字串
    sw.WriteLine("Hello World!"); // 寫入後換行
}

// 使用 StreamWriter 寫入檔案 try 方法
StreamWriter writer = new StreamWriter("write.txt");
try
{
    //......
}
finally
{
    writer.Close();// 關閉檔案
}

// File 讀取檔案
string fileContent = File.ReadAllText("C:\\path\\to\\file.txt");
string[] lines = File.ReadAllLines("C:\\path\\to\\file.txt"); //每行存成字串陣列
// File 寫入檔案
File.WriteAllText("C:\\path\\to\\file.txt", "This is the new content of the file.");
File.WriteAllLines("C:\\path\\to\\file.txt", new string[] { "line 1", "line 2", "line 3" }); //每個字串存成檔案中的一行



//利用 StringBuilder 開讀黨
// StringBuilder sb = new StringBuilder();

// // 讀取 read.txt 檔案
// using (StreamReader sr = new StreamReader("read.txt"))
// {
//     // 逐行讀取檔案內容
//     while (!sr.EndOfStream)
//     {
//         string line = sr.ReadLine();
//         sb.AppendLine(line);
//     }
// }

// // 將字串轉換為字串陣列
// string[] lines = sb.ToString().Split('\n');

// // 修改第 3 行資料
// lines[2] = "3,John,1983/3/10,C000000003,90,86,70";

// // 將修改後的資料寫回 StringBuilder 物件
// sb.Clear();
// foreach (string line in lines)
// {
//     sb.AppendLine(line);
// }

// // 將修改後的資料寫入 write.txt
// using (StreamWriter sw = new StreamWriter("write.txt"))
// {
//     sw.Write(sb.ToString());
// }
```

```C#


//多參數輸入分割
// string[] ins = Console.ReadLine().Split(' ');


//判斷質數
// static bool IsPrime(int number)
// {
//     if (number <= 1)
//     {
//         return false;
//     }
//     for (int i = 2; i <= Math.Sqrt(number); i++)
//     {
//         if (number % i == 0)
//         {
//             return false;
//         }
//     }
//     return true;
// }

//計算最大公因數
// static int FindGCD(int a, int b)
// {
// if( b==0 )
//     return a;
// return FindGCD( b, a%b );
// }


// 字串出現次數
// string strReplace = dreams.Replace(x1, "");
// int times = (dreams.Length - strReplace.Length) / x1.Length;
// Console.WriteLine(times);

// 循序回傳 ASCII 字串
// foreach (char c in input)
// {
//     Console.WriteLine("ASCII code for '{0}' is {1}", c, (int)c);
// }

//星座計算
// string[] names = { "Aquarius", "Pisces", "Aries", "Taurus", "Gemini", "Cancer", "Leo", "Virgo", "Libra", "Scorpio", "Sagittarius", "Capricorn" };
// // string[] dates = { "1/20", "2/19", "3/21", "4/20", "5/21", "6/21", "7/23", "8/23", "9/23", "10/23", "11/22", "12/22" };
// int[] startDates = { 120, 219, 320, 419, 520, 621, 722, 823, 923, 1023, 1122, 1222 };
// for (int i = 0; i < names.Length; i++)
// {
//     if (time < startDates[i])
//     {
//         star = i - 1;
//         break;
//     }
// }
// if (star == -1) star = 11;
// name = names[star];


// 取得一個介於 1 月 1 日到 12 月 31 日之間的日期
// DateTime date = new DateTime(year, 1, 1).AddDays(random.Next(365));
// 如果是週六或週日，則不是工作日
// if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
// {
// return false;
// }

// 計算兩個日期時間差
// static (int t1, int t2, int t3) TimespanToDate(DateTime self, DateTime target)
// {
//     int years, months, days;
//     // 因為只需取量，不決定誰大誰小，所以如果self < target時要交換將大的擺前面
//     if (self < target)
//     {
//         DateTime tmp = target;
//         target = self;
//         self = tmp;
//     }

//     // 將年轉換成月份以便用來計算
//     months = 12 * (self.Year - target.Year) + (self.Month - target.Month);

//     // 如果天數要相減的量不夠時要向月份借天數補滿該月再來相減
//     if (self.Day < target.Day)
//     {
//         months--;
//         days = DateTime.DaysInMonth(target.Year, target.Month) - target.Day + self.Day;
//     }
//     else
//     {
//         days = self.Day - target.Day;
//     }

//     // 天數計算完成後將月份轉成年
//     years = months / 12;
//     months = months % 12;

//     return (years, months, days);
// }
```