using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Vlc.DotNet.Core;

namespace VlcBroadcast
{
    public partial class VideoServer : Form
    {
        private string _currentDirectory;
        private FileInfo _file;
        private DirectoryInfo _libDirectory;
        private VlcMediaPlayer _mediaPlayer;

        public VideoServer()
        {
            InitializeComponent();
            VlcInitialize();
        }

        private void VlcInitialize()
        {
            _currentDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            // Default installation path of VideoLAN.LibVLC.Windows
            // You should copy it into bin Debug or Release folder.
            _libDirectory =
                new DirectoryInfo(Path.Combine(_currentDirectory, "libvlc", IntPtr.Size == 4 ? "win-x86" : "win-x64"));
            _file = new FileInfo(@"your path to file");
        }

        private void OnStartClick(object sender, EventArgs e)
        {
            btnStop.Enabled = true;
            btnStart.Enabled = false;

            try
            {
                _mediaPlayer = new VlcMediaPlayer(_libDirectory);

                #region RTSP option

                //var mediaOptions = new[]
                //{
                //    ":sout=#rtp{sdp=rtsp://192.168.1.162:8008/test}",
                //    ":sout-keep"
                //};

                #endregion

                #region HTTP option

                // Options with video and audio encoders/
                var mediaOptions = new[]
                {
                    ":sout=#transcode{vcodec=h264,acodec=mp3,ab=128,channels=2,samplerate=44100}:http{mux=ffmpeg{mux=flv},dst=:8080/}",
                    ":sout-all"
                };

                #endregion

                _mediaPlayer.SetMedia(_file, mediaOptions);
                _mediaPlayer.Play();

                Debug.WriteLine($"Streaming is started on http://192.168.1.162:8080/ {DateTime.Now}");
            }
            catch (Exception exception)
            {
                Debug.WriteLine($"Error : {exception}");
            }
        }

        private void OnStopClick(object sender, EventArgs e)
        {
            btnStart.Enabled = true;
            btnStop.Enabled = false;

            try
            {
                _mediaPlayer.Stop();
                _mediaPlayer.Dispose();
            }
            catch (Exception exception)
            {
                Debug.WriteLine($"Error: {exception}");
            }
        }
    }
}