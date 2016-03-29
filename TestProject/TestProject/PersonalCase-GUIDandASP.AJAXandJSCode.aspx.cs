///////////////////////////////
///调用GUID方法生成GUID
///前台使用AJAX技术刷新
///////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TestProject
{
    public partial class WebFormTest13 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            if (System.DateTime.Now.Second % 10 == 0)
            {
                lblTest.Text = Guid.NewGuid().ToString();
            }
        }
    }
}