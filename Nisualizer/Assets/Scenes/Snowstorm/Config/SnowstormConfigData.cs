using Config;
using Newtonsoft.Json;
using NnUtils.Scripts;
using UnityEngine;

namespace Scenes.Snowstorm.Config
{
    [CreateAssetMenu(fileName = "SnowstormConfigData", menuName = "Config/SnowstormConfigData")]
    public class SnowstormConfigData : ConfigData
    {
        [JsonIgnore] public string DefaultBackground = "";
        [ReadOnly] public string Background;

        public override void Load()
        {
            base.Load();
        }
        
        public override void ResetToDefault(bool silent = false)
        {
            Background       = DefaultBackground;
            
            base.ResetToDefault(silent);
        }
    }
}