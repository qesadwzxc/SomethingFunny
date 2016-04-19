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
        static async void Test()
        {
            //var rootUrl = "http://www.douban.com/group/asshole/discussion";
            for (int j = 0; j < 4500; j += 50)
            {
                var rootUrl = string.Format("http://tieba.baidu.com/f?kw=%E9%A6%99%E6%B2%B3&ie=utf-8&pn={0}", j);
                var rootHtml = HttpHelper.Get(rootUrl, proxy: ProxyPool.GetProxy());
                var refererUrl = rootUrl;
                var topicList = await FormatHtml.TopicFormat(rootHtml);
                int i = 1;
                foreach (var topic in topicList)
                {
                    Thread.Sleep(rand.Next(100, 400));
                    try
                    {
                        //var commentsHtml = HttpHelper.Get(topic.Href, proxy: ProxyPool.GetProxy());
                        //refererUrl = topic.Href;
                        //if (string.IsNullOrWhiteSpace(commentsHtml))
                        //    continue;
                        //topic.TopicFormat(commentsHtml);
                        //var nextUrl = topic.CommentFormat(commentsHtml);
                        //while (!string.IsNullOrWhiteSpace(nextUrl))
                        //{
                        //    commentsHtml = HttpHelper.Get(nextUrl, referer: refererUrl, proxy: ProxyPool.GetProxy());
                        //    refererUrl = nextUrl;
                        //    nextUrl = topic.CommentFormat(commentsHtml);
                        //}
                        //Console.WriteLine($"{i++}\t{topic.PageCount}\t{topic.Comments.Count}\t{topic.Title}");
                        //topic.Comments.ForEach(p =>
                        //{
                        //    if (p.Quote != null)
                        //        Console.WriteLine($"quote {p.Quote.Author.Name}\t{p.Quote.Context}");
                        //    Console.WriteLine($"{p.Author.Name}\t{p.Context}\r\n");
                        //});
                        Console.WriteLine($"{i++}\t{topic.Title}");
                    }
                    catch (Exception ex)
                    {
                        Console.Write("error\r\n" + ex);
                    }
                }
            }
        }
    }
}
