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
        
        [JsonIgnore] public Vector2 DefaultSpawnRateRange = new(1000, 10000);
        [ReadOnly] public ConfigVector2 SpawnRateRange;

        [JsonIgnore] public float DefaultSpawnRadius = 0.03f;
        [ReadOnly] public float SpawnRadius;

        [JsonIgnore] public Vector2 DefaultLifetimeRange = new(1, 2.5f);
        [ReadOnly] public ConfigVector2 LifetimeRange;

        [JsonIgnore] public Vector2 DefaultSpeedRange = new(1, 2.5f);
        [ReadOnly] public ConfigVector2 SpeedRange;
        
        [Header("Turbulence")]
        
        public ChaoticCubesVFXTurbulenceData Turbulence;
        
        [Header("Mesh")]

        [JsonIgnore] public float DefaultMeshSmoothness = 0.5f;
        [ReadOnly] public float MeshSmoothness;

        [JsonIgnore] public float DefaultMeshMetallic = 1f;
        [ReadOnly] public float MeshMetallic;
        
        [JsonIgnore] public float DefaultMeshSize = 0.1f;
        [ReadOnly] public float MeshSize;
        
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
            //ColorOverLife  ??= DefaultColorOverLife;
        }

        public void ResetToDefault()
        {
            // General
            SpawnRateRange = null;
            SpawnRadius    = DefaultSpawnRadius;
            LifetimeRange  = null;
            SpeedRange     = null;

            // Turbulence
            Turbulence.ResetToDefault();

            // Mesh
            MeshSmoothness = DefaultMeshSmoothness;
            MeshMetallic   = DefaultMeshMetallic;
            MeshSize       = DefaultMeshSize;
            //ColorOverLife  = null;
            ColorOverLife = DefaultColorOverLife;
        }
    }
}