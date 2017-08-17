////////////////////////////////////////////////////////////////////////////////
///C#7.0测试
////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Console;

namespace TestConsoleApplication
{
    public class TestCSharp7
    {
        public void TestFunction()
        {
            //1、out直接生成变量
            string str = "3";
            if(int.TryParse(str,out int num))
            {

            }
            num += 1;

            //2、关于元组的改动
            var vTuple = (1, 2);                           // 使用语法糖创建元组
            var vTuple2 = ValueTuple.Create(1, 2);         // 使用静态方法【Create】创建元组
            var vTuple3 = new ValueTuple<int, int>(1, 2);  // 使用 new 运算符创建元组
            WriteLine($"first：{vTuple.Item1}, second：{vTuple.Item2}, 上面三种方式都是等价的。");

            (int one, int two) tuple = (1, 2);                  // 左边指定字段名称
            var tuple2 = (one: 1, two: 2);                      // 右边指定字段名称
            (int one, int two) tuple3 = (first: 1, second: 2);  // 左右两边同时指定字段名称    
            /* 此处会有警告：由于目标类型（xx）已指定了其它名称，因为忽略元组名称xxx 
             实际会以左边指定的名称为准。*/
            WriteLine($"first：{tuple3.one}, second：{tuple3.two}");
            //取元素的方法
            var testTuple10 = new ValueTuple<int, int, int, int, int, int, int, ValueTuple<int, int, int>>(1, 2, 3, 4, 5, 6, 7, new ValueTuple<int, int, int>(8, 9, 10));
            WriteLine($"Item 10: {testTuple10.Rest.Item3}, Item 10: {testTuple10.Item10}");//以前多于7个元素则只能用前者，现在可以用后者


            //3、Deconstruct解构，也就是将元组中的值（类型中的属性）生成新的变量
            (int item1, int item2) = tuple;
            (_, int item3) = tuple;//用_来忽略不需要的元素
            //解构可以应用于任意类型，但需要编写 Deconstruct 方法成员
            (int id, string name) = new Student(1, "2");

            //4、Pattern matching模式匹配
            object i = 1__23_456.0__3;//5、数字中可以用_分隔，仅用于便于阅读没有实际意义（我觉得这个以后能坑死一大帮人）
            if (i is double j)//用在if中，其实挺2的
            {
                WriteLine(j.GetType());
            }
            else if (i is string k)
            {
                WriteLine(k.GetType());
            }

            switch (i)//正确的用法√
            {
                case 0: break;
                case int l: break;
                case double m when m > 0: break;//可以用case..when了~总觉得哪里怪怪的。。
                case string n: break;
                default:break;
            }

            //6、Ref locals and returns局部引用和引用返回
            int x1 = 0;
            ref int x2 = ref x1;
            x1 = 3;
            WriteLine(x2);

            ref int GetByIndex(int[] arr, int ix) => ref arr[ix];//7、Local functions局部函数
            int[] array = new int[] { 10, 20 };
            ref int x3 = ref GetByIndex(array, 1);
            x3 = 100;
            WriteLine(array[1]);

            //7、Throw expressions Throw表达式
            object e1 = i ?? throw new NullReferenceException();
            object e2 = i == null ? throw new NullReferenceException() : i;

            //8、Generalized async return types 通用异步返回类型
            var cache = cacheResult == null ? new ValueTask<string>(cacheResult) : new ValueTask<string>(loadCache());
        }

        private string cacheResult;
        private async Task<string> loadCache()
        {
            // simulate async work:
            await Task.Delay(5000);
            cacheResult = "100";
            return cacheResult;
        }
    }

    public partial class Student
    {
        public Student() { }
        private string sex;

        //9、More expression-bodied members
        public Student(int id, string name) => Construct(id,name); //可用于构造函数(多参数无力，只能这个吊样了)
        //public Student(int id, string name) => string.Format("{0}{1}", stuName = name, stuID = id);
        ~Student() => WriteLine("Finalized!");//可用于析构函数
        public string Sex
        {
            get => sex;//可用于访问器
            set => sex = value;
        }

        public void Construct(int id, string name)
        {
            stuID = id;
            stuName = name;
        }

        public void Deconstruct(out int id, out string name)
        {
            id = stuID;
            name = stuName;
        }
    }
}
