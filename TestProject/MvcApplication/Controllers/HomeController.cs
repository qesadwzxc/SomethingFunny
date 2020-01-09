using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text;
using System.Net;

namespace MvcApplication.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string pageIndex)
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            if (pageIndex != null)
            {
                if (string.IsNullOrWhiteSpace(pageIndex))
                {
                    ViewBag.Date = "WTF";
                }
                else if (pageIndex == "1")
                {
                    ViewBag.Date = "Ass";
                }
                else
                {
                    ViewBag.Date = "Hole";
                }
            }
            return View();
        }

        [HttpGet]
        public JsonResult Page(string pageIndex)
        {
            if (string.IsNullOrWhiteSpace(pageIndex))
            {
                return Json(new { id = 0, name = "WTF" }, JsonRequestBehavior.AllowGet);
            }
            else if (pageIndex == "1")
            {
                return Json(new { id = 1, name = "ass" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { id = 2, name = "hole" }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult About()
        {
            return View();
        }

        [HttpPost]
        public string Delete(string name)
        {
            string msg = string.Empty;
            string fileName = name;
            string path = AppDomain.CurrentDomain.BaseDirectory + "uploads/";
            string delPath = Path.Combine(path, fileName);
            if (System.IO.File.Exists(delPath))
            {
                System.IO.File.Delete(delPath);
                msg = "删除成功！";
            }
            else
            {
                msg = "删除失败，未找到文件";
            }
            return msg;
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public static void FTPUpLoad(string fileName)
        {
            //构造一个web服务器的请求对象 
            FtpWebRequest ftp;
            //实例化一个文件对象 
            FileInfo f = new FileInfo(fileName);
            ftp = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://10.1.25.57/" + f.Name));
            //创建用户名和密码 
            ftp.Credentials = new NetworkCredential("LMW12960", "asdfghjkl;123");
            ftp.KeepAlive = false;
            ftp.Method = WebRequestMethods.Ftp.UploadFile;
            ftp.UseBinary = true;
            ftp.ContentLength = f.Length;
            int buffLength = 20480;
            byte[] buff = new byte[buffLength];
            int contentLen;
            try
            {
                //获得请求对象的输入流 
                FileStream fs = f.OpenRead();
                Stream sw = ftp.GetRequestStream();
                contentLen = fs.Read(buff, 0, buffLength);
                while (contentLen != 0)
                {
                    sw.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }
                sw.Close();
                fs.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void test(string data)
        {
            var f = HttpContext.Request.Form;
            var d = HttpContext.Request.InputStream;
            string re = string.Empty;
            if (d != null && d.CanRead)
            {
                var httpStreamReader = new StreamReader(d, Encoding.UTF8);
                re = httpStreamReader.ReadToEnd();
                httpStreamReader.Close();
            }

            var j = HttpContext.Request.QueryString;
            var k = HttpContext.Request.Params;
            int i = 1;
        }
    }
}
