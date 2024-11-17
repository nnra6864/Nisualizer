using System;
using NnUtils.Scripts;
using UnityEngine;

namespace Config
{
    // TODO: Move BackgroundSprite to SceneConfig or delete it altogether
    [Serializable]
    public class ConfigData
    {
        public Action<ConfigData> OnConfigLoaded;
        
        public GeneralSettings General;

        /// Takes care of post loading stuff that has to be done
        public void Load()
        {
            var bg = Misc.SpriteFromFile(General.Background);
            if (bg) General.BackgroundSprite = Misc.SpriteFromFile(General.Background);

            OnConfigLoaded?.Invoke(this);
        }
        
        public void Reset()
        {
            ResetSilent();
            OnConfigLoaded?.Invoke(this);
        }

        public void ResetSilent()
        {
            General.Reset();
        }
        
        [Serializable]
        public class GeneralSettings
        {
            public int DefaultFPS = 60;
            public int FPS;

            public float DefaultSensitivity = 1;
            public float Sensitivity;

            public string DefaultFont = "Default";
            public string Font;
            
            public string DefaultBackground = "";
            public string Background;

            public Sprite DefaultBackgroundSprite;
            public Sprite BackgroundSprite;

            public void LoadBackgroundSprite(string path) => BackgroundSprite = Misc.SpriteFromFile(path);

            public void Reset()
            {
                FPS              = DefaultFPS;
                Sensitivity      = DefaultSensitivity;
                Background       = DefaultBackground;
                BackgroundSprite = DefaultBackgroundSprite;
            }
        }
    }
}