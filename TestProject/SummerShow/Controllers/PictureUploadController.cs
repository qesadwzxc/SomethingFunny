using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SummerShow.Controllers
{
    public class PictureUploadController : Controller
    {
        //
        // GET: /PictureUpload/

        public ActionResult PictureUpload()
        {
            return View("PictureUploadView");
        }

    }
}
