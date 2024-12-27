using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NnUtils.Scripts;
using UnityEngine;

namespace Config
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
        
        // How sensitive Nisualizer is to audio
        [JsonIgnore] public float DefaultSensitivity = 0.5f;
        [ReadOnly] public float Sensitivity;

        // Name of the input device being monitored
        [JsonIgnore] public string DefaultInputName = "OutputInput";
        [ReadOnly] public string InputName;
        
        // Name of the font being used
        [JsonIgnore] public string DefaultFont = "Default";
        [ReadOnly] public string Font;

        // Name of the scene being used
        [JsonIgnore] public string DefaultScene = "Snowstorm";
        [ReadOnly] public string Scene;
        
        // Shell to be used
        [JsonIgnore] public string DefaultShell = "/bin/bash";
        [ReadOnly] public string Shell;

        /// Resets all the fields to their default values
        public override void ResetToDefault(bool silent = false)
        {
            ReloadDelay = DefaultReloadDelay;
            FPS         = DefaultFPS;
            WindowMode  = DefaultWindowMode;
            Sensitivity = DefaultSensitivity;
            InputName   = DefaultInputName;
            Font        = DefaultFont;
            Scene       = DefaultScene;
            Shell       = DefaultShell;

            base.ResetToDefault(silent);
        }
    }
}