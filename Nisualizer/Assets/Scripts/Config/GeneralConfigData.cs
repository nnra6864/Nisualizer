using NnUtils.Scripts;
using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "GeneralConfigData", menuName = "Config/GeneralConfigData")]
    public class GeneralConfigData : ConfigData
    {
        public int DefaultFPS = 60;
        [ReadOnly] public int FPS;

        public float DefaultSensitivity = 1;
        [ReadOnly] public float Sensitivity;

        public string DefaultFont = "Default";
        [ReadOnly] public string Font;

        public string DefaultBackground = "";
        [ReadOnly] public string Background;

        public Sprite DefaultBackgroundSprite;
        [ReadOnly] public Sprite BackgroundSprite;

        public void LoadBackgroundSprite(string path) => BackgroundSprite = Misc.SpriteFromFile(path);

        /// Loads the General Config Data
        public override void Load()
        {
            var bg = Misc.SpriteFromFile(Background);
            if (bg) BackgroundSprite = Misc.SpriteFromFile(Background);
            
            base.Load();
        }
        
        public override void Reset()
        {
            FPS              = DefaultFPS;
            Sensitivity      = DefaultSensitivity;
            Background       = DefaultBackground;
            BackgroundSprite = DefaultBackgroundSprite;
            
            base.Reset();
        }
    }
}