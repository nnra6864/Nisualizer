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
            //Generate the default config if it doesn't exist
            GenerateDefaultConfig();
            
            //Load the config file
            LoadConfigFile();
            
            //Watch for changes in the config file
            //TODO: Implement a live reload check
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
        //Used to detect file changes
        private FileSystemWatcher _configWatcher;

        //Path to the config
        private readonly string _configPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), ".config/Nisualizer/config.json");

        //TODO: Paste the default config to ~/.config/Nisualizer/Config.json
        private void GenerateDefaultConfig()
        {
            
        }
        
        //Sets up a file watcher and config change handling
        private void WatchForConfigChanges()
        {
            //Set up the FileSystemWatcher to monitor changes to the config file
            _configWatcher = new()
            {
                Path = Path.GetDirectoryName(_configPath),
                Filter = Path.GetFileName(_configPath),
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.Size
            };
            _configWatcher.Changed += HandleConfigChanges;
            _configWatcher.EnableRaisingEvents = true;
        }

        //Regenerate if deleted or renamed, reload if changed
        private void HandleConfigChanges(object sender, FileSystemEventArgs args)
        {
            switch (args.ChangeType)
            {
                case WatcherChangeTypes.Deleted:
                    GenerateDefaultConfig();
                    break;
                case WatcherChangeTypes.Renamed:
                    GenerateDefaultConfig();
                    break;
                case WatcherChangeTypes.Changed:
                    LoadConfigFile();
                    break;
            }
        }

        private void LoadConfigFile()
        {
            Debug.Assert(File.Exists(_configPath), $"{_configPath} not found");
            
        }
        #endregion

        private void Awake() => LoadConfigFile();

        //TODO: This will trigger OnChanged events, cba to optimize now
        public void ResetToDefault()
        {
            FPS = _defaultFPS;
            Background = _defaultBackground;
        }

        public void Load()
        {
            //Reset to default to make sure all vars are set
            ResetToDefault();

            //TODO: Load all the values from a JSON config
            FPS = 60;
            Background = LoadBackground("~/.config/Backgrounds/Nord/GalaxyWaifu.jpg");
        }
    }
}
