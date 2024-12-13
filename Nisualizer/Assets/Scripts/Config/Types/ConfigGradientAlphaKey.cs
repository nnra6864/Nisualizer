using System;
using UnityEngine;

namespace Config.Types
{
    /// This class is used as a bridge between <see cref="GradientAlphaKey"/> and JSON <br/>
    /// When used in <see cref="ConfigData"/>, make sure to assign null in <see cref="ConfigData.ResetToDefault"/> and default value in <see cref="ConfigData.Load"/> if the value is still null <br/>
    /// This approach prevents data stacking in case not all data is defined in the config
    [Serializable]
    public class ConfigGradientAlphaKey
    {
        public float Alpha;
        public float Time;

        /// Default ctor
        public ConfigGradientAlphaKey()
        {
            Alpha = 1;
            Time  = 0;
        }

        /// Ctor from values
        public ConfigGradientAlphaKey(float alpha, float time)
        {
            Alpha = alpha;
            Time = time;
        }
        
        /// Ctor from <see cref="GradientAlphaKey"/>
        public ConfigGradientAlphaKey(GradientAlphaKey key)
        {
            Alpha = key.alpha;
            Time  = key.time;
        }

        /// Implicit operator from <see cref="GradientAlphaKey"/>
        public static implicit operator ConfigGradientAlphaKey(GradientAlphaKey key) => new(key);
        
        /// Implicit operator to <see cref="GradientAlphaKey"/>
        public static implicit operator GradientAlphaKey(ConfigGradientAlphaKey key) => new(key.Alpha, key.Time);
    }
}