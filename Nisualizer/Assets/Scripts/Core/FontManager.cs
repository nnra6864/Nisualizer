using System;
using Config;
using NnUtils.Scripts;
using TMPro;
using UnityEngine;

namespace Core
{
    public class FontManager : MonoBehaviour
    {
        private static ConfigScript Config => GameManager.ConfigScript;
        
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
            UpdateFont(Config.Data);
            Config.Data.OnConfigLoaded += UpdateFont;
        }

        private void OnDestroy() => Config.Data.OnConfigLoaded -= UpdateFont;

        private void UpdateFont(ConfigData configData)
        {
            var font = configData.General.Font;

            if (font == "Default")
            {
                FontAsset = _defaultFont;
                return;
            }
            
            FontAsset = SystemTMP.GenerateFontFromName(configData.General.Font);
            
            foreach (var tmpText in FindObjectsByType<TMP_Text>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                tmpText.font = FontAsset;
            }
        }
    }
}