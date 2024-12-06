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
        private static GeneralConfigData ConfigData => (GeneralConfigData)Config.Data;
        
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
            ConfigData.OnLoaded += UpdateFont;
        }

        private void OnDestroy() => ConfigData.OnLoaded -= UpdateFont;

        private void UpdateFont()
        {
            var font = ConfigData.Font;
            FontAsset = font == "Default" ? _defaultFont : SystemTMP.GenerateFontFromName(ConfigData.Font);
            
            foreach (var tmpText in FindObjectsByType<TMP_Text>(FindObjectsInactive.Include, FindObjectsSortMode.None))
                tmpText.font = FontAsset;
        }
    }
}