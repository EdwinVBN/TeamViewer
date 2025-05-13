using System.Net.Sockets;
using System.Text;
using System.Windows;

namespace TeamViewer
{
    public class TCPClient
    {
        private TcpClient? _client;
        private NetworkStream? _stream;

        public async Task<bool> Connect(string inputIp, int inputPort)
        {
            int port = inputPort;
            string ip = inputIp;

            Console.WriteLine($"Connecting to {ip}:{port}");

            try
            {
                _client = new TcpClient();
                await _client.ConnectAsync(ip, port);
                _stream = _client.GetStream();
                MessageBox.Show("Connected to server");
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error connecting to server: {ex.Message}");
                return false;
            }
        }

        public async Task SendMessage(byte[] image)
        {
            if (_client != null && _client.Connected && _stream != null)
            {
                byte[] lengthPrefix = BitConverter.GetBytes(image.Length);
                await _stream.WriteAsync(lengthPrefix, 0, lengthPrefix.Length);
                await _stream.WriteAsync(image, 0, image.Length);
                Console.WriteLine($"Sent {image.Length} bytes");
            }
        }


        public void stop_Connection()
        {
            if (_client != null && _client.Connected)
            {
                _client.Client.Shutdown(SocketShutdown.Both);

                _client.Close();
                _client = null;
                MessageBox.Show("Disconnected from server");
            }
        }
    }
}
