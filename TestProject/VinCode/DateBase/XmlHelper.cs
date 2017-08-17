using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml;

namespace VinCode.DateBase
{
    public class XmlHelper
    {
        public static DataTable GetDataTable(string xmlString, string rootPath = "//DataTable/Rows")
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlString);

            XmlNodeList xlist = doc.SelectNodes(rootPath);
            DataTable dt = new DataTable();
            DataRow dr;

            for (int i = 0; i < xlist.Count; i++)
            {
                dr = dt.NewRow();
                XmlElement xe = (XmlElement)xlist.Item(i);
                for (int j = 0; j < xe.Attributes.Count; j++)
                {
                    if (!dt.Columns.Contains("@" + xe.Attributes[j].Name))
                        dt.Columns.Add("@" + xe.Attributes[j].Name);
                    dr["@" + xe.Attributes[j].Name] = xe.Attributes[j].Value;
                }
                for (int j = 0; j < xe.ChildNodes.Count; j++)
                {
                    if (!dt.Columns.Contains(xe.ChildNodes.Item(j).Name))
                        dt.Columns.Add(xe.ChildNodes.Item(j).Name);
                    dr[xe.ChildNodes.Item(j).Name] = xe.ChildNodes.Item(j).InnerText;
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
    }
}
