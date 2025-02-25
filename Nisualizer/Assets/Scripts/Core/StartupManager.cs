using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace Scripts.Core
{
    // BUG: THIS DON'T WORKY
    public class StartupManager : MonoBehaviour
    {
        private void Start()
        {
            
        }

        private void UpdateLaunchOnStartup()
        {
            
        }
        
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR_WIN
        private readonly string _startupFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);

        private void LaunchOnStartup()
        {
            if (Directory.GetCurrentDirectory() != _startupFolder)
            {
                var path = Path.Combine(_startupFolder, "Nisualizer.exe");
                var ownPath = Assembly.GetExecutingAssembly().Location;
                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                File.Copy(ownPath, path);
            }
        }
#endif
    }
}