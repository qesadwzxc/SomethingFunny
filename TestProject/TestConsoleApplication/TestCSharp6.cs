////////////////////////////////////////////////////////////////////////////////
///C#6.0测试
////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using static System.Console;//1、静态引用，可以直接调用方法不需要加类名

namespace TestConsoleApplication
{
    public class TestCSharp6
    {
        public List<string> list = new List<string>(2) { [0] = "A", [1] = "B" };//2、value for index

        public void TestFunction()
        {
            try
            {
                Student stu = new Student();
                //3、?用于自动判断对象是否为空，需要注意任何类型T都会被转换为Nullable<T>
                string item = list?[1];//3.1、?[]
                int? id = stu?.stuID;//3.2、?.Property
                string name = stu?.ToString();//3.3、?.Func()
            }
            catch (Exception ex) when(ex.Message != "")//4、catch...when...
            {
                WriteLine($"{nameof(ex)}:{ex.Message}");
            }
        }
    }

    public partial class Student
    {
        public int stuID { get; set; } = 1;//5、default value

        public string stuName { get; set; }

        public string fullName => stuID.ToString() + " " + stuName;//6.1、delegate for Property

        public override string ToString() => $"ID is {stuID},Name is {stuName}";//6.2、delegate for Func && $"{}"
    }
}
