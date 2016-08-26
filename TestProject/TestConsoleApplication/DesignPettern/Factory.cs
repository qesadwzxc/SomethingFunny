using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsoleApplication.DesignPettern
{
    public class Factory
    {
        public void Run()
        {
            IFactory f = new PotatoFactory();
            IProduct p = f.Product();
            p.Create();
        }
    }

    public interface IFactory
    {
        IProduct Product();
    }

    class PotatoFactory : IFactory
    {
        public IProduct Product()
        {
            return new Potato();
        }
    }

    class TomatoFactory : IFactory
    {
        public IProduct Product()
        {
            return new Tomato();
        }
    }

    class CabbageFactory : IFactory
    {
        public IProduct Product()
        {
            return new Cabbage();
        }
    }

    public interface IProduct
    {
        void Create();
    }

    class Potato : IProduct
    {
        public void Create()
        {
            Console.WriteLine("Rectangle");
        }
    }

    class Tomato : IProduct
    {
        public void Create()
        {
            Console.WriteLine("Square");
        }
    }

    class Cabbage : IProduct
    {
        public void Create()
        {
            Console.WriteLine("Circle ");
        }
    }
}
