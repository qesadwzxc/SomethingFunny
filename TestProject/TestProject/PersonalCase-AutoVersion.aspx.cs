////////////////////////////////////
///自动生成版本号及版本号获取实例
////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;

namespace TestProject
{
    public partial class PersonalCase_AutoVersion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            /*默认版本号1.0.0.0
              需把AssemblyInfo.cs文件中的[assembly:AssemblyVersion("1.0.0.0")]
              改成[assembly:AssemblyVersion("1.0.*")]
              另外还需要把[assembly:AssemblyFileVersion("1.0.0.0")]注释屏蔽掉*/

            //自动生成的第一位（1.0.【*】.*），即内部修订版本号（第三个字段）是2000年1月1日到编译日期的天数
            DateTime d1 = DateTime.Parse("2000-01-01");
            DateTime d2 = DateTime.Today;
            TimeSpan d3 = d2.Subtract(d1);

            //自动生成的第二位（1.0.*.【*】），即内部修订号（第四个字段）是当天从0点到当前时间的秒数/2
            DateTime d4 = DateTime.Today;
            DateTime d5 = DateTime.Now;
            TimeSpan d6 = d5 - d4;

            //通过反射获取当前版本号
            lblOutput.Text = (d3.TotalDays.ToString() + "</br>" + Math.Floor(d6.TotalSeconds / 2).ToString() + "</br>" + Assembly.GetExecutingAssembly().GetName().Version.ToString());
        }
    }
}