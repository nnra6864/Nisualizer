using Core;
using Scenes.Test2.Config;

namespace Scenes.Test2.Scripts
{
    public class Test2Manager : SceneManagerScript
    {
        public static Test2ConfigData ConfigData => (Test2ConfigData)Config.Data;
    }
}
