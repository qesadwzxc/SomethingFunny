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
