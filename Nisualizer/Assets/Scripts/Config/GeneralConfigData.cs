using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NnUtils.Scripts;
using Scripts.Core;
using UnityEngine;

namespace Scripts.Config
{
    [CreateAssetMenu(fileName = "GeneralConfigData", menuName = "Config/GeneralConfigData")]
    public class GeneralConfigData : ConfigData
    {
        // Delays the config reload to avoid multiple reloads and config overwrites
        [JsonIgnore] public float DefaultReloadDelay = 0.1f;
        [ReadOnly] public float ReloadDelay;
        
        // FPS of the app
        [JsonIgnore] public int DefaultFPS = 60;
        [ReadOnly] public int FPS;

        // Window mode of the app
        [JsonIgnore] public FullScreenMode DefaultWindowMode = FullScreenMode.Windowed;
        [JsonConverter(typeof(StringEnumConverter))]
        [ReadOnly] public FullScreenMode WindowMode;

        // Whether the app window will be sent to the background layer
        // Windows only
        [JsonIgnore] public WindowLayer DefaultWindowLayer = WindowLayer.Background;
        [JsonConverter(typeof(StringEnumConverter))]
        public WindowLayer WindowLayer;
        
        // Buffer size for the audio capture
        [JsonIgnore] public int DefaultBufferSize = 64;
        [ReadOnly] public int BufferSize;
        
        // How sensitive Nisualizer is to audio
        [JsonIgnore] public float DefaultSensitivity = 0.5f;
        [ReadOnly] public float Sensitivity;

        // Name of the input device being monitored
        // Pulse Audio only
        [JsonIgnore] public string DefaultAudioDevice = "@DEFAULT_MONITOR@";
        [ReadOnly] public string AudioDevice;
        
        // Name of the font being used
        // Linux only
        [JsonIgnore] public string DefaultFont = "Default";
        [ReadOnly] public string Font;

        // Name of the scene being used
        [JsonIgnore] public string DefaultScene = "Snowstorm";
        [ReadOnly] public string Scene;
        
        // Shell to be used
        [JsonIgnore] public string DefaultShell = Application.platform switch
        {
            RuntimePlatform.LinuxPlayer or RuntimePlatform.LinuxEditor or RuntimePlatform.LinuxServer or RuntimePlatform.LinuxHeadlessSimulation => "/bin/bash",
            RuntimePlatform.WindowsPlayer or RuntimePlatform.WindowsEditor or RuntimePlatform.WindowsServer => "cmd.exe",
            _ => string.Empty
        };
        [ReadOnly] public string Shell;

        /// Resets all the fields to their default values
        public override void ResetToDefault(bool silent = false)
        {
            ReloadDelay = DefaultReloadDelay;
            FPS         = DefaultFPS;
            WindowMode  = DefaultWindowMode;
            WindowLayer = DefaultWindowLayer;
            BufferSize  = DefaultBufferSize;
            Sensitivity = DefaultSensitivity;
            AudioDevice = DefaultAudioDevice;
            Font        = DefaultFont;
            Scene       = DefaultScene;
            Shell       = DefaultShell;

            base.ResetToDefault(silent);
        }
    }
}