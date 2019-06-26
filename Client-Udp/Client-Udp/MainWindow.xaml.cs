using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Client_Udp
{
    public partial class MainWindow : Window
    {
        private UdpClient _client;

        public MainWindow()
        {
            InitializeComponent();

            int localPort = 12345;
            string ipAddress = "192.168.56.1";

        //    try
        //    {
                _client = new UdpClient(new IPEndPoint(IPAddress.Parse(ipAddress), localPort));
         //   }
         //   catch (Exception exception)
         //   {
          //      MessageBox.Show($"Error: {exception.Message}");
          //  }
        }

        private void EnterButtonClick(object sender, RoutedEventArgs e)
        {
            const int OFFSET = 0;
            int remotePort = 54321;

            ThreadPool.QueueUserWorkItem((state) =>
            {
                var ipEndPoint = new IPEndPoint(IPAddress.Any, remotePort);

            //    try
            //    {
                    byte[] buffer = _client.Receive(ref ipEndPoint);
                    using (var memoryStream = new MemoryStream(buffer, OFFSET, buffer.Length))
                    {
                        memoryStream.Write(buffer, OFFSET, buffer.Length);
                        Dispatcher.Invoke(new Action(() =>
                        {
                            screenshotImage.Source = BitmapFrame.Create(memoryStream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                        }), System.Windows.Threading.DispatcherPriority.Background);
                    }
                //}
              //  catch (Exception exception)
              //  {
               //     MessageBox.Show($"Error: {exception.Message}");
               // }
            });
        }
    }
}
