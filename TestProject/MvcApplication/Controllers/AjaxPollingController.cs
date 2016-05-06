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
}
