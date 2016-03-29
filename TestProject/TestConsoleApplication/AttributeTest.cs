using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace TestConsoleApplication
{
    class AttributeTest
    {
        //TODO:需要看看非托管代码
        [DllImport("User32.dll")]
        public static extern int MessageBox(int hParent, string msg, string caption, int type);

        public int Main()
        {
            //object obj = Activator.CreateInstance(typeof(int));//TODO:多点反射？？
            return MessageBox(0, "How to use attribute in .NET", "Anytao_net", 0);
        }
    }
}