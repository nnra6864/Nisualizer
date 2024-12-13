using UnityEngine;

namespace Config.Types
{
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