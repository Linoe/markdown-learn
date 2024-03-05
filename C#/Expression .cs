using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Expression
{
    #region 註解
    /// <summary>
    /// 文件註解
    /// </summary>
    class Comments
    {
        #region 程式區塊
        void Say(string[] args)
        {
            //Console.WriteLine("單行註解");
            /*
            Console.WriteLine("多行註解");
            */

            // 標記用註解
            // HACK: 黑客
            // NOTE: 筆記
            // TODO: 代辦
            // UNDONE: 別動
        }
        #endregion

        /// <summary>
        /// 文件註解
        /// </summary>
        /// <param name="id">參數註解</param>
        /// <returns>回傳註解</returns>
        /// <remarks>
        /// 更長的文件註解
        /// </remarks>
        static int Msg(int id)
        {
            return 0;
        }
    }
    #endregion
    #region if/switch 條件
    class iffoo
    {
        void Say()
        {
            {//if else，(條件) 巢科
                int i = 5;
                if (i < 20.0)
                {
                    Console.WriteLine("Cold.");
                }
                else if (i < 40.0)
                {
                    Console.WriteLine("Perfect!");
                }
                else
                {
                    Console.WriteLine("Perfect!");
                }
            }
            {//switch case，(對象) ，case (條件) break;中斷 default都沒有
                double measurement = 5.0;
                switch (measurement)
                {
                    case < 0.0:
                        Console.WriteLine($"Measured value is {measurement}; too low.");
                        break;

                    case > 15.0:
                    case double.NaN:
                        Console.WriteLine("Failed measurement.");
                        break;

                    default:
                        Console.WriteLine($"Measured value is {measurement}.");
                        break;
                }
            }
        }
    }

    #endregion
    #region for/doforeach/do while 迴圈
    class LoopFoo
    {
        void Say()
        {
            {//for 迴圈，(建立;條件;結束)
                for (int i = 0; i < 3; i++)
                {
                    Console.Write(i);
                }
            }
            {//foreach 歷遍，(取得 in 來源)
                var fibNumbers = new List<int> { 0, 1, 1, 2, 3, 5, 8, 13 };
                foreach (int element in fibNumbers)
                {
                    Console.Write($"{element} ");
                }
            }
            {//do while，必定執行一次(條件)
                int n = 5;
                do
                {
                    Console.Write(n);
                    n++;
                } while (n < 5);
            }
            {//while 迴圈，(條件)
                int n = 0;
                while (n < 5)
                {
                    Console.Write(n);
                    n++;
                }
            }
        }
    }

    #endregion
    #region break/continue/return/goto 跳躍
    class jumpfoo
    {
        public enum CoffeeChoice
        {
            Plain,
            WithMilk,
            WithIceCream,
        }
        void Say()
        {
            {//break，for/do while/foreach/switch  直接跳出迴圈外
                int[] numbers = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                foreach (int number in numbers)
                {
                    if (number == 3) break;

                    Console.Write($"{number} ");
                }
            }
            {//continue，for/do while/foreach 迴圈直接進入下一輪
                for (int i = 0; i < 5; i++)
                {
                    Console.Write($"Iteration {i}: ");

                    if (i < 3)
                    {
                        Console.WriteLine("skip");
                        continue;
                    }

                    Console.WriteLine("done");
                }
            }
            {//return，方法直接結束並回傳數值
                int number = 1;
                if (number % 2 == 0)
                {
                    return;
                }
            }
            {//goto，直接跳躍到標籤處，一般不使用
                goto Found;
            // Console.WriteLine("can not");
            Found:
                Console.WriteLine("is Found");

                //如果switch 可以跳躍到 case 常數
                CoffeeChoice choice = CoffeeChoice.Plain;
                decimal price = 0;
                switch (choice)
                {
                    case CoffeeChoice.Plain:
                        price += 10.0m;
                        break;

                    case CoffeeChoice.WithMilk:
                        price += 5.0m;
                        goto case CoffeeChoice.Plain;

                    case CoffeeChoice.WithIceCream:
                        price += 7.0m;
                        goto case CoffeeChoice.Plain;
                }
            }
        }
    }
    #endregion
    #region try-catch 測試錯誤
    class tryfoo
    {
        void Say()
        {
            {//測試錯誤用區塊
                try
                {
                    Console.WriteLine("測試");
                }
                catch (Exception e)
                {
                    Console.WriteLine("錯誤" + e.ToString());
                }
                finally
                {
                    Console.WriteLine("結束");
                }
            }
        }
    }
    #endregion

    #region is / switch 檢查表達式
    // 使用以下 is / switch 可以使用的表達式

    // 宣告模式：檢查運算式，如果相符專案成功，請將運算式結果指派給宣告的變數。 C# 8.0
    class DeclarationPattern
    {
        void Say()
        {

            { //簡化宣告判定運算式
                int? x = 3;
                if (x is int v) { }
                // 等同下面
                // int? x = 3;
                // var v = x as Type;
                // if (v != null) {}
            }

            { //宣告模式會檢查運算式，符合運算式時將變數指派已轉換的運算式結果
                int? xNullable = 7;
                int y = 23;
                object yBoxed = y;
                object greeting = "Hello, World!";
                if (greeting is string message)  // 示範一個條件
                {
                    Console.WriteLine(message.ToLower());  // output: hello, world!
                }
                if (xNullable is int a && yBoxed is int b)  // 示範兩個條件
                {
                    Console.WriteLine(a + b);  // output: 30
                }
                //檢查非 Null
                //if (input is not null){...}
            }
        }
    }
    // 類型模式：檢查運算式的類型。 在 C# 9.0 中引進。
    class TypePattern
    {
        //可以在 if/switch 運算式檢查類別
        void M(object o1, object o2)
        {
            var t = (o1, o2);
            if (t is (int, string)) { } // test if o1 is an int and o2 is a string
            switch (o1)
            {
                case int: break; // test if o1 is an int
                case System.String: break; // test if o1 is a string
            }
        }

        void Say()
        {

            {// 當運算式結果為非 Null 且類型為 T 或 衍生自類型 T，宣告模式會比對運算式：
                GetSourceLabel(new int[] { 10, 20, 30 });  // output: 1
                GetSourceLabel(new List<char> { 'a', 'b', 'c', 'd' });  // output: 2
                static int GetSourceLabel<T>(IEnumerable<T> source) => source switch
                {
                    Array array => 1,
                    ICollection<T> collection => 2,
                    null => throw new ArgumentNullException(nameof(source)),
                    _ => throw new ArgumentException("Unknown type of a vehicle", nameof(source)),
                };
            }
        }
    }
    // 常數模式：測試運算式結果是否等於指定的常數。
    class ConstantPattern
    {
        // 常數為以下
        // 10 int
        // 4.5 float
        // 'a' char
        // "hello" string
        // true / false boolean
        // enum
        // const
        // null

        void Say()
        {
            {// switch 使用常數
                static decimal GetGroupTicketPrice(int visitorCount) => visitorCount switch
                {
                    1 => 12.0m,
                    2 => 20.0m,
                    3 => 27.0m,
                    0 => 0.0m,
                    _ => throw new ArgumentException($"Not supported number of visitors: {visitorCount}", nameof(visitorCount)),
                };
                GetGroupTicketPrice(1);
            }
            {// if 使用常數
                object? input = null;
                if (input is null)  //如果是 null
                { }
                if (input is not null)  //如果不是 null
                { }
            }
        }
    }
    // 關聯式模式：比較運算式結果與指定的常數。 在 C# 9.0 中引進。
    class RelationalPattern
    {
        // 使用 < > 等比較運算子 執行結果與回傳
        void Say()
        {
            {//switch 使用
                Classify(13);          // output: Too high
                Classify(double.NaN);  // output: Unknown
                Classify(2.4);         // output: Acceptable

                static string Classify(double measurement) => measurement switch
                {
                    < -4.0 => "Too low",
                    > 10.0 => "Too high",
                    double.NaN => "Unknown",
                    _ => "Acceptable",
                };
            }
        }
    }
    // 邏輯模式：測試運算式是否符合模式的邏輯組合。 在 C# 9.0 中引進。
    class LogicalPattern
    {
        // 使用邏輯運算子，檢查順序為
        // not > and > or
        void Say()
        {
            {// Switch and
                Classify(13);  // output: High
                Classify(-100);  // output: Too low
                Classify(5.7);  // output: Acceptable

                static string Classify(double measurement) => measurement switch
                {
                    < -40.0 => "Too low",
                    >= -40.0 and < 0 => "Low",
                    >= 0 and < 10.0 => "Acceptable",
                    >= 10.0 and < 20.0 => "High",
                    >= 20.0 => "Too high",
                    double.NaN => "Unknown",
                };
            }

            {// Switch or
                GetCalendarSeason(new DateTime(2021, 1, 19));  // output: winter
                GetCalendarSeason(new DateTime(2021, 10, 9));  // output: autumn
                GetCalendarSeason(new DateTime(2021, 5, 11));  // output: spring

                static string GetCalendarSeason(DateTime date) => date.Month switch
                {
                    3 or 4 or 5 => "spring",
                    6 or 7 or 8 => "summer",
                    9 or 10 or 11 => "autumn",
                    12 or 1 or 2 => "winter",
                    _ => throw new ArgumentOutOfRangeException(nameof(date), $"Date with unexpected month: {date.Month}."),
                };
            }

            {// 可以在用 () 優先比對
                object? input = null;
                if (input is not (float or double))
                {
                }
            }
        }
    }
    // 屬性模式：測試運算式的屬性或成員是否符合。
    class PropertyPattern
    {
        // {} 內比對成員是否符合
        // date is {Year : 2020}
        // date.Year == 2020
        void Say()
        {

            {// is 成員比對
                static bool IsConferenceDay(DateTime date) => date is
                { Year: 2020, Month: 5, Day: 19 or 20 or 21 };
                IsConferenceDay(DateTime.Now);
            }
            {// switch 成員比對
                TakeFive("Hello, world!");  // output: Hello
                TakeFive("Hi!");  // output: Hi!
                TakeFive(new[] { '1', '2', '3', '4', '5', '6', '7' });  // output: 12345
                TakeFive(new[] { 'a', 'b', 'c' });  // output: abc

                static string TakeFive(object input) => input switch
                {
                    string { Length: >= 5 } s => s.Substring(0, 5),
                    string s => s,

                    ICollection<char> { Count: >= 5 } symbols => new string(symbols.Take(5).ToArray()),
                    ICollection<char> symbols => new string(symbols.ToArray()),

                    null => throw new ArgumentNullException(nameof(input)),
                    _ => throw new ArgumentException("Not supported input type."),
                };
            }
        }
    }
    // 位置模式：解構參數比較運算式得到結果
    class PositionalPattern
    {
        //最常見到 record 元組
        void Say()
        {
            {// switch 解析 元組
                static decimal GetGroupTicketPriceDiscount(int groupSize, DateTime visitDate)
                    => (groupSize, visitDate.DayOfWeek) switch
                    {
                        ( <= 0, _) => throw new ArgumentException("Group size must be positive."),
                        (_, DayOfWeek.Saturday or DayOfWeek.Sunday) => 0.0m,
                        ( >= 5 and < 10, DayOfWeek.Monday) => 20.0m,
                        ( >= 10, DayOfWeek.Monday) => 30.0m,
                        ( >= 5 and < 10, _) => 12.0m,
                        ( >= 10, _) => 15.0m,
                        _ => 0.0m,
                    };
                GetGroupTicketPriceDiscount(1, DateTime.Now);
            }
            {// is 解析 元祖，var 重新宣告
                var numbers = new List<int> { 1, 2, 3 };
                if (SumAndCount(numbers) is (Sum: var sum, Count: > 0))
                {
                    Console.WriteLine($"Sum of [{string.Join(" ", numbers)}] is {sum}");  // output: Sum of [1 2 3] is 6
                }

                static (double Sum, int Count) SumAndCount(IEnumerable<int> numbers)
                {
                    int sum = 0;
                    int count = 0;
                    foreach (int number in numbers)
                    {
                        sum += number;
                        count++;
                    }
                    return (sum, count);
                }
            }

            {
            }
        }

        // switch 類型 解析 元組
        public readonly struct Point
        {
            public int X { get; }
            public int Y { get; }

            public Point(int x, int y) => (X, Y) = (x, y);

            public void Deconstruct(out int x, out int y) => (x, y) = (X, Y);
        }

        static string Classify(Point point) => point switch
        {
            (0, 0) => "Origin",
            (1, 0) => "positive X basis end",
            (0, 1) => "positive Y basis end",
            _ => "Just a point",
        };


        // switch 類型分類 重新宣告
        public record Point2D(int X, int Y) { };
        public record Point3D(int X, int Y, int Z) { };

        static string PrintIfAllCoordinatesArePositive(object point) => point switch
        {
            Point2D(> 0, > 0) p => p.ToString(),
            Point3D(> 0, > 0, > 0) p => p.ToString(),
            _ => string.Empty,
        };


        //is 解析 record 比對成員
        public record WeightedPoint(int X, int Y)
        {
            public double Weight { get; set; }
        }

        static bool IsInDomain(WeightedPoint point) => point is
            ( >= 0, >= 0) { Weight: >= 0.0 };
        //搭配重新宣告 p = point
        //if (input is WeightedPoint(> 0, > 0) { Weight: > 0.0 } p) 
    }
    // var 模式：比對運算式中，重新指派給宣告的變數。
    class VarPattern
    {
        //使用 var 結果重新宣告繼續比較
        void Say()
        {
            {// is 重新宣告 
                static bool IsAcceptable(int id, int absLimit) => SimulateDataFetch(id) is var results
                    && results.Min() >= -absLimit
                    && results.Max() <= absLimit;

                static int[] SimulateDataFetch(int id)
                {
                    var rand = new Random();
                    return Enumerable
                               .Range(start: 0, count: 5)
                               .Select(s => rand.Next(minValue: -10, maxValue: 11))
                               .ToArray();
                }
                IsAcceptable(1, 1);
            }
        }

        //switch record 重新 宣告成元組
        public record Point(int X, int Y) { };

        static Point Transform(Point point) => point switch
        {
            var (x, y) when x < y => new Point(-x, y),
            var (x, y) when x > y => new Point(x, -y),
            var (x, y) => new Point(x, y),
        };
    }
    // 捨棄模式：沒有任何比對成功，默認結果
    class DiscardPattern
    {
        //switch 最後比對 _ => ，當沒有任何比對成功時執行此
        void Say()
        {
            Console.WriteLine(GetDiscountInPercent(DayOfWeek.Friday));  // output: 5.0
            Console.WriteLine(GetDiscountInPercent(null));  // output: 0.0
            Console.WriteLine(GetDiscountInPercent((DayOfWeek)10));  // output: 0.0

            static decimal GetDiscountInPercent(DayOfWeek? dayOfWeek) => dayOfWeek switch
            {
                DayOfWeek.Monday => 0.5m,
                DayOfWeek.Tuesday => 12.5m,
                DayOfWeek.Wednesday => 7.5m,
                DayOfWeek.Thursday => 12.5m,
                DayOfWeek.Friday => 5.0m,
                DayOfWeek.Saturday => 2.5m,
                DayOfWeek.Sunday => 2.0m,
                _ => 0.0m,
            };
        }
    }
    // 清單模式：測試序列專案是否符合對應的巢狀模式。 在 C# 11 中引進。
    class ListPattern
    {
        //一連串比對中進行比較
        /*   
        void Say()
        {
            {
                int[] numbers = { 1, 2, 3 };

                Console.WriteLine(numbers is [1, 2, 3]);  // True
                Console.WriteLine(numbers is [1, 2, 4]);  // False
                Console.WriteLine(numbers is [1, 2, 3, 4]);  // False
                Console.WriteLine(numbers is [0 or 1, <= 2, >= 3]);  // True

            }
            {
                List<int> numbers = new() { 1, 2, 3 };

                if (numbers is [var first, _, _])
                {
                    Console.WriteLine($"The first element of a three-item list is {first}.");
                }
                // Output:
                // The first element of a three-item list is 1.
            }
            {
                Console.WriteLine(new[] { 1, 2, 3, 4, 5 } is [> 0, > 0, ..]);  // True
                Console.WriteLine(new[] { 1, 1 } is [_, _, ..]);  // True
                Console.WriteLine(new[] { 0, 1, 2, 3, 4 } is [> 0, > 0, ..]);  // False
                Console.WriteLine(new[] { 1 } is [1, 2, ..]);  // False

                Console.WriteLine(new[] { 1, 2, 3, 4 } is [.., > 0, > 0]);  // True
                Console.WriteLine(new[] { 2, 4 } is [.., > 0, 2, 4]);  // False
                Console.WriteLine(new[] { 2, 4 } is [.., 2, 4]);  // True

                Console.WriteLine(new[] { 1, 2, 3, 4 } is [>= 0, .., 2 or 4]);  // True
                Console.WriteLine(new[] { 1, 0, 0, 1 } is [1, 0, .., 0, 1]);  // True
                Console.WriteLine(new[] { 1, 0, 1 } is [1, 0, .., 0, 1]);  // False
            }
            {
                void MatchMessage(string message)
                {
                    var result = message is ['a' or 'A', ..var s, 'a' or 'A']
                        ? $"Message {message} matches; inner part is {s}."
                        : $"Message {message} doesn't match.";
                    Console.WriteLine(result);
                }

                MatchMessage("aBBA");  // output: Message aBBA matches; inner part is BB.
                MatchMessage("apron");  // output: Message apron doesn't match.

                void Validate(int[] numbers)
                {
                    var result = numbers is [< 0, .. { Length: 2 or 4 }, > 0] ? "valid" : "not valid";
                Console.WriteLine(result);
            }

            Validate(new[] { -1, 0, 1 });  // output: not valid
            Validate(new[] { -1, 0, 0, 1 });  // output: valid

        }
        */
    }

    // switch 巢狀模式比對
    // public record Point(int X, int Y) { };
    // public record Segment(Point Start, Point End) { };

    // static bool IsAnyEndOnXAxis(Segment segment) => segment is
    // { Start: { Y: 0 } } or { End: { Y: 0 } };
    //segment.Start.Y == 0 or segment.End.Y == 0

    // 另一種
    // static bool IsEndOnXAxis(Segment segment) => segment is 
    //     { Start.Y: 0 } or { End.Y: 0 };
    #endregion
    // LINQ
    // https://learn.microsoft.com/zh-tw/dotnet/csharp/programming-guide/concepts/linq/
    // https://learn.microsoft.com/zh-tw/dotnet/csharp/language-reference/keywords/query-keywords
    //    from in where select 查詢式
    // 語法 SQL 風格 查詢
    // int[] numbers = { 2, 5, 7, 8, 9, 10 };
    // var result = from n in numbers  //資料 等同 foreach
    //              where n > 5        //條件 篩選
    //              select n;          //輸出 int[]

    // var result = from e in employees
    //              where e.Gender == "Female" && e.Salary > 50000
    //              select e;



    // var sortedData = from data in texts
    //                  let fields = data.Split(',')
    //                  let id = int.Parse(fields[0])
    //                  where id <= row
    //                  orderby id
    //                  select data;
}