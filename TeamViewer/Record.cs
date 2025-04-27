using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace TeamViewer
{
    public class Record
    {
        private static bool recording_Btn_Is_Active = false;
        public static async Task record_Button(object sender, RoutedEventArgs e, Button btn, TCPClient tcpClient)
        {
            //int i = 0;
            recording_Btn_Is_Active = !recording_Btn_Is_Active;
            btn.Content = recording_Btn_Is_Active ? "Stop" : "Start";
            //Connect("127.0.0.1");

            while (recording_Btn_Is_Active)
            {
                byte[] imageBytes = record_Screen();
                await tcpClient.SendMessage(imageBytes);
                //i++;
                //string msg = $"test {i}";
                //Connect("127.0.0.1", msg);
                await Task.Delay(33);
            }
        }

        private static byte[] record_Screen()
        {
            string filename = "Screenshot-" + DateTime.Now.ToString("HHmmss_fff") + ".jpeg";

            int screenLeft = (int)SystemParameters.VirtualScreenLeft;
            int screenTop = (int)SystemParameters.VirtualScreenTop;
            int screenWidth = (int)SystemParameters.VirtualScreenWidth;
            int screenHeight = (int)SystemParameters.VirtualScreenHeight;

            Bitmap bitmap_Screen = new Bitmap(screenWidth, screenHeight);
            Graphics g = Graphics.FromImage(bitmap_Screen);

            g.CopyFromScreen(screenLeft, screenTop, 0, 0, bitmap_Screen.Size);

            //bitmap_Screen.Save("D:\\TeamViewerRepo\\TeamViewer\\video\\" + filename);
            byte[] imageByte = image_To_Byte(bitmap_Screen);

            return imageByte;

        }

        private static byte[] image_To_Byte(System.Drawing.Image img)
        {
            using (MemoryStream ms = new MemoryStream())
            {

                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();

            }
        }
    }
}
