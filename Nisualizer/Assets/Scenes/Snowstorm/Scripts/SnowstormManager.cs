using Core;
using InteractiveComponents;
using Scenes.Snowstorm.Config;
using UnityEngine;
using UnityEngine.UI;

namespace Scenes.Snowstorm.Scripts
{
    public class SnowstormManager : SceneManagerScript
    {
        public static SnowstormConfigData ConfigData => (SnowstormConfigData)Config.Data;

        [SerializeField] private InteractiveImageScript _background;

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
        
        private void UpdateBackground() => _background.LoadImage(ConfigData.Background);
    }
}