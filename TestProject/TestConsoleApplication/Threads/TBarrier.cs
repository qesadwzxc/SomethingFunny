using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestConsoleApplication.Threads
{
    class TBarrier
    {
        static Task[] tasks = new Task[4];
        static Barrier barrier = null;
        static CancellationTokenSource cts = new CancellationTokenSource();
        static CancellationToken ct = cts.Token;

        public void Run()
        {
            #region 屏障
            barrier = new Barrier(tasks.Length, (i) =>
            {
                Console.WriteLine(i.CurrentPhaseNumber);
            });

            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = Task.Factory.StartNew((obj) =>
                {
                    var single = Convert.ToInt32(obj);
                    Console.WriteLine("当前任务:{0}正在加载第一部分数据！", single);
                    Thread.Sleep(2000);
                    if (!barrier.SignalAndWait(1000))//超时模拟
                    {
                        //抛出异常，取消后面加载的执行
                        throw new OperationCanceledException(string.Format("我是当前任务{0},我抛出异常了！", single), ct);
                    }
                    Console.WriteLine("当前任务:{0}正在加载第二部分数据！", single);
                    barrier.SignalAndWait();

                }, i, ct);
            }

            try
            {
                //等待所有tasks 10s
                Task.WaitAll(tasks, 10000);
                for (int i = 0; i < tasks.Length; i++)
                {
                    if (tasks[i].Status == TaskStatus.Faulted)
                    {
                        //获取task中的异常
                        foreach (var single in tasks[i].Exception.InnerExceptions)
                        {
                            Console.WriteLine(single.Message);
                        }
                    }
                }

                barrier.Dispose();
            }
            catch (AggregateException e)
            {
                Console.WriteLine("我是总异常:{0}", e.Message);
            }
            #endregion
        }
    }
}
