using System;
using System.IO;
using NnUtils.Modules.JSONUtils.Scripts.Types.Components.UI;
using NnUtils.Modules.SystemAudioMonitor;
using NnUtils.Modules.TextUtils.Scripts.InteractiveText;
using NnUtils.Scripts;
using Scripts.Config;
using UnityEngine;

namespace Scripts.Core
{
    [RequireComponent(typeof(ConfigScript))]
    [RequireComponent(typeof(LiveConfigReload))]
    [RequireComponent(typeof(WindowManager))]
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
        
        [ReadOnly] [SerializeField] private WindowManager _windowManager;
        public static WindowManager WindowManager => Instance._windowManager;
        
        /// Directory for configs
        public static string ConfigDirectory;
        
        [ReadOnly] [SerializeField] private AudioDataScript _audioData;
        public static AudioDataScript AudioData => Instance._audioData;
        
        [ReadOnly] [SerializeField] private NisualizerSceneManagerScript _nisualizerSceneManager;
        public static NisualizerSceneManagerScript NisualizerSceneManager => Instance._nisualizerSceneManager;

        private void Reset()
        {
            _config                 = GetComponent<ConfigScript>();
            _configData             = (GeneralConfigData)_config.Data;
            _liveConfigReload       = GetComponent<LiveConfigReload>();
            _windowManager          = GetComponent<WindowManager>();
            _audioData              = GetComponent<AudioDataScript>();
            _nisualizerSceneManager = GetComponent<NisualizerSceneManagerScript>();
        }

        private void Awake()
        {
            //Set Instance and return if not this
            Instance = this;
            if (Instance != this) return;
            
            // Add the fps command to functions
            InteractiveTextProcessing.Functions["fps"] = param => (1 / DeltaTime).ToString(param);

            // Update the config directory and path
            UpdateConfigPath();
            
            // Set DefaultFont for ConfigText
            ConfigText.DefaultFont = ConfigData.Font;
        }

        private void Start()
        {
            // Load the Config in Start to allow for other scripts to subscribe to events in Awake
            ConfigScript.Init();
            
            // Load data from config
            OnConfigLoaded();
            ConfigData.OnLoaded += OnConfigLoaded;
            
            // Initialize AudioData
            AudioData.Initialize();
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

        // This has to be done on apk exit
        private void OnApplicationQuit()
        {
            AudioMonitorManager.Instance.Dispose();
        }

        // Trust me, / at the end is needed or file system monitor dies o_0
        private static void UpdateConfigPath()
        {
            // Update the config name if found
            var configName = Misc.GetArg("ConfigName");
            if (configName != null) ConfigScript.ConfigName = configName;
            
            // Update the config directory
            var configDir = Misc.GetArg("ConfigDir");
            if (configDir != null)
            {
                if (!configDir.EndsWith('/') && !configDir.EndsWith('\\')) configDir += '/';
                ConfigDirectory = configDir;
            }
            else
            {
                ConfigDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                if (Application.platform != RuntimePlatform.WindowsEditor) ConfigDirectory = Path.Combine(ConfigDirectory, ".config");
                ConfigDirectory = Path.Combine(ConfigDirectory, "Nisualizer/");
            }
        }

        private static void UpdateLiveConfigReload()
        {
            // It's dogshit to init every time but it works
            LiveConfigReload.Init();
            LiveConfigReload.MonitorPaths(ConfigData.Monitor);
        }

        private void OnConfigLoaded()
        {
            UpdateLiveConfigReload();
            ConfigText.DefaultFont = ConfigData.Font;
            InteractiveTextProcessing.Shell =  ConfigData.Shell;
            SetFPS();
            SetWindowMode();
            WindowManager.SwitchLayer(ConfigData.WindowLayer);
        }
        
        private void SetFPS() => Application.targetFrameRate = ConfigData.FPS;
        
        private void SetWindowMode() => Screen.fullScreenMode = ConfigData.WindowMode;
    }
}