using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace TestConsoleApplication
{
    internal class CodeTimer
    {
        private CodeTimer() { }

        public static void Initialize()
        {
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
            Time("", 1, () => { });
        }

        /// <summary>
        /// 时间统计（无参方法，统计CPU圈数、GC、运行时间）
        /// </summary>
        /// <param name="name">方法名</param>
        /// <param name="iteration">运行次数</param>
        /// <param name="action">方法</param>
        public static void Time(string name, int iteration, Action action)
        {
            if (String.IsNullOrEmpty(name)) return;

            // 1.
            ConsoleColor currentForeColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(name);

            // 2.
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            int[] gcCounts = new int[GC.MaxGeneration + 1];
            for (int i = 0; i <= GC.MaxGeneration; i++)
            {
                gcCounts[i] = GC.CollectionCount(i);
            }

            // 3.
            Stopwatch watch = new Stopwatch();
            watch.Start();
            ulong cycleCount = GetCycleCount();
            for (int i = 0; i < iteration; i++) action();
            ulong cpuCycles = GetCycleCount() - cycleCount;
            watch.Stop();

            // 4.
            Console.ForegroundColor = currentForeColor;
            Console.WriteLine("\tTime Elapsed:\t" + watch.ElapsedMilliseconds.ToString("N0") + "ms");
            Console.WriteLine("\tCPU Cycles:\t" + cpuCycles.ToString("N0"));

            // 5.
            for (int i = 0; i <= GC.MaxGeneration; i++)
            {
                int count = GC.CollectionCount(i) - gcCounts[i];
                Console.WriteLine("\tGen " + i + ": \t\t" + count);
            }

            Console.WriteLine();
        }

        /// <summary>
        /// 时间统计
        /// </summary>
        /// <typeparam name="TArg">参数类型</typeparam>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="func">方法</param>
        /// <param name="arg">参数</param>
        /// <param name="loop">运行次数</param>
        /// <returns></returns>
        public static TResult MeasurePerformance<TArg, TResult>(Func<TArg, TResult> func, TArg arg, int loop)
        {
            GC.Collect();
            int gc0 = GC.CollectionCount(0);
            int gc1 = GC.CollectionCount(1);
            int gc2 = GC.CollectionCount(2);
            TResult result = default(TResult);
            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < loop; i++)
            {
                result = func(arg);
            }
            Console.WriteLine(sw.ElapsedMilliseconds.ToString() + "ms");
            Console.WriteLine("GC 0:" + (GC.CollectionCount(0) - gc0).ToString());
            Console.WriteLine("GC 1:" + (GC.CollectionCount(1) - gc1).ToString());
            Console.WriteLine("GC 2:" + (GC.CollectionCount(2) - gc2).ToString());
            return result;
        }

        private static ulong GetCycleCount()
        {
            ulong cycleCount = 0;
            NativeMethods.QueryThreadCycleTime(NativeMethods.GetCurrentThread(), ref cycleCount);
            return cycleCount;
        }
    }
}
