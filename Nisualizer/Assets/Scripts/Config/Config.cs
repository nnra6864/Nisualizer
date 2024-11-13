using System;
using NnUtils.Scripts;
using UnityEngine;

namespace Config
{
    [Serializable]
    public class Config
    {
        public GeneralSettings General;

        public void Reset()
        {
            General.Reset();
        }

        [Serializable]
        public class GeneralSettings
        {
            public GeneralSettings()
            {
                OnBackgroundChanged += LoadBackgroundSprite;
            }

            ~GeneralSettings()
            {
                OnBackgroundChanged -= LoadBackgroundSprite;
            }
            
            #region FPS

            [SerializeField] private int _defaultFPS = 60;
            private int _fps;

            /// FPS at which the app will run
            public int FPS
            {
                get => _fps;
                set
                {
                    if (_fps == value) return;
                    _fps = value;
                    OnFPSChanged?.Invoke(value);
                }
            }

            public Action<int> OnFPSChanged;

            #endregion

            #region Sensitivity

            [SerializeField] private float _defaultSensitivity = 1;
            private float _sensitivity = 1;

            /// Multiplies audio loudness data
            public float Sensitivity
            {
                get => _sensitivity;
                set
                {
                    if (Mathf.Approximately(_sensitivity, value)) return;
                    _sensitivity = value;
                    OnSensitivityChanged?.Invoke(value);
                }
            }

            public Action<float> OnSensitivityChanged;
            
            #endregion

            #region Background
            
            [SerializeField] private string _defaultBackground = "";
            private string _background;

            /// Path to the background image, do not mistake for <see cref="BackgroundSprite"/>
            public string Background
            {
                get => _background;
                set
                {
                    if (_background == value) return;
                    _background = value;
                    OnBackgroundChanged?.Invoke(value);
                }
            }
            
            public Action<string> OnBackgroundChanged;

            #endregion

            #region BackgroundSprite
            // TODO: Implement a change only file watcher for live reloading
            
            /// Must be set in the inspector of the script
            [SerializeField] private Sprite _defaultBackgroundSprite;
            private Sprite _backgroundSprite;

            /// Sprite that should be used as a background
            public Sprite BackgroundSprite
            {
                get => _backgroundSprite;
                set
                {
                    _backgroundSprite = value;
                    OnBackgroundSpriteChanged?.Invoke(value);
                }
            }

            public Action<Sprite> OnBackgroundSpriteChanged;

            private void LoadBackgroundSprite(string path) => BackgroundSprite = Misc.SpriteFromFile(path);

            #endregion

            public void Reset()
            {
                FPS         = _defaultFPS;
                Sensitivity = _defaultSensitivity;
                Background  = _defaultBackground; // This resets the BackgroundSprite too
            }

            public void SilentReset()
            {
                _fps              = _defaultFPS;
                _sensitivity      = _defaultSensitivity;
                _background       = _defaultBackground;
                _backgroundSprite = _defaultBackgroundSprite; // Must be explicitly set because OnBackgroundChanged event is not triggered
            }
        }
    }
}