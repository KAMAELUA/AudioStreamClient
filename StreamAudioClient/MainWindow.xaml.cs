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
        //public int port = 11001; //умолчание
        //public string adres = "127.0.0.1"; //умолчание
        WaveOut waveOut;

        WaveFormat waveFormat;

        IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, 11001);

        public MainWindow()
        {
            InitializeComponent();

            waveOut = new WaveOut();
            waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(44100, 2);
        }

        public void ConnectToServer(string address, string port)
        {

            TcpClient client = new TcpClient(address, Int32.Parse(port)); //IP адрес сервера и порт на котором он висит
            NetworkStream NWS = client.GetStream();
            BinaryReader R = new BinaryReader(NWS); //поток для принятия данных

            var rawSource = new RawSourceWaveStream(NWS, waveFormat);

            waveOut.Init(rawSource);
            waveOut.Play();
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            ConnectToServer(IpAddress.Text, Port.Text);
        }
    }
}
