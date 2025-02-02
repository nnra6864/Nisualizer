using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using NnUtils.Scripts;
using Scripts.Core;
using UnityEngine;

namespace Scripts.Config
{
    public class LiveConfigReload : MonoBehaviour
    {
        private static string ConfigDir => GameManagerScript.ConfigDirectory;

        /// List of all the monitors
        private readonly List<FileSystemMonitor> _fsm = new();

        /// Set to true when there is a change
        private bool _hasChanged;

        /// Subscribe to this event from all the configs
        public event Action OnChanged;
        
        private List<string> _paths = new();
        
        /// Call when after initializing the general config
        public void Init()
        {
            // Get all the paths
            _paths = new()
            {
                ConfigDir,
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), ".config/fontconfig/")
            };

            // Clear previously initialized monitors
            ClearMonitors();
            
            // Start new monitors
            foreach (var path in _paths) _fsm.Add(new(path, () => _hasChanged = true));
        }
        
        private void Update() => HandleChange();

        /// Disposes all the monitors
        private void ClearMonitors()
        {
            foreach (var fsm in _fsm) fsm.Dispose();
            _fsm.Clear();
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