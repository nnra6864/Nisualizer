using Config;
using UnityEngine;

namespace Core
{
    public class SceneManager : MonoBehaviour
    {
        private static ConfigScript Config => GameManager.ConfigScript;
        private static GeneralConfigData ConfigData => (GeneralConfigData)Config.Data;
        
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