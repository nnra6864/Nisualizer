using NnUtils.Scripts;
using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "GeneralConfigData", menuName = "Config/GeneralConfigData")]
    public class GeneralConfigData : ConfigData
    {
        public int DefaultFPS = 60;
        [ReadOnly] public int FPS;

        public float DefaultSensitivity = 0.5f;
        [ReadOnly] public float Sensitivity;

        public string DefaultFont = "Default";
        [ReadOnly] public string Font;

        public string DefaultScene = "Snowstorm";
        [ReadOnly] public string Scene;
        
        public override void Reset()
        {
            FPS              = DefaultFPS;
            Sensitivity      = DefaultSensitivity;
            Font             = DefaultFont;
            Scene            = DefaultScene;
            
            base.Reset();
        }
    }
}