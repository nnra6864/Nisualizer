using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Core;
using NnUtils.Scripts;
using UnityEngine;

namespace Config
{
    public class LiveConfigReload : MonoBehaviour
    {
        private static string ConfigDir => GameManagerScript.ConfigDirectory;

        private readonly List<FileSystemMonitor> _fsm = new();

        private bool _hasChanged;

        /// Subscribe to this event from all the configs
        public event Action OnChanged;
        
        private List<string> _paths = new();
        
        /// Call when after initializing the general config
        public void Init()
        {
            _paths = new()
            {
                ConfigDir,
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), ".config/fontconfig/")
            };

            _fsm.Clear();
            
            foreach (var path in _paths) _fsm.Add(new(path, () => _hasChanged = true));
        }
        
        private void Update()
        {
            HandleChange();
        }

        private void HandleChange()
        {
            if (!_hasChanged) return;
            _hasChanged = false;
            this.RestartRoutine(ref _handleConfigChangedRoutine, HandleConfigChangedRoutine());
        }

        private Coroutine _handleConfigChangedRoutine;
        
        private IEnumerator HandleConfigChangedRoutine()
        {
            yield return new WaitForSecondsRealtime(GameManagerScript.ConfigData.ReloadDelay);
            OnChanged?.Invoke();
        }
    }
}