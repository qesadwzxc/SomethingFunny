using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TestProject
{
    public partial class PersonalCase_Regex : System.Web.UI.Page
    {
        //正则类型选择枚举
        public enum RegexSelect
        {
            Email = 0,
            MobilePhone = 1,
            TelPhone = 2,
            QQNumber = 3,
            English = 4
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ddlRegexType.DataSource = null;
            ddlRegexType.DataSource = Enum.GetValues(typeof(RegexSelect));
            ddlRegexType.DataBind();
        }

        /// <summary>
        /// 正则判断
        /// </summary>
        /// <param name="strTel">要验证的字符串</param>
        /// <param name="select">正则类型</param>
        /// <returns></returns>
        public static bool RegexValid(string strTel, int select)
        {
            List<string> strPatern = new List<string>();
            //邮箱正则
            strPatern.Add(@"(^(\w)+(\.\w+)*@(\w)+((\.\w+)+)$)");
            //手机正则
            strPatern.Add(@"(^1[3-8]\d{9}$)");
            //电话正则
            strPatern.Add(@"^[0-9]{6,20}$");
            //QQ正则
            strPatern.Add(@"(^[1-9][0-9]{4,10}$)");
            //2~50位英文正则
            strPatern.Add(@"^[A-Za-z]{2,50}$");
            Regex reg = new Regex(strPatern[select]);
            return reg.IsMatch(strTel);
        }

        protected void btnRegex_Click(object sender, EventArgs e)
        {
            string regexType = ddlRegexType.SelectedValue;
            int type = (int)Enum.Parse(typeof(RegexSelect), regexType);
            lblRegex.Text = RegexValid(txtRegex.Text, type) ? "正确" : "错误";
        }
    }
}