using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace VinCode.Web
{
    public static class WebRequestHelper
    {
        /// <summary>创建Get请求
        /// </summary>
        /// <param name="url">请求地址及参数</param>
        /// <param name="timeout">超时时间(单位：毫秒，默认10秒)</param>
        /// <returns></returns>
        public static T Get<T>(string url, int timeout = 10000)
        {
            return Submit<T>(url, null, "GET", timeout);
        }

        /// <summary>创建Post请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="requestParameter">请求参数</param>
        /// <param name="timeout">超时时间(单位：毫秒，默认10秒)</param>
        /// <returns></returns>
        public static T Post<T>(string url, object requestParameter, int timeout = 10000)
        {
            return Submit<T>(url, requestParameter, "POST", timeout);
        }

        /// <summary>创建请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="requestParameter">POST请求参数</param>
        /// <param name="method">GET/POST</param>
        /// <param name="timeout">超时时间(单位：毫秒，默认10秒)</param>
        /// <returns></returns>
        private static T Submit<T>(string url, object requestParameter, string method, int timeout = 10000)
        {
            string result = "";

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
                            byte[] data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(requestParameter));
                            request.ContentType = "application/json;charset=utf-8";
                            request.ContentLength = data.Length;

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
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (result.Length > 0)
            {
                return JsonConvert.DeserializeObject<T>(result);
            }
            else
            {
                return default(T);
            }
        }
    }
}