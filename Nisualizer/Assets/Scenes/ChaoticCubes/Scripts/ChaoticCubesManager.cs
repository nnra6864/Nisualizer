using Core;
using Scenes.ChaoticCubes.Config;
using UnityEngine;
using UnityEngine.VFX;

namespace Scenes.ChaoticCubes.Scripts
{
    public class ChaoticCubesManager : SceneManagerScript
    {
        public static ChaoticCubesConfigData ConfigData => (ChaoticCubesConfigData)Config.Data;

        [SerializeField] private VisualEffect _chaoticCubesVFX;
        [SerializeField] private Camera _camera;
        [SerializeField] private Light _directionalLight;
        
        private void Start()
        {
            OnConfigLoaded();
            ConfigData.OnLoaded += OnConfigLoaded;
        }

        private void OnDestroy()
        {
            ConfigData.OnLoaded -= OnConfigLoaded;
        }

        private void OnConfigLoaded()
        {
            UpdateCamera();
            _directionalLight.intensity = ConfigData.DirectionalLightIntensity;
            UpdateChaoticCubesVFX();
        }

        private void UpdateCamera()
        {
            var camPos = _camera.transform.localPosition;
            _camera.transform.localPosition = new(camPos.x, camPos.y, -ConfigData.CameraDistance);

            _camera.nearClipPlane = ConfigData.CameraClippingPlanes.X;
            _camera.farClipPlane  = ConfigData.CameraClippingPlanes.Y;
        }

        private void UpdateChaoticCubesVFX()
        {
            var vfx = ConfigData.VFX;
            
            _chaoticCubesVFX.SetVector2("SpawnRateRange", vfx.SpawnRateRange);
            _chaoticCubesVFX.SetFloat("SpawnRadius", vfx.SpawnRadius);
            _chaoticCubesVFX.SetVector2("LifetimeRange", vfx.LifetimeRange);
            _chaoticCubesVFX.SetVector2("SpeedRange", vfx.SpeedRange);
            
            _chaoticCubesVFX.SetFloat("TurbulenceRotationSpeed", vfx.Turbulence.RotationSpeed);
            _chaoticCubesVFX.SetVector2("TurbulenceIntensityRange", vfx.Turbulence.IntensityRange);
            _chaoticCubesVFX.SetVector2("TurbulenceDragRange", vfx.Turbulence.DragRange);
            _chaoticCubesVFX.SetFloat("TurbulenceFrequency", vfx.Turbulence.Frequency);
            _chaoticCubesVFX.SetInt("TurbulenceOctaves", vfx.Turbulence.Octaves);
            _chaoticCubesVFX.SetFloat("TurbulenceRoughness", vfx.Turbulence.Roughness);
            _chaoticCubesVFX.SetFloat("TurbulenceLacunarity", vfx.Turbulence.Lacunarity);
            
            _chaoticCubesVFX.SetFloat("MeshSmoothness", vfx.MeshSmoothness);
            _chaoticCubesVFX.SetFloat("MeshMetallic", vfx.MeshMetallic);
            _chaoticCubesVFX.SetFloat("MeshSize", vfx.MeshSize);
            _chaoticCubesVFX.SetGradient("ColorOverLife", ConfigData.VFX.ColorOverLife);
        }
    }
}
