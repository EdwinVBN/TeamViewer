using System.Windows;
using System.Drawing;
using System.Threading.Tasks;

namespace TeamViewer
{
    
    public partial class MainWindow : Window
    {
        private bool recording_Btn_Is_Active = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void record_Button(object sender, RoutedEventArgs e)
        {
            recording_Btn_Is_Active = !recording_Btn_Is_Active;
            start_Rec_Btn.Content = recording_Btn_Is_Active ? "Stop" : "Start";

            while (recording_Btn_Is_Active)
            {
                record_Screen();
                await Task.Delay(33);
            }
        }

        private void record_Screen()
        {
            string filename = "Screenshot-" + DateTime.Now.ToString("HHmmss_fff") + ".jpeg";

            int screenLeft = (int)SystemParameters.VirtualScreenLeft;
            int screenTop = (int)SystemParameters.VirtualScreenTop;
            int screenWidth = (int)SystemParameters.VirtualScreenWidth;
            int screenHeight = (int)SystemParameters.VirtualScreenHeight;

            Bitmap bitmap_Screen = new Bitmap(screenWidth, screenHeight);
            Graphics g = Graphics.FromImage(bitmap_Screen);

            g.CopyFromScreen(screenLeft, screenTop, 0, 0, bitmap_Screen.Size);

            bitmap_Screen.Save("D:\\TeamViewerRepo\\TeamViewer\\video\\" + filename);
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