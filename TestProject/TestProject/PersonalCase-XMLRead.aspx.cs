using System;
using System.IO;
using System.Text;
using System.Xml;

namespace TestProject
{
    public partial class TestWebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //从给定位置载入xml文档
            string path = @"C:\Users\lmw12960\Documents\Visual Studio 2013\Projects\TestProject\TestProject\testXML.xml";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);

            //打开一个输入流用于读取xml
            StreamReader reader = new StreamReader(path);
            Stream sm = reader.BaseStream;

            //可以如上面一样以流的方式读取xml，也可以如更上面一样直接载入xml文档
            XmlTextReader xmlReader = new XmlTextReader(sm);
            
            //忽略空格
            xmlReader.WhitespaceHandling = WhitespaceHandling.None;

            StringBuilder sb = new StringBuilder();

            //由于是Web所以都转义了，一般直接写就行
            while (xmlReader.Read())
            {
                for (int i = 0; i < xmlReader.Depth; i++)
                {
                    sb.Append(@"&emsp;");
                }
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element: sb.AppendFormat(@"&lt;{0}&gt;<br/>", xmlReader.Name); break;
                    case XmlNodeType.EndElement: sb.AppendFormat(@"&lt;/{0}&gt;<br/>", xmlReader.Name); break;
                    case XmlNodeType.Text: sb.AppendFormat(@"{0}<br/>", xmlReader.Value); break;
                    case XmlNodeType.Comment: sb.AppendFormat(@"&lt;!--{0}--&gt;<br/>", xmlReader.Value); break;
                    case XmlNodeType.XmlDeclaration: break;
                    case XmlNodeType.DocumentType: break;
                    default: break;
                }
            }

            if (xmlReader != null)
            {
                xmlReader.Close();
                reader.Close();
                sm.Close();
            }

            Response.Write(sb.ToString());
        }
    }
}