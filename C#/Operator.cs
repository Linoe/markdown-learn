using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using forwpf = System.Windows;

namespace Operator
{
    #region Operator 基礎
    class OperatorFoo
    {
        void math_oper()  //+-*/% 數學運算子
        {
            int a = 0;
            {//+ 加數
                a = 1 + 1;
            }

            {//- 減數
                a = 2 - 1;
            }

            {//* 乘數
                a = 3 * 3;
            }

            {/// 除數，依據類型整數 int 小數點後捨去
                a = 10 / 3;
            }

            {//% 餘數
                a = 10 % 3;
            }

            {//T++ 運算子，運算前輸出a 後+1
                a++;
            }

            {//++T 運算子，先+1 運算後輸出a
                ++a;
            }

            {//T-- 運算子，運算前輸出a 後-1
                a--;
            }

            {//--T 運算子，先-1 運算後輸出a
                --a;
            }
        }
        void logic_oper()  //!&|^ 邏輯運算子
        {
            bool b = true;
            {// ! 邏輯 not 運算
                b = !b;
            }

            {// & 邏輯 and 運算
                b = true & true;
            }

            {// | 邏輯 or 運算
                b = true | false;
            }

            {// ^ 邏輯 xor 運算
                b = true ^ true;
            }

            {// && 邏輯 and 判斷，可以比較物件
                b = true && true;
            }

            {// || 邏輯 or 判斷，可以比較物件
                b = true || false;
            }

            Console.WriteLine(b);
        }
        void null_oper()  //?.! null運算子
        {

            {//? 可為null
                int? a = null;
            }

            {//?? 判定不是null，則賦值
                int? a = null;
                int b = a ?? -1;  // if(a != null) b = a;
                a ??= 10; //if(a != null) a = 10;
            }

            {//?. 可否執行
                Object? person = null;
                person?.ToString(); //if (person != null) person.ToString();
            }

            {//! 容許參數使用 null，可能出錯
                object? p = null;
                Console.WriteLine($"Found {p!.ToString}");
            }

            {//關於 null 的 logic 運算
                //C# 假定 null 與 true/false 進行邏輯運算
                bool? test = null;
                Console.WriteLine(test is null ? "null" : test.Value.ToString());

                // x 	 	y 	 	x&y 	x|y
                // true 	true 	true 	true
                // true 	false 	false 	true
                // true 	null 	null 	true 
                //
                // false 	true 	false 	true
                // false 	false 	false 	false
                // false 	null 	false 	null 
                //
                // null 	true 	null 	true 
                // null 	false 	false 	null 
                // null 	null 	null 	null 
                //
                // x&y 邏輯為 相同不變，不同則 false > null > true 優先
                // x|y 邏輯為 相同不變，不同則 true > null > false 優先
            }
        }
        void bit_oper()  //~<<>>&^|位元運算子
        {
            {//~ 位元反轉
                uint a1 = 0b_0000_1111_0000_1111_0000_1111_0000_1100;
                uint b1 = ~a1;// 11110000111100001111000011110011
            }

            {//<< 位元左移
                uint x1 = 0b_1100_1001_0000_0000_0000_0000_0001_0001; // Before: 11001001000000000000000000010001
                uint y1 = x1 << 4; // After:  10010000000000000000000100010000
            }

            {//>> 位元右移
                uint x2 = 0b_1001;// Before: 1001
                uint y2 = x2 >> 2;// After:  0010
            }

            {//& 位元 and運算
                uint a2 = 0b_1111_1000;
                uint b2 = 0b_1001_1101;
                uint c2 = a2 & b2;// 10011000
            }

            {//| 位元 or運算
                uint a4 = 0b_1010_0000;
                uint b4 = 0b_1001_0001;
                uint c4 = a4 | b4;// 10110001
            }

            {//^ 位元 xor運算
                uint a3 = 0b_1111_1000;
                uint b3 = 0b_0001_1100;
                uint c3 = a3 ^ b3;// 11100100
            }
        }
        void comp_oper()  //><!= 比較運算子
        {
            bool a = true;
            {//> 大於
                a = 5 > 0;  //true
            }

            {//>= 大於等於
                a = 5 >= 0;  //true
            }

            {//< 小於
                a = 5 < 0;  //false
            }

            {//<= 小於等於
                a = 5 <= 0;  //false
            }

            {//== 等於，可以比較物件
                a = 5 == 0;  //false
            }

            {//!= 不等於，可以比較物件
                a = 5 != 0;  //true
            }

            Console.WriteLine(a);
        }
        void field_oper()  //.()^..[]索引運算子
        {
            {//[] 陣列索引子
                int[] fib = new int[10];
                fib[0] = fib[1] = 1;
            }

            {//[] 字典/雜湊索引子
                var dict = new Dictionary<string, double>();
                dict["one"] = 1;
                dict["pi"] = Math.PI;
            }

            {//^ 結束索引子，表示序列結尾位置，[^n] 指向 length - n
                int[] xs = new[] { 0, 10, 20, 30, 40 };
                int last = xs[^1]; // output: 40
                Index toFirst = ^xs.Length; // xs[toFirst] output: 0
            }

            {//.. 範圍索引子
                int[] numbers = new[] { 0, 10, 20, 30, 40, 50 };
                int[] subset = numbers[1..3]; // output: {10 20 30}
                int[] inner = numbers[1..^1];// output: {10 20 30 40}
            }
        }
        void type_oper()  //typeof default nameof sizeof new is switch as (T)型態運算子
        {

            {//typeof 查詢類型
                Type t = typeof(int);  //x.GetType();
                Console.WriteLine(t.Name); // 輸出 "Int32"
            }

            {//default 數值型態默認值，如果類別則是null
                int a = default(int); //0
            }

            {//nameof 查詢名稱
                Console.WriteLine(nameof(System.Collections.Generic));  // output: Generic
                Console.WriteLine(nameof(List<int>));  // output: List
                Console.WriteLine(nameof(List<int>.Count));  // output: Count
            }

            {//sizeof 型態大小
                Console.WriteLine(sizeof(byte));  // output: 1
                Console.WriteLine(sizeof(double));  // output: 8
            }

            {//new 實例化，以下使用匿名型別
                var v = new { Amount = 108, Message = "Hello" };

                Console.WriteLine(v.Amount + v.Message);
            }

            {//is 判斷運算，詳情查看Expression  匹配運算
                object a = 10;
                if (a is Int32) Console.WriteLine("x is an integer");
            }

            {//switch 比對運算，詳情查看Expression  匹配運算
                object t = "asdf";
                switch (t)
                {
                    case string:
                        break;
                    case int:
                        break;
                }

                string Classify(double measurement) => measurement switch
                {
                    < -40.0 => "Too low",
                    >= -40.0 and < 0 => "Low",
                    >= 0 and < 10.0 => "Acceptable",
                    >= 10.0 and < 20.0 => "High",
                    >= 20.0 => "Too high",
                    double.NaN => "Unknown",
                };
            }

            {//as 型態轉換，嘗試轉換成功賦值，失敗則null
                Object x = 'a';
                string? s = x as string; //s = "a"
            }

            {//(T) 轉換運算子，有可能錯誤
                double c = 1234.7;
                int d = (int)c; // d = 1234
            }
        }
        void pointer_oper()  //&*->[]指標
        {
            //需添加 <AllowUnsafeBlocks>true</AllowUnsafeBlocks>
            unsafe
            {
                {//& 傳址運算子，取得指標，需要對應 * 指標類型
                    int number = 27;
                    int* pointerToNumber = &number; //Address : 6C1457DBD4
                }
                {//* 指標間接運算子
                    char letter = 'A';
                    char* pointerToLetter = &letter; //Address : DCB977DDF4
                    *pointerToLetter = 'Z'; // letter = Z
                }

                {//-> 指標成員存取運算子 
                    Coords coords;
                    Coords* p = &coords;
                    p->X = 3; //coords.X = 3;
                    p->Y = 4; //coords.Y = 4;
                }

                {//[] 指標元素存取運算子，以指標類型 sizeof(T) 移動
                    char* pointerToChars = stackalloc char[123]; //配置 char[123] 記憶體區塊

                    for (int i = 65; i < 91; i++) pointerToChars[i] = (char)i;
                    //pointerToChars[i] = {A,B,C,D,E,F,G,H,IJ,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z}
                }

                {// 指標中加減，加減 sizeof(T)
                    int[] numbers = new int[3] { 10, 20, 30 };
                    fixed (int* pointerToFirst = &numbers[0]) //防止記憶體回收行程重新放置可移動的變數
                    {
                        int* pointerToLast = pointerToFirst + 2;
                        // *pointerToFirst = 10 , address 1818345918136
                        // *pointerToLast = 30 , address 1818345918144
                        //p + n = p + n * sizeof(T)
                        //p - n = p - n * sizeof(T)

                        Console.WriteLine($"Value {*pointerToFirst} at address {(long)pointerToFirst}");
                        Console.WriteLine($"Value {*pointerToLast} at address {(long)pointerToLast}");
                    }
                }

                {// 指標減法，減少 sizeof(T)
                    int* numbers = stackalloc int[] { 0, 1, 2, 3, 4, 5 }; //配置 int[6] 記憶體區塊
                    int* p1 = &numbers[1];
                    int* p2 = &numbers[5];
                    // &numbers[5] - &numbers[1] // P2 address - P1 address = 4
                    // ((long)(p1) - (long)(p2)) / sizeof(T)
                }

                {// 指標遞增和遞減，加減 sizeof(T)
                    int* numbers = stackalloc int[] { 0, 1, 2 };
                    int* p1 = &numbers[0];
                    //++p1 = p1+1
                    //--p1 = p1+1
                }

                {// stackalloc 堆疊記憶體，離開創建範圍後立刻回收
                    int* numbers = stackalloc int[] { 0, 1, 2 };
                }
            }

        }
        public struct Coords
        {
            public int X;
            public int Y;
        }
        void other_oper() //特殊運算子
        {
            {//?: 三元條件，條件內為 bool
                var rand = new Random();

                var x = (rand.NextDouble() > 0.5) ? 12 : (int?)null;

                // 如果多重三元條件如下
                // a? b : c? d : e
                // a ? b: (c ? d : e)
            }

            {//=> Lambda 運算子
                int[] numbers = { 4, 7, 10 };
                int product = numbers.Aggregate(1, (int interim, int next) => interim * next);
            }

            {//:: 命名空間別名
                forwpf::Input.ICommand i;
            }

            {//_ 參數捨棄，直接丟棄參數
                Func<int, int, int> constant = delegate (int _, int _) { return 42; };
            }

            {//$ 字串插入
                string name = "Mark";
                var date = DateTime.Now;

                Console.WriteLine("Hello, {0}! Today is {1}, it's {2:HH:mm} now.", name, date.DayOfWeek, date);
                Console.WriteLine($"Hello, {name}! Today is {date.DayOfWeek}, it's {date:HH:mm} now.");

            }

            {//@ 特殊字元失效
                string[] @for = { "John", "James", "Joan", "Jamie" }; //可以使用 for 作為變數名稱
                string filename1 = @"c:\documents\files\u0066.txt"; //依據字串完整輸出，沒有特殊自元
            }
        }
    }
    #endregion
    #region ()自訂轉換運算子
    //使用 implicit/explicit operator 增加定義
    // public static implicit operator  自動轉型
    // public static explicit operator  強轉型
    struct Digit
    {
        byte digit;
        public Digit(byte digit) => this.digit = digit;

        //自動轉型
        public static implicit operator byte(Digit d) => d.digit;
        //強轉型
        public static explicit operator Digit(byte b) => new Digit(b);
    }
    //使用
    // var d = new Digit(7);
    // byte number = d; // 自動轉型 output: 7
    // Digit digit = (Digit)number; // 強轉型 output: 7

    //自訂運算子
    public readonly struct Fraction
    {
        private readonly int num;
        private readonly int den;

        public Fraction(int numerator, int denominator)
        {
            if (denominator == 0)
            {
                throw new ArgumentException("Denominator cannot be zero.", nameof(denominator));
            }
            num = numerator;
            den = denominator;
        }

        public static Fraction operator +(Fraction a) => a;
        public static Fraction operator -(Fraction a) => new Fraction(-a.num, a.den);

        public static Fraction operator +(Fraction a, Fraction b)
            => new Fraction(a.num * b.den + b.num * a.den, a.den * b.den);

        public static Fraction operator -(Fraction a, Fraction b)
            => a + (-b);

        public static Fraction operator *(Fraction a, Fraction b)
            => new Fraction(a.num * b.num, a.den * b.den);

        public static Fraction operator /(Fraction a, Fraction b)
        {
            if (b.num == 0)
            {
                throw new DivideByZeroException();
            }
            return new Fraction(a.num * b.den, a.den * b.num);
        }

        public override string ToString() => $"{num} / {den}";
    }

    #endregion
}