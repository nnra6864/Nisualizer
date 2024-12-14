using Config;
using Config.Types;
using Newtonsoft.Json;
using NnUtils.Scripts;
using UnityEngine;

namespace Scenes.ChaoticCubes.Config
{
    [CreateAssetMenu(fileName = "ChaoticCubesConfigData", menuName = "Config/ChaoticCubesConfigData")]
    public class ChaoticCubesConfigData : ConfigData
    {
        [JsonIgnore] public string DefaultBackground = "";
        [ReadOnly] public string Background;
        
        [JsonIgnore] public float DefaultCameraDistance = 10;
        [ReadOnly] public float CameraDistance;

        [JsonIgnore] public Vector2 DefaultCameraClippingPlanes = new(1, 1000);
        [ReadOnly] public ConfigVector2 CameraClippingPlanes;

        [JsonIgnore] public float DefaultDirectionalLightIntensity = 1;
        [ReadOnly] public float DirectionalLightIntensity;
        
        public ChaoticCubesVFXData VFX;
        
        public override void Load()
        {
            CameraClippingPlanes ??= DefaultCameraClippingPlanes;
            
            VFX.Load();

            base.Load();
        }

        public override void ResetToDefault(bool silent = false)
        {
            VFX.ResetToDefault();

            Background                = DefaultBackground;
            CameraDistance            = DefaultCameraDistance;
            CameraClippingPlanes      = null;
            DirectionalLightIntensity = DefaultDirectionalLightIntensity;

            base.ResetToDefault(silent);
        }
    }
}
