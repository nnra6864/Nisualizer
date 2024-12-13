using System;
using Config.Types;
using Newtonsoft.Json;
using NnUtils.Scripts;
using UnityEngine;

namespace Scenes.ChaoticCubes.Config
{
    [Serializable]
    public class ChaoticCubesVFXData
    {
        [Header("General")]
        
        [JsonIgnore] public Vector2 DefaultSpawnRateRange = new(100, 1000);
        [ReadOnly] public ConfigVector2 SpawnRateRange;

        [JsonIgnore] public float DefaultSpawnRadius = 0.03f;
        [ReadOnly] public float SpawnRadius;

        [JsonIgnore] public Vector2 DefaultLifetimeRange = new(2, 4);
        [ReadOnly] public ConfigVector2 LifetimeRange;

        [JsonIgnore] public Vector2 DefaultSpeedRange = new(1, 3);
        [ReadOnly] public ConfigVector2 SpeedRange;
        
        [Header("Turbulence")]
        
        [ReadOnly] public ChaoticCubesVFXTurbulenceData Turbulence;
        
        [Header("Mesh")]
        
        [JsonIgnore] [GradientUsage(true)] public Gradient DefaultColorOverLife;
        public ConfigGradient ColorOverLife;

        public void Load()
        {
            // General
            SpawnRateRange ??= DefaultSpawnRateRange;
            LifetimeRange  ??= DefaultLifetimeRange;
            SpeedRange     ??= DefaultSpeedRange;
            
            // Turbulence
            Turbulence.Load();
            
            // Mesh
            ColorOverLife  ??= DefaultColorOverLife;
        }

        public void ResetToDefault()
        {
            // General
            SpawnRateRange = null;
            SpawnRadius    = 0;
            LifetimeRange  = null;
            SpeedRange     = null;
            
            // Turbulence
            Turbulence.ResetToDefault();
            
            // Mesh
            ColorOverLife  = null;
        }
    }
}