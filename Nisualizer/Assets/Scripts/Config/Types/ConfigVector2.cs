using System;
using UnityEngine;

namespace Config.Types
{
    /// This class is used as a bridge between <see cref="Vector2"/> and JSON <br/>
    /// When used in <see cref="ConfigData"/>, make sure to assign null in <see cref="ConfigData.ResetToDefault"/> and default value in <see cref="ConfigData.Load"/> if the value is still null <br/>
    /// This approach prevents data stacking in case not all data is defined in the config
    [Serializable]
    public class ConfigVector2
    {
        public float X;
        public float Y;

        public ConfigVector2()
        {
            X = 0;
            Y = 0;
        }
        
        public ConfigVector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public ConfigVector2(Vector2 v)
        {
            X = v.x;
            Y = v.y;
        }
        
        /// Implicit conversion from <see cref="Vector2"/>
        public static implicit operator ConfigVector2(Vector2 v) => new(v.x, v.y);
        
        /// Implicit conversion to <see cref="Vector2"/>
        public static implicit operator Vector2(ConfigVector2 v) => new(v.X, v.Y);
    }
}