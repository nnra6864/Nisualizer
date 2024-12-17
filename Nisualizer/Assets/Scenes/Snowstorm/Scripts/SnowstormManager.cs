using Core;
using InteractiveComponents;
using Scenes.Snowstorm.Config;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

namespace Scenes.Snowstorm.Scripts
{
    public class SnowstormManager : SceneManagerScript
    {
        public static SnowstormConfigData ConfigData => (SnowstormConfigData)Config.Data;

        [SerializeField] private InteractiveImageScript _background;
        [SerializeField] private VisualEffect _snowstormVFX;

        private void Start()
        {
            OnConfigLoaded();
            ConfigData.OnLoaded += OnConfigLoaded;
        }

        protected void OnDestroy()
        {
            ConfigData.OnLoaded -= OnConfigLoaded;
        }

        private void OnConfigLoaded()
        {
            UpdateBackground();
            UpdateSnowstormVFX();
        }
        
        private void UpdateBackground() => _background.LoadImage(ConfigData.Background);

        private void UpdateSnowstormVFX()
        {
            // Store the vfx component
            var vfx = ConfigData.VFX;
            
            // Snow
            _snowstormVFX.SetFloat("SnowLifetime", vfx.SnowLifetime);
            _snowstormVFX.SetVector2("SnowDensityRange", vfx.SnowDensityRange);
            _snowstormVFX.SetVector2("SnowSizeRange", vfx.SnowSizeRange);
            _snowstormVFX.SetVector2("SnowSpeedRange", vfx.SnowSpeedRange);
            
            // Wind
            _snowstormVFX.SetFloat("WindSpeed", vfx.WindSpeed);
            _snowstormVFX.SetVector2("WindStrengthRange", vfx.WindStrengthRange);
        }
    }
}