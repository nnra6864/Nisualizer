using System;
using Newtonsoft.Json;
using NnUtils.Modules.JSONUtils.Scripts.Types;
using NnUtils.Scripts;
using UnityEngine;

namespace Scenes.ChaoticCubes.Config
{
    [Serializable]
    public class ChaoticCubesVFXData
    {
        [Header("General")]
        
        [JsonIgnore] public ConfigVector2 DefaultSpawnRateRange = new(1000, 10000);
        [JsonProperty] public ConfigVector2 SpawnRateRange;

        [JsonIgnore] public float DefaultSpawnRadius = 0.03f;
        [JsonProperty] public float SpawnRadius;

        [JsonIgnore] public ConfigVector2 DefaultLifetimeRange = new(1, 2.5f);
        [JsonProperty] public ConfigVector2 LifetimeRange;

        [JsonIgnore] public ConfigVector2 DefaultSpeedRange = new(1, 2.5f);
        [JsonProperty] public ConfigVector2 SpeedRange;
        
        [Header("Turbulence")]
        
        public ChaoticCubesVFXTurbulenceData Turbulence;
        
        [Header("Mesh")]

        [JsonIgnore] public float DefaultMeshSmoothness = 0.5f;
        [JsonProperty] public float MeshSmoothness;

        [JsonIgnore] public float DefaultMeshMetallic = 1f;
        [JsonProperty] public float MeshMetallic;
        
        [JsonIgnore] public float DefaultMeshSize = 0.1f;
        [JsonProperty] public float MeshSize;
        
        [JsonIgnore] [GradientUsage(true)] public ConfigGradient DefaultColorOverLife;
        public ConfigGradient ColorOverLife;

        public void Load()
        {
            Turbulence.Load();
        }

        public void ResetToDefault()
        {
            // General
            SpawnRateRange = DefaultSpawnRateRange;
            SpawnRadius    = DefaultSpawnRadius;
            LifetimeRange  = DefaultLifetimeRange;
            SpeedRange     = DefaultSpeedRange;

            // Turbulence
            Turbulence.ResetToDefault();

            // Mesh
            MeshSmoothness = DefaultMeshSmoothness;
            MeshMetallic   = DefaultMeshMetallic;
            MeshSize       = DefaultMeshSize;
            ColorOverLife  = DefaultColorOverLife;
        }
    }
}