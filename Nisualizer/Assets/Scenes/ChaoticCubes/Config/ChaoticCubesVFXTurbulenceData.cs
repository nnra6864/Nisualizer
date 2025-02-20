using System;
using Newtonsoft.Json;
using NnUtils.Modules.JSONUtils.Scripts.Types;
using UnityEngine;

namespace Scenes.ChaoticCubes.Config
{
    [Serializable]
    public class ChaoticCubesVFXTurbulenceData
    {
        [JsonIgnore] public Vector3 DefaultShiftSpeed = new(0.001f, 0.0025f, 0.005f);
        [JsonProperty] public ConfigVector3 ShiftSpeed;
        
        [JsonIgnore] public Vector3 DefaultRotationSpeed = new(10, 5, 2.5f);
        [JsonProperty] public ConfigVector3 RotationSpeed;
        
        [JsonIgnore] public ConfigVector2 DefaultIntensityRange = new(5, 7);
        [JsonProperty] public ConfigVector2 IntensityRange;
        
        [JsonIgnore] public ConfigVector2 DefaultDragRange = new(1.5f, 2);
        [JsonProperty] public ConfigVector2 DragRange;
        
        [JsonIgnore] public float DefaultFrequency = 0.5f;
        [JsonProperty] public float Frequency;
        
        [JsonIgnore] public int DefaultOctaves = 4;
        [JsonProperty] public int Octaves;

        [JsonIgnore] public float DefaultRoughness = 0.5f;
        [JsonProperty] public float Roughness;
        
        [JsonIgnore] public float DefaultLacunarity = 0.25f;
        [JsonProperty] public float Lacunarity;

        public void Load()
        {
            
        }

        public void ResetToDefault()
        {
            ShiftSpeed     = DefaultShiftSpeed;
            RotationSpeed  = DefaultRotationSpeed;
            IntensityRange = DefaultIntensityRange;
            DragRange      = DefaultDragRange;
            Frequency      = DefaultFrequency;
            Octaves        = DefaultOctaves;
            Roughness      = DefaultRoughness;
            Lacunarity     = DefaultLacunarity;
        }
    }
}