using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsQuery;
using System.Net;
using System.IO;
using VinCode;

namespace TestConsoleApplication.Spider
{
    public static class FormatHtml
    {
        //static IConfiguration config = Configuration.Default.WithDefaultLoader();
        static readonly string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"Temp\";
        private static object objLock = "";
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
        public static void DownloadPic(string topic,List<string> picUrlList)
        {         
            char[] NOTLIMITONDOCUMENTNAME = new char[] { '\\', '.', '/', ':', '*', '?', '"', '|', '<', '>' };
            Task.Run(() => 
            {
                foreach (char ITEM in NOTLIMITONDOCUMENTNAME)
                {
                    topic.Replace(ITEM, ' ');
                }
                string todayPath = LogHelper.CreateFolder(path + topic);
                foreach (string picUrl in picUrlList)
                {
                    try
                    {
                        WebClient client = new WebClient();                     
                        client.DownloadFile(picUrl, $"{todayPath}\\{picUrl.Substring(picUrl.LastIndexOf('/'))}");
                    }
                    catch (Exception ex)
                    {
                        lock(objLock)
                        {
                            LogHelper.Write($"DownloadError:{picUrl}", ex.Message, "AutoImageSpider");
                        }
                        continue;
                    }
                }
            });
        }

        public static string CommentFormat(this Topic topic, string htmlText)
        {
            return null;
        }
    }
}
