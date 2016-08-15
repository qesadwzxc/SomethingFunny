using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace TestConsoleApplication
{
    public delegate bool ConsoleCtrlDelegate(int ctrlType);

    class PracticeTaskBarIcon
    {
        static bool _IsExit = false;

        public static void Run()
        {
            Console.Title = "TestConsoleLikeWin32";
            ConsoleWin32Helper.ShowNotifyIcon();
            ConsoleWin32Helper.DisableCloseButton(Console.Title);

            Thread threadMonitorInput = new Thread(new ThreadStart(MonitorInput));
            threadMonitorInput.Start();

            while (true)
            {
                Application.DoEvents();
                if (_IsExit)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 提供关闭字符输入监控
        /// </summary>
        static void MonitorInput()
        {
            while (true)
            {
                string input = Console.ReadLine();
                if (input == "exit")
                {
                    ConsoleWin32Helper.HideNotifyIcon();
                    _IsExit = true;
                    Thread.CurrentThread.Abort();
                }
            }
        }
    }

    class ConsoleWin32Helper
    {
        private static bool WindowVisable;
        //检测到 ConsoleCtrlDelegate
        //Message: 对“TestConsoleApplication.ConsoleCtrlDelegate::Invoke”类型的已垃圾回收委托进行了回调。这可能会导致应用程序崩溃、损坏和数据丢失。向非托管代码传递委托时，托管应用程序必须让这些委托保持活动状态，直到确信不会再次调用它们。
        //问题原因: 委托作为局部变量被生成赋值则会被GC回收，在传递给非托管方法时就会报错
        //解决办法: 如下，在类中声明一次委托（静态非静态均可），然后再赋值调用即可。
        static ConsoleCtrlDelegate consoleDelegete;
        static ConsoleWin32Helper()
        {
            WindowVisable = true;
            NegativeCtrlAndC();
            _NotifyIcon.Icon = new Icon(@"D:\274728e822b89c9b62b894ae226b8_128X128.ico");
            _NotifyIcon.Visible = false;
            _NotifyIcon.Text = "tray";

            ContextMenu menu = new ContextMenu();
            MenuItem item = new MenuItem();
            item.Text = "右键菜单，还没有添加事件";
            item.Index = 0;

            menu.MenuItems.Add(item);
            _NotifyIcon.ContextMenu = menu;
            _NotifyIcon.MouseClick += new MouseEventHandler(_NotifyIcon_MouseClick);
            //UNDONE:双击事件同时会触发单击事件…BUG
            //_NotifyIcon.MouseDoubleClick += new MouseEventHandler(_NotifyIcon_MouseDoubleClick);

        }

        static void _NotifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            IntPtr intptr = NativeMethods.FindWindow("ConsoleWindowClass", "TestConsoleLikeWin32");
            if (intptr != IntPtr.Zero)
            {
                if (WindowVisable)
                {
                    NativeMethods.ShowWindow(intptr, 0);
                    WindowVisable = false;
                }
                else
                {
                    NativeMethods.ShowWindow(intptr, 1);
                    WindowVisable = true;
                }
            }
        }

        static void _NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Console.WriteLine("托盘被双击.");
        }

        #region 禁用关闭按钮
        ///<summary>
        /// 禁用关闭按钮
        ///</summary>
        ///<param name="consoleName">控制台名字</param>
        public static void DisableCloseButton(string title)
        {
            //线程睡眠，确保closebtn中能够正常FindWindow，否则有时会Find失败。。
            Thread.Sleep(100);

            IntPtr windowHandle = NativeMethods.FindWindow("ConsoleWindowClass", title);
            IntPtr closeMenu = NativeMethods.GetSystemMenu(windowHandle, IntPtr.Zero);
            uint SC_CLOSE = 0xF060;
            NativeMethods.RemoveMenu(closeMenu, SC_CLOSE, 0x0);
        }
        public static bool IsExistsConsole(string title)
        {
            IntPtr windowHandle = NativeMethods.FindWindow(null, title);
            if (windowHandle.Equals(IntPtr.Zero))
                return false;
            return true;
        }
        #endregion

        #region 托盘图标
        static NotifyIcon _NotifyIcon = new NotifyIcon();
        public static void ShowNotifyIcon()
        {
            _NotifyIcon.Visible = true;
            _NotifyIcon.ShowBalloonTip(3000, "", "我是托盘图标，点击我试试，还可以双击看看。", ToolTipIcon.None);
        }
        public static void HideNotifyIcon()
        {
            _NotifyIcon.Visible = false;
        }
        #endregion

        #region 禁用Ctrl+C
        public static void NegativeCtrlAndC()
        {
            consoleDelegete = new ConsoleCtrlDelegate(HandlerRoutine);
            bool bRet = NativeMethods.SetConsoleCtrlHandler(consoleDelegete, true);
            if (bRet == false) //安装事件处理失败
            {
                Console.WriteLine("NegativeCtrlAndC Error");
            }

            else
            {
                Console.WriteLine("NegativeCtrlAndC Ok");
            }
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        private static bool HandlerRoutine(int ctrlType)
        {
            //Ctrl+C，系统会发送次消息
            const int CTRL_C_EVENT = 0;
            //Ctrl+break，系统会发送次消息
            const int CTRL_BREAK_EVENT = 1;
            const int CTRL_CLOSE_EVENT = 2;
            const int CTRL_LOGOFF_EVENT = 5;
            //系统关闭，系统会发送次消息
            const int CTRL_SHUTDOWN_EVENT = 6;

            switch (ctrlType)
            {
                case CTRL_C_EVENT:
                    Console.WriteLine("C");
                    return true; //这里返回true，表示阻止响应系统对该程序的操作
                //break;
                case CTRL_BREAK_EVENT:
                    Console.WriteLine("BREAK");
                    break;
                case CTRL_CLOSE_EVENT:
                    Console.WriteLine("CLOSE");
                    break;
                case CTRL_LOGOFF_EVENT:
                    Console.WriteLine("LOGOFF");
                    break;
                case CTRL_SHUTDOWN_EVENT:
                    Console.WriteLine("SHUTDOWN");
                    break;
            }
            return false; //忽略处理，让系统进行默认操作
        }
        #endregion
    }
}
