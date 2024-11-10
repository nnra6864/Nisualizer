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
        public static ConfigScript Config => Instance._config;
        
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
            Config.Init();
        }
    }
}