using System;
using Config.Types;
using Newtonsoft.Json;
using UnityEngine;

namespace Scenes.ChaoticCubes.Config
{
    [Serializable]
    public class ChaoticCubesVFXData
    {
        [JsonIgnore] [SerializeField] public Vector2 DefaultSpawnRateRange = new(100, 1000);
        public ConfigVector2 SpawnRateRange;
        
        [JsonIgnore] [SerializeField] [GradientUsage(true)] public Gradient DefaultColorOverLife;
        public ConfigGradient ColorOverLife;

        public void Load()
        {
            // Assign default values if still null after reload
            SpawnRateRange ??= DefaultSpawnRateRange;
            ColorOverLife  ??= DefaultColorOverLife;
        }

        public void ResetToDefault()
        {
            // Assign null in Reset so that appropriate values can be loaded in Load
            SpawnRateRange = null;
            ColorOverLife  = null;
        }
    }
}