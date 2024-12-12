using NnUtils.Scripts;
using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "GeneralConfigData", menuName = "Config/GeneralConfigData")]
    public class GeneralConfigData : ConfigData
    {
        // Delays the config reload to avoid multiple reloads and config overwrites
        public float DefaultReloadDelay = 0.1f;
        [ReadOnly] public float ReloadDelay;
        
        // FPS of the app
        public int DefaultFPS = 60;
        [ReadOnly] public int FPS;

        // How sensitive Nisualizer is to audio
        public float DefaultSensitivity = 0.5f;
        [ReadOnly] public float Sensitivity;

        // Name of the input device being monitored
        public string DefaultInputName = "OutputInput";
        [ReadOnly] public string InputName;
        
        // Name of the font being used
        public string DefaultFont = "Default";
        [ReadOnly] public string Font;

        // Name of the scene being used
        public string DefaultScene = "Snowstorm";
        [ReadOnly] public string Scene;
        
        /// Resets all the fields to their default values
        public override void ResetToDefault(bool silent = false)
        {
            ReloadDelay = DefaultReloadDelay;
            FPS         = DefaultFPS;
            Sensitivity = DefaultSensitivity;
            InputName   = DefaultInputName;
            Font        = DefaultFont;
            Scene       = DefaultScene;
            
            base.ResetToDefault(silent);
        }
    }
}