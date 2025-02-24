using Newtonsoft.Json;
using NnUtils.Modules.JSONUtils.Scripts.Types;
using NnUtils.Modules.JSONUtils.Scripts.Types.Components;
using Scripts.Config;
using UnityEngine;

namespace Scenes.ChaoticCubes.Config
{
    [CreateAssetMenu(fileName = "ChaoticCubesConfigData", menuName = "Config/ChaoticCubesConfigData")]
    public class ChaoticCubesConfigData : ConfigData
    {
        [JsonIgnore] public float DefaultCameraDistance = 10;
        [JsonProperty] public float CameraDistance;

        [JsonIgnore] public ConfigVector2 DefaultCameraClippingPlanes = new(1, 1000);
        [JsonProperty] public ConfigVector2 CameraClippingPlanes;

        [JsonIgnore] public float DefaultDirectionalLightIntensity = 1;
        [JsonProperty] public float DirectionalLightIntensity;
        
        [JsonIgnore] public Vector3 DefaultVFXPosition = Vector3.zero;
        [JsonProperty] public ConfigVector3 VFXPosition;
        
        [JsonProperty] public ChaoticCubesVFXData VFX;
        [JsonProperty] public ConfigGameObject[] Objects;

        public override void Load()
        {
            VFX.Load();
            base.Load();
        }

        public override void ResetToDefault(bool silent = false)
        {
            CameraDistance            = DefaultCameraDistance;
            CameraClippingPlanes      = DefaultCameraClippingPlanes;
            DirectionalLightIntensity = DefaultDirectionalLightIntensity;
            VFXPosition               = DefaultVFXPosition;
            VFX.ResetToDefault();
            Objects = new ConfigGameObject[] { };
            base.ResetToDefault(silent);
        }
    }
}
