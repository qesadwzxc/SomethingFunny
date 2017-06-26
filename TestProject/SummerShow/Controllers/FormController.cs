using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SummerShow.Controllers
{
    public class FormController : Controller
    {
        //
        // GET: /Form/

        public ActionResult Index()
        {
            return View();
        }

        //画布
        public ActionResult CanvasShow()
        {
            return View();
        }

        //css3动画效果
        public ActionResult Animation()
        {
            return View("AnimationView");
        }

        public ActionResult ThreeJS()
        {
            return View();
        }

        /// <summary>
        /// 响应式布局
        /// </summary>
        /// <returns></returns>
        public ActionResult MediasLayout()
        {
            return View();
        }
    }
}
