using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;
using System.Web;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Security;
using System.Data;
using System.Security.Cryptography;
using System.Collections;
using System.Threading;
using VinCode.Web;
using System.Windows.Forms;
using TestConsoleApplication.Spider;

namespace TestConsoleApplication
{
    #region 加密算法
    /// <summary> 
    /// 类名：HashEncrypt 
    /// 作用：对传入的字符串进行Hash运算，返回通过Hash算法加密过的字串。 
    /// 属性：［无］ 
    /// 构造函数额参数： 
    /// IsReturnNum:是否返回为加密后字符的Byte代码 
    /// IsCaseSensitive：是否区分大小写。 
    /// 方法：此类提供MD5，SHA1，SHA256，SHA512等四种算法，加密字串的长度依次增大。 
    /// </summary> 
    public class HashEncrypt
    {
        //private string strIN; 
        private bool isReturnNum;
        private bool isCaseSensitive;
        /**/
        /// <summary> 
        /// 类初始化，此类提供MD5，SHA1，SHA256，SHA512等四种算法，加密字串的长度依次增大。 
        /// </summary> 
        /// <param name="IsCaseSensitive">是否区分大小写</param> 
        /// <param name="IsReturnNum">是否返回为加密后字符的Byte代码</param> 
        private string GetStringValue(byte[] Byte)
        {
            string tmpString = "";
            if (this.isReturnNum == false)
            {
                ASCIIEncoding Asc = new ASCIIEncoding();
                tmpString = Asc.GetString(Byte);
            }
            else
            {
                int iCounter;
                for (iCounter = 0; iCounter < Byte.Length; iCounter++)
                {
                    tmpString = tmpString + Byte[iCounter].ToString();
                }
            }
            return tmpString;
        }
        private byte[] GetKeyByteArray(string strKey)
        {
            ASCIIEncoding Asc = new ASCIIEncoding();
            int tmpStrLen = strKey.Length;
            byte[] tmpByte = new byte[tmpStrLen - 1];
            tmpByte = Asc.GetBytes(strKey);
            return tmpByte;
        }
        private string getstrIN(string strIN)
        {
            //string strIN = strIN; 
            if (strIN.Length == 0)
            {
                strIN = "~NULL~";
            }
            if (isCaseSensitive == false)
            {
                strIN = strIN.ToUpper();
            }
            return strIN;
        }

        #region Hash
        public HashEncrypt(bool IsCaseSensitive, bool IsReturnNum)
        {
            this.isReturnNum = IsReturnNum;
            this.isCaseSensitive = IsCaseSensitive;
        }
        #endregion

        #region MD5
        public string MD5Encrypt(string strIN)
        {
            //string strIN = getstrIN(strIN); 
            byte[] tmpByte;
            MD5 md5 = new MD5CryptoServiceProvider();
            tmpByte = md5.ComputeHash(GetKeyByteArray(getstrIN(strIN)));
            md5.Clear();
            return GetStringValue(tmpByte);
        }
        #endregion

        #region SHA
        public string SHA1Encrypt(string strIN)
        {
            //string strIN = getstrIN(strIN); 
            byte[] tmpByte;
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            tmpByte = sha1.ComputeHash(GetKeyByteArray(strIN));
            sha1.Clear();
            return GetStringValue(tmpByte);
        }
        public string SHA256Encrypt(string strIN)
        {
            //string strIN = getstrIN(strIN); 
            byte[] tmpByte;
            SHA256 sha256 = new SHA256Managed();
            tmpByte = sha256.ComputeHash(GetKeyByteArray(strIN));
            sha256.Clear();
            return GetStringValue(tmpByte);
        }
        public string SHA512Encrypt(string strIN)
        {
            //string strIN = getstrIN(strIN); 
            byte[] tmpByte;
            SHA512 sha512 = new SHA512Managed();
            tmpByte = sha512.ComputeHash(GetKeyByteArray(strIN));
            sha512.Clear();
            return GetStringValue(tmpByte);
        }
        #endregion

        #region DES
        /// <summary> 
        /// 使用DES加密（Added by niehl 2005-4-6） 
        /// </summary> 
        /// <param name="originalValue">待加密的字符串</param> 
        /// <param name="key">密钥(最大长度8)</param> 
        /// <param name="IV">初始化向量(最大长度8)</param> 
        /// <returns>加密后的字符串</returns> 
        public string DESEncrypt(string originalValue, string key, string IV)
        {
            //将key和IV处理成8个字符 
            key += "12345678";
            IV += "12345678";
            key = key.Substring(0, 8);
            IV = IV.Substring(0, 8);
            SymmetricAlgorithm sa;
            ICryptoTransform ct;
            MemoryStream ms;
            CryptoStream cs;
            byte[] byt;
            sa = new DESCryptoServiceProvider();
            sa.Key = Encoding.UTF8.GetBytes(key);
            sa.IV = Encoding.UTF8.GetBytes(IV);
            ct = sa.CreateEncryptor();
            byt = Encoding.UTF8.GetBytes(originalValue);
            ms = new MemoryStream();
            cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();
            cs.Close();
            return Convert.ToBase64String(ms.ToArray());
        }
        public string DESEncrypt(string originalValue, string key)
        {
            return DESEncrypt(originalValue, key, key);
        }

        /// <summary> 
        /// 使用DES解密（Added by niehl 2005-4-6） 
        /// </summary> 
        /// <param name="encryptedValue">待解密的字符串</param> 
        /// <param name="key">密钥(最大长度8)</param> 
        /// <param name="IV">m初始化向量(最大长度8)</param> 
        /// <returns>解密后的字符串</returns> 
        public string DESDecrypt(string encryptedValue, string key, string IV)
        {
            //将key和IV处理成8个字符 
            key += "12345678";
            IV += "12345678";
            key = key.Substring(0, 8);
            IV = IV.Substring(0, 8);
            SymmetricAlgorithm sa;
            ICryptoTransform ct;
            MemoryStream ms;
            CryptoStream cs;
            byte[] byt;
            sa = new DESCryptoServiceProvider();
            sa.Key = Encoding.UTF8.GetBytes(key);
            sa.IV = Encoding.UTF8.GetBytes(IV);
            ct = sa.CreateDecryptor();
            byt = Convert.FromBase64String(encryptedValue);
            ms = new MemoryStream();
            cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();
            cs.Close();
            return Encoding.UTF8.GetString(ms.ToArray());
        }
        public string DESDecrypt(string encryptedValue, string key)
        {
            return DESDecrypt(encryptedValue, key, key);
        }
        #endregion
    }
    #endregion

    #region 小玩意
    class LittleFun
    {
        static int x = y;
        static int y = 5;

        public static void PrintXandY()
        {
            //What`s result?
            Console.WriteLine(LittleFun.x);
            Console.WriteLine(LittleFun.y);

            LittleFun.x = 99;
            Console.WriteLine(LittleFun.x);
        }

        #region String和StringBuilder拼接性能对比
        public static void AddString()
        {
            int yiwan = 1000;
            string pp = string.Empty;
            for (int i = 0; i < yiwan; i++)
            {
                pp += "p";
            }
        }

        public static void AddStringBuilder()
        {
            int yiwan = 1000;
            StringBuilder bb = new StringBuilder();
            for (int j = 0; j < yiwan; j++)
            {
                bb.Append("b");
            }
        }
        #endregion
    }
    #endregion

    #region 小练习
    static class LittlePrictice
    {
        /// <summary>
        /// 反转句子
        /// </summary>
        //Sample Input：“how are you”
        //Sample Output：“you are how”
        public static void TurnSentence()
        {
            Console.WriteLine("Please input the text you want to change.");
            string inputText = Console.ReadLine();
            string[] outputList = inputText.Split(' ');
            for (int i = outputList.Length - 1; i >= 0; i--)
            {
                Console.Write(outputList[i] + " ");
            }

            Console.ReadKey();
        }

        /// <summary>
        /// 反转单词
        /// </summary>
        //Sample Input：“how”
        //Sample Output：“woh”
        public static void TurnWord()
        {
            Console.WriteLine("Please input the text you want to change.");
            string inputText = Console.ReadLine();
            char[] outputList = inputText.ToCharArray();
            for (int i = outputList.Length - 1; i >= 0; i--)
            {
                Console.Write(outputList[i]);
            }

            Console.ReadKey();
        }

        /// <summary>
        /// 统计输入内容中的元音字母
        /// </summary>
        /// <param name="strInput">待统计的字符串</param>
        public static void CountVowel()
        {
            Console.WriteLine("请输入要统计的字符串"); //TODO:有空做成统计任意字母
            string strInput = Console.ReadLine();

            char[] VowelList = { 'a', 'e', 'i', 'o', 'u' };
            int[] countList = new int[VowelList.Length];

            char[] charInput = strInput.ToCharArray();
            var charInputTrim = from i in charInput
                                where i != ' '
                                select i;
            foreach (var item in charInputTrim)
            {
                for (int i = 0; i < VowelList.Length; i++)
                {
                    if (item == VowelList[i])
                    {
                        countList[i] += 1;
                    }
                }
            }

            StringBuilder strOutput = new StringBuilder();
            strOutput.Append("元音字母统计：\n");
            for (int i = 0; i < countList.Length; i++)
            {
                strOutput.AppendFormat("{0}:{1};\n", VowelList[i], countList[i]);
            }
            Console.WriteLine(strOutput.ToString());
        }

        /// <summary>
        /// 统计字符串中的单词数
        /// </summary>
        /// <param name="strInput">待统计的字符串</param>
        public static void CountWord()
        {
            string strInput = string.Empty;
            //文件读取
            using (FileStream fileSm = new FileStream("C:\\ss.txt", FileMode.Open))
            {
                if (fileSm.Length < int.MaxValue)
                {
                    byte[] array = new byte[fileSm.Length];
                    int offset = 0;
                    int length = 500;

                    //每次只读取500长度。不足的则一次性读取完毕。
                    while (true)
                    {
                        if (offset + length > fileSm.Length)
                        {
                            length = (int)fileSm.Length - offset;
                        }
                        fileSm.Read(array, offset, length);
                        offset += length;
                        if (offset == fileSm.Length)
                        {
                            break;
                        }
                    }

                    //去掉符号'?'、'"'、','、'.'。空格为32、'\r'为13、'\n'为10
                    //Linq实现T-SQL中的Not In
                    var bb = (from item in array
                              where !(new int?[] { 34, 44, 46, 63 }).Contains(item)
                              select item).ToArray();

                    //GB2312对应CodePage编码是936
                    string encodingName = Encoding.Default.EncodingName;
                    strInput = Encoding.GetEncoding(936).GetString(bb).ToLower();
                }
            }

            //Console.WriteLine("请输入要统计的字符串"); 
            List<string> strList = strInput.Split(new string[] { " ", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            Dictionary<string, int> dicResult = new Dictionary<string, int>();
            foreach (var str in strList)
            {
                if (dicResult.ContainsKey(str))
                {
                    dicResult[str] += 1;
                }
                else
                {
                    dicResult.Add(str, 1);
                }
            }
            StringBuilder strOutput = new StringBuilder();
            strOutput.Append("单词统计结果：\n");
            var ldicResult = dicResult.OrderBy(m => m.Key);
            foreach (var item in ldicResult)
            {
                strOutput.AppendFormat("{0}:{1};\n", item.Key, item.Value);
            }
            Console.Write(strOutput.ToString());
        }

        public static void IsPalindroms(string word)
        {
            bool isPalindroms = false;
            int len = word.Length % 2;
            string firstHalf = string.Empty;
            string lastHalf = string.Empty;
            switch (len)
            {
                case 0:
                    firstHalf = word.Substring(0, word.Length / 2);
                    lastHalf = word.Substring(word.Length / 2);
                    break;
                case 1:
                    firstHalf = word.Substring(0, word.Length / 2);
                    lastHalf = word.Substring((word.Length / 2) + 1);
                    break;
            }
            Stopwatch w1 = new Stopwatch();

            //1
            w1.Start();
            char[] lastHalfChar = lastHalf.ToCharArray();
            StringBuilder lastHalfCopy = new StringBuilder();
            foreach (char s in lastHalfChar)
            {
                lastHalfCopy.Insert(0, s);
            }
            if (firstHalf.Equals(lastHalfCopy.ToString()))
            {
                isPalindroms = true;
            }
            w1.Stop();
            Console.WriteLine(isPalindroms.ToString() + ",Time:" + w1.Elapsed.ToString());
            w1.Reset();

            //2
            w1.Start();
            lastHalfChar = lastHalf.ToCharArray();
            char[] firstHalfChar = firstHalf.ToCharArray();
            isPalindroms = true;
            for (int i = 0; i < firstHalf.Length; i++)
            {
                if (!firstHalfChar[i].Equals(lastHalfChar[lastHalfChar.Length - 1 - i]))
                {
                    isPalindroms = false;
                }
            }
            w1.Stop();
            Console.WriteLine(isPalindroms.ToString() + ",Time:" + w1.Elapsed.ToString());
        }
    }
    #endregion

    #region 数据结构
    /// <summary>
    /// 创建单例
    /// </summary>
    public class Singleton
    {
        private static Singleton instance;
        private Singleton() { }
        public static Singleton Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Singleton();
                }
                return instance;
            }
        }
    }

    /// <summary>
    /// 单链表
    /// </summary>
    public class LinkList
    {
        /// <summary>
        /// 实体
        /// </summary>
        public class LinkNode
        {
            public object data;
            public LinkNode next;
        }

        /// <summary>
        /// 头结点
        /// </summary>
        private LinkNode head;

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="data"></param>
        public void Add(object data)
        {
            if (head == null)
            {
                head = new LinkNode() { data = data };
            }
            else
            {
                Add(head, data);
            }
        }

        private void Add(LinkNode node, object data)
        {
            if (node.next == null)
            {
                node.next = new LinkNode() { data = data };
                return;
            }

            Add(node.next, data);
        }

        /// <summary>
        /// 倒置
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public LinkNode Reverse(LinkNode node)
        {
            if (node.next == null)
            {
                return node;
            }
            var prevNode = Reverse(node.next);
            var temp = node.next;
            temp.next = node;
            node.next = null;
            return prevNode;
        }
    }

    /// <summary>
    /// 自定义运算符和转换
    /// </summary>
    public struct Complex
    {
        public Complex(int num) { }

        /// <summary>
        /// 隐式转换调用的方法
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator Complex(int value)
        {
            return new Complex(value);
        }

        /// <summary>
        /// 强制转换调用的方法
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static explicit operator int(Complex value)
        {
            return Convert.ToInt32(value);
        }

        public static int operator +(Complex num1)
        {
            return 0;
        }

        public static int operator +(Complex num1, Complex num2)
        {
            return 1;
        }
    }
    #endregion

    class Program
    {
        static void Main(string[] args)
        {

            //Queue<int> q = new Queue<int>();
            //List<int> l = new List<int>();
            //System.Timers.Timer t = new System.Timers.Timer();
            //System.Threading.Timer tt = new System.Threading.Timer(null);
            //System.Windows.Forms.Timer ttt = new System.Windows.Forms.Timer();

            //5次比较
            for (int i = 1; i <= 5; i++)
            {
                List<int> list = new List<int>();

                //插入200个随机数到数组中
                for (int j = 0; j < 200; j++)
                {
                    Thread.Sleep(1);
                    list.Add(new Random((int)DateTime.Now.Ticks).Next(0, 10000));
                }

                Console.WriteLine("\n第" + i + "次比较：");

                Stopwatch watch = new Stopwatch();

                watch.Start();
                var result = list.OrderBy(single => single).ToList();
                watch.Stop();

                Console.WriteLine("\n系统定义的快速排序耗费时间：" + watch.ElapsedMilliseconds);
                Console.WriteLine("输出前是十个数:" + string.Join(",", result.Take(10).ToList()));

                watch.Start();
                SortTest.QuickSort(list, 0, list.Count - 1);
                watch.Stop();

                Console.WriteLine("\n俺自己写的快速排序耗费时间：" + watch.ElapsedMilliseconds);
                Console.WriteLine("输出前是十个数:" + string.Join(",", list.Take(10).ToList()));
            }
            Console.ReadLine();
        }

        int CountBinary(long num)
        {
            var count = 0;
            while (num > 0)
            {
                num &= num - 1;
                count++;
            }

            return count;
        }

        //static void Main(string[] args)
        //{
        //    Stopwatch sw = new Stopwatch();
        //    NewSpider.Test();
        //    Console.WriteLine(sw.ElapsedMilliseconds + "ms");
        //    Console.Read();
        //}

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        //[STAThread]
        //static void Main(string[] args)
        //{
        //    Application.EnableVisualStyles();
        //    Application.SetCompatibleTextRenderingDefault(false);
        //    Application.Run(new TextEditor());
        //}
    }
}

