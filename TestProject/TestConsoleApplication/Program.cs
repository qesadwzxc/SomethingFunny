using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Data;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

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
            //string reverse = outputList.Aggregate((a, n) => n.Reverse() + " " + a);
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

        /// <summary>
        /// 判断是否回文
        /// </summary>
        /// <param name="word"></param>
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
            Console.WriteLine(isPalindroms.ToString() + ",Time:" + w1.Elapsed.ToString());
            w1.Restart();

            //2
            lastHalfChar = lastHalf.ToCharArray();
            char[] firstHalfChar = firstHalf.ToCharArray();
            isPalindroms = true;
            for (int i = 0; i < firstHalf.Length; i++)
            {
                if (!firstHalfChar[i].Equals(lastHalfChar[lastHalfChar.Length - 1 - i]))
                {
                    isPalindroms = false;
                    break;
                }
            }
            w1.Stop();
            Console.WriteLine(isPalindroms.ToString() + ",Time:" + w1.Elapsed.ToString());
        }

        /// <summary>          
        /// 金额阿拉伯数字转换为大写(前面自己写的后面百度的)
        /// </summary>         
        /// <param name="value"></param>         
        /// <returns></returns>          
        public static string GetDaXieMoney(double money)
        {
            //string[] moneyUnit = { "分", "角", "圆", "拾", "佰", "仟", "萬", "拾", "佰", "仟", "亿", "拾", "佰", "仟", "萬" };
            //string result = "";         //←定义结果             
            //int unitPointer = 0;        //←定义单位位置             
            ////↓格式化金额字符串              
            //string valueStr = value.ToString("#0.00");
            ////↓判断是否超出万亿的限制             
            //if (valueStr.Length > 16)
            //{
            //    throw new Exception("不支持超过万亿级别的数字！");
            //}
            ////↓遍历字符串，获取金额大写              
            //for (int i = valueStr.Length - 1; i >= 0; i--)
            //{
            //    //↓判断是否小数点                  
            //    if (valueStr[i] != '.')
            //    {
            //        //↓获取中文大写字符
            //        string chineseNum = string.Empty;
            //        switch (valueStr[i])
            //        {
            //            case '0': chineseNum = "零"; break;
            //            case '1': chineseNum = "壹"; break;
            //            case '2': chineseNum = "贰"; break;
            //            case '3': chineseNum = "叁"; break;
            //            case '4': chineseNum = "肆"; break;
            //            case '5': chineseNum = "伍"; break;
            //            case '6': chineseNum = "陆"; break;
            //            case '7': chineseNum = "柒"; break;
            //            case '8': chineseNum = "捌"; break;
            //            case '9': chineseNum = "玖"; break;
            //        }
            //        //↓后推方式增加内容                      
            //        result = chineseNum + moneyUnit[unitPointer] + result;
            //        //↓设置单位位置                    
            //        unitPointer++;
            //    }
            //}
            //result = result.Replace("零分", "").Replace("零角", "").Replace("零仟", "零").Replace("零佰", "零").Replace("零拾", "零").Replace("零圆", "").Replace("零零零", "零").Replace("零零", "零").Replace("零萬", "萬").Replace("零亿", "亿").Replace("亿萬", "萬").TrimEnd('零');
            //return result;
            if (money < 0)
                throw new ArgumentOutOfRangeException("参数money不能为负值！");
            string s = money.ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A");
            s = Regex.Replace(s, @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}");
            return Regex.Replace(s, ".", m => { return "负元空零壹贰叁肆伍陆柒捌玖空空空空空空空分角拾佰仟萬億兆京垓秭穰"[m.Value[0] - '-'].ToString(); });
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
        //static void Main(string[] args)
        //{

        //    //Queue<int> q = new Queue<int>();
        //    //List<int> l = new List<int>();
        //    //System.Timers.Timer t = new System.Timers.Timer();
        //    //System.Threading.Timer tt = new System.Threading.Timer(null);
        //    //System.Windows.Forms.Timer ttt = new System.Windows.Forms.Timer();

        //    //5次比较
        //    for (int i = 1; i <= 5; i++)
        //    {
        //        List<int> list = new List<int>();

        //        //插入200个随机数到数组中
        //        for (int j = 0; j < 200; j++)
        //        {
        //            Thread.Sleep(1);
        //            list.Add(new Random((int)DateTime.Now.Ticks).Next(0, 10000));
        //        }

        //        Console.WriteLine("\n第" + i + "次比较：");

        //        Stopwatch watch = new Stopwatch();

        //        watch.Start();
        //        var result = list.OrderBy(single => single).ToList();
        //        watch.Stop();

        //        Console.WriteLine("\n系统定义的快速排序耗费时间：" + watch.ElapsedMilliseconds);
        //        Console.WriteLine("输出前是十个数:" + string.Join(",", result.Take(10).ToList()));

        //        watch.Start();
        //        SortTest.QuickSort(list, 0, list.Count - 1);
        //        watch.Stop();

        //        Console.WriteLine("\n俺自己写的快速排序耗费时间：" + watch.ElapsedMilliseconds);
        //        Console.WriteLine("输出前是十个数:" + string.Join(",", list.Take(10).ToList()));
        //    }
        //    Console.ReadLine();
        //}

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

        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("输入起始数和圈数");
            int start = Convert.ToInt32(Console.ReadLine());
            int round = Convert.ToInt32(Console.ReadLine());
            NewSpider.Test(start, round);

            Console.Read();
        }

        public static int Puzzle(string s)
        {
            StringBuilder sb = new StringBuilder();
            string[] strarray = { "" };
            strarray.Aggregate<string, StringBuilder>(sb, (sbobj, str) => sbobj.Append(str).Append(","));
            int u = 10, l = 15;
            int i = Enumerable.Range(l, u - l + 1).Aggregate(1, (y, x) => y * x);
            //if (!string.IsNullOrEmpty(s))
            //{
            //    i += s.First() == 'a' ? 1 : 0;
            //    i += Puzzle(s.Substring(1));
            //}
            //return i;
            string p = "dsadsadsadsad";
            var sr = p.Select(x => "_ ").ToString().TrimEnd(' ');
            return string.IsNullOrEmpty(s) ? 0 : (s.First() == 'a' ? 1 : 0) + Puzzle(s.Substring(1));
        }

        /// <summary>
        /// 运行窗口程序
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

