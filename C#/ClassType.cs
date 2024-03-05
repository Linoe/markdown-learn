using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.ComponentModel;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Text;
using System.Windows.Input;

namespace ClassType
{
    #region class 繼承
    //類別
    public class Aclass
    {
        //變數家族，外部無法使用，繼承類可以
        protected int cost;
        //建構式
        public Aclass(int cc) { cost = cc; }
        //方法
        public virtual int getCostA()
        {
            return cost;
        }
        //可複寫方法
        public virtual int getCostB()
        {
            return cost;
        }
    }

    //繼承
    public class Bclass : Aclass
    {
        //變數公開與私有
        private int _cost;
        public int mCost { get { return _cost; } set { _cost = value; } }

        public int mYear;
        //父建構，base呼叫父類建構式
        public Bclass(int cc) : base(cc)
        { mCost = cc; }
        //子建構，this呼叫自己建構式
        public Bclass(int cc, int year) : this(cc)
        { mYear = year; }
        //覆寫父方法
        public override int getCostB()
        { return mCost; }
    }
    #endregion
    #region abstract 抽象
    //抽象，類似class，不能直接 new()，abstract 修飾項目繼承必須實現
    abstract class Shape
    {
        public int Width;
        public int Height;
        public int Length;
        //abstract 修飾後不能實作
        public abstract double Area { get; }
        public abstract double Size();
    }
    //抽象繼承
    class Rectangle : Shape
    {
        //必須實現修飾 abstract 方法
        public override double Area
        {
            get { return Width * Height; }
        }

        public override double Size()
        {
            return Area * Length;
        }
    }
    #endregion
    #region interface 介面
    //介面，必須實作所有方法
    interface IPrintable1
    {
        //方法，不能實作
        void Print();
    }
    interface IPrintable2
    {
        //方法，不能實作
        void Say();
    }

    class Document : IPrintable1, IPrintable2
    {
        //繼承的介面方法必須實現
        public void Print()
        {
            Console.WriteLine("Document");
        }

        public void Say()
        {
            Console.WriteLine("Say");
        }
    }
    //抽象跟介面差異
    // 介面只能宣告方法不能實現。抽象類別可以。
    // 介面不能建立成員變數（field），抽象類別可以。
    // 介面可以同時繼承，抽象類別同一般類別只能唯一繼承。
    #endregion
    #region enum 枚舉型態
    class EnumFoo
    {
        //枚舉，只定義數值，傳遞上與 Value Type 等同，可以透過繼承更改數值
        // public enum Color : byte
        public enum Color
        {
            //指定數字，當沒有指定時從 0 開始
            RED = 1,
            BLUE = 2,
            WHITE    //3，按照 int 順序增加
        }

        //枚舉添加屬性而外增加訊息
        public enum Sex
        {
            [Description("男")]
            Male = 1,

            [Description("女")]
            Female = 2
        }

        //允許邏輯運算
        [Flags]
        public enum Days
        {
            None = 0b_0000_0000,  // 0
            Monday = 0b_0000_0001,  // 1
            Tuesday = 0b_0000_0010,  // 2
            Wednesday = 0b_0000_0100,  // 4
            Thursday = 0b_0000_1000,  // 8
            Friday = 0b_0001_0000,  // 16
            Saturday = 0b_0010_0000,  // 32
            Sunday = 0b_0100_0000,  // 64
            Weekend = Saturday | Sunday
        }

        void Say()
        {
            //Enum 宣告，實際上並沒有創建
            Color color = Color.BLUE;

            {//轉型
                //將 enum 轉換為 int
                int num = (int)Color.RED;
                //將 int 轉換為 enum
                Color c1 = (Color)num;
                Color c3 = (Color)1;
                //將 enum 轉換為 string
                String str = c1.ToString();
                //將 string 轉換為 enum
                Color c2 = (Color)Enum.Parse(typeof(Color), str);
                //將 enum 轉換為 enum，因為是 value Type
                Sex s = (Sex)Color.RED;
            }

            {//switch case 
                switch (color)
                {
                    case Color.RED:
                    case Color.BLUE:
                    case Color.WHITE:
                        break;
                }
            }

            {//透過過充 enum 映射抓取訊息
                var value = color;
                string s = value.GetType()
                .GetRuntimeField(value.ToString())
                ?.GetCustomAttributes<System.ComponentModel.DescriptionAttribute>()
                .FirstOrDefault()?.Description ?? string.Empty;
            }

        }
    }
    #endregion
    #region struct 結構型態
    //結構，傳遞會用 value type，變數會被儲存在記憶體(static?)，通常較快
    //不能繼承但可以介面
    struct Coords
    {
        double X { get; }
        double Y { get; }

        //不允許默認建構式
        // Coords(){}
        Coords(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override string ToString() => $"({X}, {Y})";
    }
    //結構 struct 與 類別 class 差異
    // 1. struct 是傳遞值，而 class 是傳遞參考。
    // 2. struct 預設繼承 System.ValueType
    // 3. struct 不能繼承其他，可以實作介面
    // 4. struct 不能被繼承其他類別或介面。

    //修飾 readonly 實例化後不能再改變，底下成員皆唯獨，將增加不變性
    readonly struct Coords2
    {
        double X { get; init; }//不可使用 set
        double Y { get; init; }

        Coords2(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override string ToString() => $"({X}, {Y})";
    }
    //想要轉值使用 with 
    // var p1 = new Coords(0, 0);
    // var p2 = p1 with { X = 3 };

    //record 可以簡略 struct
    public record struct Point(int X, int Y) { };
    //等同下面
    // public struct Point : IEquatable<Point>
    // {
    //     public int X { get; set; }
    //     public int Y { get; set; }
    // }

    //ref struct C# 11
    #endregion
    #region record 紀錄型態
    //record 數值類型，定義一個結構，初始化後不可變
    class RecordFoo
    {
        public record Person(string FirstName, string LastName) { }
        //等同以下程式碼
        // public record Person1
        // {
        //     public string FirstName { get; init; } = default!;
        //     public string LastName { get; init; } = default!;
        // }

        //可以添加屬性
        record Person2([property: JsonPropertyName("firstName")] string FirstName) { }

        //可內部建立成員或方法
        record Person3()
        {
            public string[]? PhoneNumbers { get; init; }
            public string Say() { return "Hello"; }
        }

        void Use() //使用方式
        {
            //實例
            Person person1 = new("Nancy", "Davolio");
            Person3 person3 = new() { PhoneNumbers = new string[1] };

            //with 複製修改值，只能用在新變數
            Person person2 = person1 with { FirstName = "John" };

            //比較(==)時比較數值非參考
            Person person4 = person1 with { };
            // person1 == person4; // output: True

            //直接解構到元祖
            var (firstName, lastName) = person1;

        }

        //繼承只能與同紀錄型態
        public record Person4(string FirstName, string LastName, string MinName) : Person(FirstName, LastName) { }

        //可以建立抽象
        public abstract record Person5(string FirstName, string LastName) { };
        public record Teacher(string FirstName, string LastName, int Grade) : Person5(FirstName, LastName) { };
        public record Student(string FirstName, string LastName, int Grade) : Person5(FirstName, LastName) { };

        // class/struct/record 比較時差異
        // class : 參考比較，如果兩個物件參考記憶體中的相同物件，則相等。
        // struct : 數值與類型比較，如果兩個物件屬於相同類型，並儲存相同的值，則等。
        // record / record struct / readonly record struct : 等同 struct

        //紀錄不限制以下宣告，會造成不變性消失
        public record Person6
        {
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
        }
    }
    #endregion
    #region generic <T> 泛型
    ////泛型，<T> 用來給使用者任意填充
    class MyGenericArray<T>
    {
        private T[]? array;
        public T getArray(T a) { return a; }
    }
    //使用
    //MyGenericArray<int> intArray = new MyGenericArray<int>(5);

    //泛型約束，<T>限制介面
    class GenericType<T> where T : IAsyncDisposable { }
    //可約束的種類
    // T : class 限制 T 泛型類型必須是引用類型。
    // T : struct：限制 T 泛型類型必須是值類型不能為 Nullable。
    // T : new()：限制 T 泛型類型必須具有默認建構函數。
    // T : base class name：限制 T 泛型類型必須繼承自指定的類別。
    // T : interface name：限制 T 泛型類型必須實現指定的介面。

    //兩個泛型約束
    class Test<T, U>
        where U : struct
        where T : new()
    { }

    //同時繼承與泛型約束
    interface MyWhereInterface<T>
    { }
    class MyWhereClass<T> : MyWhereInterface<T> where T : struct
    { }

    //方法泛型
    class SampleClass
    {
        //委派
        delegate void StackEventHandler<T, U>(T sender, U eventArgs);
        //方法
        public void HandleStackChange<T>(Stack<T> stack) { }
    }
    #endregion
    #region <in/out T> 泛型變數
    class genericInOut
    {
        interface ICovariant<out R>
        {
            R GetSomething();
            // void SetSomething(R sampleArg); //錯誤
            void DoSomething(Action<R> callback);
            // void DoSomething<T>() where T : R; //錯誤
        }
        interface IContravariant<in A>
        {
            void SetSomething(A sampleArg);
            void DoSomething<T>() where T : A;
            // A GetSomething(); //錯誤
        }
        interface IVariant<out R, in A>
        {
            R GetSomething();
            void SetSomething(A sampleArg);
            R GetSetSomethings(A sampleArg);
        }
    }

    //<out T>自訂泛型轉型，限定 泛型可以用在回傳上
    //允許泛型可以默認轉換為基底類別(父類別)
    class VariantOut
    {
        interface ICovariant<out R>
        {
            R GetSomething();
        }
        class SampleImplementation<R> : ICovariant<R>
        {
            public R GetSomething()
            {
                // Some code.
                return default(R);
            }
        }
        class Button { }

        void Say()
        {
            // The interface is covariant.
            ICovariant<Button> ibutton = new SampleImplementation<Button>();
            ICovariant<Object> iobj = ibutton;

            // The class is invariant.
            SampleImplementation<Button> button = new SampleImplementation<Button>();
            // SampleImplementation<Object> obj = button; //錯誤

        }
    }

    //<out T>自訂泛型轉型，限定 泛型可以用在參數上
    //默認轉換為延伸類別(子類別)
    class VariantIn
    {
        interface IObject<in A>
        {
            string typeOf();
        }
        class samlpe<A> : IObject<A>
        {

            string IObject<A>.typeOf()
            {
                return typeof(A).ToString();
            }
        }
        class samlpeB<A> : IObject<A>
        {

            string IObject<A>.typeOf()
            {
                return typeof(A).ToString();
            }
        }


        void Say(string[] args)
        {
            IObject<object> _object = new samlpe<Object>();
            IObject<string> _string = new samlpeB<String>();
            _string = _object;//可以轉換成子型別
        }
    }
    //委派本身就自帶 in
    class VariantFunc
    {
        void Say(string[] args)
        {
            //public delegate TResult Func<in T, out TResult>(T arg);
            //方法的雖然規定傳入object 回傳string
            //但因 FUNC 允許隱含轉換，因此可以傳入string 回傳object
            Func<object, string> testFunc = test;
            object yy = testFunc("");
        }

        static string test(object a)
        {
            return "";
        }
    }
    #endregion
    #region Extension Methods 擴充方法
    //擴充方法，只能用在 static
    //class 隨意命名
    public static class StringExtensions
    {
        //參數使用想要擴充的對象，修飾 this
        public static int WordCount(this string str)
        {
            return str.Split(' ').Length;
        }
        //需要其他參數追加在後方
        public static int WordCount(this string str, int i)
        {
            return str.Split(' ').Length;
        }
    }
    //使用方式
    // string s = "Hello world";
    // int count = s.WordCount(); // Output: 2

    //擴充搭配介面
    public interface IMyInterface
    {
        void MethodB();
    }
    public static class Extension
    {
        public static void MethodA(this IMyInterface myInterface, int i)
        {
            Console.WriteLine("Extension.MethodA(this IMyInterface myInterface, int i)");
        }

        // 無效，介面繼承後必定實作
        public static void MethodB(this IMyInterface myInterface)
        {
            Console.WriteLine("Extension.MethodB(this IMyInterface myInterface)");
        }
    }
    class A : IMyInterface
    {
        public void MethodB() { Console.WriteLine("A.MethodB()"); }
    }
    class B : IMyInterface
    {
        public void MethodB() { Console.WriteLine("B.MethodB()"); }
        public void MethodA(int i) { Console.WriteLine("B.MethodA(int i)"); }
    }
    //結果
    //A.MethodA(1);  沒有實作執行 "Extension.MethodA(this IMyInterface myInterface, int i)"
    //B.MethodA(1);  有實作執行 "B.MethodA(int i)"
    //A.MethodB(1);  有實作執行 "A.MethodB()"
    //B.MethodB(1);  有實作執行 "B.MethodB(int i)"
    #endregion
    #region class partial 類別合併
    //partial 類別合併，編譯時會將所有同名合併
    [SerializableAttribute]
    public partial class Employee
    {
        public void DoWork() { }
    }
    [ObsoleteAttribute]
    public partial class Employee
    {
        public void GoToLunch() { }
    }
    //等同於
    // [SerializableAttribute]
    // [ObsoleteAttribute]
    // public  class Employee
    // {
    //     public void DoWork() { }
    //     public void GoToLunch() { }
    // }
    // 合併可以使用在 class interface struct
    #endregion
    #region delegate/func 委派
    //delegate 委託，用來當作參數傳遞
    class ClassDelegate
    {
        // 定義一個 delegate 型別的 宣告
        delegate int ProcessDelegate(int x);

        // 定義一個 delegate 引數的函式
        void ProcessData(ProcessDelegate process)
        {
            int result = process(5);
            Console.WriteLine(result);
        }

        void Say()
        {
            {//建立內部函式，宣告相同的參數與回傳
                int AddTen(int x) { return x + 10; }
                ProcessDelegate pd = AddTen;
                ProcessData(AddTen);  // 輸出 15
            }

            {//具有加減同時呼叫
                int AddTen1(int x) { return x + 20; }
                int AddTen2(int x) { return x + 30; }
                ProcessDelegate pd = AddTen1;
                pd += AddTen2;
                ProcessData(pd);  // 輸出 25, 35
            }
        }
    }
    //使用方式

    // 參數為 Func 委派型態，用來省略宣告部分
    // Func<in T, out TResult> 第一個參數引數，第二個參數是回傳
    class ClassFunc
    {
        // 定義一個 Func 引數的函式
        void ProcessData(Func<int, int> process)
        {
            int result = process(5);
            Console.WriteLine(result);
        }

        void Say()
        {
            {//建立內部函式，宣告相同的參數與回傳
                int AddTen(int x) { return x + 10; }
                Func<int, int> fu = AddTen;
                ProcessData(fu);  // 輸出 15
            }

            {//具有加減同時呼叫
                int AddTen1(int x) { return x + 20; }
                int AddTen2(int x) { return x + 30; }
                Func<int, int> fu = AddTen1;
                fu += AddTen2;
                ProcessData(fu);  // 輸出 25, 35
            }
        }

        // delegate 與 Func 差異
        // delegate 關鍵字，Func 是一个泛型委托類型
        // delegate 必須要先定義，Func 使用時指定
    }
    #endregion
    #region method in/out/ref 方法參數修飾
    // 方法傳遞參數 in out ref。
    //     in  ：表示參數只能用於輸入，不能用於輸出。方法不能修改 in 參數的值。
    //     out ：表示參數只能用於輸出，不能用於輸入。方法必須在執行完之前將值賦給 out 參數。
    //     ref ：表示參數既可用於輸入，也可用於輸出。方法可以修改 ref 參數的值。
    class SomeMethod
    {
        static void SomeMethod1(in int x, out int y, ref int z)
        {
            // x = 10; // 錯誤：無法修改 in 參數的值
            y = 20;//必須值賦，否則錯誤，會影響外部，因為是參考
            z = 30;//如果修改數值，會直接影響外部，因為是參考
        }
    }
    //使用
    // int a = 1, b = 2, c = 3;
    // SomeMethod(a, out b, ref c); //結束後 a = 1; b = 20; c = 30
    #endregion
    #region method virtual/override/new 方法覆蓋
    //virtual 允許方法可以被覆蓋
    //override 繼承方法覆蓋，優先使用實例對象
    //new 繼承方法隱藏，優先使用包裝對象
    class Type1
    {
        public virtual void Method1()
        {
            Console.WriteLine("Base - Method1");
        }

        public virtual void Method2()
        {
            Console.WriteLine("Base - Method2");
        }
    }

    class DerivedClass : Type1
    {
        public override void Method1()
        {
            Console.WriteLine("Derived - Method1");
        }

        public new void Method2()
        {
            Console.WriteLine("Derived - Method2");
        }
    }
    //使用
    // BaseClass bc = new BaseClass();
    // DerivedClass dc = new DerivedClass();
    // BaseClass bcdc = new DerivedClass();

    // bc.Method1();  //Base - Method1  
    // bc.Method2();  //Base - Method2  

    // dc.Method1();  //Derived - Method1  //優先使用實例 DerivedClass
    // dc.Method2();  //Derived - Method2  //優先使用包裝 DerivedClass

    // bcdc.Method1();  //Derived - Method1  //優先使用實例 DerivedClass
    // bcdc.Method2();  //Base - Method2  //優先使用包裝 BaseClass

    #endregion
    #region fields new 成員隱藏

    //new 修飾在屬性時，隱藏基底優先使用修飾 new 的屬性
    class BaseC
    {
        public static int x = 55;
    }

    class DerivedC : BaseC
    {
        // Hide field 'x'.
        new public static int x = 100;
    }
    //使用
    // Console.WriteLine(x); //100
    // Console.WriteLine(BaseC.x); //55

    //假如在巢狀類別中修飾，隱藏基底優先使用修飾 new 的class
    class BaseD
    {
        public class NestedC
        {
            public int x = 200;
        }
    }

    class DerivedD : BaseD
    {
        new public class NestedC
        {
            public int x = 100;
        }
    }
    //使用
    // Console.WriteLine(NestedC.x); //100
    // Console.WriteLine(BaseD.NestedC.x); //200
    #endregion
    #region fields readonly/const 成員常數
    //readonly/const 定義屬性不可變的
    class ClassReadConst
    {
        //一旦初始化後就不能在修改
        readonly int Start;
        //只能在宣告時定義
        const int End = 10;
        ClassReadConst()
        {
            Start = 10;
        }
        //定義不可改變的靜態數值
        static readonly int sStart = 10;
    }
    //readonly/const 差異
    //readonly 可以在宣告後在方法內初始化，const 不行
    //readonly 是在運行時定義，const 在編譯時直接覆蓋數值(直接修改程式碼)
    //readonly 可以任意型態，const 只能基礎型態
    #endregion
    #region fields get/set/init 成員存取
    //設定屬性能否被外部讀取修改
    //get 允許外部讀取
    //set 允許外部修改
    //init 只能初始化一次
    public class DateGetSet
    {
        //通常用來保護另一個內部變數
        private int _month = 7;
        public int Month
        {
            get => _month; // get {return _month;}
            set => _month = value;// set {value = _month;}
        }
        //可以簡化類部成員
        public DateGetSet(string firstName, string name)
        {
            this.FirstName = firstName;
            Name = name;

        }
        public string FirstName { get; set; } = "Jane";
        //必須在建構是初始化
        //或是允許可設為 null
        public string Name { get; init; }
    }
    //init 可以搭配 readonly 保持內部效果
    class TestInitModel
    {
        private readonly string? _name;
        public string? Name
        {
            get => _name;
            init => _name = value;
        }
    }
    #endregion
    #region public protected private internal 存取範圍
    //用來規範類別，成員，方法可以存取的範圍
    class ClassPPP
    {
        //外部可以存取
        public int public_int = 10;
        //只有自己與繼承對象可以存取
        protected int protected_int = 20;
        //只有自己可以存取
        private int private_int = 30;
        //相同組件(dll)可以存取
        internal int internal_int = 40;
        //protected internal 自己或相同組件(dll)或是不同組件(dll)的繼承對象可以存取
        //private protected 自己或相同組件(dll)且繼承對象可以存取
    }
    //反射抓取 private
    // Type type = typeof(ClassPPP);
    // FieldInfo fieldInfo = type.GetField("private_int", BindingFlags.Instance | BindingFlags.NonPublic);
    // fieldInfo.GetValue(new ClassPPP());
    #endregion
    #region unsafe/fixed/stackalloc 不安全記憶體操作
    class UnsafeClass
    {
        //unsafe 用來表示這個區塊內可以會造成程式崩潰
        //用在修飾類別或方法，代表整個不安全
        //public unsafe struct Node
        //public unsafe void F() {}
        //public fixed char fixedBuffer[128];  //使用能在 unsafe 類別，代表固定緩衝
        void Say()
        {
            unsafe
            {//fixed 區塊內指標唯讀，不會被回收
                byte[] bytes = { 1, 2, 3 };
                fixed (byte* pointerToFirst = bytes)
                {
                    Console.WriteLine($"The address of the first array element: {(long)pointerToFirst:X}.");
                    Console.WriteLine($"The value of the first array element: {*pointerToFirst}.");
                }
            }
            unsafe
            {//stackalloc 當離開區塊時會被自動回收
                int length = 3;
                int* numbers = stackalloc int[length];
                for (var i = 0; i < length; i++)
                {
                    numbers[i] = i;
                }
            }
        }
    }
    #endregion
    #region ref 參考
    class refFoo
    {
        //ref將對象以傳址方式重新宣告，當操作時會影響到原本變數
        //當作一種不使用指標運算子保存指標位置
        void Say()
        {
            {//參數以傳址方式傳遞
                void Method(ref int refArgument) { refArgument = refArgument + 44; }

                int number = 1;
                Method(ref number);
                Console.WriteLine(number); // Output: 45
            }
            {//回傳以傳址方式傳遞，必須以修飾 ref 作為傳值，之後存取只能使用 ref 修飾
                ref int Find(int[] matrix)
                {
                    return ref matrix[0];
                }
                ref int estValue = ref Find(new int[] { 0 });
            }
            {//不使用指標運算子，傳址操作陣列

                double[] arr = { 0.0, 0.0, 0.0 };

                ref double arrayElement = ref arr[0];
                arrayElement = 3.0; //arr = {3, 0, 0}

                arrayElement = ref arr[arr.Length - 1];
                arrayElement = 5.0; //arr = {3, 0, 5}
            }
        }
    }

    #endregion
    #region readonly ref struct 參考唯讀
    //
    //readonly ref：引用只能唯讀，引用使用在在建構式或 init 方法之外被修改
    //ref readonly：唯讀對象的引用，这个引用指向的对象不能在构造方法或 init 方法之外被修改
    //readonly ref readonly：唯讀對象的引用只能唯讀，上述兩種组合
    // ref struct Foo
    // {
    //     ref readonly int f1;
    //     readonly ref int f2;
    //     readonly ref readonly int f3;

    //     void Bar(int[] array)
    //     {
    //         f1 = ref array[0];  // 没问题
    //         f1 = array[0];      // 错误，因为 f1 引用的值不能被修改
    //         f2 = ref array[0];  // 错误，因为 f2 本身不能被修改
    //         f2 = array[0];      // 没问题
    //         f3 = ref array[0];  // 错误：因为 f3 本身不能被修改
    //         f3 = array[0];      // 错误：因为 f3 引用的值不能被修改
    //     }
    // }
    #endregion
    #region Covariance/Contravariance 共變數/反變數
    // Covariance 共變數, 代表可以在原本的型別處使用衍伸型別(使用繼承後的子類)
    // Contravariance 反變數, 代表可以在原本的型別處使用更泛化的型別(使用自身的父類)
    class Covaravariance
    {
        class Type1 { }
        class Type2 : Type1 { }
        class Type3 : Type2 { }

        void Say()
        {
            {//衍生對象
                string str = "test";
                object obj = str;
            }
            {//陣列
                Type1[] fruits2 = new Type2[1];
            }
            {//泛型衍生
                IEnumerable<string> strings = new List<string>();
                IEnumerable<object> objects = strings;

                IEnumerable<Type1> fruits3 = new List<Type2>();
            }
            {//泛型委派
                static void SetObject(object o) { }
                Action<object> actObject = SetObject;
                Action<string> actString = actObject;
            }
            {//委派
                Type3 MyMethod(Type1 t) { return new Type3(); }

                Func<Type2, Type2> f1 = MyMethod;
                Func<Type3, Type1> f2 = f1;
                Type1 t1 = f2(new Type3());
            }
            {//危險的操作
             // List<Type1> animals = new List<Type2>() { new Type3() };
             // animals.Add(new Type3());  
            }
        }
    }
    #endregion
    // 運算式樹狀架構
    // https://learn.microsoft.com/zh-tw/dotnet/csharp/programming-guide/concepts/expression-trees/
    // 迭代器
    // https://learn.microsoft.com/zh-tw/dotnet/csharp/programming-guide/concepts/iterators
    // 反映
    // https://learn.microsoft.com/zh-tw/dotnet/csharp/programming-guide/concepts/reflection
    // 序列化
    // https://learn.microsoft.com/zh-tw/dotnet/csharp/programming-guide/concepts/serialization/
}