using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using VinCode;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace TestConsoleApplication
{
    public class SendMobileMessage
    {
        Random random = new Random();
        private string strjson;
        //默认为86
        public string nationcode = "86";
        //随机数
        private string strRand
        {
            get { return random.Next(10000, 99999).ToString(); }
        }
        //得到签名模板 这是默认 
        private int tpl_id = 279204;
        //默认appkey
        private string strAppKey = "249eee5a8e57b4c46233c2452318dee8";
        //加密sig
        private string sig;
        //时间
        private long strTime
        {
            get { return (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000; }
        }
        //sdkappid
        private int sdkappid = 1400172942;

        private const string TENCENT_MESSAGE_HOST = "https://yun.tim.qq.com/v5";
        //请求路径
        private string url;

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="Mobile"></param>
        /// <returns></returns>
        public string SendMessage(string mobile)
        {
            string r = this.SetRandomUrl(InterfaceType.SendSms);

            //得到加密sig
            this.sig = GetSHA256HashFromString(string.Format("appkey={0}&random={1}&time={2}&mobile={3}", this.strAppKey, r, this.strTime, mobile));
            //拼接json
            //this.strjson = JsonConvert.SerializeObject(new { ext = "", extent = "", @params = new List<string>() { strRand, "3" }, sig, tel = new { mobile, nationcode }, time = strTime, tpl_id });
            this.strjson = JsonConvert.SerializeObject(new { ext = "", extent = "", @params = new List<string>() { "HJL", "1", "2", mobile }, sig, tel = new { mobile, nationcode }, time = strTime, tpl_id = "287596" });

            string result = this.httpWebResponse1(this.url, this.strjson);
            //返回JSON
            return result;
        }

        /// <summary>
        /// 发送数据统计
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public string PullSendStatus(DateTime startTime, DateTime endTime)
        {
            string r = this.SetRandomUrl(InterfaceType.PullSendStatus);

            //得到加密sig
            this.sig = GetSHA256HashFromString(string.Format("appkey={0}&random={1}&time={2}", this.strAppKey, r, this.strTime));
            //拼接json
            this.strjson = JsonConvert.SerializeObject(new { begin_date = startTime.ToString("yyyyMMddHH"), end_date = endTime.ToString("yyyyMMddHH"), sig, time = this.strTime });

            string result = this.httpWebResponse1(this.url, this.strjson);
            //返回JSON
            return result;
        }

        /// <summary>
        /// 查询短信套餐包信息
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public string GetSmsPackages(int offset = 0, int length = 10)
        {
            string r = this.SetRandomUrl(InterfaceType.GetSmsPackages);

            //得到加密sig
            this.sig = GetSHA256HashFromString(string.Format("appkey={0}&random={1}&time={2}", this.strAppKey, r, this.strTime));
            //拼接json
            this.strjson = JsonConvert.SerializeObject(new { offset, length, sig, time = this.strTime });

            string result = this.httpWebResponse1(this.url, this.strjson);
            //返回JSON
            return result;
        }

        /// <summary>
        /// 获取模板信息
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public string GetTemplate(string[] templateId = null)
        {
            string r = this.SetRandomUrl(InterfaceType.GetTemplate);

            //得到加密sig
            this.sig = GetSHA256HashFromString(string.Format("appkey={0}&random={1}&time={2}", this.strAppKey, r, this.strTime));
            //拼接json
            this.strjson = JsonConvert.SerializeObject(new { tpl_page = new { max = 20, offset = 0 }, sig, time = this.strTime });
            //得到当前路径

            string result = this.httpWebResponse1(this.url, this.strjson);
            //返回JSON
            return result;
        }

        /// <summary>
        /// 拼接url
        /// </summary>
        /// <param name="type"></param>
        private string SetRandomUrl(InterfaceType type)
        {
            string interfaceName = string.Empty;
            switch (type)
            {
                case InterfaceType.SendSms:
                    interfaceName = "/tlssmssvr/sendsms?";
                    break;
                case InterfaceType.GetSmsPackages:
                    interfaceName = "/tlssmssvr/getsmspackages?";
                    break;
                case InterfaceType.PullSendStatus:
                    interfaceName = "/tlssmssvr/pullsendstatus?";
                    break;
                case InterfaceType.GetTemplate:
                    interfaceName = "/tlssmssvr/get_template?";
                    break;
                default: break;
            }
            string random = strRand;
            this.url = TENCENT_MESSAGE_HOST + interfaceName + "sdkappid=" + sdkappid + "&random=" + random;
            return random;
        }

        /// <summary>
        /// 
        /// sha256算法
        /// </summary>
        /// <param name="strData">传入需要加密的string</param>
        /// <returns>加密后的码</returns>
        private static string GetSHA256HashFromString(string strData)
        {
            byte[] bytValue = Encoding.UTF8.GetBytes(strData);
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
        /// 发送请求
        /// </summary>
        /// <param name="url">路径</param>
        /// <param name="json">发送json</param>
        /// <returns></returns>
        private string httpWebResponse1(string url, string json)
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
            LogHelper.Write("sms", returnstr);
            return returnstr;

        }

        private enum InterfaceType
        {
            SendSms = 1,
            GetSmsPackages = 2,
            PullSendStatus = 3,
            GetTemplate = 4
        }
    }
}
