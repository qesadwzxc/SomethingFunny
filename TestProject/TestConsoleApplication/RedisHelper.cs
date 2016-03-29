using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.Redis;
using Newtonsoft.Json;

namespace TestConsoleApplication
{
    public class RedisHelper
    {
        static readonly string host = "10.1.25.57";
        static readonly int port = 6379;
        static readonly string password = "";
        RedisClient client;
        public RedisHelper()
        {
            client = new RedisClient(host, port, password);
        }

        ~RedisHelper()
        {
            client.Dispose();
        }

        public string Get(string key)
        {
            string returnStr = string.Empty;
            returnStr = Encoding.UTF8.GetString(client.Get(key));
            return returnStr;
        }

        public bool Set(string key, string value)
        {
            byte[] bValue = Encoding.UTF8.GetBytes(value);
            return client.Set(key, bValue);
        }

        public bool Set<T>(string key, T model)
        {
            string strValue = JsonConvert.SerializeObject(model);
            byte[] bValue = Encoding.UTF8.GetBytes(strValue);
            return client.Set(key, bValue);
        }

        public T Get<T>(string key) where T : new()
        {
            string strValue = string.Empty;
            strValue = Encoding.UTF8.GetString(client.Get(key));
            T model = new T();
            model = JsonConvert.DeserializeObject<T>(strValue);
            return model;
        }

        public bool Exists(string key)
        {
            return client.Exists(key) > 0;
        }

        public bool Delete(string key)
        {
            return client.Del(key) > 0;
        }

        public void FlushAll()
        {
            client.FlushAll();
        }

        public string GetValueFromHash(string hashID, string key)
        {
            return client.GetValueFromHash(hashID, key);
        }

        public bool SetEntryInHash(string hashID, string key, string value)
        {
            return client.SetEntryInHash(hashID, key, value);
        }

        public long IncrementValueInHash(string hashID, string key, int value)
        {
            return client.IncrementValueInHash(hashID, key, value);
        }
    }
}
