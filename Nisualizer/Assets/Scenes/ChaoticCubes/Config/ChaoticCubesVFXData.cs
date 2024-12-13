using System;
using Config.Types;
using UnityEngine;

namespace Scenes.ChaoticCubes.Config
{
    [Serializable]
    public class ChaoticCubesVFXData
    {
        [SerializeField] [GradientUsage(true)] public Gradient DefaultCubeColorOverLife;
        public ConfigGradient CubeColorOverLife;
    }
}