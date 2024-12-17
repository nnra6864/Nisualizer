using Config.Types;
using Newtonsoft.Json;
using UnityEngine;

namespace Scenes.Snowstorm.Config
{
    public class SnowstormVFXConfigData
    {
        [Header("Snow")]
        
        [JsonIgnore] public Vector2 DefaultSnowSizeRange = new(0.01f, 0.25f);
        public ConfigVector2 SnowSizeRange;
        
        public void Load()
        {
            SnowSizeRange ??= DefaultSnowSizeRange;
        }

        public void ResetToDefault()
        {
            SnowSizeRange = null;
        }
    }
}