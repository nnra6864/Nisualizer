using System;
using System.IO;
using Config;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

namespace SceneCreator.Editor
{
    public class NisualizerSceneCreator : EditorWindow
    {
        private static Button _createButton;
        private static TextField _sceneNameField;

        // Stage of execution
        private static int _stage;
        
        private static string _sceneName, _sceneDir, _scenePath;
        
        // Storing the path instead of a direct reference to simplify the usage, especially in static functions
        private const string GameManagerPath = "Assets/Prefabs/GameManager.prefab";

        [SerializeField] private VisualTreeAsset _visualTreeAsset;
        
        [MenuItem("Assets/Create/Nisualizer Scene", false, -202)]
        public static void ShowSceneCreator()
        {
            ResetEditorData();
            GetWindow<NisualizerSceneCreator>("Nisualizer Scene Creator");
        }

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

        private static void CreateNisualizerScene()
        {
            if (_stage == 0) Stage0();
        }

        [UnityEditor.Callbacks.DidReloadScripts]
        private static void PostCreation()
        {
            LoadEditorData();
            
            // Return if not working
            if (_stage == 0) return;
            switch (_stage)
            {
                case 1: Stage1(); break;
                case 2: Stage2(); break;
            }
        }

        private static void Stage0()
        {
            // Store scene name, dir and path
            _sceneName = _sceneNameField.text;
            _sceneDir = Path.Combine("Assets/Scenes", _sceneName);
            _scenePath = Path.Combine(_sceneDir, _sceneName) + ".unity";
            
            // Return if scene name is empty
            if (string.IsNullOrEmpty(_sceneName))
            {
                Debug.LogError("Scene name can't be empty");
                return;
            }
            
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
            
            _stage = 0;
            ResetEditorData();
        }

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

        /// Always call with <see cref="EditorApplication.delayCall"/>, weird shit can happen otherwise
        private static void ReloadAssets()
        {
            // Saving all the data before reloading
            SaveEditorData();
            
            // Reload
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
            CompilationPipeline.RequestScriptCompilation();
        }

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

        private static void CreateSceneAsset()
        {
            var newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            EditorSceneManager.SaveScene(newScene, _scenePath);
        }

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
        
        private static void CreateDefaultConfig()
        {
            var configPath = Path.Combine(_sceneDir, $"{_sceneName}Config.json");
            File.WriteAllText(configPath, "{\n\n}");
        }
        
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

        private static void CreateConfigDataSO()
        {
            var so = CreateInstance($"{_sceneName}ConfigData");
            var soPath = Path.Combine(_sceneDir, $"{_sceneName}ConfigData.asset");
            AssetDatabase.CreateAsset(so, soPath);
        }

        private static void AddGameManager() => PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(GameManagerPath));

        private static void AddSceneManager()
        {
            // Create a new Game Object
            var go = new GameObject($"{_sceneName}Manager");
            
            // Add the Scene Script to it
            go.AddComponent(Type.GetType($"Scenes.{_sceneName}.{_sceneName}Manager, Assembly-CSharp"));
            
            // Change Config Script values
            var conf = go.GetComponent<ConfigScript>();
            conf.ConfigName = _sceneName;
            conf.DefaultConfig = AssetDatabase.LoadAssetAtPath<TextAsset>(Path.Combine(_sceneDir, $"{_sceneName}Config.json"));
            var sm = AssetDatabase.LoadAssetAtPath<ConfigData>(Path.Combine(_sceneDir, $"{_sceneName}ConfigData.asset"));
            Debug.Log(sm);
            conf.Data = sm;
        }
    }
}
