using Config;
using Config.Types;
using NnUtils.Scripts;
using UnityEngine;

namespace Scenes.ChaoticCubes.Config
{
    // This class should be a perfect match of your JSON config
    [CreateAssetMenu(fileName = "ChaoticCubesConfigData", menuName = "Config/ChaoticCubesConfigData")]
    public class ChaoticCubesConfigData : ConfigData
    {
        [SerializeField] [GradientUsage(true)] public Gradient DefaultCubeColorOverLife;
        [ReadOnly] public ConfigGradient CubeColorOverLife;

        public override void ResetToDefault(bool silent = false)
        {
            CubeColorOverLife = DefaultCubeColorOverLife;
            
            base.ResetToDefault(silent);
        }
    }
}
