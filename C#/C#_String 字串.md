# C# String 字串

紀錄 `string` 字串使用方法

## 常用

```C#
// 宣告
string s;
string[] ss;
char c;

// 字串表示
s = "Text";  //字串
c = 'C';  //字元，可被當作數字
int i = (int)c;
c = s[0];  //可以把字串當成char[] ，要注意 char 被視作數字無法被當作 string
s = $"請輸入第 {i + 1} 個整數:";  // 字串內部{}視為程式碼

// 字串相接
s = "Text" + ", Text2";
```

## 方法

```C#
// 字串分割
ss = "asdf asdf".Split(' ');  //字元分割成string[]
ss = "asdfPOWasdf".Split("POW", System.StringSplitOptions.None);  //字元分割 不包還空字串

//判斷字串
"Hello".Contains("ll");  //是否存在
"Hello".IndexOf("ll");  //字串出現索引
"Hello".LastIndexOf("ll");  //字串從後方檢所第一個出現索引
"Hello".Substring(1, 3);  //字串索引從~到~的字串

//字串處理
s.Trim();  //移除字串前後空白

//單字處理
char.IsLetterOrDigit(c);  //是否數字或文字
char.IsUpper(c);  //是否是大寫
char.ToLower(c);  //轉換成小寫，非文字不處理
string.Join(",", new int[] { 1, 2, 3 });  // 陣列值插入字串輸出字串

```

## 字串讀取

```C#
//字串串流
StringBuilder sb = new StringBuilder("Hello, world!");

//字串串流修改
sb.Append("Hello ");
sb.AppendLine("World!");
sb.Replace("world", "universe"); // world 字元覆蓋 universe
sb.Replace("world", "", 7, 5);  // sb 的第7個字元起，往後5個字元
sb.Remove(0, 5);

//字串串流 方法
sb.ToString();  //輸出字串

```

## 格式化

```C#
// 字串格式化
string.Format("{0} {1}", 1, "123");  //{0} {1} 對應參數

// 數字格式化
string.Format("{0:F3}", 123.12);  // 123.120 數字顯示小數點後3位補0 
string.Format("{0:N0}", 1123.12);  // 1,123 數字顯示小數點後0位補0，即不顯示
string.Format("{0:0.##}", 1123.12);  // 1,123 數字顯示小數點後0不顯示
string.Format("{0:0.0}", 1123.12);  // 1,123 數字顯示小數點後最少保留一個0
string.Format("{0,-10:C}", 1123.12);  // 靠左10寬度，C貨幣顯示
string.Format("{0:D4}", 23);  // 4格寬補0

//日期格式化
DateTime dt = new DateTime(1991, 3, 7);  //參數 年/月/日
DateTime dt = DateTime.ParseExact(
    " 2008/03/18 PM 02: 50:23  ",       //輸入
    "yyyy/MM/dd",                       //格式，可以使用 string[] 複數解析
    CultureInfo.InvariantCulture,       //地區
    DateTimeStyles.AllowWhiteSpaces     //忽略空白
    );

//日期方法
dt.AddDays(6); // 增加日期
dt.ToString("yyyy/MM/dd ddd", new CultureInfo("en-US"));  //輸出 年/月/日 星期 地區美國
dt.DayOfWeek == DayOfWeek.Saturday;  // 判斷是否星期六

DateTime.DaysInMonth(1991, 3); // 取的當年月的日數

//日期時間差
TimeSpan diff = dt - new DateTime(1991, 3, 6); //日期相減，負數存在
diff.TotalDays;  //顯示總共 日
diff.Days;  //顯示 年/月/日 中的日

/*
d：短日期格式，例如："9/3/2022"。
D：长日期格式，例如："Tuesday, September 3, 2022"。
f：友好日期格式，例如："Tuesday, September 3, 2022 3:05 PM"。
F：完整日期格式，例如："Tuesday, September 3, 2022 3:05:07 PM"。
g：通用日期格式，例如："9/3/2022 3:05 PM"。
G：通用日期时间格式，例如："9/3/2022 3:05:07 PM"。
m：月份日期格式，例如："September 3"。
M：月份日期格式，例如："Sep 3"。
o：ISO 8601 格式的日期格式，例如："2022-09-03T15:05:07.0000000"。
r：RFC 1123 格式的日期格式，例如："Tue, 03 Sep 2022 15:05:07 GMT"。
s： ISO 8601 格式的日期时间格式，例如："2022-09-03T15:05:07"。
t：短时间格式，例如："3:05 PM"。
T：长时间格式，例如："3:05:07 PM"。
u： ISO 8601 格式的日期时间格式，例如："2022-09-03 15:05:07Z"。
U：完整的 UTC 日期时间格式，例如："Tuesday, September 3, 2022 8:05:07 PM"。
y：年份和月份格式，例如："September, 2022"。
Y：年份和月份格式，例如：
*/

/*
"yyyy" 表示 4 位数的年份。
"yy" 表示 2 位数的年份。
"MM" 表示月份，带前导零。
"M" 表示月份，不带前导零。
"dd" 表示日期，带前导零。
"d" 表示日期，不带前导零。
"hh" 表示 12 小时制的小时数，带前导零。
"h" 表示 12 小时制的小时数，不带前导零。
"HH" 表示 24 小时制的小时数，带前导零。
"H" 表示 24 小时制的小时数，不带前导零。
"mm" 表示分钟数，带前导零。
"m" 表示分钟数，不带前导零。
"ss" 表示秒数，带前导零。
"s" 表示秒数，不带前导零。
"fffffff" 表示毫秒数，7 位数字。
"ffffff" 表示毫秒数，6 位数字。
"fffff" 表示毫秒数，5 位数字。
"ffff" 表示毫秒数，4 位数字。
"fff" 表示毫秒数，3 位数字。
"ff" 表示毫秒数，2 位数字。
"f" 表示毫秒数，1 位数字。
"tt" 表示
*/

```