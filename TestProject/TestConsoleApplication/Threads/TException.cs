using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestConsoleApplication.Threads
{
    public class TException
    {
        static CancellationTokenSource cts = new CancellationTokenSource();
        static CancellationToken ct = cts.Token;

        public void Run()
        {
            #region 异常处理
            //通过后续任务处理前驱任务异常
            Task task = Task.Run(() => throw new Exception("前驱任务异常了"));
            task.ContinueWith(antecedentTask =>
            {
                antecedentTask.Exception.Handle(eachE =>
                {
                    Console.WriteLine($"Error: {eachE.Message}");
                    return true;
                });
            }, TaskContinuationOptions.OnlyOnFaulted);

            //通过canceltoken获取取消任务
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Token.Register(() =>
            {
                Console.WriteLine("任务取消了");
            });
            cancellationTokenSource.CancelAfter(2000);
            Task task2 = Task.Run(() =>
            {
                while (true && !cancellationTokenSource.IsCancellationRequested)
                {
                    Console.WriteLine("任务执行中...");
                    Thread.Sleep(300);
                }
            },
            cancellationTokenSource.Token);
            task2.Wait();
            Console.WriteLine($"任务的最终状态是:{task2.Status}");
            #endregion
        }
    }
}
