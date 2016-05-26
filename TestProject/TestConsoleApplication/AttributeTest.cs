using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace TestConsoleApplication
{
    internal class AttributeTest
    {
        public int Main()
        {
            //TODO:多点反射？？
            //object obj = Activator.CreateInstance(typeof(int));
            return NativeMethods.MessageBox(0, "How to use attribute in .NET", "Anytao_net", 0);
        }
    }
}