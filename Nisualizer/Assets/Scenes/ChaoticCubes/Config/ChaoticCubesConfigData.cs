using Config;
using UnityEngine;

namespace Scenes.ChaoticCubes.Config
{
    // This class should be a perfect match of your JSON config
    [CreateAssetMenu(fileName = "ChaoticCubesConfigData", menuName = "Config/ChaoticCubesConfigData")]
    public class ChaoticCubesConfigData : ConfigData
    {
        // Gets called when config is loaded
        public override void Load()
        {
            // Your code here

            base.Load();
        }
        
        // Use this function to reset all values to default
        public override void ResetToDefault(bool silent = false)
        {
            // Your code here

            base.ResetToDefault(silent);
        }
    }
}
