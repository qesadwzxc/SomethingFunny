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
                SND_SYNC = 0x0000,  /* ͬ�������������ڲ������PlaySound�����ŷ��� */
                SND_ASYNC = 0x0001,  /* ���첽��ʽ����������PlaySound�����ڿ�ʼ���ź��������� */
                SND_NODEFAULT = 0x0002,  /* ������ȱʡ���������޴˱�־����PlaySound��û�ҵ�����ʱ�Ქ��ȱʡ���� */
                SND_MEMORY = 0x0004,  /* �������뵽�ڴ��е���������ʱpszSound��ָ���������ݵ�ָ��*/
                SND_LOOP = 0x0008,  /* �ظ�����������������SND_ASYNC��־һ��ʹ�� */
                SND_NOSTOP = 0x0010,  /* PlaySound�����ԭ����������������������FALSE */
                SND_NOWAIT = 0x00002000, /* �������������æ�����Ͳ������������������� */
                SND_ALIAS = 0x00010000, /* ָ����ע����WIN.INI�е�ϵͳ�¼��ı��� */
                SND_ALIAS_ID = 0x00110000, /* ָ����Ԥ�����������ʶ�� */
                SND_FILENAME = 0x00020000, /* ָ����WAVE�ļ��� */
                SND_RESOURCE = 0x00040004  /* WAVE��Դ�ı�ʶ������ʱҪ�õ�hmod���� */
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
