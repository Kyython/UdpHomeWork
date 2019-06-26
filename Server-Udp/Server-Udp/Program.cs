using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace Server_Udp
{
    public class Program
    {
        private static void Main(string[] args)
        {
            ThreadPool.QueueUserWorkItem((state) =>
            {
                CreateScreenshot();
                SendScreenshot();
            });

            Console.ReadLine();
        }

        private static void CreateScreenshot()
        {
            const int COORDINATE = 0;

            var screenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            using (var graphics = Graphics.FromImage(screenshot))
            {
                graphics.CopyFromScreen(COORDINATE, COORDINATE, COORDINATE, COORDINATE, Screen.PrimaryScreen.Bounds.Size);
                screenshot.Save("UserScreenshot.png");
            }
        }

        private static void SendScreenshot()
        {
            const int BUFFER_SIZE = 64512;
            byte[] buffer = new byte[BUFFER_SIZE];
            int localPort = 54321;
            int remotePort = 12345;
            string ipAddress = "192.168.56.1";

            try
            {
                using (var fileStream = new FileStream("UserScreenshot.png", FileMode.Open, FileAccess.Read))
                using (var binaryReader = new BinaryReader(fileStream))
                {
                    buffer = binaryReader.ReadBytes((int)fileStream.Length);
                }

                using (var server = new UdpClient(new IPEndPoint(IPAddress.Any, localPort)))
                {
                    server.Connect(new IPEndPoint(IPAddress.Parse(ipAddress), remotePort));
                    server.Send(buffer, BUFFER_SIZE);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error: {exception.Message}");
            }
        }      
    }
}
