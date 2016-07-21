using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web.Mvc;
using System.Web.Caching;

namespace MvcApplication.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)

        {
            var requestHeader = filterContext.RequestContext.HttpContext.Request.Headers;
            if (!requestHeader.AllKeys.Contains("BP"))
            {
                string url = filterContext.RequestContext.HttpContext.Request.Url.ToString();
                object html = HttpContext.Cache.Get(url);
                if (html != null)
                {
                    //read html
                    filterContext.Result = Content(html.ToString());
                }
                else
                {
                    HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                    request.Method = "GET";
                    request.Timeout = 10000;
                    request.Headers.Add("BP", "true");
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        html = GetResponseBody(response);
                    }
                    HttpContext.Cache.Add(url, html, null, DateTime.Now.AddSeconds(10), Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
                }
            }
            base.OnActionExecuting(filterContext);
        }

        static Random _rand = new Random();

        public static string Get(string url, int timeout = 1000, int retry = 1, string referer = null, WebProxy proxy = null)
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
                request.Referer = referer ?? "http://www.douban.com/group/asshole/discussion?start=0";
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
            return string.Empty;

        }

        private static string GetResponseBody(HttpWebResponse response)
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
