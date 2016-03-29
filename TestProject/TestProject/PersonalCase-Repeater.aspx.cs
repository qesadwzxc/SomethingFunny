///////////////////////////////
///Repeater控件实例应用
///////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace TestProject
{
    public partial class TestWebForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable dtTest = new DataTable();
                DataColumn dcAdd1 = new DataColumn("MemberEmpName");
                DataColumn dcAdd2 = new DataColumn("MemberDeptName");
                DataColumn dcAdd3 = new DataColumn("rowNumber");
                DataColumn dcAdd4 = new DataColumn("ReplyTime");
                DataColumn dcAdd5 = new DataColumn("ReplyContent");
                DataColumn dcAdd6 = new DataColumn("ReplyID");
                dtTest.Columns.Add(dcAdd1);
                dtTest.Columns.Add(dcAdd2);
                dtTest.Columns.Add(dcAdd3);
                dtTest.Columns.Add(dcAdd4);
                dtTest.Columns.Add(dcAdd5);
                dtTest.Columns.Add(dcAdd6);

                for (int i = 0; i < 5; i++)
                {
                    dtTest.Rows.Add("Vincent", 1, i, DateTime.Now.Date, "Test", "Lee");
                }
                rptReply.DataSource = dtTest;
                rptReply.DataBind();
            }

        }

        protected void rptReply_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Insert")
            {
                string id = e.CommandArgument.ToString();
                string str = ((TextBox)e.Item.FindControl("TextBox3")).Text;
                Response.Write(str + "," + id);
                //插入数据库
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string txtBox = string.Empty;
            //foreach (RepeaterItem prtItem in this.rptReply.Items)
            //{
            //    if (!string.IsNullOrEmpty(Request.Form["txtReply"]))
            //    {
            //        txtBox = Request.Form["txtReply"];
            //    }
            //}
            txtBox = Request.Form["txtReply"];
            Response.Write("<p>" + txtBox + "</p>");
        }
    }
}