using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication.Attribute;

namespace MvcApplication.Controllers
{
    public class MutiButtonController : Controller
    {
        public ActionResult List()
        {
            return View();
        }

        [HttpPost]
        public ActionResult List(string txtRole)
        {
            var keys = Request.Form.AllKeys;
            return Content(txtRole);
        }

        [HttpPost]
        [MutiButton("action1")]
        [ActionName("List")]
        public ActionResult List1()
        {
            return Content("action1");
        }

        [HttpPost]
        [MutiButton("action2")]
        [ActionName("List")]
        public ActionResult List2()
        {
            return Content("action2");
        }

        [HttpPost]
        [MutiButton("action3")]
        [ActionName("List")]
        public ActionResult List3()
        {
            return Content("action3");
        }


        public ActionResult Item()
        {
            return View();
        }

        [HttpPost]
        [MutiButtonV2(Name="action",Argument="id")]
        public ActionResult Item(string id,string txtUser)
        {
            return Content(id + txtUser);
        }
    }
}
