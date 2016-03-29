using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;

namespace TestProject
{
    public partial class WebFormTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string dir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            Response.Write(dir);
        }

        [WebMethod]
        public static int Submit(string aa)
        {
            int bb = 1;
            return bb;
        }

        [WebMethod]
        public static object Submit(CardApplyViewModel model)
        {
            return new { id = 1, success = true };
        }
    }

    public class CardApplyViewModel
    {
        public string Name { get; set; }
        public string EngName { get; set; }
        public string Telephone { get; set; }
        public string Array { get; set; }
    }
}