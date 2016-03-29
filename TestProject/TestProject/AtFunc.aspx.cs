using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TestProject
{
    public partial class AtFunc : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public string OnTextChange(string text)
        {
            string reMsg = string.Empty;
            if (text.Contains("@"))
            {
                if (text.LastIndexOf('@') == text.Length - 1)
                { reMsg = "你想@谁？"; }
                reMsg = "en";
            }
            return reMsg;
        }
    }
}