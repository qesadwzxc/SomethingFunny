using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using System.IO;

namespace TestConsoleApplication.Reconder
{
    class SmSound
    {
       // class SmSound
       // {
            //private byte[] m_soundBytes;
            private string m_fileName;

            private enum Flags
            {
                SND_SYNC = 0x0000,  /* 同步播放声音，在播放完后PlaySound函数才返回 */
                SND_ASYNC = 0x0001,  /* 用异步方式播放声音，PlaySound函数在开始播放后立即返回 */
                SND_NODEFAULT = 0x0002,  /* 不播放缺省声音，若无此标志，则PlaySound在没找到声音时会播放缺省声音 */
                SND_MEMORY = 0x0004,  /* 播放载入到内存中的声音，此时pszSound是指向声音数据的指针*/
                SND_LOOP = 0x0008,  /* 重复播放声音，必须与SND_ASYNC标志一块使用 */
                SND_NOSTOP = 0x0010,  /* PlaySound不打断原来的声音播出并立即返回FALSE */
                SND_NOWAIT = 0x00002000, /* 如果驱动程序正忙则函数就不播放声音并立即返回 */
                SND_ALIAS = 0x00010000, /* 指定了注册表或WIN.INI中的系统事件的别名 */
                SND_ALIAS_ID = 0x00110000, /* 指定了预定义的声音标识符 */
                SND_FILENAME = 0x00020000, /* 指定了WAVE文件名 */
                SND_RESOURCE = 0x00040004  /* WAVE资源的标识符，这时要用到hmod参数 */
            }

            [DllImport("winmm.DLL", EntryPoint = "PlaySound", SetLastError = true)]
            private extern static int WCE_PlaySound(string szSound, IntPtr hMod, int flags);

            [DllImport("winmm.DLL", EntryPoint = "PlaySound", SetLastError = true)]
            private extern static int WCE_PlaySoundBytes(byte[] szSound, IntPtr hMod, int flags);

            /// <summary>
            /// Construct the Sound object to play sound data from the specified file.
            /// </summary>
            //public void Sound(string fileName)
            //{
            //   m_fileName = fileName;
            //}

            /// <summary>
            /// Construct the Sound object to play sound data from the specified stream.
            /// </summary>
            //public SmSound(Stream stream)

            public SmSound()
            {
                // read the data from the stream
                //m_soundBytes = new byte[stream.Length];
                //stream.Read(m_soundBytes, 0, (int)stream.Length);
            }

            /// <summary>
            /// Play the sound
            /// </summary>
            public void Play(byte[] m_soundBytes)
            {
                // if a file name has been registered, call WCE_PlaySound, 
                //  otherwise call WCE_PlaySoundBytes
                //if (m_fileName != null)
                //WCE_PlaySound(m_fileName, IntPtr.Zero, (int)(Flags.SND_ASYNC | Flags.SND_FILENAME));
                //else
                WCE_PlaySoundBytes(m_soundBytes, IntPtr.Zero, (int)(Flags.SND_SYNC | Flags.SND_MEMORY));
            }
        //
        public void Play(byte[] m_soundBytes,string path)
        {
            // if a file name has been registered, call WCE_PlaySound, 
            //  otherwise call WCE_PlaySoundBytes
            //if (m_fileName != null)
            //WCE_PlaySound(m_fileName, IntPtr.Zero, (int)(Flags.SND_ASYNC | Flags.SND_FILENAME));
            //else
            WCE_PlaySoundBytes(m_soundBytes, IntPtr.Zero, (int)(Flags.SND_SYNC | Flags.SND_MEMORY));
            playwav.Play(path);
        }
        //
        public void PlayA(byte[] m_soundBytes)
        {
            // if a file name has been registered, call WCE_PlaySound, 
            //  otherwise call WCE_PlaySoundBytes
            //if (m_fileName != null)
            //WCE_PlaySound(m_fileName, IntPtr.Zero, (int)(Flags.SND_ASYNC | Flags.SND_FILENAME));
            //else
            WCE_PlaySoundBytes(m_soundBytes, IntPtr.Zero, (int)(Flags.SND_ASYNC | Flags.SND_MEMORY));
        }

            public void replay()
            {
                WCE_PlaySound(m_fileName, IntPtr.Zero, (int)(Flags.SND_ASYNC | Flags.SND_LOOP));

                //(int)(Flags.SND_ASYNC | Flags.SND_LOOP)
            }
        }
   
}
