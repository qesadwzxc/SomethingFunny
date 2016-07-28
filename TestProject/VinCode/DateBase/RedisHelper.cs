﻿using Newtonsoft.Json;
using ServiceStack.Redis;
using System.Text;

namespace VinCode.DateBase
{ 
    public class RedisHelper
    {
        static readonly string host = "10.01.28.90";
        static readonly int port = 6379;
        static readonly string password = "";

        private static RedisClient instance;
        public static RedisClient Client
        {
            get
            {
                if (instance == null)
                {
                    instance = new RedisClient(host, port, password);
                }
                return instance;
            }
        }

        ~RedisHelper()
        {
            instance.Dispose();
        }

        public string Get(string key)
        {
            string returnStr = string.Empty;
            returnStr = Encoding.UTF8.GetString(instance.Get(key));
            return returnStr;
        }

        public bool Set(string key, string value)
        {
            byte[] bValue = Encoding.UTF8.GetBytes(value);
            return instance.Set(key, bValue);
        }

        public bool Set<T>(string key, T model)
        {
            string strValue = JsonConvert.SerializeObject(model);
            byte[] bValue = Encoding.UTF8.GetBytes(strValue);
            return instance.Set(key, bValue);
        }

        public T Get<T>(string key) where T : new()
        {
            string strValue = string.Empty;
            strValue = Encoding.UTF8.GetString(instance.Get(key));
            T model = new T();
            model = JsonConvert.DeserializeObject<T>(strValue);
            return model;
        }

        public bool Exists(string key)
        {
            return instance.Exists(key) > 0;
        }

        public bool Delete(string key)
        {
            return instance.Del(key) > 0;
        }

        public void FlushAll()
        {
            instance.FlushAll();
        }

        public string GetValueFromHash(string hashID, string key)
        {
            return instance.GetValueFromHash(hashID, key);
        }

        public bool SetEntryInHash(string hashID, string key, string value)
        {
            return instance.SetEntryInHash(hashID, key, value);
        }

        public long IncrementValueInHash(string hashID, string key, int value)
        {
            return instance.IncrementValueInHash(hashID, key, value);
        }
    }
}
