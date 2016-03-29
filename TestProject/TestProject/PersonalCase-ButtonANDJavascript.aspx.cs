///////////////////////////////
///后台调用前台JS方法
///////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TestProject
{
    public partial class WebFormTest11 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //给button1添加客户端事件
                button1.Attributes.Add("OnClick", "return  jsFunction()");
                //jsFunction()是js函数
            }
        }

        protected void button1_Click(object sender, EventArgs e)
        {
            Response.Write("Congratulation!");
        }
    }
}