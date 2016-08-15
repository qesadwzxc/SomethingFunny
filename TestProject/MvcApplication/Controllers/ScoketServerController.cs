using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace MvcApi.Controllers
{
    public class SocketServerController : Controller
    {
        //客户端数据接收缓存区
        static byte[] buffer = new byte[1024];
        static StringBuilder messageRe = new StringBuilder();

        public ActionResult Main()
        {
            //创建一个新的Socket,这里我们使用最常用的基于TCP的Stream Socket（流式套接字）
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                //将该socket绑定到主机上面的某个端口
                //方法参考：http://msdn.microsoft.com/zh-cn/library/system.net.sockets.socket.bind.aspx
                socket.Bind(new IPEndPoint(IPAddress.Any, 12345));

                //启动监听，并且设置一个最大的队列长度
                //方法参考：http://msdn.microsoft.com/zh-cn/library/system.net.sockets.socket.listen(v=VS.100).aspx
                socket.Listen(2);

                //开始接受客户端连接请求
                //方法参考：http://msdn.microsoft.com/zh-cn/library/system.net.sockets.socket.beginaccept.aspx
                socket.BeginAccept(new AsyncCallback((ar) =>
                {
                    //客户端的Socket实例，我们后续可以将其保存起来
                    var client = socket.EndAccept(ar);
                    //给客户端发送一个欢迎消息
                    client.Send(Encoding.Unicode.GetBytes("Hi there, I accept you request at " + DateTime.Now.ToString()));

                    //实现每隔两秒钟给服务器发一个消息
                    //这里我们使用了一个定时器
                    var timer = new System.Timers.Timer();
                    timer.Interval = 2000D;
                    timer.Enabled = true;
                    timer.Elapsed += (o, a) =>
                    {
                        if (client.Connected)
                        {
                            client.Send(Encoding.Unicode.GetBytes("Message from server at " + DateTime.Now.ToString()));
                        }
                        else
                        {
                            timer.Stop();
                            timer.Enabled = false;
                            Response.Write("Client is disconnected, the timer is stop.");
                        }
                    };
                    timer.Start();

                    //接受客户端的消息
                    client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveMessage), client);
                }), null);
            }
            catch (Exception ex) when (ex.GetType() != typeof(SocketException))
            {
                if (ex.GetType() != typeof(SocketException))
                {
                    Response.Write(ex.Message);
                }          
                socket.Close();
            }

            //开始接受客户端连接请求
            //socket.BeginAccept(new AsyncCallback(ClientAccepted), socket);

            Response.Write("Server is Ready!");
            return View();
        }

        public static void ClientAccepted(IAsyncResult ar)
        {
            var socket = ar.AsyncState as Socket;

            //这就是客户端的Socket实例，我们后续可以将其保存起来
            var client = socket.EndAccept(ar);

            //给客户端发送一个欢迎消息
            client.Send(Encoding.Unicode.GetBytes("Hi there, I accept you request at " + DateTime.Now.ToString()));

            //实现每隔两秒钟给服务器发一个消息
            //这里我们使用了一个定时器
            var timer = new System.Timers.Timer();
            timer.Interval = 2000D;
            timer.Enabled = true;
            timer.Elapsed += (o, a) =>
            {
                //检测客户端Socket的状态
                if (client.Connected)
                {
                    try
                    {
                        client.Send(Encoding.Unicode.GetBytes("Message from server at " + DateTime.Now.ToString()));
                    }
                    catch (SocketException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {
                    timer.Stop();
                    timer.Enabled = false;
                    Console.WriteLine("Client is disconnected, the timer is stop.");
                }
            };
            timer.Start();

            //接收客户端的消息(这个和在客户端实现的方式是一样的）
            client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveMessage), client);

            //准备接受下一个客户端请求
            socket.BeginAccept(new AsyncCallback(ClientAccepted), socket);
        }

        public ActionResult Client()
        {
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //连接到指定服务器的指定端口
            //方法参考：http://msdn.microsoft.com/zh-cn/library/system.net.sockets.socket.connect.aspx
            socket.Connect("localhost", 12345);

            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveMessage), socket);

            Response.Write("Client is Ready!");

            //接受用户输入，将消息发送给服务器端
            //while (true)
            //{
            //    var message = "Message from client : " + Console.ReadLine();
            //    var outputBuffer = Encoding.Unicode.GetBytes(message);
            //    socket.BeginSend(outputBuffer, 0, outputBuffer.Length, SocketFlags.None, null, null);
            //}
            return View();
        }

        public JsonResult ReturnMessage()
        {
            return Json(messageRe.ToString(), JsonRequestBehavior.AllowGet);
        }

        public static void ReceiveMessage(IAsyncResult ar)
        {
            try
            {
                var socket = ar.AsyncState as Socket;
                var length = socket.EndReceive(ar);
                var message = Encoding.Unicode.GetString(buffer);
                if (!string.IsNullOrEmpty(message))
                {
                    messageRe.Append(message);
                }
                socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveMessage), socket);
            }
            catch (Exception ex)
            {
                messageRe.Append(ex.Message);
            }
        }
    }
}
