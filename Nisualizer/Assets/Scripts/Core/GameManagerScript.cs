using Config;
using NnUtils.Scripts;
using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(ConfigScript))]
    [RequireComponent(typeof(MicrophoneDataScript))]
    [RequireComponent(typeof(NisualizerSceneManagerScript))]
    public class GameManagerScript : MonoBehaviour
    {
        //Simple singleton implementation
        private static GameManagerScript _instance;
        public static GameManagerScript Instance
        {
            get => _instance;
            private set
            {
                //If instance exists and is not this, destroy this
                if (_instance != null && _instance != value)
                {
                    Destroy(value.gameObject);
                    return;
                }
                
                _instance = value;
                DontDestroyOnLoad(_instance);
            }
        }

        /// Contains all the Config data and logic
        [ReadOnly] [SerializeField] private ConfigScript _config;
        public static ConfigScript ConfigScript => Instance._config;
        private static GeneralConfigData ConfigData => (GeneralConfigData)Instance._config.Data;
        
        [ReadOnly] [SerializeField] private MicrophoneDataScript _microphoneData;
        public static MicrophoneDataScript MicrophoneData => Instance._microphoneData;
        
        [ReadOnly] [SerializeField] private NisualizerSceneManagerScript _nisualizerSceneManager;
        public static NisualizerSceneManagerScript NisualizerSceneManager => Instance._nisualizerSceneManager;
        
        private void Reset()
        {
            _config = GetComponent<ConfigScript>();
            _microphoneData = FindFirstObjectByType<MicrophoneDataScript>();
            _nisualizerSceneManager = GetComponent<NisualizerSceneManagerScript>();
        }


        private void Awake()
        {
            //Set Instance
            Instance = this;
        }

        private void Start()
        {
            // Load the Config in Start to allow for other scripts to subscribe to events in Awake
            ConfigScript.Init();
            MicrophoneData.InitializeMicrophone();
            
            // Set FPS
            SetFPS();
            ConfigData.OnLoaded += SetFPS;
        }

        private void OnDestroy()
        {
            // Trust me, this if check is highly needed because you'll end up with major fuckery where the singleton just disappears on the next play, just trust me
            if (ConfigScript?.Data == null) return;
            
            ConfigData.OnLoaded -= SetFPS;
        }

        private void SetFPS() => Application.targetFrameRate = ConfigData.FPS;
    }
}