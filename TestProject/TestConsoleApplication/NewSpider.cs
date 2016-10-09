using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestConsoleApplication.Spider;

namespace TestConsoleApplication
{
    public class NewSpider
    {
        static Random rand = new Random();
        /// <summary>
        /// 多玩图库
        /// </summary>
        /// <param name="start"></param>
        /// <param name="round"></param>
        public static async void Test(int start, int round)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Random rand = new Random();
            //var rootUrl = "http://www.douban.com/group/asshole/discussion";
            //for (int j = 100000; j < 100010; j++)
            //{
            //    var rootUrl = $"http://tu.duowan.com/scroll/{j}.html";
            //    var rootHtml = HttpHelper.Get(rootUrl);//HttpHelper.Get(rootUrl, proxy: ProxyPool.GetProxy());
            //    var refererUrl = rootUrl;
            //    var topic = await FormatHtml.TopicFormat(rootHtml);
            //    try
            //    {
            //        Console.WriteLine($"{j}\t{topic.Title}");
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.Write("error\r\n" + ex);
            //    }
            //    Thread.Sleep(rand.Next(100, 400));
            //}
            var list = GetIndex(start, round);
            foreach (var j in list)
            {
                var rootUrl = $"http://tu.duowan.com/scroll/{j}.html";
                var rootHtml = HttpHelper.Get(rootUrl);//HttpHelper.Get(rootUrl, proxy: ProxyPool.GetProxy());

                Task.Run(() => FormatHtml.TopicFormat(rootHtml));
                //var topic = await FormatHtml.TopicFormat(rootHtml);
                //暂时去掉这边的文字输出，太乱了
                //try
                //{
                //    Console.WriteLine($"{j}\t{topic.Title}");
                //}
                //catch (Exception ex)
                //{
                //    Console.Write("error\r\n" + ex);
                //}
                Thread.Sleep(rand.Next(100, 200));
            }
            Console.WriteLine($"分析完毕,共{list.Count}个链接，历时：{sw.ElapsedMilliseconds / 1000}s");
        }

        /// <summary>
        /// pantyhosepink
        /// </summary>
        /// <param name="start"></param>
        /// <param name="round"></param>
        public static async void Test2(int start, int round)
        {
            List<string> firstListUrl = new List<string>();
            int i = 0;
            firstListUrl = FormatHtml.GetRootDom();
            foreach (var url in firstListUrl)
            {
                i++;
                if (i < start) continue;
                if (i > round) break;
                await Task.Run(() => FormatHtml.TopicFormat2(url));
            }
            Thread.Sleep(rand.Next(100, 200));
        }

        /// <summary>
        /// 美桌
        /// </summary>
        /// <param name="start"></param>
        /// <param name="round"></param>
        public static async void Test3(int start, int round)
        {
            //http://www.win4000.com/meinvtag35.html
        }

        /// <summary>
        /// 图片之家
        /// </summary>
        /// <param name="start"></param>
        /// <param name="round"></param>
        public static async void Test4(int start, int round)
        {
            //http://tupian.101711.com/meinv/xinggan
        }

        /// <summary>
        /// 获取待下载链接
        /// </summary>
        /// <param name="start">起始数字</param>
        /// <param name="round">循环次数</param>
        /// <returns></returns>
        public static List<int> GetIndex(int start, int round)
        {
            List<int> result = new List<int>();
            for (int i = round; i > 0; i--)
            {
                string url = "http://tu.duowan.com/m/meinv?offset={0}&order=created&math=0.39950458076782525";
                string content = HttpHelper.Get(string.Format(url, 30 * i + start));
                var urlList = content.Split(new string[] { "gallery" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in urlList)
                {
                    string strIndex = item.Substring(2, 6);
                    int index = 0;
                    if (int.TryParse(strIndex, out index) && !result.Contains(index))
                    {
                        result.Add(index);
                    }
                }
            }

            return result;
        }
    }
}
