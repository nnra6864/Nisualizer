using Scripts.Core;
using Scenes.Test.Config;

namespace Scenes.Test.Scripts
{
    public class TestManager : SceneManagerScript
    {
        public static TestConfigData ConfigData => (TestConfigData)Config.Data;

        private void Start()
        {
            // Execute OnConfigLoaded once to load all the config values
            OnConfigLoaded();

            // Execute OnConfigLoaded when the config loads
            ConfigData.OnLoaded += OnConfigLoaded;
        }

        private void OnDestroy()
        {
            // Stop executing the OnConfigLoaded if this object is destroyed
            ConfigData.OnLoaded -= OnConfigLoaded;
        }

        private void OnConfigLoaded()
        {
            // Execute stuff that should happen when the config loads here
        }
    }
}
