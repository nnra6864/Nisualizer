using System;
using UnityEngine;

namespace Config.Types
{
    [Serializable]
    public class ConfigColor
    {
        /// Red
        public float R;
        /// Green
        public float G;
        /// Blue
        public float B;
        /// Alpha
        public float A;
        /// Intensity
        public float I;

        public ConfigColor()
        {
            R = 1;
            G = 1;
            B = 1;
            A = 1;
            I = 0;
        }
        
        public ConfigColor(ConfigColor c)
        {
            R = c.R;
            G = c.G;
            B = c.B;
            A = c.A;
            I = c.I;
        }

        public ConfigColor(Color c)
        {
            R = c.r;
            G = c.g;
            B = c.b;
            A = c.a;
            I = 0;
        }

        /// Implicit conversion from <see cref="UnityEngine.Color"/>
        public static implicit operator ConfigColor(Color c) => new(c);
        
        /// Implicit conversion to <see cref="UnityEngine.Color"/>
        public static implicit operator Color(ConfigColor c) => new (c.R * c.I, c.G * c.I , c.B * c.I, c.A);
    }
}