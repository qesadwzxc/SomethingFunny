using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication.Controllers
{
    public class AtController : Controller
    {
        //
        // GET: /At/
        #region At Function
        public ActionResult Index()
        {
            return View();
        }

        public string OnTextChange(string text)
        {
            string reMsg = string.Empty;
            if (text.Contains("@"))
            {
                if (text.LastIndexOf('@') == text.Length - 1)
                { reMsg = "你想@谁？"; }
                else
                { reMsg = "啊…接着做吧、"; }
            }
            return reMsg;
        }
        #endregion
    }
}
