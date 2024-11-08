using System;
using System.IO;
using NnUtils.Scripts;
using UnityEngine;

namespace Core
{
    public class ConfigScript : MonoBehaviour
    {
        #region FPS
        [SerializeField] private int _defaultFPS = 60;
        
        private int _fps;
        public int FPS
        {
            get => _fps;
            private set
            {
                if (_fps == value) return;
                _fps                        = value;
                Application.targetFrameRate = _fps;
                OnFPSChanged?.Invoke(value);
            }
        }

        public Action<int> OnFPSChanged;
        #endregion
        
        #region Background
        //Used to store the background path in order to avoid unnecessary updates
        private string _backgroundPath;
        
        [SerializeField] private Sprite _defaultBackground;
        
        private Sprite _background;
        public Sprite Background
        {
            get => _background;
            private set
            {
                if (_background?.texture == value?.texture) return;
                _background = value;
                OnBackgroundChanged?.Invoke(value);
            }
        }
        
        public Action<Sprite> OnBackgroundChanged;
        
        private Sprite LoadBackground(string bg)
        {
            //Read the config value and replace relative with full path
            bg = bg.Replace("~", Environment.GetFolderPath(System.Environment.SpecialFolder.Personal));
        
            //TODO: Add custom logging when implemented
            //Check if the file exists, if not, return the default bg
            if (!File.Exists(bg)) return _defaultBackground;
            
            //Read image data and store it into a Texture2D
            var bgData = File.ReadAllBytes(bg);
            Texture2D bgTex = new(0, 0);
            //TODO: Add custom logging when implemented
            if (!bgTex.LoadImage(bgData)) return _defaultBackground;

            return Misc.Texture2DToSprite(bgTex);
        }
        #endregion

        //TODO: This will trigger OnChanged events, cba to optimize now
        public void ResetToDefault()
        {
            FPS        = _defaultFPS;
            Background = _defaultBackground;
        }
        
        public void Load()
        {
            //Reset to default to make sure all vars are set
            ResetToDefault();
            
            //TODO: Load all the values from a JSON config
            FPS        = 60;
            Background = LoadBackground("~/.config/Backgrounds/Nord/GalaxyWaifu.jpg");
        }
    }
}