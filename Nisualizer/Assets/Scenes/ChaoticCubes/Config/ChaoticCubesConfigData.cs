using Config;
using Newtonsoft.Json;
using NnUtils.Modules.JSONUtils.Scripts.Types;
using NnUtils.Modules.JSONUtils.Scripts.Types.Components;
using NnUtils.Scripts;
using UnityEngine;

namespace Scenes.ChaoticCubes.Config
{
    [CreateAssetMenu(fileName = "ChaoticCubesConfigData", menuName = "Config/ChaoticCubesConfigData")]
    public class ChaoticCubesConfigData : ConfigData
    {
        [JsonIgnore] public string DefaultBackground = "";
        [JsonProperty] public string Background;
        
        [JsonIgnore] public float DefaultCameraDistance = 10;
        [JsonProperty] public float CameraDistance;

        [JsonIgnore] public ConfigVector2 DefaultCameraClippingPlanes = new(1, 1000);
        [JsonProperty] public ConfigVector2 CameraClippingPlanes;

        [JsonIgnore] public float DefaultDirectionalLightIntensity = 1;
        [JsonProperty] public float DirectionalLightIntensity;
        
        [JsonProperty] public ChaoticCubesVFXData VFX;
        [JsonProperty] public ConfigGameObject[] Objects;

        public override void Load()
        {
            VFX.Load();

            base.Load();
        }

        public override void ResetToDefault(bool silent = false)
        {
            Background                = DefaultBackground;
            CameraDistance            = DefaultCameraDistance;
            CameraClippingPlanes      = DefaultCameraClippingPlanes;
            DirectionalLightIntensity = DefaultDirectionalLightIntensity;
            VFX.ResetToDefault();
            Objects = new ConfigGameObject[] { };
            base.ResetToDefault(silent);
        }
    }
}
