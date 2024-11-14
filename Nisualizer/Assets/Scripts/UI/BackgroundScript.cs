using Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Image))]
    public class BackgroundScript : MonoBehaviour
    {
        private static Config.ConfigScript ConfigScript => GameManager.ConfigScript;
        
        [SerializeField] private Image _image;
        
        private void Reset() => _image = GetComponent<Image>();

        private void Start()
        {
            UpdateBackground(ConfigScript.Config.General.BackgroundSprite);
            ConfigScript.Config.OnConfigLoaded += conf => UpdateBackground(conf.General.BackgroundSprite);
        }

        private void OnDestroy()
        {
            // Trust me, this if check is highly needed because you'll end up with major fuckery where the singleton just disappears on the next play, just trust me
            if (ConfigScript == null || ConfigScript.Config == null) return;
            
            ConfigScript.Config.OnConfigLoaded -= conf => UpdateBackground(conf.General.BackgroundSprite);
        }

        private void UpdateBackground(Sprite sprite)
        {
            _image.sprite = sprite;
        }
    }
}