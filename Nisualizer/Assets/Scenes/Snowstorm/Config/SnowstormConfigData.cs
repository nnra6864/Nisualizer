using Newtonsoft.Json;
using NnUtils.Modules.JSONUtils.Scripts.Types.Components;
using NnUtils.Modules.JSONUtils.Scripts.Types.Components.UI.Image;
using Scripts.Config;
using UnityEngine;

namespace Scenes.Snowstorm.Config
{
    [CreateAssetMenu(fileName = "SnowstormConfigData", menuName = "Config/SnowstormConfigData")]
    public class SnowstormConfigData : ConfigData
    {
        [JsonProperty] public SnowstormVFXConfigData VFX;
        [JsonProperty] public ConfigGameObject[] Objects;

        public override void Load()
        {
            VFX.Load();
            base.Load();
        }
        
        public override void ResetToDefault(bool silent = false)
        {
            VFX.ResetToDefault();
            Objects = new ConfigGameObject[] { };
            base.ResetToDefault(silent);
        }
    }
}