using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TestConsoleApplication
{
    class TestConsoleProgress
    {
        public void Run()
        {
            bool isBreak = false;
            ConsoleColor colorBack = Console.BackgroundColor;
            ConsoleColor colorFore = Console.ForegroundColor;

            // 第一行信息 
            Console.WriteLine(" *********** jinjazz now working...****** ");

            // 第二行绘制进度条背景 
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            for (int i = 0; ++i <= 25;)
            {
                Console.Write("   ");
            }
            Console.WriteLine("   ");
            Console.BackgroundColor = colorBack;

            // 第三行输出进度 
            Console.WriteLine(" 0% ");
            // 第四行输出提示,按下回车可以取消当前进度 
            Console.WriteLine(" <Press Enter To Break.> ");

            // -----------------------上面绘制了一个完整的工作区域,下面开始工作

            // 开始控制进度条和进度变化 
            for (int i = 0; ++i <= 100;)
            {
                // 先检查是否有按键请求,如果有,判断是否为回车键,如果是则退出循环 
                if (Console.KeyAvailable && System.Console.ReadKey(true).Key == ConsoleKey.Enter)
                {
                    isBreak = true;
                    break;
                }
                // 绘制进度条进度 
                Console.BackgroundColor = ConsoleColor.Yellow; // 设置进度条颜色 
                Console.SetCursorPosition(i / 4, 1); // 设置光标位置,参数为第几列和第几行 
                Console.Write("   "); // 移动进度条 
                Console.BackgroundColor = colorBack; // 恢复输出颜色
                                                     // 更新进度百分比,原理同上. 
                Console.ForegroundColor = ConsoleColor.Green;
                Console.SetCursorPosition(0, 2);
                Console.Write(" {0}% ", i);
                Console.ForegroundColor = colorFore;
                // 模拟实际工作中的延迟,否则进度太快 
                System.Threading.Thread.Sleep(100);
            }
            // 工作完成,根据实际情况输出信息,而且清楚提示退出的信息 
            Console.SetCursorPosition(0, 3);
            Console.Write(isBreak ? " break!!! " : " finished. ");
            Console.WriteLine("                        ");
            // 等待退出 
            Console.ReadKey(true);
        }
    }

    class ConsoleProgressBar
    {
        int left = 0;
        int backgroundLength = 50;

        #region [ windows api ]
        ConsoleColor colorBack = Console.BackgroundColor;
        ConsoleColor colorFore = Console.ForegroundColor;

        private const int STD_OUTPUT_HANDLE = -11;
        private int mHConsoleHandle;
        COORD barCoord;

        [StructLayout(LayoutKind.Sequential)]
        public struct COORD
        {
            public short X;
            public short Y;
            public COORD(short x, short y)
            {
                X = x;
                Y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SMALL_RECT
        {
            public short Left;
            public short Top;
            public short Right;
            public short Bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CONSOLE_SCREEN_BUFFER_INFO
        {
            public COORD dwSize;
            public COORD dwCursorPosition;
            public int wAttributes;
            public SMALL_RECT srWindow;
            public COORD dwMaximumWindowSize;
        }

        public void SetCursorPos(short x, short y)
        {
            NativeMethods.SetConsoleCursorPosition(mHConsoleHandle, new COORD(x, y));
        }

        public COORD GetCursorPos()
        {
            CONSOLE_SCREEN_BUFFER_INFO res;
            NativeMethods.GetConsoleScreenBufferInfo(mHConsoleHandle, out res);
            return res.dwCursorPosition;
        }
        #endregion

        public ConsoleProgressBar(string title, int left = 10)
        {
            Console.WriteLine();
            //获取当前窗体句柄
            mHConsoleHandle = NativeMethods.GetStdHandle(STD_OUTPUT_HANDLE);
            //获取当前窗体偏移量
            barCoord = this.GetCursorPos();

            this.left = left;
            //获取字符长度
            int len = GetStringLength(title);
            //设置标题的相对居中位置
            Console.SetCursorPosition(left + (backgroundLength / 2 - len), barCoord.Y);
            Console.Write(title);

            //写入进度条背景
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.SetCursorPosition(left, barCoord.Y + 1);

            for (int i = 0; ++i <= backgroundLength;)
                Console.Write(" ");

            Console.WriteLine();
            Console.BackgroundColor = colorBack;
        }

        /// <summary>
        /// 更新进度条
        /// </summary>
        /// <param name="current">当前进度</param>
        /// <param name="total">总进度</param>
        /// <param name="message">说明文字</param>
        public void Update(int current, int total, string message)
        {
            //计算百分比
            int i = (int)Math.Ceiling(current / (double)total * 100);

            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(left, barCoord.Y + 1);

            //写入进度条
            StringBuilder bar = new StringBuilder();
            //当前百分比*进度条总长度=要输出的进度最小单位数量
            int count = (int)Math.Ceiling((double)i / 100 * backgroundLength);
            for (int n = 0; n < count; n++) bar.Append(" ");
            Console.Write(bar);
            //设置和写入百分比
            Console.BackgroundColor = colorBack;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(left + backgroundLength, barCoord.Y + 1);
            Console.Write(" {0}% ", i);
            Console.ForegroundColor = colorFore;
            //获取字符长度
            int len = GetStringLength(message);
            //获取相对居中的message偏移量
            Console.SetCursorPosition(left + (backgroundLength / 2 - len), barCoord.Y + 2);
            Console.Write(message);
            //进度完成另起新行作为输出
            if (i >= 100) Console.WriteLine();
        }

        /// <summary>
        /// 获取字符长度
        /// </summary>
        /// <remarks>中文和全角占长度1，英文和半角字符2个字母占长度1</remarks>
        /// <param name="message"></param>
        /// <returns></returns>
        private int GetStringLength(string message)
        {
            int len = Encoding.ASCII.GetBytes(message).Count(b => b == 63);
            return (message.Length - len) / 2 + len;
        }
    }
}
