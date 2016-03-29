using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApi.Business;

namespace MvcApi.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            string result = ApiHelper.Get<string>("http://localhost:5597/api/user");

            return View();
        }
    }
}
