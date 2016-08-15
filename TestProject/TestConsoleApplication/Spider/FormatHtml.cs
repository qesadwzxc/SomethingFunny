using CsQuery;
using Microsoft.WindowsAPICodePack.Taskbar;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VinCode;

namespace TestConsoleApplication.Spider
{
    public static class FormatHtml
    {
        public const int OF_READWRITE = 2;
        public const int OF_SHARE_DENY_NONE = 0x40;
        public static readonly IntPtr HFILE_ERROR = new IntPtr(-1);

        //static IConfiguration config = Configuration.Default.WithDefaultLoader();
        static readonly string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"Temp\";
        private static object objLock = "";
        static int countNum = 0;
        static int progress = 0;
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

        public static void TopicFormat2(string url)
        {
            List<string> listImgUrl = new List<string>();
            Topic topic = new Topic();
            CQ eachDom = HttpHelper.Get(url, 10000);
            var link = eachDom["a"];
            foreach (var i in link)
            {
                if (i.Attributes["href"] != null && i.Attributes["href"].StartsWith("./"))
                {
                    listImgUrl.Add($"{url.Substring(0, url.LastIndexOf("/"))}{i.Attributes["href"].Substring(1)}");
                }
            }

            if (listImgUrl.Count>0 && listImgUrl[0].EndsWith(".html"))
            {
                CQ d = HttpHelper.Get(listImgUrl[0], 3000);
                var t = d["img"].Eq(1);
                string realUrl = t.Attr("src");
                for (int i = 0; i < listImgUrl.Count; i++)
                {
                    if (realUrl != null && !realUrl.Contains("/"))
                    {
                        listImgUrl[i] = $"{listImgUrl[i].Substring(0, listImgUrl[i].LastIndexOf("/"))}/{realUrl.Replace("1", (i + 1).ToString())}";
                    }
                    else
                    {
                        listImgUrl[i] = realUrl;
                    }
                }
            }

            //for (int i=0;i<listImgUrl.Count;i++)
            //{
            //    if (listImgUrl[i].EndsWith(".html"))
            //    {
            //        CQ d = HttpHelper.Get(listImgUrl[i]);
            //        var t = d["img"].Eq(1);
            //        string realUrl = t.Attr("src");
            //        if (realUrl!=null && !realUrl.Contains("/"))
            //        {
            //            listImgUrl[i] = $"{listImgUrl[i].Substring(0, listImgUrl[i].LastIndexOf("/"))}/{realUrl}";
            //        }
            //        else
            //        {
            //            listImgUrl[i] = realUrl;
            //        }
            //    }
            //}
            if (listImgUrl.Count > 0)
            {
                DownloadPic(url.Substring(url.LastIndexOf("/") + 1), listImgUrl);
            }
        }

        public static List<string> GetRootDom()
        {
            List<string> firstListUrl = new List<string>();
            var rootUrl = $"http://www.pantyhosepink.com";
            CQ dom = HttpHelper.Get(rootUrl);
            var top = dom["a"];
            foreach (var item in top)
            {
                if (item.Attributes["href"].Contains("pantyhoseminx"))
                {
                    firstListUrl.Add(item.Attributes["href"]);
                }
            }
            return firstListUrl;
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
                Parallel.ForEach(picUrlList, new ParallelOptions() { MaxDegreeOfParallelism = 10 }, (picUrl) =>
                {
                    string filePath = string.Empty;
                    if (!string.IsNullOrWhiteSpace(picUrl))
                    {
                        try
                        {
                            filePath = $"{todayPath}\\{picUrl.Substring(picUrl.LastIndexOf('/'))}";
                            FileInfo info = new FileInfo(filePath);
                            if (!File.Exists(filePath) || info.Length < 10240)
                            {
                                WebClient client = new WebClient();
                                client.DownloadFile(picUrl, filePath);
                            }
                            //标题栏百分比显示（按文件数）
                            progress += 1;
                            Console.Title = string.Format(@"下载中... {0}% ", progress * 100 / picUrlList.Count);
                            //任务栏进度条
                            TaskbarManager.Instance.SetProgressValue(progress, picUrlList.Count);
                        }
                        catch (Exception ex)
                        {
                            if (File.Exists(filePath) && !IsInUsed(filePath))
                            {
                                File.Delete(filePath);
                            }
                            //标题栏百分比显示（按文件数）
                            progress += 1;
                            Console.Title = string.Format(@"下载中... {0}% ", progress * 100 / picUrlList.Count);
                            //任务栏进度条
                            TaskbarManager.Instance.SetProgressValue(progress, picUrlList.Count);
                            lock (objLock)
                            {
                                LogHelper.Write($"DownloadError:{picUrl}", ex.Message, "AutoImageSpider");
                            }
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

        /// <summary>
        /// 判断文件是否被占用
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private static bool IsInUsed(string filePath)
        {
            if (File.Exists(filePath))
            {
                IntPtr vHandle = NativeMethods._lopen(filePath, OF_READWRITE | OF_SHARE_DENY_NONE);
                //获取指针
                //string txt = "test";
                //IntPtr p = Marshal.StringToCoTaskMemAuto(txt);

                if (vHandle == HFILE_ERROR)
                {
                    return true;
                }
                NativeMethods.CloseHandle(vHandle);
            }           
            return false;
        }
    }
}
