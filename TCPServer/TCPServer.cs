using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace TCPServer
{
    public class TCPServer
    {
        private TcpListener? server = null;
        private TcpClient? _client;

        public TCPServer()
        {
            start_Server();
        }

        private void start_Server()
        {
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddr = ipHost.AddressList[0];

            Console.WriteLine("IP Address: " + ipAddr.ToString());
            Console.WriteLine("Host Name: " + ipHost.HostName.ToString());

            IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 13000);

            Console.WriteLine("Local End Point: " + localEndPoint.ToString());

            Socket listener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try 
            { 
                listener.Bind(localEndPoint);

                listener.Listen(10);

                while (true)
                {
                    Console.Write("Waiting for a connection... ");

                    Socket clientSocket = listener.Accept();
                    Console.WriteLine("Connected!");

                    //_client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

                    handle_Client(clientSocket);
                }
            }
            catch
            {

            }


        }

        private void handle_Client(Socket clientSocket)
        {
            Byte[] buffer = new Byte[256];
            string data;

            try
            {
                // int stream = clientSocket.Receive(buffer);
                int stream;
                
                while ((stream = clientSocket.Receive(buffer)) != 0)
                {
                    data = System.Text.Encoding.ASCII.GetString(buffer, 0, stream);
                    if (!string.IsNullOrWhiteSpace(data))
                    {
                        Console.WriteLine("Received: {0}", data);
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes("Received Succesfully!");
                        clientSocket.Send(msg);
                        Console.WriteLine("sent: {0}", System.Text.Encoding.ASCII.GetString(msg));
                    }
                }
                Console.WriteLine("Client disconnected");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Connection error: " + ex.Message);
            }
            finally
            {
                clientSocket.Close();
            }
        }
    }
}
