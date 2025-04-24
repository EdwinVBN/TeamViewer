using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCPServer
{
    public class TCPServer
    {
        private TcpListener server = null;

        public TCPServer()
        {
            start_Server();
        }

        private void start_Server()
        {
            int port = 13000;
            IPAddress local_Ip = IPAddress.Parse("127.0.0.1");

            server = new TcpListener(local_Ip, port);

            server.Start();

            Byte[] buffer = new Byte[256];
            string data;

            while (true)
            {
                Console.Write("Waiting for a connection... ");

                using TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("Connected!");

                data = null;

                NetworkStream stream = client.GetStream();

                int i;

                while ((i = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    data = System.Text.Encoding.ASCII.GetString(buffer, 0, i);

                    if (!string.IsNullOrWhiteSpace(data))
                    {
                        Console.WriteLine("Received: {0}", data);
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes("Received Succesfully!");
                        stream.Write(msg, 0, msg.Length);
                        Console.WriteLine("sent: {0}", System.Text.Encoding.ASCII.GetString(msg));
                    }
                }
            }
        }
    }
}
