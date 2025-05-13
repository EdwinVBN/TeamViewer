using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace TeamViewer
{
    public class Record
    {
        private static bool recording_Btn_Is_Active = false;
        private static CancellationTokenSource? _cts;
        private static Task? _recordingTask;
        public static void ToggleRecording(Button btn, TCPClient tcpClient)
        {
            if (_recordingTask != null && !_recordingTask.IsCompleted)
            {
                // Stop recording
                _cts?.Cancel();
                btn.Content = "Start";
            }
            else
            {
                // Start recording
                _cts = new CancellationTokenSource();
                btn.Content = "Stop";

                _recordingTask = Task.Run(async () =>
                {
                    try
                    {
                        while (!_cts.Token.IsCancellationRequested)
                        {
                            byte[] imageBytes = record_Screen();
                            await tcpClient.SendMessage(imageBytes);
                            await Task.Delay(33, _cts.Token); // ~30 FPS
                        }
                    }
                    catch (TaskCanceledException)
                    {
                        // Gracefully stop
                    }
                });
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
