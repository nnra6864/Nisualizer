using System;
using System.Collections.Generic;
using System.IO;
using Config;
using Core;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

namespace SceneCreator.Editor
{
    public class NisualizerSceneCreator : EditorWindow
    {
        /// Stage of execution
        private static int _stage;
        
        // Scene data
        private static string _sceneName, _sceneDir, _scenePath, _sceneScriptsDir, _sceneConfigDir;
        
        // Storing the path instead of a direct reference to simplify the usage, especially in static functions
        private const string GameManagerPath = "Assets/Prefabs/GameManager.prefab";

        // UI Toolkit elements
        private static Button _createButton;
        private static TextField _sceneNameField;

        [SerializeField] private VisualTreeAsset _visualTreeAsset;
        
        #region EditorData
        
        /// Saves all data to editor prefs
        private static void SaveEditorData()
        {
            EditorPrefs.SetString("SceneName", _sceneName);
            EditorPrefs.SetString("SceneDir", _sceneDir);
            EditorPrefs.SetString("ScenePath", _scenePath);
            EditorPrefs.SetString("SceneScriptsDir", _sceneScriptsDir);
            EditorPrefs.SetString("SceneConfigDir", _sceneConfigDir);
            EditorPrefs.SetInt("Stage", _stage);
        }

        /// Loads all data from editor prefs
        private static void LoadEditorData()
        {
            _sceneName = EditorPrefs.GetString("SceneName", _sceneName);
            _sceneDir = EditorPrefs.GetString("SceneDir", _sceneDir);
            _scenePath = EditorPrefs.GetString("ScenePath", _scenePath);
            _sceneScriptsDir = EditorPrefs.GetString("SceneScriptsDir", _sceneScriptsDir);
            _sceneConfigDir = EditorPrefs.GetString("SceneConfigDir", _sceneConfigDir);
            _stage = EditorPrefs.GetInt("Stage", _stage);
        }

        /// Resets all data in editor prefs
        private static void ResetEditorData()
        {
            EditorPrefs.SetString("SceneName", "");
            EditorPrefs.SetString("SceneDir", "");
            EditorPrefs.SetString("ScenePath", "");
            EditorPrefs.SetString("SceneScriptsDir", "");
            EditorPrefs.SetString("SceneConfigDir", "");
            EditorPrefs.SetInt("Stage", 0);
        }
        
        #endregion

        #region MenuCreation
        
        /// Creates the Nisualizer Scene Creator window
        [MenuItem("Assets/Create/Nisualizer Scene", false, -202)]
        public static void ShowSceneCreator()
        {
            // Reset editor data and stage in case operation was cancelled midway through
            ResetEditorData();
            _stage = 0;
            
            // Create the window
            GetWindow<NisualizerSceneCreator>("Nisualizer Scene Creator");
        }

        /// Creates the GUI and subscribes to needed UI events
        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            var root = rootVisualElement;
            _visualTreeAsset.CloneTree(root);
            
            // Link the text field
            _sceneNameField = root.Q<TextField>("SceneName");
            
            // Create the Nisualizer scene if Return is pressed
            _sceneNameField.RegisterCallback<KeyDownEvent>(evt =>
                { if (evt.keyCode == KeyCode.Return) CreateNisualizerScene(); }, TrickleDown.TrickleDown);
            
            // Link the button
            _createButton = root.Q<Button>("CreateButton");
            
            // Create the Nisualizer scene if button is pressed
            _createButton.RegisterCallback<ClickEvent>(_ => CreateNisualizerScene());
        }
        
        #endregion
        
        #region SceneCreation

        /// Starts the scene creation process if it's not already happening
        private static void CreateNisualizerScene()
        {
            // Load the editor data to get the current stage
            LoadEditorData();
            
            // Start the scene creation if it's not already happening
            if (_stage == 0) ExecuteCurrentStage();
        }

        /// Takes care of resuming the execution after reload
        [UnityEditor.Callbacks.DidReloadScripts]
        private static void HandleReload()
        {
            // Load the editor data to get the current stage
            LoadEditorData();
            
            // Continue execution if it was stopped
            if (_stage != 0) ExecuteCurrentStage();
        }
        
        /// Executes current stage and handles data reloading and saving
        private static void ExecuteCurrentStage()
        {
            // Execute current stage
            Stages[_stage]?.Invoke();
            
            // Check if this is the last stage
            if (_stage == Stages.Count - 1)
            {
                // Set stage back to 0
                _stage = 0;
                
                // Save the scene
                EditorSceneManager.SaveOpenScenes();
                
                // Reload and save data
                Reload();
                
                // Reset editor data
                ResetEditorData();

                return;
            }

            // Increment stage
            _stage++;
            
            // Save editor data
            SaveEditorData();
            
            // Reload and compile
            EditorApplication.delayCall += ReloadAndCompile;
        }

        /// Gets called after each editor reload and handles all that later stages <br/>
        /// This is needed because some parts of the creation require newly created scripts to be compiled
        
        /// Saves and reloads assets
        private static void Reload()
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        /// Always call with <see cref="EditorApplication.delayCall"/>, weird shit can happen otherwise <br/>
        /// This function will stop all the code execution, everything that should be called after it must be done through a function with a <see cref="UnityEditor.Callbacks.DidReloadScripts"/> attribute
        private static void ReloadAndCompile()
        {
            // Saving all the data before reloading
            SaveEditorData();
            
            // Reload
            Reload();
            
            // Compile
            CompilationPipeline.RequestScriptCompilation();
        }

        #endregion
        
        #region Stages

        /// Holds all the stages to be executed by the <see cref="ExecuteCurrentStage"/> method
        private static readonly List<Action> Stages = new()
        {
            Stage0,
            Stage1,
            Stage2,
            Stage3
        };
        
        /// Takes care of: <br/>
        /// Getting the scene data <br/>
        /// Checking if the scene data is invalid or already used <br/>
        /// Creating scene directory and asset <br/>
        /// Creating the scene manager script, default config and config data script
        private static void Stage0()
        {
            // Get new scene data
            if (!GetSceneData()) return;

            // Create scene directory and asset
            if (!CreateSceneDirectory()) return;
            CreateSceneAsset();
            
            // Create the Scripts dir and Scene Manager
            CreateScriptsDirectory();
            CreateSceneManagerScript();
            
            // Create config dir and data
            CreateConfigDirectory();
            CreateDefaultConfig();
            CreateConfigDataScript();
        }

        /// Creates an instance of a config data scriptable object
        private static void Stage1()
        {
            // Create an instance of the config SO
            CreateConfigDataSO();
        }

        /// Takes care of: <br/>
        /// Adding a <see cref="GameManagerScript"/> instance to the scene <br/>
        /// Adding a <see cref="SceneManagerScript"/> instance to the scene <br/>
        /// Creating a <see cref="VolumeProfile"/> for the scene
        private static void Stage2()
        {
            // Add GameManager to the scene
            AddGameManager();
            
            // Add SceneManager to the scene
            AddSceneManager();
            
            // Create the volume profile for the scene
            CreateVolumeProfile();
        }

        /// Takes care of: <br/>
        /// Adding a <see cref="Volume"/> with a previously created <see cref="VolumeProfile"/> to the scene <br/>
        /// Adding a camera to the scene
        private static void Stage3()
        {
            // Add volume to the scene and apply the scene volume profile
            AddVolume();
            
            // Add the camera
            AddCamera();
        }
        
        #endregion

        #region Stage0

        /// <summary>
        /// Stores values into <see cref="_sceneName"/>, <see cref="_sceneDir"/>, <see cref="_scenePath"/>
        /// </summary>
        /// <returns>Whether the scene name is valid</returns>
        private static bool GetSceneData()
        {
            // Store scene name, dir and path
            _sceneName = _sceneNameField.text;
            _sceneDir = Path.Combine("Assets/Scenes", _sceneName);
            _scenePath = Path.Combine(_sceneDir, _sceneName) + ".unity";
            _sceneScriptsDir = Path.Combine(_sceneDir, "Scripts");
            _sceneConfigDir = Path.Combine(_sceneDir, "Config");

            if (string.IsNullOrEmpty(_sceneName))
            {
                Debug.LogWarning("Scene name can't be empty");
                return false;
            }

            return true;
        }
        
        /// <summary>
        /// Creates the scene directory in Assets/Scenes/
        /// </summary>
        /// <returns>Whether the scene directory was successfully created</returns>
        private static bool CreateSceneDirectory()
        {
            // Check if the scene with that name already exists
            if (AssetDatabase.AssetPathExists(_sceneDir))
            {
                Debug.LogError($"A scene directory named '{_sceneName}' already exists");
                return false;
            }
            
            // Create the scene directory
            AssetDatabase.CreateFolder("Assets/Scenes", _sceneName);
            return true;
        }

        /// Creates a new scene asset
        private static void CreateSceneAsset()
        {
            var newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            EditorSceneManager.SaveScene(newScene, _scenePath);
        }

        /// Creates the Config directory in Assets/Scenes/SceneName/
        private static void CreateScriptsDirectory() => AssetDatabase.CreateFolder(_sceneDir, "Scripts");
        
        /// Creates a new scene manager inheriting from the <see cref="SceneManagerScript"/>
        private static void CreateSceneManagerScript()
        {
            var scriptContent = $@"using Core;
using Scenes.{_sceneName}.Config;

namespace Scenes.{_sceneName}.Scripts
{{
    public class {_sceneName}Manager : SceneManagerScript
    {{
        public static {_sceneName}ConfigData ConfigData => ({_sceneName}ConfigData)Config.Data;
    }}
}}
";
            
            var scriptPath = Path.Combine(_sceneScriptsDir, $"{_sceneName}Manager.cs");
            File.WriteAllText(scriptPath, scriptContent);
        }
        
        /// Creates the Config directory in Assets/Scenes/SceneName/
        private static void CreateConfigDirectory() => AssetDatabase.CreateFolder(_sceneDir, "Config");
        
        /// Creates default config for the newly created scene
        private static void CreateDefaultConfig()
        {
            var configPath = Path.Combine(_sceneConfigDir, $"{_sceneName}Config.json");
            File.WriteAllText(configPath, "{\n\n}");
        }
        
        /// Creates a new script inheriting from the <see cref="ConfigData"/>
        private static void CreateConfigDataScript()
        {
            var scriptContent = $@"using Config;
using UnityEngine;

namespace Scenes.{_sceneName}.Config
{{
    // This class should be a perfect match of your JSON config
    [CreateAssetMenu(fileName = ""{_sceneName}ConfigData"", menuName = ""Config/{_sceneName}ConfigData"")]
    public class {_sceneName}ConfigData : ConfigData
    {{
        // Gets called when config is loaded
        public override void Load()
        {{
            // Your code here

            base.Load();
        }}
        
        // Use this function to reset all values to default
        public override void ResetToDefault(bool silent = false)
        {{
            // Your code here

            base.ResetToDefault(silent);
        }}
    }}
}}
";
            
            var scriptPath = Path.Combine(_sceneConfigDir, $"{_sceneName}ConfigData.cs");
            File.WriteAllText(scriptPath, scriptContent);
        }
        
        #endregion

        #region Stage1
        
        /// Creates a scriptable object instance of the newly create config data type <br/>
        /// Requires the newly created config data type to be compiled which can be achieved with the <see cref="ReloadAndCompile"/> method
        private static void CreateConfigDataSO()
        {
            var so = CreateInstance($"{_sceneName}ConfigData");
            var soPath = Path.Combine(_sceneConfigDir, $"{_sceneName}ConfigData.asset");
            AssetDatabase.CreateAsset(so, soPath);
        }

        #endregion
        
        #region Stage2
        
        /// Adds <see cref="GameManagerScript"/> to the newly created scene
        private static void AddGameManager()
        {
            // Instantiate GameManager prefab
            var go = PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(GameManagerPath));
            
            // Mark the object as dirty so it doesn't get deleted on scene reload
            EditorUtility.SetDirty(go);
        }

        /// Adds a newly created scene manager to the scene <br/>
        /// Requires created scripts to be compiled, call <see cref="ReloadAndCompile"/> before this function
        private static void AddSceneManager()
        {
            // Create a new Game Object
            GameObject go = new($"{_sceneName}Manager");
            
            // Add the Scene Script to it
            go.AddComponent(Type.GetType($"Scenes.{_sceneName}.Scripts.{_sceneName}Manager, Assembly-CSharp"));
            
            // Change Config Script values
            var conf = go.GetComponent<ConfigScript>();
            conf.ConfigName = _sceneName;
            conf.DefaultConfig = AssetDatabase.LoadAssetAtPath<TextAsset>(Path.Combine(_sceneConfigDir, $"{_sceneName}Config.json"));
            
            // Change Scene Manager values
            var sm = AssetDatabase.LoadAssetAtPath<ConfigData>(Path.Combine(_sceneConfigDir, $"{_sceneName}ConfigData.asset"));
            conf.Data = sm;
            
            // Mark the object as dirty so it doesn't get deleted on scene reload
            EditorUtility.SetDirty(go);
        }
        
        /// Creates a new <see cref="UnityEngine.Rendering.VolumeProfile"/> in the scene dir
        private static void CreateVolumeProfile() =>
            AssetDatabase.CreateAsset(CreateInstance<VolumeProfile>(), Path.Combine(_sceneDir, $"{_sceneName}VolumeProfile.asset"));

        #endregion
        
        #region Stage3
        
        /// Adds a volume with the previously created profile to the scene
        private static void AddVolume()
        {
            // Create a new game object
            GameObject go = new("GlobalVolume");
            
            // Attach volume component to it
            var vol = go.AddComponent<Volume>();
            
            // Assign the newly created profile to it
            var ass = AssetDatabase.LoadAssetAtPath<VolumeProfile>(Path.Combine(_sceneDir, $"{_sceneName}VolumeProfile.asset"));
            vol.sharedProfile = ass;
            
            // Mark the object as dirty so it doesn't get deleted on scene reload
            EditorUtility.SetDirty(go);
        }

        /// Creates a new camera
        private static void AddCamera()
        {
            // Create a new Game Object
            GameObject go = new("Camera")
            {
                // Adjust the camera position
                transform = { position = new(0, 0, -10) }
            };

            // Add the camera component
            go.AddComponent<Camera>();
            
            // Add the audio listener to the camera
            go.AddComponent<AudioListener>();

            // Enable post-processing
            var uacd = go.AddComponent<UniversalAdditionalCameraData>();
            uacd.renderPostProcessing = true;
            
            // Mark the object as dirty so it doesn't get deleted on scene reload
            EditorUtility.SetDirty(go);
        }
        
        #endregion
    }
}
