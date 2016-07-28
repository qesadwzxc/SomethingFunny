////////////////////////////////////////////////////////////////////////////////
///C#6.0测试
////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using static System.Console;

namespace TestConsoleApplication
{
    public class TestCSharp6
    {
        public List<string> list = new List<string>(2) { [0] = "A", [1] = "B" };//value for index

        public void TestFunction()
        {
            try
            {
                Student stu = new Student();
                string item = list?[1];//?[]

                int? id = stu?.stuID;//?.Property
                string name = stu?.ToString();//?.Func()

                //var res=await publics();
            }
            catch (Exception ex)when(ex.Message != "")//catch...when...
            {
                WriteLine($"{nameof(ex)}:{ex.Message}");
            }
        }

        //public async Task<string> publics()
        //{
        //    return Task.Run(() => { return ""; })
        //} 
    }

    public class Student
    {
        public int stuID { get; set; } = 1;//default value
        public string stuName { get; set; }

        public string fullName => stuID.ToString() + " " + stuName;//delegate for Property

        public override string ToString() => $"ID is {stuID},Name is {stuName}";//delegate for Func && $"{}"
    }
}
