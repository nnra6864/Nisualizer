using System;
using UnityEngine;

namespace Scripts.Config
{
    [CreateAssetMenu(fileName = "ConfigData", menuName = "Config/ConfigData", order = -201)]
    public class ConfigData : ScriptableObject
    {
        public Action OnLoaded;

        /// Call base.Load() at the end of overriden functions to trigger the <see cref="OnLoaded"/> event
        public virtual void Load()
        {
            OnLoaded?.Invoke();
        }
        
        /// Call base.Reset() at the end of overriden functions to trigger the <see cref="OnLoaded"/> event
        public virtual void ResetToDefault(bool silent = false)
        {
            if (!silent) OnLoaded?.Invoke();
        }
    }
}