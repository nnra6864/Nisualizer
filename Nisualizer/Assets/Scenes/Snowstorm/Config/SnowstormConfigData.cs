using Newtonsoft.Json;
using NnUtils.Modules.JSONUtils.Scripts.Types.Components;
using Scripts.Config;
using UnityEngine;

namespace Scenes.Snowstorm.Config
{
    [CreateAssetMenu(fileName = "SnowstormConfigData", menuName = "Config/SnowstormConfigData")]
    public class SnowstormConfigData : ConfigData
    {
        [JsonIgnore] public string DefaultBackground = "";
        [JsonProperty] public string Background;
        
        [JsonProperty] public SnowstormVFXConfigData VFX;
        [JsonProperty] public ConfigGameObject[] Objects;

        public override void Load()
        {
            VFX.Load();
            base.Load();
        }
        
        public override void ResetToDefault(bool silent = false)
        {
            Background = DefaultBackground;
            VFX.ResetToDefault();
            Objects = new ConfigGameObject[] { };
            base.ResetToDefault(silent);
        }
    }
}