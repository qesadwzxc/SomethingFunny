using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TestConsoleApplication.Spider;

namespace TestConsoleApplication
{
    public class NewSpider
    {
        static Random rand = new Random();
        public static async void Test()
        {
            Random rand = new Random();
            //var rootUrl = "http://www.douban.com/group/asshole/discussion";
            for (int j = 100000; j < 100010; j++)
            {
                var rootUrl = $"http://tu.duowan.com/scroll/{j}.html";
                var rootHtml = HttpHelper.Get(rootUrl);//HttpHelper.Get(rootUrl, proxy: ProxyPool.GetProxy());
                var refererUrl = rootUrl;
                var topic = await FormatHtml.TopicFormat(rootHtml);
                try
                {
                    Console.WriteLine($"{j}\t{topic.Title}");
                }
                catch (Exception ex)
                {
                    Console.Write("error\r\n" + ex);
                }
                Thread.Sleep(rand.Next(100, 400));
            }
        }
    }
}
