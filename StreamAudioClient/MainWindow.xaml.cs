using NAudio.Dsp;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        WaveOut waveOut;
        WaveFormat waveFormat;
        MeteringSampleProvider metering;

        [DefaultValue(-60.0)]
        public float MinDb { get; set; }

        [DefaultValue(18.0)]
        public float MaxDb { get; set; }


        public MainWindow()
        {
            InitializeComponent();

            waveOut = new WaveOut();
            waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(44100, 2);

            MinDb = -60;
            MaxDb = 18;


        }

        public void ConnectToServer()
        {
            TcpClient client = new TcpClient(FieldIpAddress.Text, port); //IP адрес сервера и порт на котором он висит
            
            var rawSource = new RawSourceWaveStream(client.GetStream(), waveFormat);
            var sampleProvider = rawSource.ToSampleProvider();
            
            
            metering = new MeteringSampleProvider(sampleProvider);
            metering.StreamVolume += realVolume;
            waveOut.Init(metering);
            waveOut.Play();
        }

        private void realVolume(object sender, StreamVolumeEventArgs e)
        {
            leftChannelBar.Value = 100 * CountBbPercent(e.MaxSampleValues[0]);
            rightChannelBar.Value = 100 * CountBbPercent(e.MaxSampleValues[1]);
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            ConnectToServer();
        }

        private double CountBbPercent(float Amplitude)
        {
            double db = 20 * Math.Log10(Amplitude);

            if (db < MinDb)
                db = MinDb;
            if (db > MaxDb)
                db = MaxDb;

            return (db - MinDb) / (MaxDb - MinDb);
        } 
    }
}
