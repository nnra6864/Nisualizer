using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

namespace SceneCreator.Editor
{
    public class SceneCreator : EditorWindow
    {
        [SerializeField] private VisualTreeAsset _visualTreeAsset;
        
        [SerializeField] private GameObject _gameManagerPrefab;
        
        private Button _createButton;
        private TextField _sceneNameField;

        [MenuItem("Window/UI Toolkit/SceneCreator")]
        public static void ShowExample()
        {
            var wnd = GetWindow<SceneCreator>();
            wnd.titleContent = new("SceneCreator");
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            var root = rootVisualElement;
            _visualTreeAsset.CloneTree(root);
            
            _sceneNameField = root.Q<TextField>("SceneName");
            
            _createButton = root.Q<Button>("CreateButton");
            _createButton.RegisterCallback<ClickEvent>(OnCreateButtonClick);
        }

        private void OnCreateButtonClick(ClickEvent evt) => CreateNisualizerScene();
        
        private void CreateNisualizerScene()
        {
            CreateScene();
        }

        private void CreateScene()
        {
            // Store scene name, dir and path
            var sceneName = _sceneNameField.text;
            var sceneDir = Path.Combine("Assets/Scenes", sceneName);
            var scenePath = Path.Combine(sceneDir, sceneName) + ".unity";
            
            // Return if scene name is empty
            if (string.IsNullOrEmpty(sceneName))
            {
                Debug.LogError("Scene name can't be empty");
                return;
            }
            
            // Create scene dir
            if (!CreateSceneDirectory(sceneName, sceneDir)) return;
            
            // Create scene asset
            CreateSceneAsset(scenePath);
        }

        private bool CreateSceneDirectory(string sceneName, string sceneDir)
        {
            // Check if the scene with that name already exists
            if (AssetDatabase.AssetPathExists(sceneDir))
            {
                Debug.LogError($"A scene directory named '{sceneName}' already exists");
                return false;
            }
            
            // Create the scene directory
            AssetDatabase.CreateFolder("Assets/Scenes", sceneName);
            return true;
        }

        private void CreateSceneAsset(string scenePath)
        {
            var newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            EditorSceneManager.SaveScene(newScene, scenePath);
        }
    }
}
