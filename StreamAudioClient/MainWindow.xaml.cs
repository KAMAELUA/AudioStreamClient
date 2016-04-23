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
        public int port = 11001; //умолчание
        public string adres = "127.0.0.1"; //умолчание
        Thread listnerThread;
        WaveOut waveOut;
        Stream stream;
        StreamWriter sw;
        WaveFormat waveFormat;

        TcpClient listener;
        IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, 11001);

        //Socket udpsock = new Socket(SocketType.Dgram, ProtocolType.Udp);
        public MainWindow()
        {
            InitializeComponent();

            waveOut = new WaveOut(WaveCallbackInfo.FunctionCallback());
            waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(44100, 2);



            //NetworkStream ns = listener.GetStream();


        }

        public void ConnectToServer()
        {
            //TcpListener l = new TcpListener(IPAddress.Parse(adres), port); //и на нем у нас висит сервер
            
            TcpClient client = new TcpClient(adres, port); //IP адрес сервера и порт на котором он висит
            NetworkStream NWS = client.GetStream();
            BinaryReader R = new BinaryReader(NWS); //поток для принятия данных

            var rawSource = new RawSourceWaveStream(NWS, waveFormat);

            waveOut.Init(rawSource);
            waveOut.Play();
            //BinaryWriter W = new BinaryWriter(NWS); //поток для отправки данных


        }

        public void InitilizeElements()
        {
            
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            ConnectToServer();
            //listnerThread.Start();
        }

        private void StartListener()
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
