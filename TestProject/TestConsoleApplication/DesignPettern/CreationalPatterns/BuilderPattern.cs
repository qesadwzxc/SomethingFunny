////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///简单工厂模式
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///优点:1、建造者独立，易扩展。 
///     2、便于控制细节风险。
///缺点:1、产品必须有共同点，范围有限制。 
///     2、如内部变化复杂，会有很多的建造类。
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;

namespace TestConsoleApplication.DesignPettern.CreationalPatterns
{
    class BuilderPattern
    {
        public void Run()
        {
            MealBuilder builder = new MealBuilder();
            var vegMeal = builder.prepareVegMeal();
            Console.WriteLine("Veg Meal");
            vegMeal.showItems();
            Console.WriteLine("Total Cost: " + vegMeal.getCost());
        }

        #region 建造者(Builder),选择建造者构建具体产品
        public class MealBuilder
        {
            #region 建造者1
            public Meal prepareVegMeal()
            {
                Meal meal = new Meal();
                meal.AddItem(new VegBurger());
                meal.AddItem(new Coke());
                return meal;
            }
            #endregion

            #region 建造者2
            public Meal prepareNonVegMeal()
            {
                Meal meal = new Meal();
                meal.AddItem(new ChickenBurger());
                meal.AddItem(new Pepsi());
                return meal;
            }
            #endregion 
        }
        #endregion

        #region 对象属性一
        public interface IPacking
        {
            string Pack();
        }

        class Wrapper : IPacking
        {
            public string Pack()
            {
                return "Wrapper";
            }
        }
        class Bottle : IPacking
        {

            public string Pack()
            {
                return "Bottle";
            }
        }
        #endregion

        #region 抽象对象
        public interface IItem
        {
            string Name();
            IPacking Packing();
            float Price();
        }

        abstract class Burger : IItem
        {
            public IPacking Packing()
            {
                return new Wrapper();
            }
            public abstract string Name();
            public abstract float Price();
        }

        abstract class ColdDrink : IItem
        {
            public IPacking Packing()
            {
                return new Bottle();
            }
            public abstract string Name();
            public abstract float Price();
        }
        #endregion

        #region 对象一
        class VegBurger : Burger
        {
            public override string Name()
            {
                return "Veg Burger";
            }

            public override float Price()
            {
                return 15.0f;
            }
        }

        class ChickenBurger : Burger
        {
            public override string Name()
            {
                return "Chicken Burger";
            }

            public override float Price()
            {
                return 12.0f;
            }
        }
        #endregion

        #region 对象二
        class Pepsi : ColdDrink
        {
            public override string Name()
            {
                return "Coke";
            }

            public override float Price()
            {
                return 6.0f;
            }
        }

        class Coke : ColdDrink
        {
            public override string Name()
            {
                return "Pepsi";
            }

            public override float Price()
            {
                return 8.0f;
            }
        }
        #endregion

        #region 包装类，打包对象成产品
        public class Meal
        {
            private List<IItem> Items = new List<IItem>();

            public void AddItem(IItem item)
            {
                Items.Add(item);
            }

            public float getCost()
            {
                float cost = 0.0f;
                foreach (IItem item in Items)
                {
                    cost += item.Price();
                }
                return cost;
            }

            public void showItems()
            {
                foreach (IItem item in Items)
                {
                    Console.WriteLine("Item : " + item.Name());
                    Console.WriteLine(", Packing : " + item.Packing().Pack());
                    Console.WriteLine(", Price : " + item.Price());
                }
            }
        }
        #endregion
    }
}
