using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication.BLL
{
    public static class HasFile
    {
        public static bool isFile(this HttpPostedFileBase file)
        {
            return (file != null && file.ContentLength > 0) ? true : false;
        }
    }
}