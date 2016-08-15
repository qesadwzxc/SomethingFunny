using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace VinCode.Web
{
    class FTPHelper
    {
        /// <summary>FTP帮助类
        /// </summary>
        public class FtpHelper
        {
            /// <summary>传输模式:二进制类型、ASCII类型
            /// </summary>
            public enum TransferType
            {
                /// <summary>Binary
                /// </summary>
                Binary,
                /// <summary>ASCII
                /// </summary>
                ASCII
            }

            /// <summary>远程主机
            /// </summary>
            private string remoteHost;

            /// <summary>远程端口
            /// </summary>
            private int remotePort;

            /// <summary>远程文件夹路径
            /// </summary>
            private string remotePath;

            /// <summary>登录用户名
            /// </summary>
            private string remoteUser;

            /// <summary>登录密码
            /// </summary>
            private string remotePassword;

            /// <summary>是否已连接
            /// </summary>
            private bool isConnected;

            /// <summary>服务器返回的应答信息(包含应答码)
            /// </summary>
            private string msg;

            /// <summary>服务器返回的应答信息(包含应答码)
            /// </summary>
            private string strReply;

            /// <summary>服务器返回的应答码
            /// </summary>
            private int iReplyCode;

            /// <summary>进行控制连接的socket
            /// </summary>
            private Socket socketControl;

            /// <summary>传输模式
            /// </summary>
            private FtpHelper.TransferType transferType;

            /// <summary>接收和发送数据的缓冲区
            /// </summary>
            private static int blockSize = 512;

            private byte[] buffer = new byte[FtpHelper.blockSize];

            /// <summary>编码方式
            /// </summary>
            private Encoding encoding = Encoding.Default;

            /// <summary>FTP服务器IP地址
            /// </summary>
            public string RemoteHost
            {
                get;
                set;
            }

            /// <summary>FTP服务器端口
            /// </summary>
            public int RemotePort
            {
                get
                {
                    return this.remotePort;
                }
                set
                {
                    this.remotePort = value;
                }
            }

            /// <summary>当前服务器目录
            /// </summary>
            public string RemotePath
            {
                get
                {
                    return this.remotePath;
                }
                set
                {
                    this.remotePath = value;
                }
            }

            /// <summary>登录用户账号
            /// </summary>
            public string RemoteUser
            {
                set
                {
                    this.remoteUser = value;
                }
            }

            /// <summary>用户登录密码
            /// </summary>
            public string RemotePassword
            {
                set
                {
                    this.remotePassword = value;
                }
            }

            /// <summary>是否登录
            /// </summary>
            public bool IsConnected
            {
                get
                {
                    return this.isConnected;
                }
            }

            /// <summary>缺省构造函数
            /// </summary>
            public FtpHelper():this("","","","",21){            }

            /// <summary>缺省构造函数
            /// </summary>
            public FtpHelper(string host) : this(host, "", "", "", 21) { }

            /// <summary>缺省构造函数
            /// </summary>
            public FtpHelper(string host, string user, string password) : this(host, "", user, password, 21) { }

            /// <summary>构造函数
            /// </summary>
            /// <param name="host"></param>
            /// <param name="path"></param>
            /// <param name="user"></param>
            /// <param name="password"></param>
            /// <param name="port"></param>
            public FtpHelper(string host, string path, string user, string password, int port)
            {
                this.remoteHost = host;
                this.remotePath = path;
                this.remoteUser = user;
                this.remotePassword = password;
                this.remotePort = port;
                this.isConnected = false;
            }

            /// <summary>建立连接 
            /// </summary>
            public void Connect()
            {
                this.socketControl = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(this.RemoteHost), this.remotePort);
                try
                {
                    this.socketControl.Connect(remoteEP);
                }
                catch (Exception)
                {
                    throw new IOException("Couldn't connect to remote server");
                }
                this.ReadReply();
                if (this.iReplyCode != 220)
                {
                    this.DisConnect();
                    throw new IOException(this.strReply.Substring(4));
                }
                this.SendCommand("USER " + this.remoteUser);
                if (this.iReplyCode != 331 && this.iReplyCode != 230)
                {
                    this.CloseSocketConnect();
                    throw new IOException(this.strReply.Substring(4));
                }
                if (this.iReplyCode != 230)
                {
                    this.SendCommand("PASS " + this.remotePassword);
                    if (this.iReplyCode != 230 && this.iReplyCode != 202)
                    {
                        this.CloseSocketConnect();
                        throw new IOException(this.strReply.Substring(4));
                    }
                }
                this.isConnected = true;
                this.ChangeFolder(this.remotePath);
            }

            /// <summary>关闭连接
            /// </summary>
            public void DisConnect()
            {
                if (this.socketControl != null)
                {
                    this.SendCommand("QUIT");
                }
                this.CloseSocketConnect();
            }

            /// <summary>设置传输模式
            /// </summary>
            /// <param name="ttType">传输模式</param>
            public void SetTransferType(FtpHelper.TransferType ttType)
            {
                if (ttType == FtpHelper.TransferType.Binary)
                {
                    this.SendCommand("TYPE I");
                }
                else
                {
                    this.SendCommand("TYPE A");
                }
                if (this.iReplyCode != 200)
                {
                    throw new IOException(this.strReply.Substring(4));
                }
                this.transferType = ttType;
            }

            /// <summary>获得传输模式
            /// </summary>
            /// <returns>传输模式</returns>
            public FtpHelper.TransferType GetTransferType()
            {
                return this.transferType;
            }

            /// <summary>获得文件列表
            /// </summary>
            /// <param name="strMask">文件名的匹配字符串</param>
            /// <returns></returns>
            public string[] Dir(string strMask)
            {
                if (!this.isConnected)
                {
                    this.Connect();
                }
                Socket socket = this.CreateDataSocket();
                this.SendCommand("NLST " + strMask);
                if (this.iReplyCode != 150 && this.iReplyCode != 125 && this.iReplyCode != 226)
                {
                    throw new IOException(this.strReply.Substring(4));
                }
                this.msg = "";
                int num;
                do
                {
                    num = socket.Receive(this.buffer, this.buffer.Length, SocketFlags.None);
                    this.msg += this.encoding.GetString(this.buffer, 0, num);
                }
                while (num >= this.buffer.Length);
                char[] separator = new char[]
                {
                '\n'
                };
                string[] result = this.msg.Split(separator);
                socket.Close();
                if (this.iReplyCode != 226)
                {
                    this.ReadReply();
                    if (this.iReplyCode != 226)
                    {
                        throw new IOException(this.strReply.Substring(4));
                    }
                }
                return result;
            }

            /// <summary>获取文件大小
            /// </summary>
            /// <param name="strFileName">文件名</param>
            /// <returns>文件大小</returns>
            private long GetFileSize(string strFileName)
            {
                if (!this.isConnected)
                {
                    this.Connect();
                }
                this.SendCommand("SIZE " + Path.GetFileName(strFileName));
                if (this.iReplyCode == 213)
                {
                    return long.Parse(this.strReply.Substring(4));
                }
                throw new IOException(this.strReply.Substring(4));
            }

            /// <summary>删除文件
            /// </summary>
            /// <param name="strFileName">待删除文件名</param>
            public void Delete(string strFileName)
            {
                if (!this.isConnected)
                {
                    this.Connect();
                }
                this.SendCommand("DELE " + strFileName);
                if (this.iReplyCode != 250)
                {
                    throw new IOException(this.strReply.Substring(4));
                }
            }

            /// <summary>重命名(如果新文件名与已有文件重名,将覆盖已有文件)
            /// </summary>
            /// <param name="strOldFileName">旧文件名</param>
            /// <param name="strNewFileName">新文件名</param>
            public void Rename(string strOldFileName, string strNewFileName)
            {
                if (!this.isConnected)
                {
                    this.Connect();
                }
                this.SendCommand("RNFR " + strOldFileName);
                if (this.iReplyCode != 350)
                {
                    throw new IOException(this.strReply.Substring(4));
                }
                this.SendCommand("RNTO " + strNewFileName);
                if (this.iReplyCode != 250)
                {
                    throw new IOException(this.strReply.Substring(4));
                }
            }

            /// <summary>下载一批文件
            /// </summary>
            /// <param name="strFileNameMask">文件名的匹配字符串</param>
            /// <param name="strFolder">本地目录(不得以\结束)</param>
            public void DownLoadFile(string strFileNameMask, string strFolder)
            {
                if (!this.isConnected)
                {
                    this.Connect();
                }
                string[] array = this.Dir(strFileNameMask);
                string[] array2 = array;
                for (int i = 0; i < array2.Length; i++)
                {
                    string text = array2[i];
                    if (!text.Equals(""))
                    {
                        this.DownLoadFile(text, strFolder, text);
                    }
                }
            }

            /// <summary>下载一个文件
            /// </summary>
            /// <param name="strRemoteFileName">要下载的文件名</param>
            /// <param name="strFolder">本地目录(不得以\结束)</param>
            /// <param name="strLocalFileName">保存在本地时的文件名</param>
            public void DownLoadFile(string strRemoteFileName, string strFolder, string strLocalFileName)
            {
                if (!this.isConnected)
                {
                    this.Connect();
                }
                this.SetTransferType(FtpHelper.TransferType.Binary);
                if (strLocalFileName.Equals(""))
                {
                    strLocalFileName = strRemoteFileName;
                }
                if (!File.Exists(strFolder + "\\" + strLocalFileName))
                {
                    Stream stream = File.Create(strFolder + "\\" + strLocalFileName);
                    stream.Close();
                }
                FileStream fileStream = new FileStream(strFolder + "\\" + strLocalFileName, FileMode.Create);
                Socket socket = this.CreateDataSocket();
                this.SendCommand("RETR " + strRemoteFileName);
                if (this.iReplyCode != 150 && this.iReplyCode != 125 && this.iReplyCode != 226 && this.iReplyCode != 250)
                {
                    fileStream.Close();
                    File.Delete(strFolder + "\\" + strLocalFileName);
                    return;
                }
                int num;
                do
                {
                    num = socket.Receive(this.buffer, this.buffer.Length, SocketFlags.None);
                    fileStream.Write(this.buffer, 0, num);
                }
                while (num > 0);
                fileStream.Close();
                if (socket.Connected)
                {
                    socket.Close();
                }
                if (this.iReplyCode != 226 && this.iReplyCode != 250)
                {
                    this.ReadReply();
                    if (this.iReplyCode != 226 && this.iReplyCode != 250)
                    {
                        throw new IOException(this.strReply.Substring(4));
                    }
                }
            }

            /// <summary>下载一个文件到内存
            /// </summary>
            /// <param name="strRemoteFileName"></param>
            /// <returns></returns>
            public MemoryStream DownLoadFile(string strRemoteFileName)
            {
                MemoryStream memoryStream = new MemoryStream();
                if (!this.isConnected)
                {
                    this.Connect();
                }
                this.SetTransferType(FtpHelper.TransferType.Binary);
                Socket socket = this.CreateDataSocket();
                this.SendCommand("RETR " + strRemoteFileName);
                if (this.iReplyCode != 150 && this.iReplyCode != 125 && this.iReplyCode != 226 && this.iReplyCode != 250)
                {
                    throw new IOException(this.strReply.Substring(4));
                }
                int num;
                do
                {
                    num = socket.Receive(this.buffer, this.buffer.Length, SocketFlags.None);
                    memoryStream.Write(this.buffer, 0, num);
                }
                while (num > 0);
                if (socket.Connected)
                {
                    socket.Close();
                }
                if (this.iReplyCode != 226 && this.iReplyCode != 250)
                {
                    this.ReadReply();
                    if (this.iReplyCode != 226 && this.iReplyCode != 250)
                    {
                        throw new IOException(this.strReply.Substring(4));
                    }
                }
                return memoryStream;
            }

            /// <summary>上传一批文件
            /// </summary>
            /// <param name="strFolder">本地目录(不得以\结束)</param>
            /// <param name="strFileNameMask">文件名匹配字符(可以包含*和?)</param>
            public void UpLoadFiles(string strFolder, string strFileNameMask)
            {
                string[] files = Directory.GetFiles(strFolder, strFileNameMask);
                string[] array = files;
                for (int i = 0; i < array.Length; i++)
                {
                    string strFileName = array[i];
                    this.UpLoadFile(strFileName);
                }
            }

            /// <summary>上传一个文件
            /// </summary>
            /// <param name="strFileName">本地文件名</param>
            public bool UpLoadFile(string strFileName, Stream input)
            {
                if (!this.isConnected)
                {
                    this.Connect();
                }
                Socket socket = this.CreateDataSocket();
                this.SendCommand("STOR " + Path.GetFileName(strFileName));
                if (this.iReplyCode != 125 && this.iReplyCode != 150)
                {
                    return false;
                }
                input.Position = 0L;
                int size;
                while ((size = input.Read(this.buffer, 0, this.buffer.Length)) > 0)
                {
                    socket.Send(this.buffer, size, SocketFlags.None);
                }
                input.Close();
                if (socket.Connected)
                {
                    socket.Close();
                }
                if (this.iReplyCode != 226 && this.iReplyCode != 250)
                {
                    this.ReadReply();
                    if (this.iReplyCode != 226 && this.iReplyCode != 250)
                    {
                        return false;
                    }
                }
                return true;
            }

            /// <summary>上传一个文件
            /// </summary>
            /// <param name="strFileName">本地文件名</param>
            public bool UpLoadFileForMemoryStream(string strFileName, MemoryStream input)
            {
                if (!this.isConnected)
                {
                    this.Connect();
                }
                Socket socket = this.CreateDataSocket();
                this.SendCommand("STOR " + Path.GetFileName(strFileName));
                if (this.iReplyCode != 125 && this.iReplyCode != 150)
                {
                    return false;
                }
                this.buffer = input.GetBuffer();
                int num = this.buffer.Length;
                if (num > 0)
                {
                    socket.Send(this.buffer, num, SocketFlags.None);
                }
                input.Close();
                if (socket.Connected)
                {
                    socket.Close();
                }
                if (this.iReplyCode != 226 && this.iReplyCode != 250)
                {
                    this.ReadReply();
                    if (this.iReplyCode != 226 && this.iReplyCode != 250)
                    {
                        return false;
                    }
                }
                return true;
            }

            /// <summary>上传一个文件
            /// </summary>
            /// <param name="strFileName">本地文件名</param>
            public bool UpLoadFile(string strFileName)
            {
                if (!this.isConnected)
                {
                    this.Connect();
                }
                Socket socket = this.CreateDataSocket();
                this.SendCommand("STOR " + Path.GetFileName(strFileName));
                if (this.iReplyCode != 125 && this.iReplyCode != 150)
                {
                    return false;
                }
                FileStream fileStream = new FileStream(strFileName, FileMode.Open);
                int size;
                while ((size = fileStream.Read(this.buffer, 0, this.buffer.Length)) > 0)
                {
                    socket.Send(this.buffer, size, SocketFlags.None);
                }
                fileStream.Close();
                if (socket.Connected)
                {
                    socket.Close();
                }
                if (this.iReplyCode != 226 && this.iReplyCode != 250)
                {
                    this.ReadReply();
                    if (this.iReplyCode != 226 && this.iReplyCode != 250)
                    {
                        return false;
                    }
                }
                return true;
            }

            /// <summary>
            /// 创建目录
            /// </summary>
            /// <param name="strDirName">目录名</param>
            public void CreateFolder(string strDirName)
            {
                if (!this.isConnected)
                {
                    this.Connect();
                }
                this.SendCommand("MKD " + strDirName);
                if (this.iReplyCode != 257)
                {
                    throw new IOException(this.strReply.Substring(4));
                }
            }

            /// <summary>
            /// 删除目录
            /// </summary>
            /// <param name="strDirName">目录名</param>
            public void DeleteFolder(string strDirName)
            {
                if (!this.isConnected)
                {
                    this.Connect();
                }
                this.SendCommand("RMD " + strDirName);
                if (this.iReplyCode != 250)
                {
                    throw new IOException(this.strReply.Substring(4));
                }
            }

            /// <summary>
            /// 改变目录
            /// </summary>
            /// <param name="strDirName">新的工作目录名</param>
            public void ChangeFolder(string strDirName)
            {
                if (strDirName.Equals(".") || strDirName.Equals(""))
                {
                    return;
                }
                if (!this.isConnected)
                {
                    this.Connect();
                }
                this.SendCommand("CWD " + strDirName);
                if (this.iReplyCode != 250)
                {
                    throw new IOException(this.strReply.Substring(4));
                }
                this.remotePath = strDirName;
            }

            /// <summary>
            /// 改变目录,自动加上年月日的文件夹（/yyyy/mm/dd/）,不存在自动创建
            /// </summary>
            /// <param name="strDirName">新的工作目录名</param>
            public void IntoYearFolder()
            {
                this.IntoYearFolder(this.remotePath);
            }

            /// <summary>
            /// 改变目录,自动加上年月日的文件夹（/yyyy/mm/dd/）,不存在自动创建
            /// </summary>
            /// <param name="strDirName">新的工作目录名</param>
            public void IntoYearFolder(string strDirName)
            {
                if (strDirName.Equals(".") || strDirName.Equals(""))
                {
                    return;
                }
                if (!this.isConnected)
                {
                    this.Connect();
                }
                strDirName = strDirName + DateTime.Now.Year.ToString() + "/";
                this.SendCommand("CWD " + strDirName);
                if (this.iReplyCode != 250)
                {
                    this.CreateFolder(strDirName);
                }
                strDirName = strDirName + DateTime.Now.Month.ToString() + "/";
                this.SendCommand("CWD " + strDirName);
                if (this.iReplyCode != 250)
                {
                    this.CreateFolder(strDirName);
                }
                strDirName = strDirName + DateTime.Now.Day.ToString() + "/";
                this.SendCommand("CWD " + strDirName);
                if (this.iReplyCode != 250)
                {
                    this.CreateFolder(strDirName);
                    this.SendCommand("CWD " + strDirName);
                }
                if (this.iReplyCode != 250)
                {
                    throw new IOException(this.strReply.Substring(4));
                }
                this.remotePath = strDirName;
            }

            /// <summary>
            /// 改变目录,自动加上年月日的文件夹（/yyyy/mm/dd/）,不存在自动创建
            /// </summary>
            /// <param name="strDirName">新的工作目录名</param>
            public void IntoYearFolder(string year, string month, string day)
            {
                string text = "/cn/img/admin/ziyouxing/";
                if (text.Equals(".") || text.Equals(""))
                {
                    return;
                }
                if (!this.isConnected)
                {
                    this.Connect();
                }
                text = text + year + "/";
                this.SendCommand("CWD " + text);
                if (this.iReplyCode != 250)
                {
                    this.CreateFolder(text);
                }
                text = text + month + "/";
                this.SendCommand("CWD " + text);
                if (this.iReplyCode != 250)
                {
                    this.CreateFolder(text);
                }
                text = text + day + "/";
                this.SendCommand("CWD " + text);
                if (this.iReplyCode != 250)
                {
                    this.CreateFolder(text);
                    this.SendCommand("CWD " + text);
                }
                if (this.iReplyCode != 250)
                {
                    throw new IOException(this.strReply.Substring(4));
                }
                this.remotePath = text;
            }

            /// <summary>将一行应答字符串记录在strReply和strMsg
            /// 应答码记录在iReplyCode
            /// </summary>
            private void ReadReply()
            {
                this.msg = "";
                this.strReply = this.ReadLine();
                this.iReplyCode = int.Parse(this.strReply.Substring(0, 3));
            }

            /// <summary>建立进行数据连接的socket
            /// </summary>
            /// <returns>数据连接socket</returns>
            private Socket CreateDataSocket()
            {
                this.SendCommand("PASV");
                if (this.iReplyCode != 227)
                {
                    throw new IOException(this.strReply.Substring(4));
                }
                int num = this.strReply.IndexOf('(');
                int num2 = this.strReply.IndexOf(')');
                string text = this.strReply.Substring(num + 1, num2 - num - 1);
                int[] array = new int[6];
                int length = text.Length;
                int num3 = 0;
                string text2 = "";
                int num4 = 0;
                while (num4 < length && num3 <= 6)
                {
                    char c = char.Parse(text.Substring(num4, 1));
                    if (char.IsDigit(c))
                    {
                        text2 += c;
                    }
                    else if (c != ',')
                    {
                        throw new IOException("Malformed PASV strReply: " + this.strReply);
                    }
                    if (c == ',')
                    {
                        goto Block_5;
                    }
                    if (num4 + 1 == length)
                    {
                        goto Block_5;
                    }
                IL_100:
                    num4++;
                    continue;
                Block_5:
                    try
                    {
                        array[num3++] = int.Parse(text2);
                        text2 = "";
                    }
                    catch (Exception)
                    {
                        throw new IOException("Malformed PASV strReply: " + this.strReply);
                    }
                    goto IL_100;
                }
                string ipString = string.Concat(new object[]
                {
                array[0],
                ".",
                array[1],
                ".",
                array[2],
                ".",
                array[3]
                });
                int port = (array[4] << 8) + array[5];
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(ipString), port);
                try
                {
                    socket.Connect(remoteEP);
                }
                catch (Exception)
                {
                    throw new IOException("Can't connect to remote server");
                }
                return socket;
            }

            /// <summary>关闭socket连接(用于登录以前)
            /// </summary>
            private void CloseSocketConnect()
            {
                if (this.socketControl != null)
                {
                    this.socketControl.Close();
                    this.socketControl = null;
                }
                this.isConnected = false;
            }

            /// <summary>读取Socket返回的所有字符串
            /// </summary>
            /// <returns>包含应答码的字符串行</returns>
            private string ReadLine()
            {
                int num;
                do
                {
                    num = this.socketControl.Receive(this.buffer, this.buffer.Length, SocketFlags.None);
                    this.msg += this.encoding.GetString(this.buffer, 0, num);
                }
                while (num >= this.buffer.Length);
                char[] separator = new char[]
                {
                '\n'
                };
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

            /// <summary>发送命令并获取应答码和最后一行应答字符串
            /// </summary>
            /// <param name="strCommand">命令</param>
            private void SendCommand(string strCommand)
            {
                byte[] bytes = this.encoding.GetBytes((strCommand + "\r\n").ToCharArray());
                this.socketControl.Send(bytes, bytes.Length, SocketFlags.None);
                this.ReadReply();
            }
        }
    }
}
