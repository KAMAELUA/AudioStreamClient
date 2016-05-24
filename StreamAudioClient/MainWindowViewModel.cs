using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.ComponentModel;
using System.Net.Sockets;
using System.Windows.Input;

namespace StreamAudioClient
{
    public class MainWindowViewModel : BasicViewModel
    {
        ICommand connectCommand;
        private double leftChannelBarValue;
        private double rightChannelBarValue;

        private int port = 11001; //умолчание

        private WaveOut waveOut;
        private WaveFormat waveFormat;
        private MeteringSampleProvider metering;

        private string ipAddress = "192.168.0.102";

        public string IpAddress
        {
            get
            {
                return ipAddress;
            }
            set
            {
                ipAddress = value;
                RisePropertyChanged("IpAddress");
            }
        }

        public MainWindowViewModel()
        {
            waveOut = new WaveOut();
            waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(44100, 2);

            MinDb = -60;
            MaxDb = 18;
        }


        public double LeftChannelDbValue
        {
            get;
        }

        public double RightChannelDbValue
        {
            get;
        }

        public double LeftChannelLoadValue
        {
            get { return leftChannelBarValue; }
            set
            {
                leftChannelBarValue = value;
                RisePropertyChanged("LeftChannelLoadValue");
            }
        }

        public double RightChannelLoadValue
        {
            get { return rightChannelBarValue; }
            set
            {
                rightChannelBarValue = value;
                RisePropertyChanged("RightChannelLoadValue");
            }
        }

        public ICommand ConnectCommand
        {
            get
            {
                if (connectCommand == null)
                {
                    connectCommand = new Command(ConnectToServer);
                }
                return connectCommand;
            }
        }

        [DefaultValue(-60.0)]
        private float MinDb { get; set; }

        [DefaultValue(18.0)]
        private float MaxDb { get; set; }

        public async void ConnectToServer(object o)
        {
            TcpClient client = new TcpClient(ipAddress, port); //IP адрес сервера и порт на котором он висит

            var rawSource = new RawSourceWaveStream(client.GetStream(), waveFormat);
            var sampleProvider = rawSource.ToSampleProvider();

            metering = new MeteringSampleProvider(sampleProvider);
            metering.StreamVolume += realVolume;
            waveOut.Init(metering);
            waveOut.Play();
        }

        private void realVolume(object sender, StreamVolumeEventArgs e)
        {
            LeftChannelLoadValue = 100 * CountBbPercent(e.MaxSampleValues[0]);
            RightChannelLoadValue = 100 * CountBbPercent(e.MaxSampleValues[1]);
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
