////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///简单工厂模式
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///优点：1、一个调用者想创建一个对象，只要知道其名称就可以了。 
///     2、扩展性高，如果想增加一个产品，只要扩展一个工厂类就可以。 
///     3、屏蔽产品的具体实现，调用者只关心产品的接口。
///缺点:1、工厂类集中了所有产品创建逻辑，一旦不能正常工作，整个系统都会受到影响（通俗地意思就是：一旦餐馆没饭或者关门了，很多不愿意做饭的人就没饭吃了）
///     2、系统扩展困难，一旦添加新产品就不得不修改工厂逻辑，这样就会造成工厂逻辑过于复杂。
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;

namespace TestConsoleApplication.DesignPettern.CreationalPatterns
{
    public class SimpleFactoryPattern
    {
        class Rectangle : ISimpleFactory
        {
            public void Draw()
            {
                Console.WriteLine("Rectangle");
            }
        }

        class Square : ISimpleFactory
        {
            public void Draw()
            {
                Console.WriteLine("Square");
            }
        }

        class Circle : ISimpleFactory
        {
            public void Draw()
            {
                Console.WriteLine("Circle ");
            }
        }

        public interface ISimpleFactory
        {
            void Draw();
        }

        public ISimpleFactory GetShape(String shapeType)
        {
            if (shapeType == null)
            {
                return null;
            }
            if (shapeType.Equals("CIRCLE"))
            {
                return new Circle();
            }
            else if (shapeType.Equals("RECTANGLE"))
            {
                return new Rectangle();
            }
            else if (shapeType.Equals("SQUARE"))
            {
                return new Square();
            }
            return null;
        }
    }
}
