//////////////////////////////////////////////////////////////////////////////////////////
///原型模式
//////////////////////////////////////////////////////////////////////////////////////////
///优点： 
///     1、性能提高。 
///     2、逃避构造函数的约束。
///缺点：
///     1、配备克隆方法需要对类的功能进行通盘考虑，这对于全新的类不是很难，但对于已有的类不一定很容易，特别当一个类引用不支持串行化的间接对象，或者引用含有循环结构的时候。 
///     2、必须实现 ICloneable 接口。 
///     3、逃避构造函数的约束。
//////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace TestConsoleApplication.DesignPettern.CreationalPatterns
{
    public class PrototypePattern : ICloneable
    {
        public string IDCode { get; set; }
        public int Age { get; set; }
        public string Department { get; set; }

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion

        public PrototypePattern DeepClone()
        {
            using (Stream objectStream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(objectStream, this);
                objectStream.Seek(0, SeekOrigin.Begin);
                return formatter.Deserialize(objectStream) as PrototypePattern;
            }
        }

        public PrototypePattern ShallowClone()
        {
            return Clone() as PrototypePattern;
        }
    }
}