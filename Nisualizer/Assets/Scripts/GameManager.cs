using UnityEngine;

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

    private string _background;
    public static string Background => Instance._background;
    
    private void Awake()
    {
        //Set Instance
        Instance = GetComponent<GameManager>();
        
        //TODO: Load all the values from config
        
        //Load and Set FPS
        _fps = 60;
        Application.targetFrameRate = _fps;
        
        //Load Background
        _background = "~/.config/Backgrounds/Nord/NordicRuinsInASnowyBlizzard.jpg";
    }
}