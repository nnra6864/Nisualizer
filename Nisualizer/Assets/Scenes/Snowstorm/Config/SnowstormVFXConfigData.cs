using System;
using Newtonsoft.Json;
using NnUtils.Modules.JSONUtils.Scripts.Types;
using UnityEngine;

namespace Scenes.Snowstorm.Config
{
    [Serializable]
    public class SnowstormVFXConfigData
    {
        [Header("Snow")]
        
        [JsonIgnore] public float DefaultSnowLifetime = 10;
        [JsonProperty] public float SnowLifetime;
        
        [JsonIgnore] public Vector2 DefaultSnowDensityRange = new(50, 150);
        [JsonProperty] public ConfigVector2 SnowDensityRange;
        
        [JsonIgnore] public Vector2 DefaultSnowSizeRange = new(0.01f, 0.25f);
        [JsonProperty] public ConfigVector2 SnowSizeRange;
        
        [JsonIgnore] public Vector2 DefaultSnowSpeedRange = new(1, 5);
        [JsonProperty] public ConfigVector2 SnowSpeedRange;

        [Header("Wind")]
        
        [JsonIgnore] public float DefaultWindSpeed = 1;
        [JsonProperty] public float WindSpeed;

        [JsonIgnore] public Vector2 DefaultWindStrengthRange = new(10, 50);
        [JsonProperty] public ConfigVector2 WindStrengthRange;
        
        public void Load()
        {
            
        }

        public void ResetToDefault()
        {
            // Snow
            SnowLifetime     = DefaultSnowLifetime;
            SnowDensityRange = DefaultSnowDensityRange;
            SnowSizeRange    = DefaultSnowSizeRange;
            SnowSpeedRange   = DefaultSnowSpeedRange;

            // Wind
            WindSpeed         = DefaultWindSpeed;
            WindStrengthRange = DefaultWindStrengthRange;
        }
    }
}