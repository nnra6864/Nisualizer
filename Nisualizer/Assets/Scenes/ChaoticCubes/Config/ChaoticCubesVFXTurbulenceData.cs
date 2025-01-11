using System;
using Newtonsoft.Json;
using NnUtils.Scripts;
using UnityEngine;
using UnityJSONUtils.Scripts.Types;

namespace Scenes.ChaoticCubes.Config
{
    [Serializable]
    public class ChaoticCubesVFXTurbulenceData
    {
        [JsonIgnore] public float DefaultRotationSpeed = 20;
        [ReadOnly] public float RotationSpeed;
        
        [JsonIgnore] public Vector2 DefaultIntensityRange = new(5, 7);
        [ReadOnly] public ConfigVector2 IntensityRange;
        
        [JsonIgnore] public Vector2 DefaultDragRange = new(1.5f, 2);
        [ReadOnly] public ConfigVector2 DragRange;
        
        [JsonIgnore] public float DefaultFrequency = 0.5f;
        [ReadOnly] public float Frequency;
        
        [JsonIgnore] public int DefaultOctaves = 4;
        [ReadOnly] public int Octaves;

        [JsonIgnore] public float DefaultRoughness = 0.5f;
        [ReadOnly] public float Roughness;
        
        [JsonIgnore] public float DefaultLacunarity = 0.25f;
        [ReadOnly] public float Lacunarity;

        public void Load()
        {
            IntensityRange ??= DefaultIntensityRange;
            DragRange      ??= DefaultDragRange;
        }

        public void ResetToDefault()
        {
            RotationSpeed = DefaultRotationSpeed;

            // Assign null in Reset so that appropriate values can be loaded in Load
            IntensityRange = null;
            DragRange      = null;
            Frequency      = DefaultFrequency;
            Octaves        = DefaultOctaves;
            Roughness       = DefaultRoughness;
            Lacunarity     = DefaultLacunarity;
        }
    }
}