using System;
using System.Collections.Specialized;
using System.Web;

namespace TestConsoleApplication
{
    /// <summary> 
    ///Person 的摘要说明 
    /// </summary> 
    public class ThisTest
    {
        /// <summary> 
        /// 姓名 
        /// </summary> 
        public string Name { set; get; }

        /// <summary> 
        /// 性别 
        /// </summary> 
        public string Sex { set; get; }

        /// <summary> 
        /// 其它属性 
        /// </summary> 
        public NameValueCollection Attr = new NameValueCollection();

        public ThisTest()
        {
        }

        /// <summary> 
        /// /******************************************/ 
        /// /*  this用法1：限定被相似的名称隐藏的成员 */ 
        /// /******************************************/ 
        /// </summary> 
        /// <param name="Name"></param> 
        public ThisTest(string Name, string Sex)
        {
            this.Name = Name;
            this.Sex = Sex;
        }

        /// <summary> 
        /// /*******************************************/ 
        /// /* this用法2：将对象作为参数传递到其他方法 */ 
        /// /*******************************************/ 
        /// </summary> 
        public void ShowName()
        {
            Helper.PrintName(this);
        }

        /// <summary> 
        /// /*************************/ 
        /// /* this用法3：声明索引器 */ 
        /// /*************************/ 
        /// </summary> 
        /// <param name="key"></param> 
        /// <returns></returns> 
        public string this[string key]
        {
            set
            {
                Attr[key] = value;
            }
            get
            {
                return Attr[key];
            }
        }
    }

    /// <summary> 
    /// 辅助类 
    /// </summary> 
    public static class Helper
    {
        /// <summary> 
        /// /*****************************/ 
        /// /* this用法4：扩展对象的方法 */ 
        /// /*****************************/ 
        /// </summary> 
        /// <param name="item"></param> 
        /// <returns></returns> 
        public static string GetSex(this ThisTest item)
        {
            return item.Sex;
        }

        /// <summary> 
        /// 打印人名 
        /// </summary> 
        /// <param name="person"></param> 
        public static void PrintName(ThisTest person)
        {
            Console.WriteLine("姓名：" + person.Name);
        }
    }
}
