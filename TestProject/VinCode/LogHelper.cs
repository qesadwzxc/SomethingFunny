using System;
using System.IO;
using System.Text;
using System.Web;

namespace VinCode
{
    public static class LogHelper
    {
        private static readonly string _path = CreateFolder(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Log\\" + DateTime.Today.ToString("yyyyMM") + "\\");
        private static readonly string _file = _path + DateTime.Today.ToString("yyyyMMdd") + ".txt";

        public static string CreateFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }

        /// <summary>记录文本日志
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        /// <param name="flag"></param>
        public static void Write(string type, string message, string Operator)
        {
            try
            {
                using (StreamWriter streamWriter = new StreamWriter(_file, true, Encoding.UTF8))
                {
                    streamWriter.WriteLine("------------------begin------------------");
                    streamWriter.WriteLine("Time: " + DateTime.Now.ToString());
                    streamWriter.WriteLine("Type: " + type.Trim());
                    streamWriter.WriteLine("Operator：" + Operator);
                    streamWriter.WriteLine("Message: " + message.Trim());                   
                    streamWriter.Flush();
                    streamWriter.Close();
                }
            }
            catch
            {
            }
        }
        /// <summary>记录文本日志
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        public static void Write(string type, string message)
        {
            try
            {
                using (StreamWriter streamWriter = new StreamWriter(_file, true, Encoding.UTF8))
                {
                    streamWriter.WriteLine("------------------begin------------------");
                    streamWriter.WriteLine("Time: " + DateTime.Now.ToString());
                    streamWriter.WriteLine("Type: " + type.Trim());
                    streamWriter.WriteLine("Message: " + message.Trim());
                    streamWriter.Flush();
                    streamWriter.Close();
                }
            }
            catch
            {
            }
        }
    }
}
