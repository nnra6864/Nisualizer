using Config;
using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(ConfigScript))]
    public class SceneManagerScript : MonoBehaviour
    {
        public static SceneManagerScript Instance { get; private set; }

        [SerializeField] protected ConfigScript _config;
        public static ConfigScript Config => Instance._config;

        protected void Reset()
        {
            _config = GetComponent<ConfigScript>();
        }

        protected void Awake()
        {
            if (Instance != null) Destroy(Instance.gameObject);
            Instance = this;
            Config.Init();
        }
    }
}