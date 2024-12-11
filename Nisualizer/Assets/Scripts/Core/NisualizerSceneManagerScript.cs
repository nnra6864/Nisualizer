using Config;
using UnityEngine;

namespace Core
{
    public class NisualizerSceneManagerScript : MonoBehaviour
    {
        private static GeneralConfigData _configData;
        private static GeneralConfigData ConfigData => _configData ??= (GeneralConfigData)GameManagerScript.ConfigScript.Data;
        
        private string _currentScene;

        private void Start()
        {
            UpdateScene();
            ConfigData.OnLoaded += UpdateScene;
        }
        
        private void OnDestroy() => ConfigData.OnLoaded -= UpdateScene;

        private void UpdateScene()
        {
            // Store scene
            var scene = ConfigData.Scene;
            
            // Return if in the same scene or scene is invalid
            if (string.IsNullOrEmpty(scene)) return;
            if (scene == _currentScene) return;
            
            // Load the scene
            _currentScene = scene;
            UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
        }
    }
}