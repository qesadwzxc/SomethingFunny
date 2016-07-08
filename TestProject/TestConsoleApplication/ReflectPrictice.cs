using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;

namespace TestConsoleApplication
{
    class Country
    {
        public string name;
    }
    class Chinese : Country
    {
        public Chinese()
        {
            name = "中国";
        }
    }
    class American : Country
    {
        public American()
        {
            name = "美国";
        }
    }
    public class ReflectPrictice
    {
        public void Assem()
        {
            string AssemblyName = @"TestConsoleApplication";
            string StrongClassName = @"TestConsoleApplication.Chinese";
            Assembly assembly = Assembly.Load(AssemblyName);
            Console.WriteLine(assembly.FullName);
            Country cy;
            //创建类China的对象
            Chinese ch = (Chinese)assembly.CreateInstance(StrongClassName);
            cy = (Country)ch;
            Console.WriteLine(cy.name);
            //查找程序集中定义的类型
            Type[] types = assembly.GetTypes();
            foreach (Type t in types)
            {
                Console.WriteLine("----------------------------------------------");
                Console.WriteLine(t.Name);
                Console.WriteLine(t.BaseType.Name);
                MemberInfo[] members = t.GetMembers();
                foreach (MemberInfo m in members)
                {
                    Console.WriteLine(m.Name + "--" + m.DeclaringType + "--" + m.MemberType);
                }
            }         
        }

        public string GetFuncAndClass()
        {
            StackTrace trace = new StackTrace();
            StackFrame[] frames = trace.GetFrames();
            MethodBase method;
            string functionName = string.Empty, className = string.Empty;
            for (int i = 0; i < frames.Length - 1; i++)
            {
                method = frames[i].GetMethod();
                if ((method.Name == "lambda_method" || method.Name.StartsWith("<")) && i > 0)
                {
                    method = frames[i - 1].GetMethod();
                    functionName = method.Name;
                    className = method.ReflectedType.Name;
                }
            }
            return functionName + className;
        }
    }
}