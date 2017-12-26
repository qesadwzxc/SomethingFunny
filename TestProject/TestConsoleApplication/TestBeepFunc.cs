////////////////////////////////////////////////////////////////////////////////
///系统蜂鸣器播放器
////////////////////////////////////////////////////////////////////////////////
using System;

namespace TestConsoleApplication
{
    internal class TestBeepFunc
    {
        public void BeepFunction()
        {
            foreach (var item in Enum.GetValues(typeof(SoundByte)))
            {
                NativeMethods.Beep(Convert.ToInt32(item), 500);
                //Console.ReadLine();
                //Console.Read();
                //NativeMethods.MessageBeep(0x00000010);
            }
            //NativeMethods.PlaySound(@"e:/Music/login.wav", 0, 1);

            Console.Read();
        }
    }
    #region 音符
    /// <summary>
    /// 音符枚举
    /// </summary>
    public enum SoundByte
    {
        low_DO = 262,
        low_DO_plus = 277,
        low_RE = 294,
        low_RE_plus = 311,
        low_M = 330,
        low_FA = 349,
        low_FA_plus = 370,
        low_SO = 395,
        low_SO_plus = 415,
        low_LA = 440,
        low_LA_plus = 466,
        low_SI = 494,

        medium_DO = 523,
        medium_DO_plus = 554,
        medium_RE = 578,
        medium_RE_plus = 622,
        medium_M = 659,
        medium_FA = 698,
        medium_FA_plus = 740,
        medium_SO = 784,
        medium_SO_plus = 831,
        medium_LA = 880,
        medium_LA_plus = 932,
        medium_SI = 988,

        high_DO = 1046,
        high_DO_plus = 1109,
        high_RE = 1175,
        high_RE_plus = 1245,
        high_M = 1318,
        high_FA = 1397,
        high_FA_plus = 1480,
        high_SO = 1568,
        high_SO_plus = 1661,
        high_LA = 1760,
        high_LA_plus = 1865,
        high_SI = 1976,
    }

    /*
  低音区  低DO	262	63628	(接左)	#FA	   740	0676
　　      #DO	277	63731		    中SO   784	0638
　　      低RE	294	63836		    #SO	   831	0602
　　      #RE	311	63928		    中LA   880	0568
　　      低M	330	64020		    #LA	   932	0536
　　      低FA	349	64103		    中SI   988	0506
　　      #FA	370	64186   高音区  高DO   1046	0478
　　      低SO	392	64260		    #DO	   1109	0451
　　      #SO	415	64331		    高RE   1175	0426
　　      低LA	440	64400		    #RE	   1245	0402
　　      #LA	466	64464		    高M	   1318	0372
　　      低SI	494	64524		    高FA   1397	0358
　中音区  中DO	523	64580		    #FA	   1480	0338
　　      #DO	554	64633		    高SO   1568	0319
　　      中RE	578	64694		    #SO	   1661	0292
　　      #RE	622	64732		    高LA   1760	0284
　　      中M	659	64777		    #LA	   1865	0268
　　      中FA	698	64820		    高SI   1976	0253*/

    /*曲调值	    延时(ms)	    曲调值	  延时(ms)
      调4/4	      125	    调4/4	    62
      调3/4	      187	    调3/4	    94
      调2/4	      250	    调2/4	    125*/
    #endregion

    #region 标准提示音
    [Flags]
    public enum StandardSound
    {
        //发出不同类型的声音的参数如下：  
        Ok = 0x0,
        Error = 0x10,
        Question = 0x20,
        Warning = 0x30,
        Information = 0x40
    }
    #endregion
}
