using Core;
using Scenes.Test.Config;

namespace Scenes.Test.Scripts
{
    public class TestManager : SceneManagerScript
    {
        public static TestConfigData ConfigData => (TestConfigData)Config.Data;
    }
}
