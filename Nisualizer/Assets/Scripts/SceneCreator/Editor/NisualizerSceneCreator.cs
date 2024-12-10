using System;
using System.IO;
using Config;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

namespace SceneCreator.Editor
{
    public class NisualizerSceneCreator : EditorWindow
    {
        // Stage of execution
        private static int _stage;
        
        // Scene data
        private static string _sceneName, _sceneDir, _scenePath;
        
        // Storing the path instead of a direct reference to simplify the usage, especially in static functions
        private const string GameManagerPath = "Assets/Prefabs/GameManager.prefab";

        // UI Toolkit elements
        private static Button _createButton;
        private static TextField _sceneNameField;

        [SerializeField] private VisualTreeAsset _visualTreeAsset;
        
        #region EditorData
        
        private static void SaveEditorData()
        {
            EditorPrefs.SetString("SceneName", _sceneName);
            EditorPrefs.SetString("SceneDir", _sceneDir);
            EditorPrefs.SetString("ScenePath", _scenePath);
            EditorPrefs.SetInt("Stage", _stage);
        }

        private static void LoadEditorData()
        {
            _sceneName = EditorPrefs.GetString("SceneName", _sceneName);
            _sceneDir = EditorPrefs.GetString("SceneDir", _sceneDir);
            _scenePath = EditorPrefs.GetString("ScenePath", _scenePath);
            _stage = EditorPrefs.GetInt("Stage", _stage);
        }

        private static void ResetEditorData()
        {
            EditorPrefs.SetString("SceneName", "");
            EditorPrefs.SetString("SceneDir", "");
            EditorPrefs.SetString("ScenePath", "");
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
        
        /// Starts the scene creation if it's not already happening
        private static void CreateNisualizerScene()
        {
            if (_stage == 0) Stage0();
        }

        /// Gets called after each editor reload and handles all that later stages <br/>
        /// This is needed because some parts of the creation require newly created scripts to be compiled
        [UnityEditor.Callbacks.DidReloadScripts]
        private static void PostCreation()
        {
            // Load the data saved before the editor reload
            LoadEditorData();
            
            // Return if not working
            if (_stage == 0) return;
            
            // Call the appropriate stage
            switch (_stage)
            {
                case 1: Stage1(); break;
                case 2: Stage2(); break;
            }
        }

        /// Always call with <see cref="EditorApplication.delayCall"/>, weird shit can happen otherwise <br/>
        /// This function will stop all the code execution, everything that should be called after it must be done through a function with a <see cref="UnityEditor.Callbacks.DidReloadScripts"/> attribute
        private static void ReloadAssets()
        {
            // Saving all the data before reloading
            SaveEditorData();
            
            // Reload
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
            CompilationPipeline.RequestScriptCompilation();
        }
        
        #endregion
        
        #region Stages
        
        /// Takes care of: <br/>
        /// Checking if the scene name is invalid or already used <br/>
        /// 
        private static void Stage0()
        {
            // Get new scene data
            if (!GetSceneData()) return;
            
            // Save data to editor prefs
            SaveEditorData();

            // Create scene directory and asset
            if (!CreateSceneDirectory()) return;
            CreateSceneAsset();
            
            // Create the scene manager
            CreateSceneManagerScript();
            
            // Create config
            CreateDefaultConfig();
            CreateConfigDataScript();
            
            // Update stage and reload assets so that assets are available for the next stage
            _stage = 1;
            EditorApplication.delayCall += ReloadAssets;
        }

        private static void Stage1()
        {
            CreateConfigDataSO();
            
            // Update stage and reload assets so that assets are available for the next stage
            _stage = 2;
            EditorApplication.delayCall += ReloadAssets;
        }

        private static void Stage2()
        {
            AddGameManager();
            AddSceneManager();
            AddCamera();
            CreateVolumeProfile();
            AddVolume();
            
            _stage = 0;
            ResetEditorData();
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

            if (string.IsNullOrEmpty(_sceneName))
            {
                Debug.LogWarning("Scene name can't be empty");
                return false;
            }

            return true;
        }
        
        /// <summary>
        /// Creates the scene directory in Assets/Scenes
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

        /// Creates a new scene manager inheriting from the <see cref="Core.SceneScript"/>
        private static void CreateSceneManagerScript()
        {
            var scriptContent = $@"using Core;

namespace Scenes.{_sceneName}
{{
    public class {_sceneName}Manager : SceneScript
    {{
        public static {_sceneName}ConfigData ConfigData => ({_sceneName}ConfigData)Config.Data;
    }}
}}
";
            
            var scriptPath = Path.Combine(_sceneDir, $"{_sceneName}Manager.cs");
            File.WriteAllText(scriptPath, scriptContent);
        }
        
        /// Creates default config for the newly created scene
        private static void CreateDefaultConfig()
        {
            var configPath = Path.Combine(_sceneDir, $"{_sceneName}Config.json");
            File.WriteAllText(configPath, "{\n\n}");
        }
        
        /// Creates a new script inheriting from the <see cref="ConfigData"/>
        private static void CreateConfigDataScript()
        {
            var scriptContent = $@"using Config;
using UnityEngine;

namespace Scenes.{_sceneName}
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
            
            var scriptPath = Path.Combine(_sceneDir, $"{_sceneName}ConfigData.cs");
            File.WriteAllText(scriptPath, scriptContent);
        }
        
        #endregion

        #region Stage1
        
        /// Creates a scriptable object instance of the newly create config data type <br/>
        /// Requires the newly created config data type to be compiled which can be achieved with the <see cref="ReloadAssets"/> method
        private static void CreateConfigDataSO()
        {
            var so = CreateInstance($"{_sceneName}ConfigData");
            var soPath = Path.Combine(_sceneDir, $"{_sceneName}ConfigData.asset");
            AssetDatabase.CreateAsset(so, soPath);
        }

        #endregion
        
        #region Stage2
        
        /// Adds <see cref="Core.GameManager"/> to the newly created scene
        private static void AddGameManager() => PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(GameManagerPath));

        /// Adds a newly created scene manager to the scene <br/>
        /// Requires created scripts to be compiled, call <see cref="ReloadAssets"/> before this function
        private static void AddSceneManager()
        {
            // Create a new Game Object
            GameObject go = new($"{_sceneName}Manager");
            
            // Add the Scene Script to it
            go.AddComponent(Type.GetType($"Scenes.{_sceneName}.{_sceneName}Manager, Assembly-CSharp"));
            
            // Change Config Script values
            var conf = go.GetComponent<ConfigScript>();
            conf.ConfigName = _sceneName;
            conf.DefaultConfig = AssetDatabase.LoadAssetAtPath<TextAsset>(Path.Combine(_sceneDir, $"{_sceneName}Config.json"));
            var sm = AssetDatabase.LoadAssetAtPath<ConfigData>(Path.Combine(_sceneDir, $"{_sceneName}ConfigData.asset"));
            conf.Data = sm;
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
        }

        // TODO: Implement following functions
        /// Creates a new <see cref="UnityEngine.Rendering.VolumeProfile"/>
        private static void CreateVolumeProfile()
        {
            
        }
        
        /// Adds a volume to the scene and 
        private static void AddVolume()
        {
            
        }
        
        #endregion
    }
}
