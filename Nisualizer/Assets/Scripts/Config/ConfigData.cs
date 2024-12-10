using System;
using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "ConfigData", menuName = "Config/ConfigData", order = -201)]
    public class ConfigData : ScriptableObject
    {
        public Action OnLoaded;

        /// <summary>
        /// Call base.Load() at the end of overriden functions to trigger the OnConfigLoaded event
        /// </summary>
        public virtual void Load()
        {
            OnLoaded?.Invoke();
        }
        
        public virtual void ResetToDefault(bool silent = false)
        {
            if (!silent) OnLoaded?.Invoke();
        }
    }
}