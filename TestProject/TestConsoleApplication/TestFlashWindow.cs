using System;
using System.Timers;

namespace TestConsoleApplication
{
    class TestFlashWindow
    {
        public static void Run()
        {
            TestFlashWindow t = new TestFlashWindow();
            Console.Title = "WAHAHA";
            IntPtr intptr = NativeMethods.FindWindow("ConsoleWindowClass", "WAHAHA");
            if (intptr != IntPtr.Zero)
            {
                t.Flash(10, 300, intptr);
            }
        }

        Timer _timer;
        int _count = 0;
        int _maxTimes = 0;
        IntPtr _window;

        public void Flash(int times, double millliseconds, IntPtr window)
        {
            _maxTimes = times;
            _window = window;

            _timer = new Timer();
            _timer.Interval = millliseconds;
            _timer.Elapsed += _timer_Elapsed;
            _timer.Start();
        }

        void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (++_count < _maxTimes)
            {
                NativeMethods.FlashWindow(_window, (_count % 2) == 0);
            }
            else
            {
                _timer.Stop();
            }
        }
    }
}
