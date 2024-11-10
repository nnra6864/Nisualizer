using System;
using System.IO;
using NnUtils.Scripts;
using UnityEngine;

namespace Core
{
    public class ConfigScript : MonoBehaviour
    {
        public void Init()
        {
            //Load the default config from Resources
            //Getting text directly because storing it as a text asset and not a string causes some code to not execute later :/
            _defaultConfig = Resources.Load<TextAsset>("DefaultConfig").text;
            Debug.Assert(_defaultConfig != null, "Default config was not found in Resources, this is really bad!");
            
            //Generate the default config if it doesn't exist
            GenerateDefaultConfig();
            
            //Load the config file
            LoadConfigFile();
            
            //Watch for changes in the config file
            WatchForConfigChanges();
        }
        
        #region FPS
        [SerializeField] private int _defaultFPS = 60;

        private int _fps;
        public int FPS
        {
            get => _fps;
            private set
            {
                if (_fps == value) return;
                _fps = value;
                Application.targetFrameRate = _fps;
                OnFPSChanged?.Invoke(value);
            }
        }

        public Action<int> OnFPSChanged;
        #endregion

        #region Background
        //Used to store the background path in order to avoid unnecessary updates
        private string _backgroundPath;

        [SerializeField] private Sprite _defaultBackground;

        private Sprite _background;
        public Sprite Background
        {
            get => _background;
            private set
            {
                //No need for duplicate check, too expensive here
                _background = value;
                OnBackgroundChanged?.Invoke(value);
            }
        }

        public Action<Sprite> OnBackgroundChanged;

        private Sprite LoadBackground(string bg)
        {
            if (_backgroundPath == bg) return _background;
            _backgroundPath = bg;

            //Read the config value and replace relative with full path
            bg = bg.Replace("~", Environment.GetFolderPath(Environment.SpecialFolder.Personal));

            //TODO: Add custom logging when implemented
            //Check if the file exists, if not, return the default bg
            if (!File.Exists(bg)) return _defaultBackground;

            //Read image data and store it into a Texture2D
            var bgData = File.ReadAllBytes(bg);
            Texture2D bgTex = new(0, 0);
            //TODO: Add custom logging when implemented
            if (!bgTex.LoadImage(bgData)) return _defaultBackground;

            return Misc.Texture2DToSprite(bgTex);
        }
        #endregion

        #region ConfigFile
        //Gets loaded from resources in Init()
        private string _defaultConfig;
        
        //Used to detect file changes
        private FileSystemWatcher _configWatcher;

        //Path to the config
        private readonly string _configPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), ".config/Nisualizer/config.json");

        private void GenerateDefaultConfig()
        {
            const string debugPrefix = "GenerateDefaultConfig: ";
            
            //Return if config already exists
            if (File.Exists(_configPath))
            {
                Debug.Log(debugPrefix + "Config already exists, returning");
                return;
            }

            //Generate parent directories if they don't exist
            var configDir = Path.GetDirectoryName(_configPath);
            var dirExists = Directory.Exists(configDir);
            if (!dirExists) Directory.CreateDirectory(configDir!);
            Debug.Log(debugPrefix + (dirExists ?
                $"Config directory already exists at {configDir}" :
                $"Generated config directory at {configDir}"));
            
            //Write the contents to the target config path
            File.WriteAllText(_configPath, _defaultConfig);
            Debug.Log(debugPrefix + $"Default config generated at: {_configPath}");
        }

        private void LoadConfigFile()
        {
            Debug.Assert(File.Exists(_configPath), $"{_configPath} not found");
        }
        
        //Sets up the FileSystemWatcher to monitor changes to the config file
        private void WatchForConfigChanges()
        {
            const string debugPrefix = "WatchForConfigChanges: ";
            
            _configWatcher = new()
            {
                Path = Path.GetDirectoryName(_configPath),
                Filter = Path.GetFileName(_configPath),
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.Size
            };
            //BUG: Renamed event gets triggered after editing the file(with nvim at least)
            _configWatcher.Changed += HandleConfigChanged;
            _configWatcher.Renamed += HandleConfigRenamed;
            _configWatcher.Deleted += HandleConfigDeleted;
            _configWatcher.EnableRaisingEvents = true;
            
            Debug.Log(debugPrefix + "Config watcher set up");
        }

        private void HandleConfigChanged(object sender, FileSystemEventArgs args)
        {
            Debug.Log("Config changed, reloading");
            LoadConfigFile();
        }
        
        private void HandleConfigRenamed(object sender, FileSystemEventArgs args)
        {
            Debug.Log("Config renamed, regenerating");
            GenerateDefaultConfig();
        }
        
        private void HandleConfigDeleted(object sender, FileSystemEventArgs args)
        {
            Debug.Log("Config deleted, regenerating");
            GenerateDefaultConfig();
        }
        #endregion

        //TODO: This will trigger OnChanged events, cba to optimize now
        private void ResetToDefault()
        {
            FPS = _defaultFPS;
            Background = _defaultBackground;
        }

        private void Load()
        {
            //Reset to default to make sure all vars are set
            ResetToDefault();

            //TODO: Load all the values from a JSON config
            FPS = 60;
            Background = LoadBackground("~/.config/Backgrounds/Nord/GalaxyWaifu.jpg");
        }
    }
}
