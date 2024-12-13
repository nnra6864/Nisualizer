using System;
using UnityEngine;

namespace Config.Types
{
    [Serializable]
    public class ConfigGradientColorKey
    {
        public ConfigColor Color;
        public float Time;

        /// Default ctor
        public ConfigGradientColorKey()
        {
            Color = new();
            Time  = 0;
        }

        /// Ctor from values
        public ConfigGradientColorKey(ConfigColor color, float time)
        {
            Color = color;
            Time  = time;
        }

        /// Ctor from <see cref="GradientAlphaKey"/>
        public ConfigGradientColorKey(GradientColorKey key)
        {
            Color = key.color;
            Time  = key.time;
        }

        /// Implicit operator from <see cref="GradientAlphaKey"/>
        public static implicit operator ConfigGradientColorKey(GradientColorKey key) => new(key);

        /// Implicit operator to <see cref="GradientAlphaKey"/>
        public static implicit operator GradientColorKey(ConfigGradientColorKey key) => new(key.Color, key.Time);
    }
}