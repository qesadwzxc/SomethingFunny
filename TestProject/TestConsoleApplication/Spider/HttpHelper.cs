﻿using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Threading;
using VinCode;

namespace TestConsoleApplication.Spider
{
    public static class HttpHelper
    {
        static Random _rand = new Random();
        private static object objLock = "";

        public static string Get(string url, int timeout = 1000, int retry = 3, string referer = null, WebProxy proxy = null)
        {
            while (retry-- > 0)
            {
                if (true)//Config.AutoDelay
                {
                    Thread.Sleep(_rand.Next(100, 1000));
                }
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                if (proxy != null)
                {
                    request.Proxy = proxy;
                }
                request.Timeout = timeout;//Config.TimeLimit
                request.Headers.Add("Accept-Language", "zh-CN");
                request.Headers.Add("DNT", "1");
                request.Headers.Add("Accept-Encoding", "gzip,deflate");
                request.Accept = "text/plain, */*; q=0.01";
                request.Host = new Uri(url).Host;
                //request.Referer = referer ?? "http://www.douban.com/group/asshole/discussion?start=0";
                var appleKitVersion = $"{ _rand.Next(477, 548)}.{_rand.Next(23, 40)}";
                request.UserAgent = $"Mozilla/5.0 (Windows NT {_rand.Next(4, 7)}.1) AppleWebKit/{appleKitVersion} (KHTML, like Gecko) Maxthon/4.0 Chrome/30.0.1599.101 Safari/{appleKitVersion}";

                try
                {
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        return GetResponseBody(response);
                    }
                }
                catch (Exception ex)
                {
                    Console.Write($"Url:{url}\t");
                    //if (proxy != null)
                    //    Console.Write($"Porxy:{proxy.Address}\t");
                    Console.WriteLine(ex.Message);
                }
            }
            lock (objLock)
            {
                LogHelper.Write($"HttpGetFailure:{url}", "请求失败", "AutoImageSpider");
            }
            return string.Empty;

        }

        public static string GetResponseBody(HttpWebResponse response)
        {

            string responseBody = string.Empty;
            if (response.ContentEncoding.ToLower().Contains("gzip"))
            {
                using (GZipStream stream = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress))
                {
                    StreamReader reader = new StreamReader(stream);
                    responseBody = reader.ReadToEnd();
                }
            }
            else if (response.ContentEncoding.ToLower().Contains("deflate"))
            {
                using (DeflateStream stream = new DeflateStream(response.GetResponseStream(), CompressionMode.Decompress))
                {
                    StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                    responseBody = reader.ReadToEnd();
                }
            }
            else
            {
                using (Stream stream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                    responseBody = reader.ReadToEnd();
                }
            }
            return responseBody;
        }
    }
}

