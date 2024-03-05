
#define DEBUG

using System.Diagnostics;
using System.Runtime.InteropServices;


namespace Attributes
{
    public class Attributes
    {
        void Say()
        {
            Conditional_fun();// #define DEBUG 不存在則會被編譯器拿掉 
        }

        //載入 DLL 方法
        [DllImport("user32.dll")]
        public static extern int MessageBox(int hWnd, string text, string caption, int type);

        static void user32_fun()
        {
            MessageBox(0, "Hello World!", "Hello", 0);
        }

        //條件編譯
        [Conditional("DEBUG")]
        static void Conditional_fun()
        {
#if DEBUG
            Console.WriteLine("#define DEBUG 不存在則會被編譯器拿掉  ");
#endif

        }

        //反射查詢
        static void GetCustomAttributes_fun()
        {
            var type = typeof(Attributes);
            var attributes = type.GetCustomAttributes(typeof(AuthorAttribute), false);

            if (attributes.Length > 0)
            {
                var author = (AuthorAttribute)attributes[0];
                Console.WriteLine("Author.version: " + author.version);
            }
        }
    }

    //自訂義 Attribute ， 呼叫時如下
    //[Author("P. Ackerman", version = 1.1)] 
    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct)]
    public class AuthorAttribute : System.Attribute
    {
        private string name;
        public double version;

        public AuthorAttribute(string name)
        {
            this.name = name;
            version = 1.0;
        }
    }

    //struct 內存依宣告順序布局
    //主要用在不同語言，平台傳遞用
    [StructLayout(LayoutKind.Sequential)]
    struct Rectangle
    {
        public int X;
        public int Y;
        public int Width;
        public int Height;
    }

}