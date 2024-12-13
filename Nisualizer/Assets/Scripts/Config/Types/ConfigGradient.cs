using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Config.Types
{
    [Serializable]
    public class ConfigGradient
    {
        public List<ConfigGradientAlphaKey> AlphaKeys;
        public List<ConfigGradientColorKey> ColorKeys;

        public ConfigGradient()
        {
            AlphaKeys = new();
            ColorKeys = new();
        }

        public ConfigGradient(Gradient gradient)
        {
            // Using Select() because it supports implicit conversion
            AlphaKeys = new(gradient.alphaKeys.Select(key => (ConfigGradientAlphaKey)key));
            ColorKeys = new(gradient.colorKeys.Select(key => (ConfigGradientColorKey)key));
        }

        /// Implicit conversion from <see cref="Gradient"/>
        public static implicit operator ConfigGradient(Gradient g) => new(g);

        /// Implicit conversion to <see cref="Gradient"/>
        public static implicit operator Gradient(ConfigGradient g) => new()
        {
            alphaKeys = g.AlphaKeys.Select(key => (GradientAlphaKey)key).ToArray(),
            colorKeys = g.ColorKeys.Select(key => (GradientColorKey)key).ToArray()
        };
    }
}