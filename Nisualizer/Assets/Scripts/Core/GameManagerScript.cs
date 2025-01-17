using System;
using System.IO;
using Audio;
using Config;
using NnUtils.Modules.JSONUtils.Scripts.Types.Components.UI;
using NnUtils.Modules.TextUtils.Scripts.InteractiveText;
using NnUtils.Scripts;
using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(ConfigScript))]
    [RequireComponent(typeof(LiveConfigReload))]
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
        
        /// Contains all the Config logic
        [ReadOnly] [SerializeField] private ConfigScript _config;
        public static ConfigScript ConfigScript => Instance._config;

        /// Contains all the Config data
        [ReadOnly] [SerializeField] private GeneralConfigData _configData;
        public static GeneralConfigData ConfigData => Instance._configData ??= (GeneralConfigData)Instance._config.Data;
        
        /// Handles config edits
        [ReadOnly] [SerializeField] private LiveConfigReload _liveConfigReload;
        public static LiveConfigReload LiveConfigReload => _instance._liveConfigReload;
        
        /// Directory for configs
        // Trust me, / at the end is needed or file system monitor dies o_0
        public static readonly string ConfigDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), ".config/Nisualizer/");
        
        [ReadOnly] [SerializeField] private AudioDataScript _audioData;
        public static AudioDataScript AudioData => Instance._audioData;
        
        [ReadOnly] [SerializeField] private NisualizerSceneManagerScript _nisualizerSceneManager;
        public static NisualizerSceneManagerScript NisualizerSceneManager => Instance._nisualizerSceneManager;
        
        private void Reset()
        {
            _config                 = GetComponent<ConfigScript>();
            _configData             = (GeneralConfigData)_config.Data;
            _liveConfigReload       = GetComponent<LiveConfigReload>();
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
            // Add the fps command to functions
            InteractiveTextProcessing.Functions["fps"] = param => (1 / DeltaTime).ToString(param);
            
            // Load the Config in Start to allow for other scripts to subscribe to events in Awake
            ConfigScript.Init();
            
            // Load the Live Config Reload
            LiveConfigReload.Init();
            
            // Initialize AudioData
            AudioData.Initialize();
            
            // Set DefaultFont for ConfigText
            ConfigText.DefaultFont = ConfigData.Font;
            
            // Set FPS
            SetFPS();
            ConfigData.OnLoaded             += OnConfigLoaded;
            InteractiveTextProcessing.Shell =  ConfigData.Shell;
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
            InteractiveTextProcessing.Shell =  ConfigData.Shell;
            SetFPS();
            SetWindowMode();
        }
        
        private void SetFPS() => Application.targetFrameRate = ConfigData.FPS;
        
        private void SetWindowMode() => Screen.fullScreenMode = ConfigData.WindowMode;
    }
}