using System.Drawing;
using System.Windows;
using System.Windows.Controls;

namespace TeamViewer
{
    public class Record
    {
        private static bool recording_Btn_Is_Active = false;
        public static async void record_Button(object sender, RoutedEventArgs e, Button btn)
        {
            recording_Btn_Is_Active = !recording_Btn_Is_Active;
            btn.Content = recording_Btn_Is_Active ? "Stop" : "Start";
            while (recording_Btn_Is_Active)
            {
               record_Screen();
               await Task.Delay(33);
            }
        }

        private static void record_Screen()
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
    }
}
