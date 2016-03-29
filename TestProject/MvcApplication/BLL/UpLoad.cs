using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text; 
using System.Net; 
using System.IO;
using iThinking.Helper.Web;
using System.Net.Sockets;

namespace MvcApplication.BLL
{
    public class UpLoad
    {
        private static string UPLOAD_URL = "10.1.25.57";  //FTP服务器IP地址
        private static int UPLOAD_PORT = 21;             //FTP服务器端口
        private static string UPLOAD_USERNAME = "LMW12960";      //登陆用户名
        private static string UPLOAD_PASSWORD = "asdfghjkl;123";      //登陆密码

        private  string msg = string.Empty;
        private  byte[] buffer = new byte[512];
        private  Socket socketControl;
        private  Encoding encoding = Encoding.Default;
        private  string strReply;
        private  int iReplyCode;

        public string FTPUpLoad(string path, string fileName)
        {
            //socketControl = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(UPLOAD_URL), UPLOAD_PORT);

            //socketControl.Connect(remoteEP);

            //this.ReadReply();
            //if (this.iReplyCode != 220)
            //{
            //    throw new IOException(this.strReply.Substring(4));
            //}
            //this.SendCommand("USER " + UPLOAD_USERNAME);
            //if (this.iReplyCode != 331 && this.iReplyCode != 230)
            //{
            //    throw new IOException(this.strReply.Substring(4));
            //}
            //if (this.iReplyCode != 230)
            //{
            //    this.SendCommand("PASS " + UPLOAD_PASSWORD);
            //    if (this.iReplyCode != 230 && this.iReplyCode != 202)
            //    {
            //        throw new IOException(this.strReply.Substring(4));
            //    }
            //}

            var ftpHelp = new FtpHelper();
            ftpHelp.RemoteHost = UPLOAD_URL;
            ftpHelp.RemotePassword = UPLOAD_PASSWORD;
            ftpHelp.RemotePath = path;
            ftpHelp.RemotePort = UPLOAD_PORT;
            ftpHelp.RemoteUser = UPLOAD_USERNAME;
            try
            {
                ftpHelp.Connect();
                if (ftpHelp.IsConnected)
                {
                    return ftpHelp.UpLoadFile(fileName) ? "上传成功！" : "上传失败，请检查文件是否存在。";
                }
            }
            catch (Exception ex)
            {
                //TODO：Lee-记录连接异常
                return ex.Message;
            }
            finally
            {
                ftpHelp.DisConnect();
            }
            return "连接服务器失败，请检查您的网络配置";
        }

        public string FTPDownLoad(string strRemoteFileName)
        {
            string returnMessage = string.Empty;
            string strFolder = @"D:\";
            string strLocalFileName = string.Empty;
            var ftpHelp = new FtpHelper();
            ftpHelp.RemoteHost = UPLOAD_URL;
            ftpHelp.RemoteUser = UPLOAD_USERNAME;
            ftpHelp.RemotePassword = UPLOAD_PASSWORD;
            ftpHelp.RemotePath = @"\TCOATask\1a2138f3-2c20-4d8a-912d-d8eade395c16\";
            try 
            { 
                ftpHelp.DownLoadFile(strRemoteFileName, strFolder, strLocalFileName);
                returnMessage = "下载成功";
            }
            catch(Exception ex) 
            {
                throw ex;
                //returnMessage = "下载失败"; 
            }
            finally { ftpHelp.DisConnect(); }
            
            return returnMessage;
        }

        /// <summary>
        /// FTP上传
        /// </summary>
        /// <param name="fileName"></param>
        //public static void FTPUpLoad(string fileName)
        //{
        //    //构造一个web服务器的请求对象 
        //    FtpWebRequest ftp;
        //    //实例化一个文件对象 
        //    FileInfo f = new FileInfo(fileName);
        //    ftp = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://10.1.25.57/" + f.Name));
        //    //创建用户名和密码 
        //    ftp.Credentials = new NetworkCredential("LMW12960", "wal1350531364*/*");
        //    ftp.KeepAlive = false;
        //    ftp.Method = WebRequestMethods.Ftp.UploadFile;
        //    ftp.UseBinary = true;
        //    ftp.ContentLength = f.Length;
        //    int buffLength = 20480;
        //    byte[] buff = new byte[buffLength];
        //    int contentLen;
        //    try
        //    {
        //        //获得请求对象的输入流 
        //        FileStream fs = f.OpenRead();
        //        Stream sw = ftp.GetRequestStream();
        //        contentLen = fs.Read(buff, 0, buffLength);
        //        while (contentLen != 0)
        //        {
        //            sw.Write(buff, 0, contentLen);
        //            contentLen = fs.Read(buff, 0, buffLength);
        //        }
        //        sw.Close();
        //        fs.Close();
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //    }
        //}

        /// <summary>
        /// FTP下载
        /// </summary>
        //public static void fload() 
        //{ 
        //FtpWebRequest ftp; 
        //ftp = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://10.1.25.57/" + f.Name)); 
        ////指定用户名和密码 
        //ftp.Credentials = new NetworkCredential("123", "123456"); 
        //WebResponse wr = ftp.GetResponse(); 
        //StreamReader sr = new StreamReader(wr.GetResponseStream(),System.Text.Encoding.Default); 
        //string s = sr.ReadLine(); 
        //while(s.Equals("")) 
        //{ 
        //s = sr.ReadLine();
        //} 

        private string ReadLine()
        {
            int num;
            do
            {
                num = this.socketControl.Receive(this.buffer, this.buffer.Length, SocketFlags.None);
                this.msg += this.encoding.GetString(this.buffer, 0, num);
            }
            while (num >= this.buffer.Length);
            char[] separator = new char[] { '\n' };
            string[] array = this.msg.Split(separator);
            if (this.msg.Length > 2)
            {
                this.msg = array[array.Length - 2];
            }
            else
            {
                this.msg = array[0];
            }
            if (!this.msg.Substring(3, 1).Equals(" "))
            {
                return this.ReadLine();
            }
            return this.msg;
        }

        private void ReadReply()
        {
            this.msg = "";
            this.strReply = this.ReadLine();
            this.iReplyCode = int.Parse(this.strReply.Substring(0, 3));
        }

        private void SendCommand(string strCommand)
        {
            byte[] bytes = this.encoding.GetBytes((strCommand + "\r\n").ToCharArray());
            this.socketControl.Send(bytes, bytes.Length, SocketFlags.None);
            this.ReadReply();
        }
    } 
}
