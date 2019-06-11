using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Vlc.DotNet.Forms;

namespace VlcClient
{
    public partial class VideoStreamClient : Form
    {
        public VideoStreamClient()
        {
            InitializeComponent();
            //vlcControl.Play(new Uri("http://download.blender.org/peach/bigbuckbunny_movies/big_buck_bunny_480p_surround-fix.avi"));
            var url = new Uri("http://192.168.1.162:8080");

            vlcControl.Play(url);
            Debug.WriteLine(url);
        }


        /// <summary>
        ///     When the Vlc control needs to find the location of the libvlc.dll.
        ///     You could have set the VlcLibDirectory in the designer, but for this sample, we are in AnyCPU mode, and we don't
        ///     know the process bitness.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VlcLibDirectoryNeededHandler(object sender, VlcLibDirectoryNeededEventArgs e)
        {
            var currentAssembly = Assembly.GetEntryAssembly();
            if (currentAssembly != null)
            {
                var currentDirectory = new FileInfo(currentAssembly.Location).DirectoryName;
                // Default installation path of VideoLAN.LibVLC.Windows
                if (currentDirectory != null)
                    e.VlcLibDirectory =
                        new DirectoryInfo(Path.Combine(currentDirectory, "libvlc",
                            IntPtr.Size == 4 ? "win-x86" : "win-x64"));
            }
        }
    }
}