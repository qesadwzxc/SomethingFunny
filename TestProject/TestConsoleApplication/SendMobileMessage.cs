using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace TestConsoleApplication
{
    public class SendMobileMessage
    {
        //手机号
        public string strMobile;
        private string strjson;
        //默认为86
        public string nationcode = "86";
        //随机数
        private string strRand;
        //得到签名模板 这是默认 
        private int tpl_id = 279204;
        //默认appkey
        private string strAppKey = "249eee5a8e57b4c46233c2452318dee8";
        //加密sig
        private string sig;
        //时间
        private long strTime;
        //sdkappid
        private int sdkappid = 1400172942;
        //请求路径
        private string url = "https://yun.tim.qq.com/v5/tlssmssvr/sendsms?";
        public SendMobileMessage(string Mobile)
        {
            this.strMobile = Mobile;
            //初始化随机数
            this.GetRandomIntance();
            //初始化时间
            this.GetCurrentTimeUnix();
            //得到当前路径
            this.geturl();

            //得到加密sig
            this.sig = GetSHA256HashFromString(string.Format("appkey={0}&random={1}&time={2}&mobile={3}", this.strAppKey, this.strRand, this.strTime, this.strMobile));
            //拼接json
            this.strjson = "{\"ext\":\"\",\"extent\":\"\",\"params\":[\"" + strRand + "\",\"3\"" +
          "],\"sig\":\"" + this.sig + "\",\"tel\":{\"mobile\":\"" + this.strMobile + "\",\"nationcode\":\"" + this.nationcode + "\"},\"time\":\"" + this.strTime + "\",\"tpl_id\":\"" + this.tpl_id + "\"}";


        }

        public string sendMessage()
        {
            string result = this.httpWebResponse1(this.url, this.strjson);
            //返回JSON
            return result;

        }
        //拼接url
        private void geturl()
        {
            this.url = this.url + "sdkappid=" + sdkappid + "&random=" + strRand;
        }
        //得到随机数
        private void GetRandomIntance()
        {
            this.strRand = new Random().Next(10000, 99999).ToString();
        }

        /// <summary>
        /// 
        /// sha256算法
        /// </summary>
        /// <param name="strData">传入需要加密的string</param>
        /// <returns>加密后的码</returns>
        private static string GetSHA256HashFromString(string strData)
        {
            byte[] bytValue = System.Text.Encoding.UTF8.GetBytes(strData);
            try
            {
                SHA256 sha256 = new SHA256CryptoServiceProvider();
                byte[] retVal = sha256.ComputeHash(bytValue);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetSHA256HashFromString() fail,error:" + ex.Message);
            }
        }
        /// <summary>
        /// 得到当前时间的timeunix
        /// </summary>
        /// <returns>一个timeunix</returns>
        public void GetCurrentTimeUnix()
        {
            TimeSpan cha = (DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)));
            strTime = (long)cha.TotalSeconds;
        }

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="url">路径</param>
        /// <param name="json">发送json</param>
        /// <returns></returns>
        public string httpWebResponse1(string url, string json)
        {
            string returnstr = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            byte[] data = Encoding.UTF8.GetBytes(json);
            request.ContentLength = data.Length;
            using (Stream reqStream = request.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }
            HttpWebResponse respose = (HttpWebResponse)request.GetResponse();
            Stream stream = respose.GetResponseStream();
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                returnstr = reader.ReadToEnd();
            }
            return returnstr;

        }
    }
}
