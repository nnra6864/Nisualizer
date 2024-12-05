using System;
using Config;
using NnUtils.Scripts;
using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(ConfigScript))]
    public class SceneScript : MonoBehaviour
    {
        public static SceneScript Instance { get; private set; }

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
            Debug.Log(Config);
            Config.Init();
        }
    }
}