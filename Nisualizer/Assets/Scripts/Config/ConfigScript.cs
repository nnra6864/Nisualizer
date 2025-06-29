using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using NnUtils.Scripts;
using Scripts.Core;
using UnityEngine;

namespace Scripts.Config
{
    public class ConfigScript : MonoBehaviour
    {
        private const string SourcingRegexString = @"(?i)""source""\s*:\s*""([^\\""]*)""";
        private static readonly Regex TextRegex = new(SourcingRegexString, RegexOptions.Compiled);
        
        private static string ConfigDir => GameManagerScript.ConfigDirectory;
        
        public void Init()
        {
            // Set config dir and path
            _configPath = Path.Combine(ConfigDir, $"{ConfigName}.json");
                
            // Getting text directly because storing it as a text asset and not a string causes some code to not execute later :/
            _defaultConfigText = DefaultConfig.text;
            
            // Generate the default config if it doesn't exist
            GenerateDefaultConfigFile();
            
            // Load the config file
            LoadConfigFile();
            
            // Handle config changes
            GameManagerScript.LiveConfigReload.OnChanged += OnConfigChanged;
            
            // Load config values
            LoadConfig();
        }
        
        private void OnConfigChanged()
        {
            GenerateDefaultConfigFile();
            LoadConfigFile();
            LoadConfig();
        }
        
        private void OnDestroy()
        {
            GameManagerScript.LiveConfigReload.OnChanged -= OnConfigChanged;
        }
        
        #region ConfigFile

        // Has to be public so that it can be set via SceneCreator script
        [Tooltip("Name of the config file. Use / to store in a subdirectory, e.g. path/to/config.")]
        public string ConfigName = "Config";
        
        
        [Tooltip("Path to the config")]
        /*[ReadOnly]*/ [SerializeField] private string _configPath;
        
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
            var dirExists = Directory.Exists(ConfigDir);
            if (!dirExists) Directory.CreateDirectory(ConfigDir!);
            Debug.Log(debugPrefix + (dirExists ?
                $"Config directory already exists at {ConfigDir}" :
                $"Generated config directory at {ConfigDir}"));
            
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
            _configText = SourceConfig(File.ReadAllText(_configPath));
            Debug.Log(debugPrefix + $"Loaded config file from: {_configPath}");
        }

        /// <summary>
        /// Sources all the files found in the config
        /// </summary>
        /// <returns>Sourced config</returns>
        private string SourceConfig(string config) =>
            TextRegex.Replace(config, match =>
            {
                // Initialize result
                string result;
                
                // Get path from config
                var path = match.Groups[1].Value;

                // Return an empty string if not set to avoid weird behaviour
                if (string.IsNullOrEmpty(path)) return string.Empty;
                
                // Check if config exists and assign it
                result = File.Exists(path) ? File.ReadAllText(Path.GetFullPath(path)) : string.Empty;

                // Get config relative part
                var cfgPath = GameManagerScript.ConfigScript._configPath;

                // Check if file exists relative to config path and assign it
                if (File.Exists(cfgPath))
                {
                    var relativePath = Path.Combine(ConfigDir, path);
                    if (File.Exists(relativePath)) result = File.ReadAllText(relativePath);
                }
                
                // Remove {} from result if they exists
                result = result.Trim();
                if (result.StartsWith("{") && result.EndsWith("}"))
                    result = result.Substring(1, result.Length - 2).Trim();
                
                return result;
            });

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
    }
}
