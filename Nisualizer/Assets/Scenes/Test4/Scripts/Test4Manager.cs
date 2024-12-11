using Core;
using Scenes.Test4.Config;

namespace Scenes.Test4.Scripts
{
    public class Test4Manager : SceneManagerScript
    {
        public static Test4ConfigData ConfigData => (Test4ConfigData)Config.Data;
    }
}
