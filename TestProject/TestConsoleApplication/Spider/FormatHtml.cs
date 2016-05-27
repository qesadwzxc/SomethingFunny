using CsQuery;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VinCode;

namespace TestConsoleApplication.Spider
{
    public static class FormatHtml
    {
        //static IConfiguration config = Configuration.Default.WithDefaultLoader();
        static readonly string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"Temp\";
        private static object objLock = "";
        static int countNum = 0;
        public static async Task<Topic> TopicFormat(string htmlText)
        {
            CQ dom = await GetHtmlContent(htmlText);

            return TopicFormat(dom);
        }

        public static Topic TopicFormat(CQ dom)
        {
            List<string> listImgUrl = new List<string>();
            Topic topic = new Topic();
            var top = dom["h1"].Eq(1);
            topic.Title = top.Text();
            var imgList = dom["img"];
            foreach (var img in imgList)
            {
                listImgUrl.Add(img.Attributes.GetAttribute("src"));
            }
            DownloadPic(topic.Title, listImgUrl);
            topic.ContentImageUrl = listImgUrl;
            return topic;
        }

        public static async Task<CQ> GetHtmlContent(string htmlText)
        {
            return await Task.Run(() =>
            {
                CQ dom = htmlText;
                return dom;
            });
        }

        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="picUrlList"></param>
        public static void DownloadPic(string topic, List<string> picUrlList)
        {
            ServicePointManager.DefaultConnectionLimit = 512;
            Stopwatch sw = new Stopwatch();
            var task = Task.WhenAll(Task.Run(() =>
            {
                sw.Start();
                topic = Regex.Replace(topic, "[\\./:*?\"|<>]", " ", RegexOptions.Compiled);
                string todayPath = LogHelper.CreateFolder(path + topic);
                Parallel.ForEach(picUrlList, (picUrl) =>
                {
                    try
                    {
                        string filePath = $"{todayPath}\\{picUrl.Substring(picUrl.LastIndexOf('/'))}";
                        FileInfo info = new FileInfo(filePath);
                        if (!File.Exists(filePath) || info.Length < 10240)
                        {
                            WebClient client = new WebClient();
                            client.DownloadFile(picUrl, filePath);
                        }
                    }
                    catch (Exception ex)
                    {
                        lock (objLock)
                        {
                            LogHelper.Write($"DownloadError:{picUrl}", ex.Message, "AutoImageSpider");
                        }
                    }
                });
                sw.Stop();
                countNum += 1;

                //foreach (string picUrl in picUrlList)
                //{
                //    try
                //    {
                //        WebClient client = new WebClient();                     
                //        client.DownloadFile(picUrl, $"{todayPath}\\{picUrl.Substring(picUrl.LastIndexOf('/'))}");
                //    }
                //    catch (Exception ex)
                //    {
                //        lock(objLock)
                //        {
                //            LogHelper.Write($"DownloadError:{picUrl}", ex.Message, "AutoImageSpider");
                //        }
                //        continue;
                //    }
                //}
            }));
            task.ContinueWith((m) => { Console.WriteLine($"{countNum}:《{topic}》下载完毕，历时{sw.ElapsedMilliseconds / 1000}s"); });
        }

        public static string CommentFormat(this Topic topic, string htmlText)
        {
            return null;
        }
    }
}
