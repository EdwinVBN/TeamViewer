using System.Net;
using System.Net.Sockets;

namespace TCPServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // oude normale sync server
            //var server = new TCPServer();

            // nieuwe async server
            //Console.WriteLine("press any key to start server...");
            //Console.ReadLine();
            AsyncSocketServer.StartListener();
        }
    }
}
