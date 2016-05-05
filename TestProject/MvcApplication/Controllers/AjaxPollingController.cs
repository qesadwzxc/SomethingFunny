using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading;
using System.Net.Sockets;

namespace MvcApplication.Controllers
{
    public class AjaxPollingController : Controller
    {
        //
        // GET: /AjaxPolling/

        public ActionResult Polling()
        {
            return View();
        }

        public JsonResult GetPolling()
        {
            return Json(DateTime.Now.ToString(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLongPolling()
        {
            int lastMinute = DateTime.Now.Minute;
            while (true)
            {
                if (DateTime.Now.Minute > lastMinute)
                {
                    return Json(DateTime.Now.ToString(), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //Thread.Sleep(100);
                }
            }
        }
    }

    //TODO:WebSocket
    public static class SessionManager
    {
        static Dictionary<Guid, string> sessionList = new Dictionary<Guid, string>();

        public static Guid AddSession(string userName)
        {
            Guid userID = Guid.NewGuid();
            sessionList.Add(userID, userName);
            return userID;
        }

        public static void RemoveSession(string sessionName)
        {
            try
            {
                var user = sessionList.First(m => m.Value == sessionName);
                sessionList.Remove(user.Key);
            }
            catch (Exception ex) when (ex.GetType() != typeof(ArgumentException))
            {
                throw ex;
            }
        }
    }
}
