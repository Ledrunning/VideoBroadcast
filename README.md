# VideoBroadcast 
## Client-Server application written on C# .NET 4.6.1

## Attention!
At the moment the project is outdated, as the author of the library stopped support and asked to move to the current versions!
> As more and more effort is put into LibVLCSharp, fewer evolutions are expected to be made to this project.
> Bugfixes will still be fixed in the maintainer's free time, but as of 2020-05-06, support for libvlc 4 is not planned. You are encouraged to migrate and create new projects with LibVLCSharp

### Description:

The repository includes simple console server application desktop Windows Forms server application 

and desktop Windows Forms application.

Video broadcasting via RTP/RTSP or HTTP using VLC library *libvlc* and VlcDotNet wrapper
from [ZeBobo5](https://github.com/ZeBobo5/Vlc.DotNet).

All apps were tested with libvlc 3.0.3, 3.0.4, and last dev Nuget from ZeBobo5 - 3.0.0-develop296 versions.

### Setup and configuration:

It is necessary to copy libvlc -> win-x86 | win-x64 to your project's bin Debug or Release folder.

Broadcasting is configured on the server side f.e in **Program.cs** with the following parameters:

```
// Options with any encoders
var mediaOptions = new[]
{
  ":sout=#rtp{sdp=rtsp://192.168.1.162:8008/test}",
  ":sout-keep"
};
```

or in desktop application in **MainForm.cs** wich is located in **VlcBroadcast folder**:

```
// Options with video and audio encoders
var mediaOptions = new[]
{
  ":sout=#transcode{vcodec=h264,acodec=mp3,ab=128,channels=2,samplerate=44100}:http{mux=ffmpeg{mux=flv},dst=:8080/}",
   ":sout-all"
};
```

### Application runtime:

![](https://habrastorage.org/webt/ky/ws/63/kyws63umuabcf1bmpfptecllhxw.png)

