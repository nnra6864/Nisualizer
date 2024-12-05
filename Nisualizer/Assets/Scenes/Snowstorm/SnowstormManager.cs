using Core;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Scenes.Snowstorm
{
    public class SnowstormManager : SceneScript
    {
        public static SnowstormConfigData ConfigData => (SnowstormConfigData)Config.Data;

        [SerializeField] private Image _background;

        protected new void Awake()
        {
            base.Awake();
            
            // Load the background and listen for changes
            UpdateBackground();
            ConfigData.OnLoaded += UpdateBackground;
        }

        protected void OnDestroy()
        {
            ConfigData.OnLoaded -= UpdateBackground;
        }
        
        private void UpdateBackground() => _background.sprite = ConfigData.BackgroundSprite;
    }
}