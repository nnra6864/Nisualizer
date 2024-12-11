using Config;
using UnityEngine;

namespace Scenes.Test3.Config
{
    // This class should be a perfect match of your JSON config
    [CreateAssetMenu(fileName = "Test3ConfigData", menuName = "Config/Test3ConfigData")]
    public class Test3ConfigData : ConfigData
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
