using System;
using NnUtils.Modules.TextUtils.Scripts;
using Scripts.Config;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Core
{
    public class FontManagerScript : MonoBehaviour
    {
        private static GeneralConfigData _configData;
        private static GeneralConfigData ConfigData => _configData ??= (GeneralConfigData)GameManagerScript.ConfigScript.Data;
        
        [SerializeField] private TMP_FontAsset _defaultFont;
        private TMP_FontAsset _fontAsset;
        public TMP_FontAsset FontAsset
        {
            get => _fontAsset;
            set
            {
                if (!value || _fontAsset == value) return;
                _fontAsset = value;
                OnFontChanged?.Invoke(value);
            }
        }
        public Action<TMP_FontAsset> OnFontChanged;

        private void Start()
        {
            UpdateFont();
            ConfigData.OnLoaded      += UpdateFont;
            SceneManager.sceneLoaded += UpdateFontOnSceneLoad;
        }

        private void OnDestroy()
        {
            ConfigData.OnLoaded      -= UpdateFont;
            SceneManager.sceneLoaded -= UpdateFontOnSceneLoad;
        }

        public void UpdateFont()
        {
            var font = ConfigData.Font;
            FontAsset = font == "Default" ? _defaultFont : SystemFont.GenerateFontFromName(font);
            
            foreach (var tmpText in FindObjectsByType<TMP_Text>(FindObjectsInactive.Include, FindObjectsSortMode.None))
                tmpText.font = FontAsset;
        }

        private void UpdateFontOnSceneLoad(Scene scene, LoadSceneMode mode) => UpdateFont();
    }
}