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

        private GameObject _objects;

        [SerializeField] private InteractiveImageScript _background;
        [SerializeField] private VisualEffect _snowstormVFX;

        private void Start()
        {
            _objects = new("Objects");
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
            AddObjects();
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

        private void AddObjects()
        {
            // Destroy all existing objects
            foreach (Transform child in _objects.transform) Destroy(child.gameObject);
            
            // Add new objects
            foreach (var configDataObject in ConfigData.Objects)
            {
                GameObject child = new();
                child.transform.SetParent(_objects.transform);
                configDataObject.Initialize(child);
            }
        }
    }
}