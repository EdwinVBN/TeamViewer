using System.Windows;
using System.Drawing;
using System.Threading.Tasks;

namespace TeamViewer
{
    
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void rec_Btn_listener(object sender, RoutedEventArgs e)
        {
            Record.record_Button(sender, e, start_Rec_Btn);
        }

        private void screenshot_Button(object sender, RoutedEventArgs e)
        { 
            make_Screenshot();
            MessageBox.Show("screenshot is gemaakt");
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