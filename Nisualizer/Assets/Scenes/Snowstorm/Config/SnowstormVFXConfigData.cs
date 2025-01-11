using System;
using Newtonsoft.Json;
using NnUtils.Scripts;
using UnityEngine;
using UnityJSONUtils.Scripts.Types;

namespace Scenes.Snowstorm.Config
{
    [Serializable]
    public class SnowstormVFXConfigData
    {
        [Header("Snow")]
        
        [JsonIgnore] public float DefaultSnowLifetime = 10;
        [ReadOnly] public float SnowLifetime;
        
        [JsonIgnore] public Vector2 DefaultSnowDensityRange = new(50, 150);
        [ReadOnly] public ConfigVector2 SnowDensityRange;
        
        [JsonIgnore] public Vector2 DefaultSnowSizeRange = new(0.01f, 0.25f);
        [ReadOnly] public ConfigVector2 SnowSizeRange;
        
        [JsonIgnore] public Vector2 DefaultSnowSpeedRange = new(1, 5);
        [ReadOnly] public ConfigVector2 SnowSpeedRange;

        [Header("Wind")]
        
        [JsonIgnore] public float DefaultWindSpeed = 1;
        [ReadOnly] public float WindSpeed;

        [JsonIgnore] public Vector2 DefaultWindStrengthRange = new(10, 50);
        [ReadOnly] public ConfigVector2 WindStrengthRange;
        
        public void Load()
        {
            // Snow
            SnowDensityRange ??= DefaultSnowDensityRange;
            SnowSizeRange ??= DefaultSnowSizeRange;
            SnowSpeedRange ??= DefaultSnowSpeedRange;
            
            // Wind
            WindStrengthRange ??= DefaultWindStrengthRange;
        }

        public void ResetToDefault()
        {
            // Snow
            SnowLifetime = DefaultSnowLifetime;
            SnowDensityRange = null;
            SnowSizeRange = null;
            SnowSpeedRange = null;
            
            // Wind
            WindSpeed = DefaultWindSpeed;
            WindStrengthRange = null;
        }
    }
}