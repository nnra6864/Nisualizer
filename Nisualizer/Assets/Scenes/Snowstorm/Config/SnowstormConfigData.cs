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

        [JsonIgnore] public Sprite DefaultBackgroundSprite;
        [ReadOnly] public Sprite BackgroundSprite;

        public override void Load()
        {
            BackgroundSprite = string.IsNullOrEmpty(Background) ? DefaultBackgroundSprite : Misc.SpriteFromFile(Background);
            
            base.Load();
        }
        
        public override void ResetToDefault(bool silent = false)
        {
            Background       = DefaultBackground;
            BackgroundSprite = DefaultBackgroundSprite;
            
            base.ResetToDefault(silent);
        }
    }
}