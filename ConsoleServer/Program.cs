using System;
using System.IO;
using System.Reflection;
using Vlc.DotNet.Core;

namespace ConsoleServer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var file = new FileInfo(@"path");

            var currentDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);

            // Default installation path of VideoLAN.LibVLC.Windows
            var libDirectory =
                new DirectoryInfo(Path.Combine(currentDirectory, "libvlc", IntPtr.Size == 4 ? "win-x86" : "win-x64"));

            using (var mediaPlayer = new VlcMediaPlayer(libDirectory))
            {
                // Options without any encode
                var mediaOptions = new[]
                {
                    ":sout=#rtp{sdp=rtsp://192.168.1.162:8008/test}",
                    ":sout-keep"
                };

                mediaPlayer.SetMedia(file, mediaOptions);

                mediaPlayer.Play();

                Console.WriteLine("Streaming on rtsp://192.168.1.162:8008/test");
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
        }
    }
}