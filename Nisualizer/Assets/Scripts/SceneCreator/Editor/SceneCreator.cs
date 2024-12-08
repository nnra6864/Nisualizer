using System.IO;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace SceneCreator.Editor
{
    public class SceneCreator : EditorWindow
    {
        private static Button _createButton;
        private static TextField _sceneNameField;

        private static bool _isWorking;
        private static string _sceneName, _sceneDir, _scenePath;
        
        [SerializeField] private VisualTreeAsset _visualTreeAsset;
        
        // This is an awful way to access the prefab from static scripts, cry about it
        private const string GameManagerPath = "Assets/Prefabs/GameManager.prefab";
        private static GameObject _gameManager;

        [MenuItem("Window/UI Toolkit/SceneCreator")]
        public static void ShowExample()
        {
            // Setting to false here in case the function was cancelled or sm
            _isWorking = false;
            
            var wnd = GetWindow<SceneCreator>();
            wnd.titleContent = new("SceneCreator");
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            var root = rootVisualElement;
            _visualTreeAsset.CloneTree(root);
            
            // Link the text field
            _sceneNameField = root.Q<TextField>("SceneName");
            
            // Link the button
            _createButton = root.Q<Button>("CreateButton");
            _createButton.RegisterCallback<ClickEvent>(OnCreateButtonClick);
        }

        private static void OnCreateButtonClick(ClickEvent evt) => CreateNisualizerScene();

        private static void CreateNisualizerScene()
        {
            // Return if creation is already in progress
            if (_isWorking) return;
            
            // Store scene name, dir and path
            _sceneName = _sceneNameField.text;
            _sceneDir = Path.Combine("Assets/Scenes", _sceneName);
            _scenePath = Path.Combine(_sceneDir, _sceneName) + ".unity";
            
            // Load the game manager prefab
            _gameManager = AssetDatabase.LoadAssetAtPath<GameObject>(GameManagerPath);
            
            // Return if scene name is empty
            if (string.IsNullOrEmpty(_sceneName))
            {
                Debug.LogError("Scene name can't be empty");
                _isWorking = false;
                return;
            }

            // Set _isWorking to true so the function can't get called twice
            _isWorking = true;
            
            // Save data to editor prefs
            SaveEditorData();

            // Create scene directory and asset
            if (!CreateSceneDirectory())
            {
                _isWorking = false;
                return;
            }
            CreateSceneAsset();
            
            // Create the scene manager
            CreateSceneManagerScript();
            
            // Create config
            CreateDefaultConfig();
            CreateConfigDataScript();
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
            CompilationPipeline.RequestScriptCompilation();
            
            // The process gets finished in the PostCreation function
            // It has to be done this way due to all processes getting cancelled on reload
        }

        [UnityEditor.Callbacks.DidReloadScripts]
        private static void PostCreation()
        {
            LoadEditorData();
            if (!_isWorking) return;
            
            _gameManager = AssetDatabase.LoadAssetAtPath<GameObject>(GameManagerPath);
            CreateConfigDataSO();
            AddGameManager();
            _isWorking = false;
        }

        private static void SaveEditorData()
        {
            EditorPrefs.SetString("SceneName", _sceneName);
            EditorPrefs.SetString("SceneDir", _sceneDir);
            EditorPrefs.SetString("ScenePath", _scenePath);
            EditorPrefs.SetBool("IsWorking", _isWorking);
        }

        private static void LoadEditorData()
        {
            _sceneName = EditorPrefs.GetString("SceneName", _sceneName);
            _sceneDir = EditorPrefs.GetString("SceneDir", _sceneDir);
            _scenePath = EditorPrefs.GetString("ScenePath", _scenePath);
            _isWorking = EditorPrefs.GetBool("IsWorking", _isWorking);
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
            AssetDatabase.Refresh();
        }
        
        private static void CreateDefaultConfig()
        {
            var configPath = Path.Combine(_sceneDir, $"{_sceneName}Config.json");
            File.WriteAllText(configPath, "{\n\n}");
            AssetDatabase.Refresh();
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
        public override void Reset()
        {{
            // Your code here

            base.Reset();
        }}
    }}
}}
";
            
            var scriptPath = Path.Combine(_sceneDir, $"{_sceneName}ConfigData.cs");
            File.WriteAllText(scriptPath, scriptContent);
            AssetDatabase.Refresh();
        }

        private static void CreateConfigDataSO()
        {
            var so = CreateInstance($"{_sceneName}ConfigData");
            var soPath = Path.Combine(_sceneDir, $"{_sceneName}ConfigData.asset");
            AssetDatabase.CreateAsset(so, soPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void AddGameManager()
        {
            PrefabUtility.InstantiatePrefab(_gameManager, SceneManager.GetSceneByName(_sceneName));
        }
    }
}
