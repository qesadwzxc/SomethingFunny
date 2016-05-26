using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using MvcApplication.BLL;
using System.Text;
using System.Net;

namespace MvcApplication.Controllers
{
    public class HomeController : Controller
    {
        string ftpPath;
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

        [HttpPost]
        public string Yes()//Guid reportContentGuid
        {
            string returnMessage = string.Empty;
            string localPath = string.Empty;//用户指定存储位置
            string filePath = "10.1.25.57\\TCOATask\\1a2138f3-2c20-4d8a-912d-d8eade395c16\\待修改.txt";//从数据库读取,此处为了方便直接指定了
            string fileName = filePath.Substring(57);//"10.1.25.57\\TCOATask\\xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx\\"后即文件名
            var downloadFile = new UpLoad();
            returnMessage = downloadFile.FTPDownLoad(fileName);
            return returnMessage;
        }

        [HttpPost]
        public string No()
        {
            string returnMessage = string.Empty;
            foreach (string upload in Request.Files)
            {
                if (!HasFile.isFile(Request.Files[upload])) { continue; }
                string localPath = AppDomain.CurrentDomain.BaseDirectory + "uploads/";//应用服务器上传路径
                string fileName = Path.GetFileName(Request.Files[upload].FileName);//文件名
                Request.Files[upload].SaveAs(Path.Combine(localPath, fileName));
                //指定应用服务器上传到FTP服务器时文件的存储路径,类似于“/uploads/{Datetime.Now}”的形式
                string remotePath = "/TCOATask/";
                //上传文件到FTP服务器,返回结果信息
                //FTPUpLoad(Path.Combine(localPath, fileName));
                var uploadsfile = new UpLoad();
                returnMessage = uploadsfile.FTPUpLoad(remotePath, Path.Combine(localPath, fileName));
                //更新存储路径
                ftpPath = Path.Combine(remotePath, fileName);
                //删除本地文件
                //System.IO.File.Delete(Path.Combine(localPath, fileName));
            }
            Response.Write(returnMessage);
            return returnMessage;
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
    }
}
