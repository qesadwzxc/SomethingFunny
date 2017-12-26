using System.Collections.Concurrent;
using System.Threading.Tasks;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace TestConsoleApplication.Threads
{
    class TCountdownEvent
    {
        static CountdownEvent cde = new CountdownEvent(Environment.ProcessorCount);

        public void Run()
        {
            int countA = 3;
            cde.Reset(countA);
            while (countA-- < 0)
            {
                Task.Factory.StartNew((obj) =>
                {
                    WorkA(obj);
                }, countA);
            }
            Console.WriteLine("工作A加载完毕");
            int countB = 5;
            cde.Reset(countB);
            while (countB-- < 0)
            {
                Task.Factory.StartNew((obj) =>
                {
                    WorkB(obj);
                }, countB);
            }
            Console.WriteLine("工作B加载完毕");
            int countC = 8;
            cde.Reset(countC);
            while (countC-- < 0)
            {
                Task.Factory.StartNew((obj) =>
                {
                    WorkC(obj);
                }, countC);
            }
            Console.WriteLine("工作C加载完毕");
            Console.WriteLine("全部工作加载完毕");
        }

        public void WorkA(object obj)
        {
            Console.WriteLine($"{DateTime.Now.ToString()}当前正在执行工作A，任务：{obj}");
            cde.Signal();
        }

        public void WorkB(object obj)
        {
            Console.WriteLine($"{DateTime.Now.ToString()}当前正在执行工作B，任务：{obj}");
            cde.Signal();
        }

        public void WorkC(object obj)
        {
            Console.WriteLine($"{DateTime.Now.ToString()}当前正在执行工作C，任务：{obj}");
            cde.Signal();
        }
    }
}
