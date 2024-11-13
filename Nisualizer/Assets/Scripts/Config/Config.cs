using System;
using NnUtils.Scripts;
using UnityEngine;

namespace Config
{
    [Serializable]
    public class Config
    {
        public Action<Config> OnConfigLoaded;
        
        public GeneralSettings General;

        /// Takes care of post loading stuff that has to be done
        public void Load()
        {
            General.BackgroundSprite = Misc.SpriteFromFile(General.Background);
            //if (General.BackgroundSprite == null) General.BackgroundSprite = General.DefaultBackgroundSprite;
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
            public float Sensitivity = 1;

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