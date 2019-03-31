using System.Collections.Generic;

namespace MvcApplication.Models
{
    /// <summary>
    /// Key值集合类
    /// </summary>
    public class CompositeKey
    {
        private Dictionary<string, object> keyDic = new Dictionary<string, object>();
        public Dictionary<string, object> KeyDic
        {
            get { return this.keyDic; }
        }

        public object this[string name]
        {
            get
            {
                if (keyDic.ContainsKey(name))
                {
                    return keyDic[name];
                }
                return null;
            }
            set
            {
                if (!keyDic.ContainsKey(name))
                {
                    keyDic.Add(name, null);
                }
                keyDic[name] = value;
            }
        }
    }
}