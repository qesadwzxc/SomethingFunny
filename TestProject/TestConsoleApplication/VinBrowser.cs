using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestConsoleApplication
{
    public partial class VinBrowser : Form
    {
        public VinBrowser()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            textBox1.Text = webBrowser1.StatusText;
            if (webBrowser1.StatusText == "完成")
            {

                timer1.Enabled = false;
                //页面加载完成,做一些其它的事
                textBox1.Text = webBrowser1.Document.Body.OuterHtml;
                //webBrowser1.DocumentText 注意不要用这个，这个和查看源文件一样的
            }
        }

        private void VinBrowser_Load(object sender, EventArgs e)
        {
            string Url = "http://www.baidu.com";
            webBrowser1.Navigate(Url);
        }

        //private readonly Dictionary<int, string> AutoCompleteString()
        //{
        //    Dictionary<int, string> dic = new Dictionary<int, string>();
        //    dic.Add(0,"int");
        //    return dic;
        //}
    }
}
