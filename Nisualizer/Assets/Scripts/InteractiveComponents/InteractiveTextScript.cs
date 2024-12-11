using Config;
using Core;
using NnUtils.Scripts;
using TMPro;
using UnityEngine;

namespace InteractiveComponents
{
    [RequireComponent(typeof(TMP_Text))]
    public class InteractiveTextScript : MonoBehaviour
    {
        private static GeneralConfigData _configData;
        private static GeneralConfigData ConfigData => _configData ??= (GeneralConfigData)GameManagerScript.ConfigScript.Data;

        [SerializeField] private TMP_Text _tmpText;
        [SerializeField] private TMP_FontAsset _defaultFont;

        private void Reset()
        {
            _tmpText = GetComponent<TMP_Text>();
        }
        
        private void Start()
        {
            UpdateFont();
            ConfigData.OnLoaded += UpdateFont;
        }

        private void OnDestroy()
        {
            ConfigData.OnLoaded -= UpdateFont;
        }

        private void UpdateFont()
        {
            var font = ConfigData.Font;
            var asset = font == "Default" ? _defaultFont : SystemTMP.GenerateFontFromName(font);
            _tmpText.font = asset;
        }
    }
}