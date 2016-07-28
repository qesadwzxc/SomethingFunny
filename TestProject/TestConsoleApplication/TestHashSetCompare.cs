////////////////////////////////////////////////////////////////////////////////
///Hash比较测试
////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;

namespace TestConsoleApplication
{
    public class TestHashSetCompare
    {
        public void Compare()
        {
            List<string> strs = CreateStrings();
            Console.WriteLine("Use ImplementByHashSet2");
            bool result = CodeTimer.MeasurePerformance(s =>
            {
                var f = ImplementByHashSet2(strs);
                bool ret = false;
                for (int i = 0; i < 1000; i++)
                {
                    ret = f(s);
                }
                return ret;
            }, "yZhs", 1);
            Console.WriteLine("result is " + result.ToString());
            Console.ReadLine();
        }

        private static Func<string, bool> ImplementByHashSet2(List<string> strs)
        {
            HashSet<string> set = new HashSet<string>(strs, StringComparer.CurrentCultureIgnoreCase);
            return set.Contains;
        }

        /// <summary>
        /// 生成测试用
        /// </summary>
        /// <returns></returns>
        private static List<string> CreateStrings()
        {
            List<string> strs = new List<string>(10000);
            char[] chs = new char[3];
            for (int i = 0; i < 10000; i++)
            {
                int j = i;
                for (int k = 0; k < chs.Length; k++)
                {
                    chs[k] = (char)('a' + j % 26);
                    j = j / 26;
                }
                strs.Add(new string(chs));
            }
            return strs;
        }
    }
}
