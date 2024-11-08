using Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Image))]
    public class BackgroundScript : MonoBehaviour
    {
        private static ConfigScript Config => GameManager.Config;
        
        [SerializeField] private Image _image;
        
        private void Reset() => _image = GetComponent<Image>();

        private void Awake() => Config.OnBackgroundChanged += UpdateBackground;

        private void OnDestroy() => Config.OnBackgroundChanged -= UpdateBackground;

        private void UpdateBackground(Sprite sprite) => _image.sprite = sprite;
    }
}