using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TestConsoleApplication
{
    internal class NativeMethods
    {
        private NativeMethods() { }

        /// <summary>
        /// 系统弹框
        /// </summary>
        /// <param name="hParent">0|1</param>
        /// <param name="msg">消息</param>
        /// <param name="caption">标题</param>
        /// <param name="type">类型</param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern int MessageBox(int hParent, string msg, string caption, int type);

        #region CodeTime用
        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool QueryThreadCycleTime(IntPtr threadHandle, ref ulong cycleTime);

        [DllImport("kernel32.dll")]
        internal static extern IntPtr GetCurrentThread();
        #endregion

        #region 系统提示音
        [DllImport("kernel32.dll")]
        internal static extern bool Beep(int frequency, int duration);

        [DllImport("winmm.dll", CharSet = CharSet.Unicode)]
        internal static extern bool PlaySound(String Filename, int Mod, int Flags);

        [DllImport("user32.dll")]
        internal static extern int MessageBeep(uint uType);
        #endregion

        [DllImport("kernel32.dll")]
        public static extern IntPtr _lopen(string lpPathName, int iReadWrite);

        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr hObject);

        /// <summary>  
        /// 窗口闪动  
        /// </summary>  
        /// <param name="hwnd">窗口句柄</param>  
        /// <param name="bInvert">是否为闪</param>  
        /// <returns>成功返回0</returns>  
        [DllImport("user32.dll")]
        public static extern bool FlashWindow(IntPtr hwnd, bool bInvert);

        /// <summary>
        /// 显示/隐藏窗口
        /// </summary>
        /// <param name="hWnd">窗口指针</param>
        /// <param name="nCmdShow">0-隐藏；1-显示</param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "ShowWindow", SetLastError = true)]
        public static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);

        /// <summary>
        /// 查找窗口
        /// </summary>
        /// <param name="lpClassName">窗口类型名</param>
        /// <param name="lpWindowName">窗口名Title</param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "GetSystemMenu")]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, IntPtr bRevert);

        [DllImport("user32.dll", EntryPoint = "RemoveMenu")]
        public static extern IntPtr RemoveMenu(IntPtr hMenu, uint uPosition, uint uFlags);

        [DllImport("kernel32.dll")]
        public static extern bool SetConsoleCtrlHandler(ConsoleCtrlDelegate HandlerRoutine, bool Add);

        #region 进度条用
        [DllImport("kernel32.dll", EntryPoint = "GetStdHandle", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", EntryPoint = "GetConsoleScreenBufferInfo", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int GetConsoleScreenBufferInfo(int hConsoleOutput, out ConsoleProgressBar.CONSOLE_SCREEN_BUFFER_INFO lpConsoleScreenBufferInfo);

        [DllImport("kernel32.dll", EntryPoint = "SetConsoleCursorPosition", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int SetConsoleCursorPosition(int hConsoleOutput, ConsoleProgressBar.COORD dwCursorPosition);
        #endregion
    }
}
