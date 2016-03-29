using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.Redis;
using System.Threading;

namespace MvcApplication.BLL
{
    public class RedisTest
    {
        public string SimpleDemo()
        {
            string host = "localhost";
            string elementKey = "mylists";

            using (RedisClient redisClient = new RedisClient(host))
            {
                if (redisClient.Get<string>(elementKey) == null)
                {
                    //// adding delay to see the difference
                    //Thread.Sleep(2000);
                    //// save value in cache
                    //redisClient.Set(elementKey, "default value");
                    return "null";
                }
                ////change the value
                //List<string> lt = new List<string>() { "aa", "bb" };
                //redisClient.Set<List<string>>("lists", lt);

                else
                {
                    return redisClient.Get<string>(elementKey);
                }
            }
        }
    }
}