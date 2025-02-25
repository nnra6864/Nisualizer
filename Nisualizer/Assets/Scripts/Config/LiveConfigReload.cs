using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NnUtils.Scripts;
using Scripts.Core;
using UnityEngine;

namespace Scripts.Config
{
    public class LiveConfigReload : MonoBehaviour
    {
        private static string ConfigDir => GameManagerScript.ConfigDirectory;

        /// List of all the monitors
        private readonly HashSet<FileSystemMonitor> _fsm = new();

        /// Set to true when there is a change
        private bool _hasChanged;

        /// Subscribe to this event from all the configs
        public event Action OnChanged;
        
        private readonly HashSet<string> _paths = new();

        /// Call when after initializing the general config
        public void Init()
        {
            // Clear paths and monitors
            _paths.Clear();
            ClearMonitors();

            // Get all the paths
            string[] paths = {
                ConfigDir,
#if UNITY_STANDALONE_LINUX || UNITY_EDITOR_LINUX
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), ".config/fontconfig/")
#endif
            };

            // Start new monitors
            MonitorPaths(paths);
        }

        private void Update() => HandleChange();

        /// Disposes all the monitors
        private void ClearMonitors()
        {
            foreach (var fsm in _fsm) fsm.Dispose();
            _fsm.Clear();
        }

        /// Starts monitoring a path
        private void MonitorPath(string path)
        {
            if (!_paths.Add(path)) return;
            
            if (_fsm.FirstOrDefault(x => x.Path == path) != null) return;
            
            // Continue if dir/file doesn't exist
            if (!Directory.Exists(path) && !File.Exists(path))
            {
                Debug.LogWarning($"Path {path} does not exist, it won't be monitored for changes.");
                return;
            }

            _fsm.Add(new(path, () => _hasChanged = true));
        }

        /// Calls <see cref="MonitorPath"/> for all paths
        public void MonitorPaths(string[] paths)
        {
            foreach(var path in paths) MonitorPath(path);
        }
        
        /// Checks if there is a change and restarts the routine
        private void HandleChange()
        {
            if (!_hasChanged) return;
            _hasChanged = false;
            this.RestartRoutine(ref _handleConfigChangedRoutine, HandleConfigChangedRoutine());
        }

        private Coroutine _handleConfigChangedRoutine;
        
        /// Handles config being changed
        private IEnumerator HandleConfigChangedRoutine()
        {
            yield return new WaitForSecondsRealtime(GameManagerScript.ConfigData.ReloadDelay);
            OnChanged?.Invoke();
        }
    }
}