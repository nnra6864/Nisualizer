using Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Image))]
    public class BackgroundScript : MonoBehaviour
    {
        private static Config.Config Config => GameManager.ConfigScript.Config;
        
        [SerializeField] private Image _image;
        
        private void Reset() => _image = GetComponent<Image>();

        private void Start()
        {
            UpdateBackground(Config.General.BackgroundSprite);
            Config.General.OnBackgroundSpriteChanged += UpdateBackground;
        }

        private void OnDestroy()
        {
            // Trust me, this if check is highly needed because you'll end up with major fuckery where the singleton just disappears on the next play, just trust me
            if (Config == null) return;
            
            Config.General.OnBackgroundSpriteChanged -= UpdateBackground;
        }

        private void UpdateBackground(Sprite sprite) => _image.sprite = sprite;
    }
}