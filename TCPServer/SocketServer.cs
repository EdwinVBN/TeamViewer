using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices.JavaScript;


namespace TCPServer
{
    public class Objectstate 
    {
        public Socket workSocket = null;
        public const int BufferSize = 256;
        public byte[] buffer = new byte[BufferSize];
        public StringBuilder sb = new StringBuilder();
    }

    public class AsyncSocketServer
    {
        public static ManualResetEvent allCompleted = new ManualResetEvent(false);

        public static void StartListener() 
        {
            byte[] bytes = new Byte[256];
            IPHostEntry iPHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress iPAddr = iPHost.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(iPAddr, 13000);
            Socket listener = new Socket(iPAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            Console.WriteLine("IP Address: " + iPAddr.ToString());
            Console.WriteLine("Host Name: " + iPHost.HostName.ToString());
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);

                while (true)
                {
                    allCompleted.Reset();
                    Console.Write("Waiting for a connection... ");
                    listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);
                    allCompleted.WaitOne();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void AcceptCallback(IAsyncResult ar)
        {
            allCompleted.Set();
            Socket listener = (Socket) ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            Objectstate state = new Objectstate();
            state.workSocket = handler;
            handler.BeginReceive(state.buffer, 0, Objectstate.BufferSize, 0, new AsyncCallback(ReadCallback), state);

        }

        private static void ReadCallback(IAsyncResult ar)
        {
            string content = String.Empty;
            Objectstate state = (Objectstate) ar.AsyncState;
            Socket handler = state.workSocket;
            int bytesRead = handler.EndReceive(ar);
            if (bytesRead > 0)
            {
                state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
                content = state.sb.ToString();
                if (content.IndexOf("<EOF>", StringComparison.Ordinal) > -1)
                {
                    Console.WriteLine($"Read: {content.Length} bytes from \n socket Data: {content}");
                    Send(handler, content);
                }
                else
                {
                    handler.BeginReceive(state.buffer, 0, Objectstate.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                }
            }
        }

        private static void Send(Socket handler, string content)
        {
            byte[] byteData = Encoding.ASCII.GetBytes(content);
            handler.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallBack), handler);
        }

        private static void SendCallBack(IAsyncResult ar)
        {
            try
            {
                Socket handler = (Socket) ar.AsyncState;
                int byteSent = handler.EndSend(ar);
                Console.WriteLine($"Sent: {byteSent} to client");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
