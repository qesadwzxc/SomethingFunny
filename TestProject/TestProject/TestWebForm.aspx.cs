using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TestProject
{
    public partial class TestWebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Con con = new Con((object)"aa");
            //string re = con.ToString(null);
            ////如果不实现IConvertible，则下面代码会报错
            //Convert.ChangeType(con, typeof(string));
            //Response.Write(re);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            IPAddress address = IPAddress.Parse(txtInput.Text);
            lblOutput.Text = Dns.GetHostEntry(address).HostName;
        }
    }

    #region 自定义类实现IConvertible接口
    public class Con : IConvertible
    {
        object o;

        public Con(object obj)
        {
            o = obj;
        }

        public string ToString(IFormatProvider provider)
        {
            return o.ToString() + "Hello";
        }

        public TypeCode GetTypeCode()
        {
            return TypeCode.Object;
        }
        public bool ToBoolean(IFormatProvider provider)
        {
            return false;
        }
        public byte ToByte(IFormatProvider provider)
        {
            return 0;
        }
        public char ToChar(IFormatProvider provider)
        {
            return '-';
        }
        public DateTime ToDateTime(IFormatProvider provider)
        {
            string time = "1900-1-1";
            return Convert.ToDateTime(time);
        }
        public decimal ToDecimal(IFormatProvider provider)
        {
            return 0;
        }
        public double ToDouble(IFormatProvider provider)
        {
            return 0;
        }
        public short ToInt16(IFormatProvider provider)
        {
            return 0;
        }
        public int ToInt32(IFormatProvider provider)
        {
            return 0;
        }
        public long ToInt64(IFormatProvider provider)
        {
            return 0;
        }
        public sbyte ToSByte(IFormatProvider provider)
        {
            return 0;
        }
        public float ToSingle(IFormatProvider provider)
        {
            return 0;
        }
        public object ToType(Type conversionType, IFormatProvider provider)
        {
            return null;
        }
        public ushort ToUInt16(IFormatProvider provider)
        {
            return 0;
        }
        public uint ToUInt32(IFormatProvider provider)
        {
            return 0;
        }
        public ulong ToUInt64(IFormatProvider provider)
        {
            return 0;
        }
    }
    #endregion
}