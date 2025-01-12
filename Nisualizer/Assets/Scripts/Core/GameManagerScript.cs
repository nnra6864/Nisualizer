using System;
using Audio;
using Config;
using NnUtils.Modules.JSONUtils.Scripts.Types.Components.UI;
using NnUtils.Scripts;
using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(ConfigScript))]
    [RequireComponent(typeof(AudioDataScript))]
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

        /// To be used in multithreaded functions
        public static float DeltaTime { get; private set; }
        
        /// Contains all the Config data and logic
        [ReadOnly] [SerializeField] private ConfigScript _config;
        public static ConfigScript ConfigScript => Instance._config;

        [ReadOnly] [SerializeField] private GeneralConfigData _configData;
        public static GeneralConfigData ConfigData => Instance._configData ??= (GeneralConfigData)Instance._config.Data;
        
        [ReadOnly] [SerializeField] private AudioDataScript _audioData;
        public static AudioDataScript AudioData => Instance._audioData;
        
        [ReadOnly] [SerializeField] private NisualizerSceneManagerScript _nisualizerSceneManager;
        public static NisualizerSceneManagerScript NisualizerSceneManager => Instance._nisualizerSceneManager;
        
        private void Reset()
        {
            _config                 = GetComponent<ConfigScript>();
            _configData             = (GeneralConfigData)_config.Data;
            _audioData              = GetComponent<AudioDataScript>();
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
            
            // Initialize AudioData
            AudioData.Initialize();
            
            // Set DefaultFont for ConfigText
            ConfigText.DefaultFont = ConfigData.Font;
            
            // Set FPS
            SetFPS();
            ConfigData.OnLoaded += OnConfigLoaded;
        }

        private void Update()
        {
            DeltaTime = Time.deltaTime;
        }

        private void OnDestroy()
        {
            // Trust me, this if check is highly needed because you'll end up with major fuckery where the singleton just disappears on the next play, just trust me
            if (ConfigScript?.Data == null) return;
            
            ConfigData.OnLoaded -= OnConfigLoaded;
        }

        private void OnConfigLoaded()
        {
            ConfigText.DefaultFont = ConfigData.Font;
            SetFPS();
            SetWindowMode();
        }
        
        private void SetFPS() => Application.targetFrameRate = ConfigData.FPS;
        
        private void SetWindowMode() => Screen.fullScreenMode = ConfigData.WindowMode;
    }
}