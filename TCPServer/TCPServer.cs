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
            int port = 13000;
            IPAddress local_Ip = IPAddress.Parse("127.0.0.1");

            server = new TcpListener(local_Ip, port);

            server.Start();

            while (true)
            {
                Console.Write("Waiting for a connection... ");

                _client = server.AcceptTcpClient();
                Console.WriteLine("Connected!");

                _client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);




                handle_Client(_client);
            }
        }

        private void handle_Client(TcpClient client)
        {
            Byte[] buffer = new Byte[256];
            string data;

            try
            {
                NetworkStream stream = client.GetStream();
                
                int i;

                while ((i = client.GetStream().Read(buffer, 0, buffer.Length)) != 0)
                {
                    data = System.Text.Encoding.ASCII.GetString(buffer, 0, i);
                    if (!string.IsNullOrWhiteSpace(data))
                    {
                        Console.WriteLine("Received: {0}", data);
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes("Received Succesfully!");
                        client.GetStream().Write(msg, 0, msg.Length);
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
                client.Close();
            }
        }
    }
}
