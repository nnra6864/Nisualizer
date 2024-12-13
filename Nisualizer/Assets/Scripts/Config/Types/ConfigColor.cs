using System;
using UnityEngine;

namespace Config.Types
{
    /// This class is used as a bridge between <see cref="Color"/> and JSON <br/>
    /// When used in <see cref="ConfigData"/>, make sure to assign null in <see cref="ConfigData.ResetToDefault"/> and default value in <see cref="ConfigData.Load"/> if the value is still null <br/>
    /// This approach prevents data stacking in case not all data is defined in the config
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
            R = 0;
            G = 0;
            B = 0;
            A = 1;
            I = 1;
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
            I = 1;
        }

        /// Implicit conversion from <see cref="UnityEngine.Color"/>
        public static implicit operator ConfigColor(Color c) => new(c);
        
        /// Implicit conversion to <see cref="UnityEngine.Color"/>
        public static implicit operator Color(ConfigColor c) => new (c.R * c.I, c.G * c.I , c.B * c.I, c.A);
    }
}