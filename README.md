Certainly! Here's a `README.md` file for your project, "VideoBroadcast":

---
# VideoBroadcast

VideoBroadcast is a Client-Server application written in C# targeting .NET 4.6.1. It focuses on video broadcasting via protocols like RTP/RTSP or HTTP, leveraging the power of the VLC library and the VlcDotNet wrapper.

## Description
This repository comprises:

- A simple console-based server application.
- A desktop Windows Forms server application.
- A desktop Windows Forms client application.

The core of video broadcasting is managed using the `libvlc` library from VLC and the `VlcDotNet` wrapper by ZeBobo5. The applications have been tested against `libvlc` versions 3.0.3, 3.0.4, and the latest development Nuget from ZeBobo5: version 3.0.0-develop296.

## Setup and Configuration

1. **Library Placement**: Ensure you copy `libvlc` -> `win-x86` or `win-x64` to your project's respective `bin/Debug` or `bin/Release` folder.
   
2. **Broadcast Configuration**:
   - In the console server application, the broadcasting can be configured in `Program.cs`:
     ```csharp
     var mediaOptions = new[]
     {
         ":sout=#rtp{sdp=rtsp://192.168.1.162:8008/test}",
         ":sout-keep"
     };
     ```
   - For the desktop application located in the `VlcBroadcast` folder (`MainForm.cs`), you can configure it as:
     ```csharp
     var mediaOptions = new[]
     {
         ":sout=#transcode{vcodec=h264,acodec=mp3,ab=128,channels=2,samplerate=44100}:http{mux=ffmpeg{mux=flv},dst=:8080/}",
         ":sout-all"
     };
     ```

## To-Do List
- [ ] Enable more flexible customization options, replacing hardcoded values.
- [ ] Undertake code refactoring for better maintainability.
- [ ] Upgrade to the latest .NET version.

## Application Runtime
(Here, you can provide information on how to run the application, any runtime prerequisites, and other related details.)

---

I've provided a basic structure based on the details you've shared. Feel free to expand and adjust based on the specific needs and intricacies of your project.
### Application runtime:


![](https://habrastorage.org/webt/ky/ws/63/kyws63umuabcf1bmpfptecllhxw.png)

