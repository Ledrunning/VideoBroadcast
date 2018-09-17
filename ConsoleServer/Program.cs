using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace ConsoleServer
{
	class Program
	{
		static void Main(string[] args)
		{
			FileInfo file = new FileInfo(@"path");

			var currentDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
			
			// Default installation path of VideoLAN.LibVLC.Windows
			var libDirectory = new DirectoryInfo(Path.Combine(currentDirectory, "libvlc", IntPtr.Size == 4 ? "win-x86" : "win-x64"));

			using (var mediaPlayer = new Vlc.DotNet.Core.VlcMediaPlayer(libDirectory))
			{
				// Options without any encode
				var mediaOptions = new[]
				{
					":sout=#rtp{sdp=rtsp://192.168.1.162:8008/test}",
					":sout-keep"
				};

				// mediaPlayer.SetMedia(new Uri("http://hls1.addictradio.net/addictrock_aac_hls/playlist.m3u8"),
				//    mediaOptions);

				mediaPlayer.SetMedia(file, mediaOptions);

				mediaPlayer.Play();

				Console.WriteLine("Streaming on rtsp://192.168.1.162:8008/test");
				Console.WriteLine("Press any key to exit");
				Console.ReadKey();
			}
		}
	}
}
