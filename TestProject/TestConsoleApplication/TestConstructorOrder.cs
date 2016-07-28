////////////////////////////////////////////////////////////////////////////////
///构造函数调用顺序测试
////////////////////////////////////////////////////////////////////////////////
using System;
namespace TestConsoleApplication
{
    class Foo
    {
        public Foo(string s)
        {
            Console.WriteLine("Foo constructor: {0}", s);
        }

        public void Bar() { Console.WriteLine("I'm Father"); }
    }

    class FooChild : Foo
    {
        public FooChild() : base("") { }
        public new void Bar() { Console.WriteLine("I'm Children"); }
    }

    class BassConstructor
    {
        readonly Foo baseFoo = new Foo("Base initializer");
        public BassConstructor()
        {
            Console.WriteLine("Base constructor");
        }
    }

    class TestConstructorOrder : BassConstructor
    {
        readonly Foo derivedFoo = new Foo("Derived initializer.");
        public TestConstructorOrder()
        {
            Console.WriteLine("Derived constructor");
        }
    }

    class ConstructorTest
    {
        public int Code { get; set; }
        public string Name { get; set; }

        public ConstructorTest() : this(0,"") { }

        public ConstructorTest(int code)
        {
            Code = code;
            Name = "";
        }

        public ConstructorTest(string name)
        {
            Code = 0;
            Name = name;
        }

        public ConstructorTest(int code, string name)
        {
            Code = code;
            Name = name;
        }
    }
}
