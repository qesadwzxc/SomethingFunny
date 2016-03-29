using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TestConsoleApplication
{
    public class RedisMutiThread
    {
        public void Run()
        {
            RedisHelper[] redisCli = new RedisHelper[8]
            {
                new RedisHelper(),
                new RedisHelper(),
                new RedisHelper(),
                new RedisHelper(),
                new RedisHelper(),
                new RedisHelper(),
                new RedisHelper(),
                new RedisHelper()
            };
            ThreadPool.QueueUserWorkItem(o =>
            {
                testTicketCount(redisCli[0], "A");
            });
            ThreadPool.QueueUserWorkItem(o =>
            {
                testTicketCount(redisCli[1], "A");
            });
            ThreadPool.QueueUserWorkItem(o =>
            {
                testTicketCount(redisCli[2], "B");
            });
            ThreadPool.QueueUserWorkItem(o =>
            {
                testTicketCount(redisCli[3], "B");
            });
            ThreadPool.QueueUserWorkItem(o =>
            {
                testTicketCount(redisCli[4], "C");
            });
            ThreadPool.QueueUserWorkItem(o =>
            {
                testTicketCount(redisCli[5], "C");
            });
            ThreadPool.QueueUserWorkItem(o =>
            {
                testTicketCount(redisCli[6], "D");
            });
            ThreadPool.QueueUserWorkItem(o =>
            {
                testTicketCount(redisCli[7], "D");
            });
            Console.Read();
        }

        /// <summary>
        /// 对某个选项进行投票，投票数加1
        /// </summary>
        /// <param name="rediscli">客户端</param>
        /// <param name="field">数据</param>
        internal static void testTicketCount(RedisHelper rediscli, string field)
        {
            int k = 0;
            for (int i = 0; i <= 99; i++)
            {
                //这样子虽然可以实现功能，但是简单测试便知，因为并发，数据修改是有问题的
                //k = int.Parse(rediscli.GetValueFromHash("TicketCount", field)) + 1;
                //rediscli.SetEntryInHash("TicketCount", field, k.ToString());

                //下面这种方式为reids对原子性的实现
                rediscli.IncrementValueInHash("TicketCount", field, 1);
            }
        }
    }
}
