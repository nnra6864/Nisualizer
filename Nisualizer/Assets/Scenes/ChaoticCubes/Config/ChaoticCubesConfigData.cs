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

        public override void Load()
        {
            // Assign the default color over life if still null after reload
            CubeColorOverLife ??= DefaultCubeColorOverLife;

            base.Load();
        }
        
        public override void ResetToDefault(bool silent = false)
        {
            // Assign null in Reset so that appropriate value can be loaded in Load
            CubeColorOverLife = null;
            
            base.ResetToDefault(silent);
        }
    }
}
