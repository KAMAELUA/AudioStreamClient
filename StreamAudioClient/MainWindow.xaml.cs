using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StreamAudioClient
{

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Thread listnerThread;
        WaveOut waveOut;
        Stream stream;
        StreamWriter sw;
        WaveFormat waveFormat;

        TcpClient listener;
        IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, 11001);

        Socket udpsock = new Socket(SocketType.Dgram, ProtocolType.Udp);
        public MainWindow()
        {
            listener = new TcpClient(groupEP);
            waveOut = new WaveOut(WaveCallbackInfo.FunctionCallback());
            
            InitializeComponent();
            //listnerThread = new Thread(StartListener);
            waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(44100, 2);
            stream = new MemoryStream();
            

            NetworkStream ns = listener.GetStream();
            var rawSource = new RawSourceWaveStream(ns, waveFormat);

            waveOut.Init(rawSource);
            waveOut.Play();
        }

        public void InitilizeElements()
        {
            
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            listnerThread.Start();
        }

        private async void StartListener()
        {
            bool done = false;

            

            try
            {
                Console.WriteLine("Waiting for broadcast");
                while (!done)
                {
                    Console.WriteLine("Waiting for broadcast");
                    //byte[] bytes = listener.
                    //stream.WriteAsync(bytes, 0, bytes.Length);
                    
                    
                    //waveOut.Play();

                    //Console.WriteLine("Received broadcast from {0} :\n {1}\n", groupEP.ToString(), bytes.Length);
                }



            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                listener.Close();
            }
        }
    }
}
