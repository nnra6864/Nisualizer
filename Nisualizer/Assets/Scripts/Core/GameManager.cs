using Config;
using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(ConfigScript), typeof(FontManager))]
    public class GameManager : MonoBehaviour
    {
        //Simple singleton implementation
        private static GameManager _instance;
        public static GameManager Instance
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
        [SerializeField] private ConfigScript _config;
        public static ConfigScript ConfigScript => Instance._config;

        private static GeneralConfigData _configData => (GeneralConfigData)Instance._config.Data;
        
        [SerializeField] private FontManager _fontManager;
        public static FontManager FontManager => Instance._fontManager;
        
        private void Reset()
        {
            _config = GetComponent<ConfigScript>();
            _fontManager = GetComponent<FontManager>();
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
            
            // Set FPS
            SetFPS();
            _configData.OnLoaded += SetFPS;
        }

        private void OnDestroy()
        {
            // Trust me, this if check is highly needed because you'll end up with major fuckery where the singleton just disappears on the next play, just trust me
            if (ConfigScript?.Data == null) return;
            
            _configData.OnLoaded -= SetFPS;
        }

        private void SetFPS() => Application.targetFrameRate = _configData.FPS;
    }
}