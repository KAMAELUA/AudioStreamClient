﻿using NAudio.Dsp;
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
        //public string adres = "192.168.0.102"; //умолчание
        WaveOut waveOut;
        StreamWriter sw;
        Stream stream;
        BinaryWriter bw;
        WaveFormat waveFormat;
        private MemoryStream audioMempryStream;

        //IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, 11001);

        public MainWindow()
        {
            InitializeComponent();

            waveOut = new WaveOut();
            waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(44100, 2);
            
            
        }

        public void ConnectToServer()
        {
            TcpClient client = new TcpClient(FieldIpAddress.Text, port); //IP адрес сервера и порт на котором он висит
            /*NetworkStream NWS = client.GetStream();
            NWS.
            BinaryReader R = new BinaryReader(NWS); //поток для принятия данных
            var rawSource = new RawSourceWaveStream(NWS, waveFormat);
            
            waveOut.Init(rawSource);
            
            waveOut.Play();*/
            stream = new MemoryStream();
            sw = new StreamWriter(stream);
            bw = new BinaryWriter(stream);
            
            CustomStreamReader csw = new CustomStreamReader(client.GetStream());
            //csw.DataAvalaible += StreamDataAvalaible;

            SampleAggregator sa = new SampleAggregator();
            

            var rawSource = new RawSourceWaveStream(client.GetStream(), waveFormat);
            waveOut.Init(rawSource);
            
            waveOut.Play();
        }

        private void StreamDataAvalaible(StreamDataAvalaible e)
        {
            bw.Write(e.data);
            bw.Flush();
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            ConnectToServer();
        }

        private void FieldIpAddress_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
