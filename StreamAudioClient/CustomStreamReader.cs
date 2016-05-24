using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StreamAudioClient
{
    class CustomStreamReader
    {
        NetworkStream stream;
        public event DataAvalaibleEvnt DataAvalaible;
        public delegate void DataAvalaibleEvnt(StreamDataAvalaible e);
        Thread updaterThread;
        BinaryReader sr;
        byte[] buffer = new byte[4096];

        public CustomStreamReader(NetworkStream stream)
        {
            this.stream = stream;
            sr = new BinaryReader(stream);
            updaterThread = new Thread(checkData);
            updaterThread.Start();
        }

        public NetworkStream SourceStream
        {
            get
            {
                return stream;
            }
        }

        private void checkData()
        {
            NetworkStream ns;
            
            while (true)
            {
                if (stream.DataAvailable)
                    {
                    int byteCount = sr.Read(buffer, 0, 4096);
                    if (byteCount > 0)
                    {
                        byte[] tmpArray = new byte[byteCount];
                        Array.Copy(buffer, tmpArray, byteCount);
                        DataAvalaible?.Invoke(new StreamDataAvalaible() { data = tmpArray, dataLength = byteCount });
                    }
                }
            }
        }

    }

    class StreamDataAvalaible : EventArgs
    {
        public byte[] data;
        public int dataLength;
    }
}
