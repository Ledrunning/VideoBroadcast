using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Win32;

namespace VlcBroadcast
{
    /// <summary>
    ///     Класс для работы с VLC API
    ///     Необходимо скопировать файлы в папку бин релиза или дебага из папки бин враппера libvlc.dll из папки с
    ///     установленным
    ///     VLC плеером в папку Windows -> System32.
    /// </summary>
    [Obsolete]
    internal class LibVlc : IDisposable
    {
        public LibVlc()
        {
            VlcInstallDir = QueryVlcInstallPath();
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (m_iVlcHandle != -1)
                try
                {
                    VLC_CleanUp(m_iVlcHandle);
                    VLC_Destroy(m_iVlcHandle);
                    VideoOutput = null;
                }
                catch
                {
                }

            m_iVlcHandle = -1;
        }

        #endregion


        #region PRIVATE METHODS

        private string QueryVlcInstallPath()
        {
            // open registry
            var regkeyVlcInstallPathKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\VideoLAN\VLC");
            if (regkeyVlcInstallPathKey == null)
                return "";
            return (string) regkeyVlcInstallPathKey.GetValue("InstallDir", "");
        }

        #endregion

        #region EVENT METHODS

        private void wndOutput_Resize(object sender, EventArgs e)
        {
            if (m_iVlcHandle != -1)
            {
                SetVariable("conf::width", m_wndOutput.ClientRectangle.Width);
                SetVariable("conf::height", m_wndOutput.ClientRectangle.Height);
            }
        }

        #endregion

        #region public structs

        [StructLayout(LayoutKind.Explicit)]
        public struct vlc_value_t
        {
            [FieldOffset(0)] public int i_int;
            [FieldOffset(0)] public int b_bool;
            [FieldOffset(0)] public float f_float;
            [FieldOffset(0)] public IntPtr psz_string;
            [FieldOffset(0)] public IntPtr p_address;
            [FieldOffset(0)] public IntPtr p_object;
            [FieldOffset(0)] public IntPtr p_list;
            [FieldOffset(0)] public long i_time;
            [FieldOffset(0)] public IntPtr psz_name;
            [FieldOffset(4)] public int i_object_id;
        }

        #endregion

        #region public enums

        public enum Error
        {
            Success = -0,
            NoMem = -1,
            Thread = -2,
            Timeout = -3,
            NoMod = -10,
            NoObj = -20,
            BadObj = -21,
            NoVar = -30,
            BadVar = -31,
            Exit = -255,
            Generic = -666,
            Execption = -998,
            NoInit = -999
        }

        private enum Mode
        {
            Insert = 0x01,
            Replace = 0x02,
            Append = 0x04,
            Go = 0x08,
            CheckInsert = 0x10
        }

        private enum Pos
        {
            End = -666
        }

        #endregion

        #region libvlc api

        [DllImport("libvlc")]
        private static extern int VLC_Create();

        [DllImport("libvlc")]
        private static extern Error VLC_Init(int iVLC, int Argc, string[] Argv);

        [DllImport("libvlc")]
        private static extern Error VLC_AddIntf(int iVLC, string Name, bool Block, bool Play);

        [DllImport("libvlc")]
        private static extern Error VLC_Die(int iVLC);

        [DllImport("libvlc")]
        private static extern string VLC_Error();

        [DllImport("libvlc")]
        private static extern string VLC_Version();

        [DllImport("libvlc")]
        private static extern Error VLC_CleanUp(int iVLC);

        [DllImport("libvlc")]
        private static extern Error VLC_Destroy(int iVLC);

        [DllImport("libvlc")]
        private static extern Error VLC_AddTarget(int iVLC, string Target, string[] Options, int OptionsCount, int Mode,
            int Pos);

        [DllImport("libvlc")]
        private static extern Error VLC_Play(int iVLC);

        [DllImport("libvlc")]
        private static extern Error VLC_Pause(int iVLC);

        [DllImport("libvlc")]
        private static extern Error VLC_Stop(int iVLC);

        [DllImport("libvlc")]
        private static extern bool VLC_IsPlaying(int iVLC);

        [DllImport("libvlc")]
        private static extern float VLC_PositionGet(int iVLC);

        [DllImport("libvlc")]
        private static extern float VLC_PositionSet(int iVLC, float Pos);

        [DllImport("libvlc")]
        private static extern int VLC_TimeGet(int iVLC);

        [DllImport("libvlc")]
        private static extern Error VLC_TimeSet(int iVLC, int Seconds, bool Relative);

        [DllImport("libvlc")]
        private static extern int VLC_LengthGet(int iVLC);

        [DllImport("libvlc")]
        private static extern float VLC_SpeedFaster(int iVLC);

        [DllImport("libvlc")]
        private static extern float VLC_SpeedSlower(int iVLC);

        [DllImport("libvlc")]
        private static extern int VLC_PlaylistIndex(int iVLC);

        [DllImport("libvlc")]
        private static extern int VLC_PlaylistNumberOfItems(int iVLC);

        [DllImport("libvlc")]
        private static extern Error VLC_PlaylistNext(int iVLC);

        [DllImport("libvlc")]
        private static extern Error VLC_PlaylistPrev(int iVLC);

        [DllImport("libvlc")]
        private static extern Error VLC_PlaylistClear(int iVLC);

        [DllImport("libvlc")]
        private static extern int VLC_VolumeSet(int iVLC, int Volume);

        [DllImport("libvlc")]
        private static extern int VLC_VolumeGet(int iVLC);

        [DllImport("libvlc")]
        private static extern Error VLC_VolumeMute(int iVLC);

        [DllImport("libvlc")]
        private static extern Error VLC_FullScreen(int iVLC);

        [DllImport("libvlc")]
        private static extern Error VLC_VariableType(int iVLC, string Name, ref int iType);

        [DllImport("libvlc")]
        private static extern Error VLC_VariableSet(int iVLC, string Name, vlc_value_t value);

        [DllImport("libvlc")]
        private static extern Error VLC_VariableGet(int iVLC, string Name, ref vlc_value_t value);

        [DllImport("libvlc")]
        private static extern string VLC_Error(int i_err);

        #endregion

        #region local members

        private int m_iVlcHandle = -1;
        private Control m_wndOutput;

        #endregion

        #region PUBLIC PROPERTIES

        public string VlcInstallDir { get; set; } = "";

        public bool IsInitialized => m_iVlcHandle != -1;

        public Control VideoOutput
        {
            get => m_wndOutput;
            set
            {
                // clear old window
                if (m_wndOutput != null)
                {
                    m_wndOutput.Resize -= wndOutput_Resize;
                    m_wndOutput = null;
                    if (m_iVlcHandle != -1)
                        SetVariable("drawable", 0);
                }

                // set new
                m_wndOutput = value;
                if (m_wndOutput != null)
                {
                    if (m_iVlcHandle != -1)
                        SetVariable("drawable", m_wndOutput.Handle.ToInt32());
                    m_wndOutput.Resize += wndOutput_Resize;
                    wndOutput_Resize(null, null);
                }
            }
        }

        public string LastError { get; private set; } = "";

        public bool IsPlaying
        {
            get
            {
                if (m_iVlcHandle == -1)
                {
                    LastError = "LibVlc is not initialzed";
                    return false;
                }

                try
                {
                    return VLC_IsPlaying(m_iVlcHandle);
                }
                catch (Exception ex)
                {
                    LastError = ex.Message;
                    return false;
                }
            }
        }

        public int LengthGet
        {
            get
            {
                if (m_iVlcHandle == -1)
                {
                    LastError = "LibVlc is not initialzed";
                    return -1;
                }

                try
                {
                    return VLC_LengthGet(m_iVlcHandle);
                }
                catch (Exception ex)
                {
                    LastError = ex.Message;
                    return -1;
                }
            }
        }

        public int TimeGet
        {
            get
            {
                if (m_iVlcHandle == -1)
                {
                    LastError = "LibVlc is not initialzed";
                    return -1;
                }

                try
                {
                    return VLC_TimeGet(m_iVlcHandle);
                }
                catch (Exception ex)
                {
                    LastError = ex.Message;
                    return -1;
                }
            }
        }

        public float PositionGet
        {
            get
            {
                if (m_iVlcHandle == -1)
                {
                    LastError = "LibVlc is not initialzed";
                    return -1;
                }

                try
                {
                    return VLC_PositionGet(m_iVlcHandle);
                }
                catch (Exception ex)
                {
                    LastError = ex.Message;
                    return -1;
                }
            }
        }

        public int VolumeGet
        {
            get
            {
                if (m_iVlcHandle == -1)
                {
                    LastError = "LibVlc is not initialzed";
                    return -1;
                }

                try
                {
                    return VLC_VolumeGet(m_iVlcHandle);
                }
                catch (Exception ex)
                {
                    LastError = ex.Message;
                    return -1;
                }
            }
        }

        public bool Fullscreen
        {
            get
            {
                var iIsFullScreen = 0;
                if (GetVariable("fullscreen", ref iIsFullScreen) == Error.Success)
                    if (iIsFullScreen != 0)
                        return true;
                return false;
            }
            set
            {
                var iFullScreen = value ? 1 : 0;
                ;
                SetVariable("fullscreen", iFullScreen);
            }
        }

        #endregion


        #region PUBLIC METHODS

        public bool Initialize()
        {
            // check if already initializes
            if (m_iVlcHandle != -1)
                return true;

            // try init
            try
            {
                // create instance
                m_iVlcHandle = VLC_Create();
                if (m_iVlcHandle < 0)
                {
                    LastError = "Failed to create VLC instance";
                    return false;
                }

                // make init optinons
                string[] strInitOptions =
                {
                    "vlc",
                    "--no-one-instance",
                    "--no-loop",
                    "--no-drop-late-frames",
                    "--disable-screensaver"
                };
                if (VlcInstallDir.Length > 0)
                    strInitOptions[0] = VlcInstallDir + @"\vlc";

                // init libvlc
                var errVlcLib = VLC_Init(m_iVlcHandle, strInitOptions.Length, strInitOptions);
                if (errVlcLib != Error.Success)
                {
                    VLC_Destroy(m_iVlcHandle);
                    LastError = "Failed to initialise VLC";
                    m_iVlcHandle = -1;
                    return false;
                }
            }
            catch
            {
                LastError = "Could not find libvlc";
                return false;
            }

            // check output window
            if (m_wndOutput != null)
            {
                SetVariable("drawable", m_wndOutput.Handle.ToInt32());
                wndOutput_Resize(null, null);
            }

            // OK
            return true;
        }


        public Error AddTarget(string Target)
        {
            if (m_iVlcHandle == -1)
            {
                LastError = "LibVlc is not initialzed";
                return Error.NoInit;
            }

            var enmErr = Error.Success;
            try
            {
                enmErr = VLC_AddTarget(m_iVlcHandle,
                    Target,
                    null,
                    0,
                    (int) Mode.Append,
                    (int) Pos.End);
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                return Error.Execption;
            }

            if ((int) enmErr < 0)
            {
                LastError = VLC_Error((int) enmErr);
                return enmErr;
            }

            // OK
            return Error.Success;
        }

        public Error AddTarget(string Target, string[] Options)
        {
            if (m_iVlcHandle == -1)
            {
                LastError = "LibVlc is not initialzed";
                return Error.NoInit;
            }

            // check options
            var iOptionsCount = 0;
            if (Options != null)
                iOptionsCount = Options.Length;
            // add
            var enmErr = Error.Success;
            try
            {
                enmErr = VLC_AddTarget(m_iVlcHandle,
                    Target,
                    Options,
                    iOptionsCount,
                    (int) Mode.Append,
                    (int) Pos.End);
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                return Error.Execption;
            }

            if ((int) enmErr < 0)
            {
                LastError = VLC_Error((int) enmErr);
                return enmErr;
            }

            // OK
            return Error.Success;
        }

        public Error Play()
        {
            if (m_iVlcHandle == -1)
            {
                LastError = "LibVlc is not initialzed";
                return Error.NoInit;
            }

            var enmErr = Error.Success;
            try
            {
                enmErr = VLC_Play(m_iVlcHandle);
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                return Error.Execption;
            }

            if ((int) enmErr < 0)
            {
                LastError = VLC_Error((int) enmErr);
                return enmErr;
            }

            // OK
            return Error.Success;
        }

        public Error Pause()
        {
            if (m_iVlcHandle == -1)
            {
                LastError = "LibVlc is not initialzed";
                return Error.NoInit;
            }

            var enmErr = Error.Success;
            try
            {
                enmErr = VLC_Pause(m_iVlcHandle);
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                return Error.Execption;
            }

            if ((int) enmErr < 0)
            {
                LastError = VLC_Error((int) enmErr);
                return enmErr;
            }

            // OK
            return Error.Success;
        }

        public Error Stop()
        {
            if (m_iVlcHandle == -1)
            {
                LastError = "LibVlc is not initialzed";
                return Error.NoInit;
            }

            var enmErr = Error.Success;
            try
            {
                enmErr = VLC_Stop(m_iVlcHandle);
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                return Error.Execption;
            }

            if ((int) enmErr < 0)
            {
                LastError = VLC_Error((int) enmErr);
                return enmErr;
            }

            // OK
            return Error.Success;
        }

        public float SpeedFaster()
        {
            if (m_iVlcHandle == -1)
            {
                LastError = "LibVlc is not initialzed";
                return -1;
            }

            var enmErr = Error.Success;
            try
            {
                return VLC_SpeedFaster(m_iVlcHandle);
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                return -1;
            }
        }

        public float SpeedSlower()
        {
            if (m_iVlcHandle == -1)
            {
                LastError = "LibVlc is not initialzed";
                return -1;
            }

            var enmErr = Error.Success;
            try
            {
                return VLC_SpeedSlower(m_iVlcHandle);
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                return -1;
            }
        }

        public Error PlaylistNext()
        {
            if (m_iVlcHandle == -1)
            {
                LastError = "LibVlc is not initialzed";
                return Error.NoInit;
            }

            var enmErr = Error.Success;
            try
            {
                enmErr = VLC_PlaylistNext(m_iVlcHandle);
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                return Error.Execption;
            }

            if ((int) enmErr < 0)
            {
                LastError = VLC_Error((int) enmErr);
                return enmErr;
            }

            // OK
            return Error.Success;
        }

        public Error PlaylistPrevious()
        {
            if (m_iVlcHandle == -1)
            {
                LastError = "LibVlc is not initialzed";
                return Error.NoInit;
            }

            var enmErr = Error.Success;
            try
            {
                enmErr = VLC_PlaylistPrev(m_iVlcHandle);
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                return Error.Execption;
            }

            if ((int) enmErr < 0)
            {
                LastError = VLC_Error((int) enmErr);
                return enmErr;
            }

            // OK
            return Error.Success;
        }

        public Error PlaylistClear()
        {
            if (m_iVlcHandle == -1)
            {
                LastError = "LibVlc is not initialzed";
                return Error.NoInit;
            }

            var enmErr = Error.Success;
            try
            {
                enmErr = VLC_PlaylistClear(m_iVlcHandle);
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                return Error.Execption;
            }

            if ((int) enmErr < 0)
            {
                LastError = VLC_Error((int) enmErr);
                return enmErr;
            }

            // OK
            return Error.Success;
        }

        public Error TimeSet(int newPosition, bool bRelative)
        {
            if (m_iVlcHandle == -1)
            {
                LastError = "LibVlc is not initialzed";
                return Error.NoInit;
            }

            var enmErr = Error.Success;
            try
            {
                enmErr = VLC_TimeSet(m_iVlcHandle, newPosition, bRelative);
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                return Error.Execption;
            }

            if ((int) enmErr < 0)
            {
                LastError = VLC_Error((int) enmErr);
                return enmErr;
            }

            // OK
            return Error.Success;
        }

        public float PositionSet(float newPosition)
        {
            if (m_iVlcHandle == -1)
            {
                LastError = "LibVlc is not initialzed";
                return -1;
            }

            try
            {
                return VLC_PositionSet(m_iVlcHandle, newPosition);
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                return -1;
            }
        }

        public int VolumeSet(int newVolume)
        {
            if (m_iVlcHandle == -1)
            {
                LastError = "LibVlc is not initialzed";
                return -1;
            }

            var enmErr = Error.Success;
            try
            {
                return VLC_VolumeSet(m_iVlcHandle, newVolume);
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                return -1;
            }
        }

        public Error VolumeMute()
        {
            if (m_iVlcHandle == -1)
            {
                LastError = "LibVlc is not initialzed";
                return Error.NoInit;
            }

            var enmErr = Error.Success;
            try
            {
                enmErr = VLC_VolumeMute(m_iVlcHandle);
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                return Error.Execption;
            }

            if ((int) enmErr < 0)
            {
                LastError = VLC_Error((int) enmErr);
                return enmErr;
            }

            // OK
            return Error.Success;
        }

        public Error SetVariable(string strName, int Value)
        {
            if (m_iVlcHandle == -1)
            {
                LastError = "LibVlc is not initialzed";
                return Error.NoInit;
            }

            var enmErr = Error.Success;
            try
            {
                // create vlc value
                var val = new vlc_value_t();
                val.i_int = Value;
                // set variable
                enmErr = VLC_VariableSet(m_iVlcHandle, strName, val);
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                return Error.Execption;
            }

            if ((int) enmErr < 0)
            {
                LastError = VLC_Error((int) enmErr);
                return enmErr;
            }

            // OK
            return Error.Success;
        }

        public Error GetVariable(string strName, ref int Value)
        {
            if (m_iVlcHandle == -1)
            {
                LastError = "LibVlc is not initialzed";
                return Error.NoInit;
            }


            var enmErr = Error.Success;
            try
            {
                // create vlc value
                var val = new vlc_value_t();
                // set variable
                enmErr = VLC_VariableGet(m_iVlcHandle, strName, ref val);
                Value = val.i_int;
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                return Error.Execption;
            }

            if ((int) enmErr < 0)
            {
                LastError = VLC_Error((int) enmErr);
                return enmErr;
            }

            // OK
            return Error.Success;
        }

        public Error ToggleFullscreen()
        {
            if (m_iVlcHandle == -1)
            {
                LastError = "LibVlc is not initialzed";
                return Error.NoInit;
            }

            var enmErr = Error.Success;
            try
            {
                enmErr = VLC_FullScreen(m_iVlcHandle);
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                return Error.Execption;
            }

            if ((int) enmErr < 0)
            {
                LastError = VLC_Error((int) enmErr);
                return enmErr;
            }

            // OK
            return Error.Success;
        }

        public Error PressKey(string strKey)
        {
            if (m_iVlcHandle == -1)
            {
                LastError = "LibVlc is not initialzed";
                return Error.NoInit;
            }

            var enmErr = Error.Success;
            try
            {
                // create vlc value
                var valKey = new vlc_value_t();
                // get variable
                enmErr = VLC_VariableGet(m_iVlcHandle, strKey, ref valKey);
                if (enmErr == Error.Success) enmErr = VLC_VariableSet(m_iVlcHandle, "key-pressed", valKey);
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                return Error.Execption;
            }

            if ((int) enmErr < 0)
            {
                LastError = VLC_Error((int) enmErr);
                return enmErr;
            }

            // OK
            return Error.Success;
        }

        #endregion
    }
}