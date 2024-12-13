using Config;
using Config.Types;
using UnityEngine;

namespace Scenes.ChaoticCubes.Config
{
    // This class should be a perfect match of your JSON config
    [CreateAssetMenu(fileName = "ChaoticCubesConfigData", menuName = "Config/ChaoticCubesConfigData")]
    public class ChaoticCubesConfigData : ConfigData
    {
        public ChaoticCubesVFXData VFX;
        
        public override void Load()
        {
            VFX.Load();

            base.Load();
        }
        
        public override void ResetToDefault(bool silent = false)
        {
            VFX.ResetToDefault();
            
            base.ResetToDefault(silent);
        }
    }
}
