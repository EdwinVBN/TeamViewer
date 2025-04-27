using System.Drawing;
using System.Windows;
using System.Runtime.InteropServices;

namespace TeamViewer
{
    public partial class MainWindow : Window
    {
        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();

        private TCPClient? _tcpClient;
        public MainWindow()
        {
            InitializeComponent();
            AllocConsole();

            _tcpClient = new TCPClient();
        }

        private async void rec_Btn(object sender, RoutedEventArgs e)
        {
            await Record.record_Button(sender, e, start_Rec_Btn, _tcpClient);
            
            
        }


        private void screenshot_Button(object sender, RoutedEventArgs e)
        { 
            make_Screenshot();
            MessageBox.Show("screenshot is gemaakt");
        }

        private async void connect_To_TcpServer(object sender, RoutedEventArgs e)
        {
            int port = int.Parse(inputPort.Text);
            string ip = inputServer.Text;


            bool connected = await _tcpClient.Connect(ip, port);
            if (!connected)
            {
                MessageBox.Show("Could not connect to server.");
            }

        }

        private void stop_Connection_To_TcpServer(object sender, RoutedEventArgs e)
        {
            
            _tcpClient.stop_Connection();
        }

        private void make_Screenshot()
        {
            string filename = "Screenshot-" + DateTime.Now.ToString("hhmmss") + ".jpeg";

            int screenLeft = (int) SystemParameters.VirtualScreenLeft;
            int screenTop = (int) SystemParameters.VirtualScreenTop;
            int screenWidth = (int) SystemParameters.VirtualScreenWidth;
            int screenHeight = (int) SystemParameters.VirtualScreenHeight;

            Bitmap bitmap_Screen = new Bitmap(screenWidth, screenHeight);
            Graphics g = Graphics.FromImage(bitmap_Screen);

            g.CopyFromScreen(screenLeft, screenTop, 0, 0, bitmap_Screen.Size);

            bitmap_Screen.Save("D:\\TeamViewerRepo\\TeamViewer\\images\\" + filename);
        }

    }
}