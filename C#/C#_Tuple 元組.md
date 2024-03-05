# C# Tuple 元組

C# 用來包裝一組資料作為傳遞參數的型態

## 基礎用法

Tuple，元組是變數組合，很像陣列，但用法完全不同<br>
視作一種新的變數取名較容易理解
```C#
//未命名時依順序呼叫
(double, int) t1 = (4.5, 3);
t1.Item1
t1.Item2

//可以取名在形態上
(double Sum, int Count) t2 = (4.5, 3);
t2.Count
t2.Sum

//可以取名在數值上
var t3 = (Sum: 4.5, Count: 3);
t3.Count
t3.Sum

//可以混和變數
var a = 4.5;
var t4 = (a, b: 2, 3);
t4.a
t4.b
t4.Item3

//不需要數值用 _ 捨棄
var (_, c) = (10, 2);

//型態可以賦值
(int, double) t5 = (17, 3.14);
(double A1, double B1) t6 = (0.0, 1.0);
(double A2, double B2) t7 = (2.0, 3.0);
t6 = t5; //t6: 17 and 3.14
t7 = t6; //t7: 17 and 3.14

//元祖可以直接解構成變數
var t8 = ("tuple", 3.6);
(string A3, double B3) = t8;
var (A4, B4) = t8;
A3; //"tuple"
B3; //3.6
A4; //"tuple"
B4; //3.6

//使用既有變數解構
var A5 = string.Empty;
var B5 = 0.0;
(A5, B5) = ("tuple", 3.6);
A5; //"tuple"
B5; //3.6

//比較數值依序比較
(int a, byte b) left = (5, 10);
(long a, int b) right = (5, 10);
(left == right);  // output: True
(left != right);  // output: False

//比較時不受變數名稱影響
var t9 = (A: 5, B: 10);
var tA = (B: 5, A: 10);
(t9 == tA);  // output: True
(t9 != tA);  // output: False
```

## 方法

基本上沒有什麼方法可以使用
```C#
//轉換字串
(double, int) t1 = (4.5, 3);
t1.ToString() //(4.5, 3)

//元組作為回傳用途
(int min, int max) Ret()
{
    return (10, 20);
}
var (min, max) = Ret();
(int min, int max) t1 = Ret();

//陣列搭配 out 解構
var limitsLookup = new Dictionary<int, (int Min, int Max)>()
{
    [2] = (4, 10),
    [4] = (10, 20),
    [6] = (0, 23)
};

if (limitsLookup.TryGetValue(4, out (int Min, int Max) limits))
{
  limits.Min; //10
  limits.Max; //20
}
```

## 自訂解構式

建構式相似，用於給元組解構
```C#
//自訂元組自動解構參數
public class TuplePerson
{
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }

    public TuplePerson(string fname, string mname, string lname)
    {
        FirstName = fname;
        MiddleName = mname;
        LastName = lname;
    }

    // Return the first and last name.
    public void Deconstruct(out string fname, out string lname)
    {
        fname = FirstName;
        lname = LastName;
    }

    public void Deconstruct(out string fname, out string mname, out string lname)
    {
        fname = FirstName;
        mname = MiddleName;
        lname = LastName;
    }
}

//使用方式
var p = new Person("John", "Quincy", "Adams", "Boston", "MA");
//自動解構
var(fName, lName, city, state) = p;
```