////////////////////////////////////////////////////////////////////////////////
///锁练习
////////////////////////////////////////////////////////////////////////////////
using System;
using System.Threading;

namespace TestConsoleApplication
{
    class PracticeLock
    {
        const string firstOrderId = "001";
        const string secondOrderId = "002";
        const string thirdOrderId = "003";

        public static void Run()
        {
            test(LockType.LockThis);
            //test(LockType.LockString);
            //test(LockType.LockObject);
            //test(LockType.LockStaticObject);

            Console.ReadLine();
        }

        static void test(LockType lockType)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("------------测试相同订单------------");
            Console.ForegroundColor = ConsoleColor.White;
            OrderPay(firstOrderId, 1, lockType);
            OrderPay(firstOrderId, 2, lockType);
            OrderPay(firstOrderId, 3, lockType);
            Thread.Sleep(10000);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("------------测试不同订单------------");
            Console.ForegroundColor = ConsoleColor.White;
            OrderPay(firstOrderId, 1, lockType);
            OrderPay(secondOrderId, 1, lockType);
            OrderPay(thirdOrderId, 1, lockType);
        }

        static void OrderPay(string orderId, int threadNo, LockType lockType)
        {
            new Thread(() => new Payment(orderId, threadNo).Pay(lockType)).Start();

            Thread.Sleep(10);
        }

        public class Payment
        {
            private readonly string LockString;
            public readonly int ThreadNo;
            private readonly Object LockObj = new object();
            private static readonly Object StaticLockObj = new object();

            public Payment(string orderID, int threadNo)
            {
                LockString = orderID;
                ThreadNo = threadNo;
            }

            public void Pay(LockType lockType)
            {
                ShowMessage("等待锁资源");
                switch (lockType)
                {
                    case LockType.LockThis:
                        lock (this)
                        {
                            showAction();
                        }
                        break;
                    case LockType.LockString:
                        lock (LockString)
                        {
                            showAction();
                        }
                        break;
                    case LockType.LockObject:
                        lock (LockObj)
                        {
                            showAction();
                        }
                        break;
                    case LockType.LockStaticObject:
                        lock (StaticLockObj)
                        {
                            showAction();
                        }
                        break;
                }
                ShowMessage("释放锁");
            }

            private void showAction()
            {
                ShowMessage("进入锁并开始操作");
                Thread.Sleep(2000);
                ShowMessage("操作完成,完成时间为" + DateTime.Now);
            }

            private void ShowMessage(string message)
            {
                Console.WriteLine(String.Format("订单{0}的第{1}个线程 {2}", LockString, ThreadNo, message));
            }

        }

        public enum LockType
        {
            LockThis = 0,
            LockString = 1,
            LockObject = 2,
            LockStaticObject = 3
        }
    }
}
