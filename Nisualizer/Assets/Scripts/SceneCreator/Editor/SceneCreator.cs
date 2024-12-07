using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace SceneCreator.Editor
{
    public class SceneCreator : EditorWindow
    {
        [SerializeField] private VisualTreeAsset _visualTreeAsset;
        
        [SerializeField] private GameObject _gameManagerPrefab;
        
        private Button _createButton;

        public string SceneName;

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
            
            var sceneNameField = root.Q<TextField>("SceneName");
            sceneNameField.RegisterValueChangedCallback(evt => SceneName = evt.newValue);
            
            _createButton = root.Q<Button>("CreateButton");
            _createButton.RegisterCallback<ClickEvent>(OnCreateButtonClick);
        }

        private void CreateNewScene()
        {
            Debug.Log(SceneName);
        }

        private void OnCreateButtonClick(ClickEvent evt) => CreateNewScene();
    }
}
