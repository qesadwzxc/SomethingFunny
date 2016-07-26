using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using Newtonsoft.Json;
using System.Threading;
using System.Configuration;
using System.Windows.Forms;
namespace WeeklyReportService
{
    public partial class WeeklyReport : ServiceBase
    {
        public const string URL = "http://japi.juhe.cn/calendar/day?date={0}&key=fa2d408d773c352d1ef7a194d68f9172";
        public const string REPORTURL = "http://home.tcent.cn/TCWorkSummary/PublishWorkSummary.aspx";
        public WeeklyReport()
        {
            InitializeComponent();
        }

        public void DebugOnStart()
        {
            this.OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
#if DEBUG
            Report(null, null);
#else
            System.Timers.Timer t = new System.Timers.Timer();
            t.Interval = 60 * 60 * 1000;
            t.Elapsed += new System.Timers.ElapsedEventHandler(Report);
            t.AutoReset = true;
            t.Enabled = true;
#endif
        }

        protected override void OnStop()
        {
        }

        protected internal void Report(object source, System.Timers.ElapsedEventArgs e)
        {
            if (IsWriteReport())
            {
                WriteReport();
            }
        }

        protected internal bool IsWriteReport()
        {
            var date = GetDateDetail(DateTime.Now);
            if (date != null)
            {
                //下午两点以后再填写
                if (DateTime.Now.Hour > 14)
                {
                    //先判断是否节假日
                    if (string.IsNullOrEmpty(date.Holiday))
                    {
                        //周一周二和周末暂时忽略
                        if (date.Weekday == "星期六" || date.Weekday == "星期日" || date.Weekday == "星期一" || date.Weekday == "星期二")
                        {

                        }
                        //周三周四如果明天是假期则填写
                        else if (date.Weekday == "星期三" || date.Weekday == "星期四")
                        {
                            var deteNext = GetDateDetail(DateTime.Now.AddDays(1));
                            if (!string.IsNullOrEmpty(deteNext.Holiday))
                            {
                                WriteReport();
                            }

                        }
                        //周五如果前天不是假期则填写
                        else
                        {
                            var deteBeforeYesterday = GetDateDetail(DateTime.Now.AddDays(-2));
                            if (string.IsNullOrEmpty(deteBeforeYesterday.Holiday))
                            {
                                WriteReport();
                            }
                        }
                    }
                }
            }
            return false;
        }

        protected internal Data GetDateDetail(DateTime date)
        {
            string url = string.Format(URL, date.ToString("yyyy-M-d"));
            var re = JsonConvert.DeserializeObject<DateModel>(Submit(url, null, "GET"));
            if (re != null && re.Result != null && re.Result.Data != null)
            {
                return re.Result.Data;
            }
            return null;
        }

        /// <summary>
        /// 写周报~
        /// </summary>
        protected internal void WriteReport()
        {
            string body = string.Format($"mod=OnSubmitWork&Title={GetAppConfig("title")}&Content=<p>{GetAppConfig("content")}</p>&WorkType={GetAppConfig("worktype")}&IsTop=0&Tag=0&BUType=0");
            var response = Submit(REPORTURL, body, "POST");
            if (response.IndexOf("发表成功") > 0)
            {
                MessageBox.Show("本周周报占坑成功~请尽早去完成周报哦。", "OK", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show("周报没发布成功，我也不知道为啥…", "Oh", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }
        }

        /// <summary>创建请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="requestParameter">POST请求参数</param>
        /// <param name="method">GET/POST</param>
        /// <param name="timeout">超时时间(单位：毫秒，默认10秒)</param>
        /// <returns></returns>
        private static string Submit(string url, string requestParameter, string method, int timeout = 10000)
        {
            string result = string.Empty;
            try
            {
                HttpWebRequest request = null;
                request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = method;
                request.Timeout = timeout;

                switch (method)
                {
                    case "GET":
                        break;
                    case "POST":
                        {
                            byte[] data = Encoding.UTF8.GetBytes(requestParameter);//(JsonConvert.SerializeObject(requestParameter));
                            request.ContentType = " application/x-www-form-urlencoded; charset=UTF-8";
                            request.ContentLength = data.Length;
                            request.Host = "home.tcent.cn";
                            request.CookieContainer = new CookieContainer();
                            request.CookieContainer.Add(new Cookie("bbscode", GetAppConfig("bbscode"), "/", ".tcent.cn"));
                            request.Referer = "http://home.tcent.cn/TCWorkSummary/PublishWorkSummary.aspx";
                            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/44.0.2403.155 Safari/537.36";
                            request.KeepAlive = true;
                            request.ProtocolVersion = HttpVersion.Version10;
                            //request.Proxy = new WebProxy("http://localhost:8888");
                            Stream streamOut = request.GetRequestStream();
                            streamOut.Write(data, 0, data.Length);
                            streamOut.Close();
                        }
                        break;
                }

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream streamIn = response.GetResponseStream();
                StreamReader reader = new StreamReader(streamIn);
                result = reader.ReadToEnd();
                reader.Close();
                streamIn.Close();
                response.Close();
                return result;
            }
            catch (Exception ex)
            {
                var e = ex as WebException;
                var st = e.Status;
                throw ex;
            }
        }

        /// <summary>
        /// 读取配置文件
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        private static string GetAppConfig(string strKey)
        {
            foreach (string key in ConfigurationManager.AppSettings)
            {
                if (key == strKey)
                {
                    return ConfigurationManager.AppSettings[strKey];
                }
            }
            return null;
        }
    }
}
