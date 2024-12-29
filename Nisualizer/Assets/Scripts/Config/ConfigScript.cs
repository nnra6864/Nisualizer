using System;
using System.Collections;
using System.IO;
using Core;
using NnUtils.Scripts;
using UnityEngine;
using Newtonsoft.Json;

namespace Config
{
    public class ConfigScript : MonoBehaviour
    {
        public void Init()
        {
            // Set config path
            _configPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), $".config/Nisualizer/{ConfigName}.json");
                
            // Getting text directly because storing it as a text asset and not a string causes some code to not execute later :/
            _defaultConfigText = DefaultConfig.text;
            
            // Generate the default config if it doesn't exist
            GenerateDefaultConfigFile();
            
            // Load the config file
            LoadConfigFile();
            
            // Watch for config file changes
            InitializeFSM();
            
            // Load config values
            LoadConfig();
        }

        public void Update()
        {
            // Check whether the config changed, and if yes, handle it
            ConfigChangeCheck();
        }
        
        #region ConfigFile

        // Has to be public so that it can be set via SceneCreator script
        [Tooltip("Name of the config file. Use / to store in a subdirectory, e.g. path/to/config.")]
        public string ConfigName = "Config";
        
        [Tooltip("Path to the config")]
        [ReadOnly] [SerializeField] private string _configPath;
        
        // Has to be public so that it can be set via SceneCreator script
        [Tooltip("Default Config")]
        public TextAsset DefaultConfig;
        
        /// Gets loaded from resources in <see cref="Init"/>
        private string _defaultConfigText;
        
        /// Config text loaded from the config file
        private string _configText;
        
        [Tooltip("Config Data Script")]
        [SerializeField] private ConfigData _data;
        
        /// Contains all the config values and gets loaded in <see cref="LoadConfig"/>
        public ConfigData Data
        {
            get => _data;
            set => _data = value;
        }

        /// <summary>
        /// Generates the default config file if one is not found
        /// </summary>
        /// <returns>Whether config is generated</returns>
        private bool GenerateDefaultConfigFile()
        {
            var debugPrefix = $"[{ConfigName}] GenerateDefaultConfig: ";

            //Return if config already exists
            if (File.Exists(_configPath))
            {
                Debug.Log(debugPrefix + "Config already exists, returning");
                return false;
            }

            //Generate parent directories if they don't exist
            var configDir = Path.GetDirectoryName(_configPath);
            var dirExists = Directory.Exists(configDir);
            if (!dirExists) Directory.CreateDirectory(configDir!);
            Debug.Log(debugPrefix + (dirExists ?
                $"Config directory already exists at {configDir}" :
                $"Generated config directory at {configDir}"));
            
            //Write the contents to the target config path
            File.WriteAllText(_configPath, _defaultConfigText);
            Debug.Log(debugPrefix + $"Default config generated at: {_configPath}");
            return true;
        }

        /// Loads the config file into memory
        private void LoadConfigFile()
        {
            var debugPrefix = $"[{ConfigName}] LoadConfigFile: ";
            
            Debug.Assert(File.Exists(_configPath), $"{_configPath} not found");
            _configText = File.ReadAllText(_configPath);
            Debug.Log(debugPrefix + $"Loaded config file from: {_configPath}");
        }
        
        /// Loads config values into a new Config object
        private void LoadConfig()
        {
            var debugPrefix = $"[{ConfigName}] LoadConfig: ";
            
            // Reset to default to make sure all vars are set and changes no longer present in config are undone
            // Using ResetSilent because the OnChanged event will get triggered by Load() anyways
            Data.ResetToDefault(true);
            Debug.Log(debugPrefix + "Reset to default config");

            // Reload the config
            JsonConvert.PopulateObject(_configText, Data);
            Data.Load();
            Debug.Log(debugPrefix + $"Loaded config from {_configPath}");
        }
        
        #endregion
        
        #region LiveConfigReload
        
        /// Used to monitor files
        private FileSystemMonitor _fsm;

        private bool _hasChanged;

        private void InitializeFSM() => _fsm = new(path: _configPath, () => _hasChanged = true);

        /// Checks whether the config has changed and restarts the <see cref="HandleConfigChangedRoutine"/> <br/>
        /// Should be used in <see cref="Update"/>
        private void ConfigChangeCheck()
        {
            if (!_hasChanged) return;
            _hasChanged = false;
            this.RestartRoutine(ref _handleConfigChangedRoutine, HandleConfigChangedRoutine());
        }

        /// Used to store the <see cref="HandleConfigChangedRoutine"/>
        private Coroutine _handleConfigChangedRoutine;
        
        /// Runs following functions: <br/>
        /// <see cref="GenerateDefaultConfigFile"/>
        /// <see cref="LoadConfigFile"/>
        /// <see cref="LoadConfig"/>
        private IEnumerator HandleConfigChangedRoutine()
        {
            yield return new WaitForSecondsRealtime(GameManagerScript.ConfigData.ReloadDelay);
            GenerateDefaultConfigFile();
            LoadConfigFile();
            LoadConfig();
        }
        
        #endregion
        
        private void OnDestroy()
        {
            _fsm?.Dispose();
        }
    }
}
