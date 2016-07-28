////////////////////////////////////////////////////////////////////////////////
///自定义字典类型
////////////////////////////////////////////////////////////////////////////////
using System.Collections.Generic;

namespace TestConsoleApplication
{
    public class CustomDictionary
    {
        private Dictionary<string, object> dic = new Dictionary<string, object>();

        public Dictionary<string,object> Dic
        {
            get { return this.dic; }
        }

        public object this[string name]
        {
            get 
            {
                if (dic.ContainsKey(name))
                {
                    return dic[name];
                }
                return null;
            }
            set
            {
                if (!dic.ContainsKey(name))
                {
                    dic.Add(name, null);
                }
                dic[name] = value;
            }
        }
    }
}
