using System;
using System.IO;
using System.Text;
using System.Xml;

namespace TestProject
{
    public partial class TestWebForm1 : System.Web.UI.Page
    {
        private static string path = @"C:\Users\lmw12960\Documents\Visual Studio 2013\Projects\TestProject\TestProject\testXML.xml";

        protected void Page_Load(object sender, EventArgs e)
        {
            //从给定位置载入xml文档
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

        static void Update(XmlDocument doc)
        {
            //查找<users>
            XmlNode root = doc.SelectSingleNode("UserList");
            //获取到所有<users>的子节点
            XmlNodeList nodeList = root.ChildNodes;
            //遍历所有子节点
            foreach (XmlNode xn in nodeList)
            {
                XmlElement xe = (XmlElement)xn;
                XmlNodeList subList = xe.ChildNodes;
                foreach (XmlNode xmlNode in subList)
                {
                    if ("Name".Equals(xmlNode.Name))
                    {

                        if (xmlNode.InnerText.Equals("张三"))
                        {
                            xmlNode.InnerText = "mao";
                            break;
                        }
                    }
                }
            }
            doc.Save(path);//保存。  
        }

        static void Delete(XmlDocument doc)
        {
            //查找<users>
            XmlNode root = doc.SelectSingleNode("UserList");
            //获取到所有<users>的子节点
            XmlNodeList nodeList = root.ChildNodes;
            //遍历所有子节点
            foreach (XmlNode xn in nodeList)
            {
                XmlElement xe = (XmlElement)xn;
                XmlNodeList subList = xe.ChildNodes;
                foreach (XmlNode xmlNode in subList)
                {
                    if ("WorkExperience".Equals(xmlNode.Name))
                    {
                        xmlNode.RemoveAll();
                    }
                }
            }

            doc.Save(path);//保存。  
        }

        static void WriteXml()
        {
            //创建一个XML对象
            XmlDocument xmlDoc = new XmlDocument();
            //创建一个根节点
            XmlElement xmlRoot = xmlDoc.CreateElement("richtextbox");
            //添加根节点
            xmlDoc.AppendChild(xmlRoot);
            //创建一个子节点
            XmlElement bgcolor = xmlDoc.CreateElement("bgcolor");
            bgcolor.InnerText = "red";//子节点的值
            xmlRoot.AppendChild(bgcolor);//添加到根节点中

            XmlElement font = xmlDoc.CreateElement("font");
            font.InnerText = "宋体";
            xmlRoot.AppendChild(font);
            xmlDoc.Save(path);

            //读
            //创建一个XML对象
            XmlDocument myxml = new XmlDocument();
            // 读取已经有的xml
            myxml.Load(path);
            //声明一个节点存储根节点
            XmlNode movie = myxml.DocumentElement;
            //遍历根节点下的子节点
            foreach (XmlNode var in movie.ChildNodes)
            {
                Console.Write(var.Name + ": ");//获取根节点的名称
                Console.Write(var.InnerText);//获取根节点的值！
                Console.WriteLine();
            }
            Console.Write("\r\nPress any key to continue....");
            Console.Read();
        }

        static void Print(XmlDocument doc)
        {
            XmlNode root = doc.SelectSingleNode("UserList");
            XmlNodeList nodeList = root.ChildNodes;
            foreach (XmlNode item in nodeList)
            {
                XmlElement xe = (XmlElement)item;
                XmlNodeList subList = xe.ChildNodes;
                foreach (XmlNode xmlNode in subList)
                {
                    if ("Name".Equals(xmlNode.Name))
                    {
                        Console.WriteLine("姓名：" + xmlNode.InnerText);
                    }
                    else if ("Age".Equals(xmlNode.Name))
                    {
                        Console.WriteLine("年龄：" + xmlNode.InnerText);
                    }
                    else if ("Birthday".Equals(xmlNode.Name))
                    {
                        Console.WriteLine("生日：" + xmlNode.InnerText);
                    }
                    else if ("Education".Equals(xmlNode.Name))
                    {
                        Console.WriteLine("学历：" + xmlNode.InnerText);
                    }
                    else if ("Address".Equals(xmlNode.Name))
                    {
                        Console.WriteLine("住址：" + xmlNode.InnerText);
                    }
                    else //if ("WorkExperience".Equals(xmlNode.Name))
                    {
                        XmlNode workList = doc.SelectSingleNode("WorkExperience");
                        //获取到所有<users>的子节点
                        XmlNodeList infoList = root.ChildNodes;
                        foreach (XmlNode info in infoList)
                        {
                            XmlElement xe1 = (XmlElement)info;
                            XmlNodeList subList1 = xe1.ChildNodes;

                            foreach (XmlNode info1 in subList1)
                            {
                                if ("StartTime".Equals(info1.Name))
                                {
                                    Console.WriteLine("入职：" + info1.InnerText);
                                }
                                else if ("EndTime".Equals(info1.Name))
                                {
                                    Console.WriteLine("离职：" + info1.InnerText);
                                }
                                else if ("Company".Equals(info1.Name))
                                {
                                    Console.WriteLine("公司：" + info1.InnerText);
                                }
                                else if ("Position".Equals(info1.Name))
                                {
                                    Console.WriteLine("职称：" + info1.InnerText);
                                }
                            }
                        }
                    }
                }
                Console.WriteLine();
            }
            Console.Write("\r\nPress any key to continue....");
            Console.Read();
        }
    }

    public class BookModel
    {
        public BookModel()
        { }
        /// <summary>
        /// 所对应的课程类型
        /// </summary>
        private string bookType;
        public string BookType
        {
            get { return bookType; }
            set { bookType = value; }
        }

        /// <summary>
        /// 书所对应的ISBN号
        /// </summary>
        private string bookISBN;
        public string BookISBN
        {
            get { return bookISBN; }
            set { bookISBN = value; }
        }

        /// <summary>
        /// 书名
        /// </summary>
        private string bookName;
        public string BookName
        {
            get { return bookName; }
            set { bookName = value; }
        }

        /// <summary>
        /// 作者
        /// </summary>
        private string bookAuthor;
        public string BookAuthor
        {
            get { return bookAuthor; }
            set { bookAuthor = value; }
        }

        /// <summary>
        /// 价格
        /// </summary>
        private double bookPrice;
        public double BookPrice
        {
            get { return bookPrice; }
            set { bookPrice = value; }
        }
    }
}