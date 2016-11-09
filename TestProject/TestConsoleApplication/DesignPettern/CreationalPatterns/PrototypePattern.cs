using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

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