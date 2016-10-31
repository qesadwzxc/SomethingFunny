////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///抽象工厂模式
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///优点：当一个产品族中的多个对象被设计成一起工作时，它能保证客户端始终只使用同一个产品族中的对象。
///缺点：产品族扩展非常困难，要增加一个系列的某一产品，既要在抽象的 Creator 里加代码，又要在具体的里面加代码。
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;

namespace TestConsoleApplication.DesignPettern.CreationalPatterns
{
    public class AbstractFactoryPattern
    {
        public void Run()
        {
            AbFactory f = new KFCFactory();
            f.CreateChicken().Create();
            f.CreateFrenchFries().Create();
        }

        #region 工厂
        public abstract class AbFactory
        {
            public abstract IChicken CreateChicken();
            public abstract IFrenchFries CreateFrenchFries();
        }

        class KFCFactory : AbFactory
        {
            public override IChicken CreateChicken()
            {
                return new KFCChicken();
            }

            public override IFrenchFries CreateFrenchFries()
            {
                return new KFCFrenchFries();
            }
        }

        class MCFactory : AbFactory
        {
            public override IChicken CreateChicken()
            {
                return new MCChicken();
            }

            public override IFrenchFries CreateFrenchFries()
            {
                return new MCFrenchFries();
            }
        }
        #endregion

        #region 产品1
        public interface IChicken
        {
            void Create();
        }

        class KFCChicken : IChicken
        {
            public void Create()
            {
                Console.WriteLine("KFCChicken");
            }
        }

        class MCChicken : IChicken
        {
            public void Create()
            {
                Console.WriteLine("MCChicken");
            }
        }
        #endregion

        #region 产品2
        public interface IFrenchFries
        {
            void Create();
        }

        class KFCFrenchFries : IFrenchFries
        {
            public void Create()
            {
                Console.WriteLine("KFCFrenchFries");
            }
        }

        class MCFrenchFries : IFrenchFries
        {
            public void Create()
            {
                Console.WriteLine("MCFrenchFries");
            }
        }
        #endregion
    }
}
