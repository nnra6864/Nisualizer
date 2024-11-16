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
        
        [SerializeField] private Image _image;
        
        private void Reset() => _image = GetComponent<Image>();

        private void Start()
        {
            UpdateBackground(Config.Data.General.BackgroundSprite);
            Config.Data.OnConfigLoaded += conf => UpdateBackground(conf.General.BackgroundSprite);
        }

        private void OnDestroy()
        {
            // Trust me, this if check is highly needed because you'll end up with major fuckery where the singleton just disappears on the next play, just trust me
            if (Config == null || Config.Data == null) return;
            
            Config.Data.OnConfigLoaded -= conf => UpdateBackground(conf.General.BackgroundSprite);
        }

        private void UpdateBackground(Sprite sprite)
        {
            _image.sprite = sprite;
        }
    }
}