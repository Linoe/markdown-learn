# C#_進階

紀錄 `C#` 特殊用法類別

## default()

`default` 用來產生預設數值，但為依據型態回傳不同
<https://learn.microsoft.com/zh-tw/dotnet/csharp/language-reference/builtin-types/default-values>

```C#
// 所有參考型態 null
default(class); // null
default(interface); // null
default(string); // null
default(object); // null
// 所有數字型態 0
default(int); // 0
default(byte); // 0
// 特殊
default(bool); // false
default(char); // '\0'
default(enum); // (E)0
default(struct); // 產生一個成員類型為null，值類型為0的 struct

// C# 7.1
string s = dafault; //產生對應預設值
```

C 7.0 - 10.0 規範
<https://learn.microsoft.com/zh-tw/dotnet/csharp/language-reference/language-specification/readme>


## async/await/Task
執行緒相關方法
<https://learn.microsoft.com/zh-tw/dotnet/csharp/language-reference/operators/await>

## lock

資源的獨佔存取權，當程式存在複數執行緒時，可以確保在其間不被其他執行緒存取
```C#
lock (x)
{
    // Your code...
}
```

範例
```C#
using System;
using System.Threading.Tasks;

public class Account
{
    private readonly object balanceLock = new object();
    private decimal balance;

    public Account(decimal initialBalance) => balance = initialBalance;

    public decimal Debit(decimal amount)
    {
        if (amount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "The debit amount cannot be negative.");
        }

        decimal appliedAmount = 0;
        lock (balanceLock)
        {
            if (balance >= amount)
            {
                balance -= amount;
                appliedAmount = amount;
            }
        }
        return appliedAmount;
    }

    public void Credit(decimal amount)
    {
        if (amount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "The credit amount cannot be negative.");
        }

        lock (balanceLock)
        {
            balance += amount;
        }
    }

    public decimal GetBalance()
    {
        lock (balanceLock)
        {
            return balance;
        }
    }
}

class AccountTest
{
    static async Task Main()
    {
        var account = new Account(1000);
        var tasks = new Task[100];
        for (int i = 0; i < tasks.Length; i++)
        {
            tasks[i] = Task.Run(() => Update(account));
        }
        await Task.WhenAll(tasks);
        Console.WriteLine($"Account's balance is {account.GetBalance()}");
        // Output:
        // Account's balance is 2000
    }

    static void Update(Account account)
    {
        decimal[] amounts = { 0, 2, -3, 6, -2, -1, 8, -5, 11, -6 };
        foreach (var amount in amounts)
        {
            if (amount >= 0)
            {
                account.Credit(amount);
            }
            else
            {
                account.Debit(Math.Abs(amount));
            }
        }
    }
}
```