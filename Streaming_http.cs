using NAudio.Wave;
using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Organizer
{
    class Streaming_http
    {
        Organizer_MainView Omv;
        Stream MemoryStream;

        public Streaming_http(Organizer_MainView omv)
        {
            Omv = omv;
        }

        public void Run()
        {
            Thread buffering = new Thread(BufferingAsync);
            Thread playback = new Thread(Playback);

            buffering.Start();
            playback.Start();
        }

        private async void BufferingAsync()
        {
            MemoryStream = null;
            MemoryStream = new MemoryStream();

            try
            {
                //WebRequest request = null;
                WebResponse response = null;
                Stream stream = null;


                Omv.State = "Buffering...";

                response = await WebRequest.Create(Omv.Url).GetResponseAsync();

                using (stream = response.GetResponseStream())
                {
                    byte[] buffer = new byte[65536];
                    int read;
                    while ((read = stream.Read(buffer, 0, buffer.Length)) > 0 && !Omv.Close_stream)
                    {
                        var pos = MemoryStream.Position;
                        MemoryStream.Position = MemoryStream.Length;
                        MemoryStream.Write(buffer, 0, read);
                        MemoryStream.Position = pos;

                        Omv.Received_bytes = MemoryStream.Length.ToString();
                        Omv.Streamposition = MemoryStream.Position.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        void Playback()
        {
            while (MemoryStream.Length < 65536)
            {
                Thread.Sleep(1000);
            }

            MemoryStream.Position = 0;


            using (WaveStream blockAlignedStream = new BlockAlignReductionStream(WaveFormatConversionStream.CreatePcmStream(new Mp3FileReader(MemoryStream))))
            {
                using (WaveOut waveOut = new WaveOut(WaveCallbackInfo.FunctionCallback()))
                {
                    Omv.State = "Playing:";
                    waveOut.Init(blockAlignedStream);
                    waveOut.Play();
                    while (waveOut.PlaybackState == PlaybackState.Playing && !Omv.Close_stream)
                    {
                        Thread.Sleep(100);
                    }
                }
            }
        }
    }
}