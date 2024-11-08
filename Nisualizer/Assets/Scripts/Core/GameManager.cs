using System.IO;
using UnityEngine;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        public static GameManager Instance
        {
            get
            {
                if (_instance == null) 
                    _instance = new GameObject("GameManager", typeof(GameManager)).GetComponent<GameManager>();
                return _instance;
            }
            private set
            {
                if (_instance != null && _instance != value)
                {
                    Destroy(value.gameObject);
                    return;
                }
                _instance = value;
            }
        }
    
        private int _fps;
        public static int FPS => Instance._fps;

        private Sprite _background;
        public static Sprite Background => Instance._background;
    
        private void Awake()
        {
            //Set Instance
            Instance = GetComponent<GameManager>();
        
            //TODO: Load all the values from config
        
            //Load and Set FPS
            _fps                        = 60;
            Application.targetFrameRate = _fps;
        
            LoadBackground();
        }

        private void LoadBackground()
        {
            //Read the config value and replace relative with full path
            var bg = "~/.config/Backgrounds/Nord/NordicRuinsInASnowyBlizzard.jpg";
            bg = bg.Replace("~", System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal));
        
            //Check if the file exists
            Debug.Assert(File.Exists(bg), $"{bg} doesn't exist.");

            //Read image data and store it into a Texture2D
            var bgData = File.ReadAllBytes(bg);
            Texture2D bgTex = new(0, 0);
            Debug.Assert(bgTex.LoadImage(bgData), "Background file failed to load into a texture.\nAre you sure it's a valid image?");

            _background = Texture2DToSprite(bgTex);
        }

        private Sprite Texture2DToSprite(Texture2D texture) => Sprite.Create(texture, new(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
    }
}