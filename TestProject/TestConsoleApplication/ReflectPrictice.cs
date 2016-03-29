using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

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
    }
}