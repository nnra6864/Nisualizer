using Config;
using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(ConfigScript))]
    public class GameManager : MonoBehaviour
    {
        //Simple singleton implementation
        private static GameManager _instance;
        public static GameManager Instance
        {
            get
            {
                //If instance is null, find the GameManager in the scene
                _instance ??= FindFirstObjectByType<GameManager>();
                
                //If instance is still null, create a new one
                if (_instance == null)
                    _instance = new GameObject("GameManager", typeof(GameManager)).GetComponent<GameManager>();

                return _instance;
            }
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

        //Contains all the Config data and logic
        [SerializeField] private ConfigScript _config;
        public static ConfigScript ConfigScript => Instance._config;
        
        private void Reset()
        {
            _config = GetComponent<ConfigScript>();
        }


        private void Awake()
        {
            //Set Instance
            Instance = this;
        }

        private void Start()
        {
            //Load the Config in Start to allow for other scripts to subscribe to events in Awake
            ConfigScript.Init();
            
            //Set FPS
            SetFPS(ConfigScript.Config.General.FPS);
            ConfigScript.Config.OnConfigLoaded += conf => SetFPS(conf.General.FPS);
        }

        private void OnDestroy()
        {
            // Trust me, this if check is highly needed because you'll end up with major fuckery where the singleton just disappears on the next play, just trust me
            if (ConfigScript?.Config == null) return;
            
            ConfigScript.Config.OnConfigLoaded -= conf => SetFPS(conf.General.FPS);
        }

        private void SetFPS(int fps) => Application.targetFrameRate = fps;
    }
}