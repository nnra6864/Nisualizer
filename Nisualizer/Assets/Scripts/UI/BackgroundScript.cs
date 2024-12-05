using Core;
using Config;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Image))]
    public class BackgroundScript : MonoBehaviour
    {
        private static ConfigScript Config => GameManager.ConfigScript;
        private static GeneralConfigData ConfigData => (GeneralConfigData)Config.Data;
        
        [SerializeField] private Image _image;
        
        private void Reset() => _image = GetComponent<Image>();

        private void Start()
        {
            UpdateBackground();
            ConfigData.OnLoaded += UpdateBackground;
        }

        private void OnDestroy()
        {
            // Trust me, this if check is highly needed because you'll end up with major fuckery where the singleton just disappears on the next play, just trust me
            if (Config == null || Config.Data == null) return;
            
            ConfigData.OnLoaded -= UpdateBackground;
        }

        private void UpdateBackground()
        {
            _image.sprite = ConfigData.BackgroundSprite;
        }
    }
}