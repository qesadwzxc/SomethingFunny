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

        //TODO:需要看看非托管代码
        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        internal static extern int MessageBox(int hParent, string msg, string caption, int type);

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool QueryThreadCycleTime(IntPtr threadHandle, ref ulong cycleTime);

        [DllImport("kernel32.dll")]
        internal static extern IntPtr GetCurrentThread();

        [DllImport("Kernel32.dll")]
        internal static extern bool Beep(int frequency, int duration);

        [DllImport("winmm.dll", CharSet = CharSet.Unicode)]
        internal static extern bool PlaySound(String Filename, int Mod, int Flags);

        [DllImport("user32.dll")]
        internal static extern int MessageBeep(uint uType);
    }
}
